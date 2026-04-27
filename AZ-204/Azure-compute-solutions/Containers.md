# Containerized solutions - notatki AZ-204

## 1. Zakres na AZ-204

W tym obszarze trzeba znać:

- tworzenie i zarządzanie container images,
- publikowanie image do Azure Container Registry,
- uruchamianie containers w Azure Container Instances,
- tworzenie rozwiązań w Azure Container Apps.

Najważniejsze usługi:

- **Docker image** - immutable package z aplikacją, runtime i zależnościami.
- **Container** - uruchomiona instancja image.
- **Azure Container Registry** - prywatny registry na images i artifacts.
- **Azure Container Instances** - szybkie uruchamianie pojedynczych containers/container groups bez orkiestracji.
- **Azure Container Apps** - serverless platforma dla containerized apps, microservices, APIs i event-driven processing.

Szybki wybór usługi:

| Wymaganie | Wybór |
| --- | --- |
| Przechowywanie i wersjonowanie images | Azure Container Registry |
| Build image w chmurze bez lokalnego Docker Engine | ACR Tasks / `az acr build` |
| Jednorazowy container task bez skalowania | Azure Container Instances |
| Prosty container z publicznym FQDN | Azure Container Instances |
| Microservices, scaling, revisions, ingress, Dapr | Azure Container Apps |
| Własny orchestrator i pełna kontrola Kubernetes | AKS, poza tym konkretnym zakresem |

## 2. Container images i Dockerfile

Container image zawiera aplikację i wszystko, co potrzebne do jej uruchomienia. Image jest budowany z `Dockerfile`.

Podstawowe komendy Docker:

```bash
docker build -t myapp:v1 .
docker run -p 8080:80 myapp:v1
docker tag myapp:v1 myregistry.azurecr.io/myapp:v1
docker push myregistry.azurecr.io/myapp:v1
docker pull myregistry.azurecr.io/myapp:v1
```

Typowy format image w ACR:

```text
<registry>.azurecr.io/<repository>/<image>:<tag>
```

Przykład:

```text
contosoregistry.azurecr.io/store/api:v1
```

Ważne:

- `repository` może działać jak namespace, np. `team1/orders-api`.
- `tag` identyfikuje wersję, np. `v1`, `1.0.3`, `20260424.1`.
- Nie polegaj na `latest` w produkcji, bo utrudnia rollback i audyt.
- Image powinien być możliwie mały.
- Sekrety nie powinny być wpisane w Dockerfile ani baked into image.

### Multi-stage Dockerfile

Multi-stage build pozwala budować aplikację w jednym image, a uruchamiać ją w mniejszym runtime image.

Przykład dla .NET:

```Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "MyApp.dll"]
```

Ważne na egzamin:

- `sdk` image służy do build.
- `aspnet` / runtime image służy do uruchomienia.
- `COPY --from=build` kopiuje output z build stage do final stage.
- Multi-stage build zmniejsza final image i zmniejsza powierzchnię ataku.

## 3. Azure Container Registry

Azure Container Registry to managed Docker registry w Azure. Służy do przechowywania:

- container images,
- Helm charts,
- OCI artifacts.

Endpoint registry:

```text
<registry-name>.azurecr.io
```

Tworzenie ACR:

```bash
az group create \
  --name rg-containers-demo \
  --location westeurope

az acr create \
  --resource-group rg-containers-demo \
  --name acrdemo204 \
  --sku Standard
```

Pobranie login server:

```bash
az acr show \
  --name acrdemo204 \
  --query loginServer \
  --output tsv
```

### ACR SKUs

| SKU | Kiedy użyć |
| --- | --- |
| Basic | Dev/test, mały wolumen, niski koszt. |
| Standard | Typowa produkcja, większy storage i throughput. |
| Premium | Największy throughput, private endpoints, firewall, geo-replication, zone redundancy, content trust/scenario enterprise. |

Ważne:

- Basic/Standard/Premium różnią się limitem storage, throughput i enterprise features.
- Premium jest wyborem dla private networking, geo-replication i wysokiego wolumenu.
- Zbyt dużo starych repositories/tags może pogarszać performance; czyść nieużywane artifacts.
- Przy przekroczeniu limitów registry może zwracać throttling / HTTP 429.

## 4. Publish image to Azure Container Registry

### Opcja 1: lokalny Docker build + push

```bash
az acr login --name acrdemo204

docker build -t orders-api:v1 .

docker tag orders-api:v1 acrdemo204.azurecr.io/orders-api:v1

docker push acrdemo204.azurecr.io/orders-api:v1
```

Weryfikacja:

```bash
az acr repository list \
  --name acrdemo204 \
  --output table

az acr repository show-tags \
  --name acrdemo204 \
  --repository orders-api \
  --output table
```

### Opcja 2: ACR Tasks quick task

`az acr build` buduje image w Azure i domyślnie pushuje go do registry.

```bash
az acr build \
  --registry acrdemo204 \
  --image orders-api:v1 .
```

Ważne:

- Działa jak `docker build` + `docker push` w chmurze.
- Nie wymaga lokalnego Docker Engine.
- Build działa blisko registry, więc upload/push jest wygodniejszy.

### Opcja 3: import image

Import pozwala przenieść image z innego registry bez lokalnego pull/push.

```bash
az acr import \
  --name acrdemo204 \
  --source mcr.microsoft.com/hello-world \
  --image samples/hello-world:v1
```

## 5. Authentication and authorization in ACR

Opcje logowania:

| Sposób | Kiedy użyć |
| --- | --- |
| Individual Microsoft Entra ID | Developer interactive push/pull. |
| Service principal | CI/CD, headless automation. |
| Managed identity | Azure resources pulling/pushing bez sekretów, np. Container Apps, App Service, ACI. |
| Admin user | Proste testy; zwykle niezalecane w produkcji. |

Role RBAC:

- `AcrPull` - pull images.
- `AcrPush` - push i pull images.
- `AcrDelete` - usuwanie repositories/tags/manifests.
- `Owner` / `Contributor` - zbyt szerokie dla samego push/pull.

Przykład: service principal z AcrPush:

```bash
az ad sp create-for-rbac \
  --name sp-acr-push \
  --role AcrPush \
  --scopes $(az acr show --name acrdemo204 --query id --output tsv)
```

Przykład: managed identity dostaje `AcrPull`:

```bash
az role assignment create \
  --assignee <principal-id> \
  --role AcrPull \
  --scope $(az acr show --name acrdemo204 --query id --output tsv)
```

Ważne na egzamin:

- Deweloperom do publikowania images daj `AcrPush`, nie `Contributor`.
- Runtime, który tylko pobiera image, powinien mieć `AcrPull`.
- Admin account jest disabled by default i ma dwa passwords, które można regenerować.
- `az acr login` dla Entra ID token jest dobre interaktywnie, ale token jest krótkotrwały.

## 6. ACR Tasks

ACR Tasks automatyzują:

- cloud-based builds,
- build + push image,
- build on source commit,
- build on pull request,
- rebuild po zmianie base image,
- scheduled tasks,
- multi-step workflows.

Rodzaje:

### Quick task

```bash
az acr build \
  --registry acrdemo204 \
  --image web:v1 .
```

### Automatically triggered task

Build po commit albo pull request.

```bash
az acr task create \
  --registry acrdemo204 \
  --name web-ci \
  --image web:{{.Run.ID}} \
  --context https://github.com/org/repo.git \
  --file Dockerfile \
  --git-access-token <token>
```

### Scheduled task

Przydatne do regularnych rebuilds, np. OS/framework patching.

### Multi-step task

Definiowany w YAML, może buildować, testować, uruchamiać i pushować kilka artifacts.

```yaml
version: v1.1.0
steps:
  - build: -t {{.Run.Registry}}/web:{{.Run.ID}} .
  - cmd: {{.Run.Registry}}/web:{{.Run.ID}}
  - push:
    - {{.Run.Registry}}/web:{{.Run.ID}}
```

Ważne:

- Domyślna platforma buildów ACR Tasks to Linux/amd64.
- Można ustawić `--platform`, np. Windows/amd64 albo Linux/arm64.
- Sekretów i tokenów nie wkładaj w command line, jeśli mogą trafić do logs.

## 7. Azure Container Instances

Azure Container Instances pozwala uruchomić container bez VM i bez pełnej orkiestracji.

Dobre zastosowania:

- proste containerized workloads,
- run-once jobs,
- batch tasks,
- build/test/image rendering,
- szybki test image,
- proste publiczne endpoints.

Nie jest dobre do:

- autoscaling,
- zaawansowanej orkiestracji,
- rolling deployments,
- traffic splitting,
- service mesh,
- microservices platform.

Do tego użyj raczej Azure Container Apps albo AKS.

### Container group

ACI uruchamia **container group**. Container group:

- ma wspólny lifecycle,
- współdzieli resources hosta,
- współdzieli network namespace,
- może mieć wspólne volumes,
- ma wspólny public IP/FQDN, jeśli wystawiony publicznie.

Wiele containers w jednej container group może komunikować się przez `localhost`.

Ważne:

- Linux container group może mieć wiele containers.
- Windows container group jest ograniczony do single container.
- Zasoby grupy są sumą requested CPU/RAM dla containers.

### Create container

```bash
az container create \
  --resource-group rg-containers-demo \
  --name aci-demo \
  --image mcr.microsoft.com/azuredocs/aci-helloworld \
  --dns-name-label aci-demo-204 \
  --ports 80 \
  --ip-address Public
```

Sprawdzenie:

```bash
az container show \
  --resource-group rg-containers-demo \
  --name aci-demo \
  --query "{FQDN:ipAddress.fqdn,State:provisioningState}" \
  --output table
```

Logs:

```bash
az container logs \
  --resource-group rg-containers-demo \
  --name aci-demo
```

Attach:

```bash
az container attach \
  --resource-group rg-containers-demo \
  --name aci-demo
```

Różnica:

- `az container logs` - pobiera logs.
- `az container attach` - stream stdout/stderr w czasie rzeczywistym.

### Restart policy

| Policy | Znaczenie |
| --- | --- |
| `Always` | Domyślne; container restartuje się zawsze. |
| `OnFailure` | Restart tylko przy nonzero exit code. |
| `Never` | Container uruchamia się najwyżej raz. |

Przykład run-once:

```bash
az container create \
  --resource-group rg-containers-demo \
  --name aci-job \
  --image myjob:v1 \
  --restart-policy Never
```

Ważne:

- ACI jest billingowane per second, gdy container działa.
- Dla run-once jobs używaj `Never` albo `OnFailure`.
- Jeśli container group z public IP restartuje się, IP może się zmienić. Nie hardcoduj IP.
- Dla stabilnego public endpoint rozważ Application Gateway / inny front door pattern.

### Environment variables and secrets

Public env vars:

```bash
az container create \
  --resource-group rg-containers-demo \
  --name aci-env \
  --image myimage:v1 \
  --environment-variables ENVIRONMENT=prod
```

Secure env vars:

```bash
az container create \
  --resource-group rg-containers-demo \
  --name aci-env \
  --image myimage:v1 \
  --secure-environment-variables DbPassword="secret"
```

Ważne:

- Secure env var value nie jest widoczny w properties container group.
- W CLI/portal zobaczysz nazwę zmiennej, ale nie wartość.

### Volumes

ACI wspiera m.in. Azure Files i secret volumes.

Azure Files:

- daje persistent storage,
- używa SMB,
- wymaga storage account name, share name i storage account key,
- może być montowane do Linux containers running as root,
- Blob Storage nie jest bezpośrednio mountowane w ACI, bo nie wspiera SMB jak Azure Files.

Przykład:

```bash
az container create \
  --resource-group rg-containers-demo \
  --name aci-files \
  --image mcr.microsoft.com/azuredocs/aci-hellofiles \
  --azure-file-volume-account-name <storage-account> \
  --azure-file-volume-account-key <storage-key> \
  --azure-file-volume-share-name <share-name> \
  --azure-file-volume-mount-path /aci/logs/
```

Secret volume:

- mounted jako files,
- secret values w YAML muszą być Base64-encoded.

### YAML vs ARM

| Scenariusz | Zalecenie |
| --- | --- |
| Multi-container group tylko z container instances | YAML jest czytelny i wygodny. |
| Container group + dodatkowe Azure resources | ARM/Bicep. |
| Prosty pojedynczy container | `az container create`. |

## 8. Azure Container Apps

Azure Container Apps to serverless platform dla containerized applications. Opiera się na środowisku Container Apps Environment i abstrahuje Kubernetes.

Dobre zastosowania:

- APIs,
- microservices,
- background workers,
- event-driven processing,
- apps skalowane do zera,
- Dapr-based services,
- KEDA-based scaling.

Ograniczenia:

- obsługuje Linux container images,
- typowy wymagany architecture: linux/amd64,
- nie uruchamia privileged containers,
- stan w filesystem container nie jest trwały; używaj zewnętrznego storage/cache/database.

### Container Apps Environment

Environment to granica dla:

- regionu,
- virtual network,
- Log Analytics workspace,
- części ustawień network/observability,
- grupy Container Apps.

Kiedy użyć wielu environments:

- różne VNets,
- różne Log Analytics workspaces,
- różne regiony,
- izolacja środowisk dev/test/prod,
- osobne wymagania network/security.

Tworzenie:

```bash
az extension add --name containerapp --upgrade

az provider register --namespace Microsoft.App
az provider register --namespace Microsoft.OperationalInsights

az containerapp env create \
  --name caenv-prod \
  --resource-group rg-containers-demo \
  --location westeurope
```

### Create Container App

```bash
az containerapp create \
  --name api-app \
  --resource-group rg-containers-demo \
  --environment caenv-prod \
  --image mcr.microsoft.com/azuredocs/containerapps-helloworld:latest \
  --target-port 80 \
  --ingress external
```

Szybsza opcja dev/test:

```bash
az containerapp up \
  --name api-app \
  --resource-group rg-containers-demo \
  --location westeurope \
  --environment caenv-prod \
  --image mcr.microsoft.com/azuredocs/containerapps-helloworld:latest \
  --target-port 80 \
  --ingress external
```

Różnica:

- `az containerapp create` - więcej kontroli, dobre do produkcji/skryptów.
- `az containerapp up` - szybki path, może utworzyć brakujące zasoby.

## 9. Ingress in Azure Container Apps

Ingress określa, czy i jak app jest dostępna sieciowo.

Typy:

- `external` - publiczny dostęp z internetu.
- `internal` - dostęp tylko wewnątrz environment/VNet.
- disabled - brak publicznego HTTP ingress, np. worker.

Ważne:

- `target-port` musi odpowiadać portowi aplikacji w container.
- Auth działa dla external ingress i HTTPS.
- `allowInsecure=false` wymusza HTTPS.
- FQDN można pobrać przez `az containerapp show`.

Przykład:

```bash
az containerapp show \
  --name api-app \
  --resource-group rg-containers-demo \
  --query properties.configuration.ingress.fqdn
```

## 10. Revisions in Azure Container Apps

Revision to immutable snapshot wersji Container App.

Nowa revision powstaje przy zmianach revision-scope, np.:

- image,
- command/args,
- environment variables,
- CPU/memory,
- scale rules,
- containers,
- revision suffix.

Application-scope changes nie tworzą revision, np.:

- secrets,
- ingress,
- registry credentials,
- Dapr config,
- revision mode.

Revision modes:

| Mode | Znaczenie |
| --- | --- |
| Single revision | Aktywna jest jedna revision; dobra dla prostego rollout. |
| Multiple revisions | Wiele revisions może działać jednocześnie; traffic splitting/canary. |

Traffic splitting:

```bash
az containerapp ingress traffic set \
  --name api-app \
  --resource-group rg-containers-demo \
  --revision-weight api-app--rev1=90 api-app--rev2=10
```

Labels:

- pozwalają nadać stable URL konkretnej revision,
- użyteczne do testów, canary, direct routing.

Na egzaminie:

- Zmiana `properties.template` tworzy nową revision.
- Zmiana `properties.configuration` zwykle dotyczy aplikacji globalnie i nie tworzy revision.
- Secrets są application-scope; zmiana secret nie tworzy revision i może wymagać restartu albo nowej revision, aby app odczytała nową wartość.

## 11. Scaling in Azure Container Apps

Azure Container Apps używa KEDA-style scaling.

Kategorie scale rules:

- HTTP - liczba concurrent HTTP requests.
- TCP - liczba concurrent TCP connections.
- Custom - CPU, memory, Azure Queue, Service Bus, Event Hubs, Kafka, Redis itd.

Przykład HTTP scaling:

```bash
az containerapp create \
  --name api-app \
  --resource-group rg-containers-demo \
  --environment caenv-prod \
  --image acrdemo204.azurecr.io/api:v1 \
  --target-port 80 \
  --ingress external \
  --min-replicas 0 \
  --max-replicas 5 \
  --scale-rule-name http-rule \
  --scale-rule-type http \
  --scale-rule-http-concurrency 50
```

Przykład Service Bus scaling z secret:

```bash
az containerapp create \
  --name worker-app \
  --resource-group rg-containers-demo \
  --environment caenv-prod \
  --image acrdemo204.azurecr.io/worker:v1 \
  --min-replicas 0 \
  --max-replicas 10 \
  --secrets "servicebus-connection=<connection-string>" \
  --scale-rule-name sb-rule \
  --scale-rule-type azure-servicebus \
  --scale-rule-metadata "queueName=orders" "namespace=sb-demo" "messageCount=5" \
  --scale-rule-auth "connection=servicebus-connection"
```

Managed identity for scale rules:

- Aktualna dokumentacja Microsoft Learn mówi, że scale rules dla Azure resources, np. Azure Queue Storage, Service Bus, Event Hubs, mogą używać managed identity.
- Gdy to możliwe, używaj managed identity zamiast sekretów.

Przykład z user-assigned identity:

```bash
az containerapp create \
  --resource-group rg-containers-demo \
  --name queue-worker \
  --environment caenv-prod \
  --image acrdemo204.azurecr.io/worker:v1 \
  --user-assigned <USER_ASSIGNED_IDENTITY_ID> \
  --scale-rule-name azure-queue \
  --scale-rule-type azure-queue \
  --scale-rule-metadata "accountName=storders" "queueName=orders" "queueLength=1" \
  --scale-rule-identity <USER_ASSIGNED_IDENTITY_ID>
```

Ważne:

- Default replicas często mieszczą się w zakresie 0-10, jeśli nie ustawisz inaczej.
- Jeśli ingress jest disabled i nie ma custom scale rule ani `minReplicas > 0`, worker może skalować do zera i nie wystartować.
- CPU/memory scale rules nie skalują do zera, bo potrzebują działającej repliki do pomiaru.

## 12. Secrets and environment variables in Container Apps

Secrets:

- są scoped to application,
- nie są przypisane do konkretnej revision,
- mogą być używane przez wiele revisions,
- zmiana secret nie tworzy nowej revision,
- istniejące revisions nie zawsze automatycznie odczytają zmianę; trzeba restartować revision albo wdrożyć nową.

Definicja secret:

```bash
az containerapp secret set \
  --name api-app \
  --resource-group rg-containers-demo \
  --secrets "db-password=super-secret"
```

Referencja w env var:

```bash
az containerapp update \
  --name api-app \
  --resource-group rg-containers-demo \
  --set-env-vars "DbPassword=secretref:db-password"
```

Key Vault reference:

```bash
az containerapp create \
  --name api-app \
  --resource-group rg-containers-demo \
  --environment caenv-prod \
  --image acrdemo204.azurecr.io/api:v1 \
  --user-assigned <USER_ASSIGNED_IDENTITY_ID> \
  --secrets "db-password=keyvaultref:<KEY_VAULT_SECRET_URI>,identityref:<USER_ASSIGNED_IDENTITY_ID>" \
  --env-vars "DbPassword=secretref:db-password"
```

Secret volume:

- secret name staje się filename,
- secret value staje się file content,
- użyteczne, gdy aplikacja oczekuje plików z sekretami.

## 13. Pull images from ACR in Container Apps

Najbezpieczniej użyć managed identity z rolą `AcrPull`.

```bash
az containerapp identity assign \
  --name api-app \
  --resource-group rg-containers-demo \
  --system-assigned

az role assignment create \
  --assignee <principal-id> \
  --role AcrPull \
  --scope $(az acr show --name acrdemo204 --query id --output tsv)
```

Następnie skonfiguruj registry:

```bash
az containerapp registry set \
  --name api-app \
  --resource-group rg-containers-demo \
  --server acrdemo204.azurecr.io \
  --identity system
```

Ważne:

- Do pull wystarczy `AcrPull`.
- Do push/build daj `AcrPush`.
- Unikaj admin user i registry password w produkcji.

## 14. Dapr in Azure Container Apps

Dapr może być włączony per Container App i działa jako sidecar.

Najważniejsze building blocks:

- service-to-service invocation,
- state management,
- pub/sub,
- bindings,
- actors,
- observability,
- secrets.

Włączenie:

```bash
az containerapp create \
  --name api-app \
  --resource-group rg-containers-demo \
  --environment caenv-prod \
  --image acrdemo204.azurecr.io/api:v1 \
  --dapr-enabled \
  --dapr-app-id api
```

Ważne:

- Aplikacja komunikuje się z Dapr przez HTTP/gRPC.
- Dapr components są konfigurowane w environment.
- Domyślnie Dapr-enabled apps mogą ładować dostępne components; app scopes ograniczają, które apps mogą używać komponentu.

## 15. Logging and diagnostics

### ACR

Sprawdzenie repositories:

```bash
az acr repository list --name acrdemo204 --output table
```

Task logs:

```bash
az acr task logs \
  --registry acrdemo204 \
  --name web-ci
```

### ACI

```bash
az container logs \
  --resource-group rg-containers-demo \
  --name aci-demo

az container attach \
  --resource-group rg-containers-demo \
  --name aci-demo
```

### Container Apps

Log types:

- system logs,
- console logs from stdout/stderr.

Log Analytics tables:

- `ContainerAppConsoleLogs_CL`,
- `ContainerAppSystemLogs_CL`.

Przykład query:

```bash
az monitor log-analytics query \
  --workspace <workspace-id> \
  --analytics-query "ContainerAppConsoleLogs_CL | where ContainerAppName_s == 'api-app' | take 20"
```

## 16. Disaster recovery and availability

ACR:

- Premium może używać geo-replication i private networking.
- Czyść stare tags i repositories.
- Używaj immutable tagging strategy.

ACI:

- Nie jest pełną platformą HA/orchestration.
- IP może zmienić się po restarcie container group.

Container Apps:

- Environment jest regionalny.
- Dla zone failure można używać zone redundancy tam, gdzie jest dostępne i spełnione są wymagania.
- Dla full region outage użyj multi-region deployment i Azure Front Door / Traffic Manager.
- Manual recovery oznacza redeployment do innego regionu.

## 17. Najczęstsze pytania egzaminacyjne - szybka powtórka

- `az acr create` tworzy registry.
- `az acr login` loguje Docker do ACR.
- `az acr build` buduje i pushuje image w ACR, czyli cloud `docker build` + `docker push`.
- `az acr repository list` sprawdza images/repositories w registry.
- `AcrPull` dla runtime pull.
- `AcrPush` dla developer/CI push.
- Service principal jest dobry do headless CI/CD.
- Managed identity jest najlepsze dla Azure resources pulling images bez sekretów.
- Admin account jest disabled by default i niezalecany w produkcji.
- ACI nie skaluje automatycznie; użyj Container Apps, jeśli potrzebujesz scaling.
- ACI container group współdzieli lifecycle, network, resources i volumes.
- ACI default restart policy to `Always`.
- `Never` oznacza run at most once.
- `OnFailure` restartuje tylko po nonzero exit code.
- ACI secure env vars nie pokazują wartości w properties.
- ACI Azure Files wymaga storage account key.
- ACI nie mountuje Blob Storage bezpośrednio jako SMB share.
- `az container logs` pobiera logs, `az container attach` streamuje stdout/stderr.
- Container Apps environment jest granicą regionu, VNet i Log Analytics.
- Container Apps revision jest immutable snapshot.
- Zmiana `template` tworzy revision.
- Zmiana `configuration` zwykle nie tworzy revision.
- Secrets w Container Apps nie tworzą revision po zmianie.
- `secretref:<secret-name>` używa secret jako env var.
- Container Apps scaling może być HTTP, TCP albo custom/KEDA.
- CPU/memory scaling nie skaluje do zera.
- Dapr działa jako sidecar.

## 18. Porównanie z arvigeus/AZ-204

Po porównaniu z `Topics/Containers.md` i `Questions/Containers.md` z repo `arvigeus/AZ-204` uwzględniłem:

- format ACR image endpoint: `<registry>.azurecr.io/<repository>/<image>:<tag>`,
- ACR SKUs i znaczenie Premium,
- ACR authentication: Entra ID, service principal, managed identity, admin user,
- role `AcrPull`, `AcrPush`, `AcrDelete`,
- ACR Tasks: quick task, triggered task, scheduled task, multi-step task,
- `az acr build` jako cloud build + push,
- `az acr repository list` i `show-tags`,
- ACI container groups, restart policies, YAML vs ARM, env vars, secure env vars, Azure Files, secret volumes,
- `az container logs` vs `az container attach`,
- Container Apps environments, ingress, auth, scaling, revisions, secrets, logs, Dapr,
- disaster recovery i multi-region routing.

Różnica względem notatek `arvigeus`:

- W pobranych notatkach jest informacja, że managed identities nie są wspierane w Container Apps scaling rules. Aktualny Microsoft Learn mówi, że scale rules dla Azure resources, takich jak Azure Queue Storage, Service Bus i Event Hubs, mogą używać managed identity. W tej notatce używam aktualnego zachowania z Microsoft Learn.

## 19. Źródła

- Microsoft Learn - Azure Container Registry: https://learn.microsoft.com/en-us/azure/container-registry/
- Microsoft Learn - ACR quickstart with Azure CLI: https://learn.microsoft.com/en-us/azure/container-registry/container-registry-get-started-azure-cli
- Microsoft Learn - ACR Tasks: https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tasks-overview
- Microsoft Learn - ACR quick task: https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tutorial-quick-task
- Microsoft Learn - Azure Container Instances: https://learn.microsoft.com/en-us/azure/container-instances/
- Microsoft Learn - ACI restart policy: https://learn.microsoft.com/en-us/azure/container-instances/container-instances-restart-policy
- Microsoft Learn - ACI Azure Files volume: https://learn.microsoft.com/en-us/azure/container-instances/container-instances-volume-azure-files
- Microsoft Learn - Azure Container Apps: https://learn.microsoft.com/en-us/azure/container-apps/
- Microsoft Learn - Container Apps scaling: https://learn.microsoft.com/en-us/azure/container-apps/scale-app
- Microsoft Learn - Container Apps revisions: https://learn.microsoft.com/en-us/azure/container-apps/revisions
- Microsoft Learn - Container Apps secrets: https://learn.microsoft.com/en-us/azure/container-apps/manage-secrets
- Microsoft Learn - Container Apps monitoring: https://learn.microsoft.com/en-us/azure/container-apps/log-monitoring
- GitHub - arvigeus/AZ-204 Topics/Containers.md: https://github.com/arvigeus/AZ-204/blob/master/Topics/Containers.md
- GitHub - arvigeus/AZ-204 Questions/Containers.md: https://github.com/arvigeus/AZ-204/blob/master/Questions/Containers.md
- GitHub - arvigeus/AZ-204 Questions/Docker.md: https://github.com/arvigeus/AZ-204/blob/master/Questions/Docker.md
