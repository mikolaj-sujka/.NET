# Containerized solutions - pytania AZ-204

Pytania obejmują Docker images, Azure Container Registry, Azure Container Instances i Azure Container Apps. Format jest scenariuszowy, bo AZ-204 często sprawdza wybór komendy, usługi, roli RBAC albo właściwego mechanizmu deployment/scaling.

---

Question: Która usługa służy do prywatnego przechowywania i zarządzania container images w Azure?

- [ ] Azure Container Instances
- [x] Azure Container Registry
- [ ] Azure Container Apps
- [ ] Azure Files

Answer: Azure Container Registry przechowuje i zarządza images oraz OCI artifacts.

---

Question: Jaki jest poprawny format adresu image w Azure Container Registry?

- [x] `<registry>.azurecr.io/<repository>/<image>:<tag>`
- [ ] `<registry>.containerapps.io/<image>:<tag>`
- [ ] `<resource-group>.azurecr.io/<tag>/<image>`
- [ ] `<registry>.blob.core.windows.net/<image>:<tag>`

Answer: ACR login server ma format `<registry>.azurecr.io`, a image używa repository/name i tag.

---

Question: Która komenda tworzy Azure Container Registry?

- [x] `az acr create`
- [ ] `az container create`
- [ ] `az containerapp create`
- [ ] `docker build`

Answer: `az acr create` tworzy registry. `az container create` uruchamia container w ACI.

---

Question: Która komenda loguje Docker client do ACR?

- [x] `az acr login --name <registry>`
- [ ] `az container login --name <registry>`
- [ ] `az acr create --login`
- [ ] `az containerapp registry login`

Answer: `az acr login` uwierzytelnia lokalny Docker client do ACR.

---

Question: Która komenda buduje image w Azure i pushuje go do ACR bez lokalnego Docker Engine?

- [ ] `docker push`
- [x] `az acr build`
- [ ] `az container create`
- [ ] `az acr repository list`

Answer: `az acr build` działa jak cloud `docker build` + `docker push`.

---

Question: `az acr build --registry acrdemo --image api:v1 .` jest odpowiednikiem których Docker operacji?

- [x] `docker build`
- [x] `docker push`
- [ ] `docker run`
- [ ] `docker compose`

Answer: ACR quick task buduje image i domyślnie pushuje go do registry.

---

Question: Która komenda pozwala sprawdzić, czy image został wypchnięty do ACR?

- [x] `az acr repository list`
- [ ] `docker images`
- [ ] `az acr show`
- [ ] `az container logs`

Answer: `az acr repository list` pokazuje repositories w registry. `docker images` pokazuje tylko lokalne images.

---

Question: Która komenda pokazuje tagi dla konkretnego repository w ACR?

- [ ] `az acr show-tags`
- [x] `az acr repository show-tags`
- [ ] `az container show-tags`
- [ ] `docker ps`

Answer: `az acr repository show-tags --repository <repo>` pokazuje tagi.

---

Question: Jaka rola RBAC jest najmniej uprzywilejowana dla aplikacji, która tylko pobiera images z ACR?

- [x] `AcrPull`
- [ ] `AcrPush`
- [ ] `Contributor`
- [ ] `Owner`

Answer: Do pull wystarczy `AcrPull`.

---

Question: Jaka rola RBAC jest najmniej uprzywilejowana dla developera/CI, który ma publikować images do ACR?

- [ ] `AcrPull`
- [x] `AcrPush`
- [ ] `Reader`
- [ ] `Owner`

Answer: `AcrPush` pozwala pushować images bez szerokich uprawnień do całego zasobu.

---

Question: Który sposób uwierzytelnienia jest najlepszy dla headless CI/CD do ACR, gdy nie używasz Azure resource z managed identity?

- [ ] Admin user.
- [ ] Individual Entra ID login.
- [x] Service principal z RBAC.
- [ ] Public anonymous access.

Answer: Service principal jest przeznaczony do headless automation i może mieć ograniczone role RBAC.

---

Question: Która opcja ACR authentication jest zwykle niezalecana w produkcji?

- [ ] Managed identity.
- [ ] Service principal.
- [x] Admin user.
- [ ] Individual Entra ID dla developera.

Answer: Admin user jest prosty, ale opiera się na registry-level passwords i zwykle nie jest zalecany produkcyjnie.

---

Question: Który ACR SKU wybierzesz dla private endpoints, geo-replication i najwyższego throughput?

- [ ] Basic
- [ ] Standard
- [x] Premium

Answer: Premium daje funkcje enterprise, m.in. private networking i większy throughput.

---

Question: W ACR widzisz pogorszenie performance i bardzo dużo starych repositories/tags. Co możesz zrobić bez podnoszenia SKU?

- [x] Usunąć nieużywane repositories i tags.
- [ ] Zmienić DNS.
- [ ] Włączyć ACI.
- [ ] Przenieść image do Azure Files.

Answer: Nadmiar metadata może pogarszać operacje registry, więc cleaning pomaga.

---

Question: Czym są ACR Tasks?

- [ ] Narzędziem do tworzenia VM.
- [x] Zestawem funkcji do cloud build, patching i automatyzacji container images.
- [ ] Usługą do hostowania HTTP endpoints.
- [ ] Alternatywą dla DNS.

Answer: ACR Tasks wspiera quick builds, triggered builds, scheduled builds i multi-step workflows.

---

Question: Jaka jest domyślna platforma buildów ACR Tasks?

- [x] Linux/amd64
- [ ] Windows/arm64
- [ ] Linux/s390x
- [ ] Windows/x86

Answer: Domyślnie ACR Tasks buduje Linux/amd64.

---

Question: Który mechanizm ACR Tasks jest najlepszy do rebuild image po zmianie base image albo cyklicznym patchingu?

- [ ] Admin user.
- [x] Triggered/scheduled ACR Task.
- [ ] ACI restart policy.
- [ ] Container Apps revision label.

Answer: ACR Tasks mogą uruchamiać build po source update, base image update albo schedule.

---

Question: Która usługa najlepiej pasuje do prostego, jednorazowego container job bez potrzeby autoscaling?

- [x] Azure Container Instances.
- [ ] Azure Container Registry.
- [ ] Azure DNS.
- [ ] Azure Logic Apps.

Answer: ACI jest dobre do prostych run-once/container tasks.

---

Question: Która komenda deployuje image do Azure Container Instances?

- [x] `az container create`
- [ ] `az acr create`
- [ ] `az acr push`
- [ ] `az containerapp env create`

Answer: `az container create` tworzy container group w ACI.

---

Question: Czy Azure Container Instances wspiera autoscaling?

- [ ] Tak, przez KEDA.
- [x] Nie, do autoscaling użyj Azure Container Apps albo AKS.
- [ ] Tak, tylko na Windows.
- [ ] Tak, przez `az acr task create`.

Answer: ACI nie jest usługą autoscaling/orchestration.

---

Question: Co współdzielą containers w jednej ACI container group?

- [x] Lifecycle.
- [x] Network namespace.
- [x] Storage volumes.
- [x] Allocated resources hosta.

Answer: Container group to grupa containers współdzielących lifecycle, network, resources i volumes.

---

Question: W ACI wiele containers w jednej container group komunikuje się ze sobą przez:

- [x] `localhost`.
- [ ] Public DNS każdego containera.
- [ ] Azure Front Door.
- [ ] ACR webhook.

Answer: Containers w tej samej group współdzielą network namespace.

---

Question: Który OS type jest wymagany dla legacy .NET Framework container?

- [ ] Linux.
- [x] Windows.
- [ ] Linux albo Windows dowolnie.

Answer: .NET Framework jest Windows-specific.

---

Question: Który OS type może być użyty dla .NET Core / .NET container?

- [ ] Tylko Windows.
- [ ] Tylko Linux.
- [x] Linux albo Windows, zależnie od image.

Answer: .NET jest cross-platform, ale image musi pasować do OS hosta.

---

Question: Jaka jest domyślna restart policy w Azure Container Instances?

- [x] `Always`
- [ ] `Never`
- [ ] `OnFailure`

Answer: Default restart policy to `Always`.

---

Question: Którą restart policy wybierzesz, jeśli container ma uruchomić się najwyżej raz?

- [ ] `Always`
- [x] `Never`
- [ ] `RetryForever`

Answer: `Never` oznacza, że container nie jest restartowany.

---

Question: Którą restart policy wybierzesz dla zadania, które ma uruchomić się ponownie tylko po błędzie?

- [ ] `Always`
- [x] `OnFailure`
- [ ] `Never`

Answer: `OnFailure` restartuje container tylko po nonzero exit code.

---

Question: Co może stać się z public IP ACI container group po restarcie?

- [x] IP może się zmienić.
- [ ] IP zawsze zostaje takie samo.
- [ ] IP zmienia się na adres ACR.
- [ ] ACI traci image.

Answer: Public IP ACI container group może się zmienić po restarcie; nie hardcoduj go.

---

Question: Chcesz mieć stable public endpoint przed ACI mimo możliwych zmian IP. Co rozważysz?

- [x] Application Gateway / fronting service.
- [ ] Hardcoded IP.
- [ ] `docker tag`.
- [ ] ACR Premium tylko.

Answer: Fronting service może zapewnić stabilny endpoint przed dynamicznym ACI.

---

Question: Który command streamuje stdout/stderr działającego containera ACI w czasie rzeczywistym?

- [ ] `az container logs`
- [x] `az container attach`
- [ ] `az acr logs`
- [ ] `az containerapp revision list`

Answer: `az container attach` attachuje terminal do output/error streams.

---

Question: Który command pobiera logs containera ACI bez real-time attach?

- [x] `az container logs`
- [ ] `az container attach`
- [ ] `az acr repository list`
- [ ] `docker tag`

Answer: `az container logs` pobiera logs.

---

Question: W ACI secure environment variable jest ustawiona przez `secureValue`. Co zobaczysz w properties przez CLI?

- [ ] Pełną wartość secretu.
- [x] Nazwę zmiennej, ale bez wartości.
- [ ] Hash wartości.
- [ ] Nic, nawet nazwa jest ukryta.

Answer: Secure value nie jest wyświetlana w properties.

---

Question: Który storage można zamontować do ACI jako persistent SMB share?

- [x] Azure Files.
- [ ] Azure Blob bezpośrednio jako SMB.
- [ ] Azure Queue Storage.
- [ ] Azure Table Storage.

Answer: Azure Files wspiera SMB. Blob Storage nie jest bezpośrednim SMB mount dla ACI.

---

Question: Co jest wymagane do mount Azure File Share w ACI?

- [x] Storage account name.
- [x] Share name.
- [x] Storage account key.
- [ ] Function key.

Answer: ACI Azure Files volume wymaga storage account, share i account key.

---

Question: Dla ACI multi-container group, która zawiera tylko containers, jaki format deploymentu jest zwykle najwygodniejszy?

- [ ] DNS zone.
- [x] YAML file.
- [ ] `docker push`.
- [ ] Key Vault certificate.

Answer: YAML jest czytelny dla samej container group. ARM/Bicep jest lepszy, gdy deployment obejmuje dodatkowe Azure resources.

---

Question: Deployment obejmuje ACI container group oraz Storage Account i Azure File Share. Co jest najlepszym IaC wyborem?

- [x] ARM/Bicep.
- [ ] Tylko `az container create`.
- [ ] Tylko Dockerfile.
- [ ] `az acr login`.

Answer: ARM/Bicep może wdrażać wiele typów Azure resources w jednym deployment.

---

Question: Która usługa najlepiej pasuje do containerized microservices z autoscaling, revisions i ingress?

- [ ] Azure Container Instances.
- [ ] Azure Container Registry.
- [x] Azure Container Apps.
- [ ] Azure Files.

Answer: Azure Container Apps oferuje scaling, revisions, ingress i Dapr dla microservices.

---

Question: Która komenda tworzy Azure Container Apps environment?

- [ ] `az container create`
- [x] `az containerapp env create`
- [ ] `az acr env create`
- [ ] `docker compose up`

Answer: `az containerapp env create` tworzy environment.

---

Question: Która komenda tworzy Container App z większą kontrolą nad konfiguracją?

- [x] `az containerapp create`
- [ ] `az container attach`
- [ ] `az acr build`
- [ ] `az group export`

Answer: `az containerapp create` tworzy Container App i pozwala dokładnie ustawić ingress, scale, secrets itd.

---

Question: Która komenda jest szybkim path do create/update Container App i może utworzyć zasoby pomocnicze?

- [ ] `az containerapp create`
- [x] `az containerapp up`
- [ ] `az acr import`
- [ ] `az container logs`

Answer: `az containerapp up` jest wygodnym szybkim workflow, często dla dev/test.

---

Question: Co jest scoped na poziomie Container Apps environment?

- [x] Region.
- [x] VNet.
- [x] Log Analytics workspace.
- [ ] Pojedynczy tag image każdej aplikacji.

Answer: Environment jest regionalną i network/observability granicą dla apps.

---

Question: Które wymagania uzasadniają wiele Container Apps environments?

- [x] Różne virtual networks.
- [x] Różne Log Analytics workspaces.
- [x] Różne regiony.
- [ ] Różne scale rules dla apps.

Answer: Scale rules są per app. VNet, workspace i region są powiązane z environment.

---

Question: Jaki ingress ustawić, jeśli Container App ma być publicznie dostępna z internetu?

- [x] `external`
- [ ] `internal`
- [ ] `disabled`
- [ ] `private-only-tag`

Answer: `external` wystawia publiczny endpoint.

---

Question: Jaki ingress ustawić dla workera, który nie obsługuje requestów HTTP?

- [ ] `external`
- [ ] `public`
- [x] Disabled/no ingress.
- [ ] `dns-only`

Answer: Worker bez endpointu HTTP nie potrzebuje ingress.

---

Question: Czym jest revision w Azure Container Apps?

- [x] Immutable snapshot wersji Container App.
- [ ] Dynamiczny log aplikacji.
- [ ] Hasło registry.
- [ ] Storage account.

Answer: Revision jest niezmiennym snapshotem konfiguracji revision-scope.

---

Question: Która zmiana tworzy nową revision w Container Apps?

- [x] Zmiana image.
- [x] Zmiana env vars.
- [x] Zmiana scale rules.
- [ ] Sama zmiana secret value.

Answer: Zmiany `properties.template` tworzą revision. Secrets są application-scope.

---

Question: Która zmiana zwykle nie tworzy nowej revision?

- [x] Zmiana secret.
- [x] Zmiana ingress configuration.
- [ ] Zmiana image.
- [ ] Zmiana CPU/memory w template.

Answer: Secrets i ingress są application-scope/configuration-scope. Image i resources są revision-scope.

---

Question: Do czego służy multiple revision mode?

- [x] Do jednoczesnego uruchomienia wielu revisions i traffic splitting.
- [ ] Do przechowywania więcej niż jednego secretu.
- [ ] Do uruchomienia ACI.
- [ ] Do push image do ACR.

Answer: Multiple revision mode pozwala na canary/blue-green style traffic distribution.

---

Question: Co daje revision label?

- [x] Stable URL kierujący do konkretnej revision.
- [ ] RBAC do ACR.
- [ ] Restart policy w ACI.
- [ ] Build image w ACR.

Answer: Label pozwala bezpośrednio kierować traffic do oznaczonej revision.

---

Question: Który scale rule w Container Apps skaluje według concurrent HTTP requests?

- [x] HTTP scale rule.
- [ ] TCP scale rule.
- [ ] Blob receipt rule.
- [ ] ACR task rule.

Answer: HTTP scale rule używa concurrency HTTP requests.

---

Question: Które scale rule types są typowe w Azure Container Apps?

- [x] HTTP.
- [x] TCP.
- [x] Custom/KEDA.
- [ ] DNS MX.

Answer: Container Apps wspiera HTTP, TCP i custom/event-driven KEDA scalers.

---

Question: Które scale rules nie skalują do zera, bo wymagają działającej repliki do pomiaru?

- [x] CPU.
- [x] Memory.
- [ ] Queue length.
- [ ] Service Bus message count.

Answer: CPU/memory scaling potrzebuje aktywnej repliki.

---

Question: Container App ma ingress disabled, `minReplicas=0` i brak custom scale rule. Co może się stać?

- [x] App może skalować do zera i nie wystartować na event.
- [ ] App będzie zawsze miała 10 replicas.
- [ ] ACR automatycznie uruchomi app.
- [ ] Zostanie utworzona nowa revision co minutę.

Answer: Worker bez ingress potrzebuje custom scale rule albo `minReplicas > 0`.

---

Question: Chcesz skalować Container App po liczbie wiadomości w Service Bus queue bez trzymania connection stringa. Co wybierzesz według aktualnych zaleceń?

- [x] Managed identity scale rule, jeśli scaler ją wspiera.
- [ ] Admin user ACR.
- [ ] Plain secret w Dockerfile.
- [ ] HTTP scale rule.

Answer: Aktualne Microsoft Learn wskazuje, że scale rules dla Azure resources mogą używać managed identity; gdy możliwe, unikaj sekretów.

---

Question: Jak odwołać się do secretu w env var w Container Apps CLI?

- [x] `ConnectionString=secretref:queue-connection-string`
- [ ] `ConnectionString=$queue-connection-string`
- [ ] `ConnectionString=keyvault://queue-connection-string`
- [ ] `ConnectionString=acrpush:queue-connection-string`

Answer: `secretref:<secret-name>` wstawia wartość secretu do env var.

---

Question: Co dzieje się po zmianie secret value w Container Apps?

- [ ] Zawsze automatycznie powstaje nowa revision.
- [x] Nowa revision nie jest tworzona automatycznie; trzeba restartować revision albo wdrożyć nową, żeby pewnie odczytać zmianę.
- [ ] App jest usuwana.
- [ ] Secret staje się publiczny.

Answer: Secrets są application-scope i ich zmiana nie tworzy revision.

---

Question: Co jest wymagane, aby Container App pobierała image z prywatnego ACR przez managed identity?

- [x] Identity przypisana do Container App.
- [x] Rola `AcrPull` na ACR.
- [x] Registry configured to use identity.
- [ ] Rola `Owner` na subskrypcji.

Answer: Do pull z ACR wystarczy identity + `AcrPull` + registry configuration.

---

Question: Czym jest Dapr sidecar w Container Apps?

- [x] Sidecar expose'ujący Dapr APIs przez HTTP/gRPC.
- [ ] Registry na images.
- [ ] Restart policy dla ACI.
- [ ] DNS record.

Answer: Dapr sidecar udostępnia building blocks, np. service invocation, state, pub/sub.

---

Question: Które Dapr building blocks są typowe?

- [x] Service invocation.
- [x] State management.
- [x] Pub/sub.
- [x] Secrets.
- [ ] Dockerfile parsing.

Answer: Dapr daje application building blocks przez sidecar APIs.

---

Question: Gdzie w Log Analytics szukać console logs z Azure Container Apps?

- [x] `ContainerAppConsoleLogs_CL`
- [ ] `AzureWebJobsHostLogs`
- [ ] `AcrPushLogsOnly`
- [ ] `DnsQueryLogs`

Answer: Console logs trafiają do `ContainerAppConsoleLogs_CL`.

---

Question: Gdzie w Log Analytics szukać system logs Azure Container Apps?

- [x] `ContainerAppSystemLogs_CL`
- [ ] `ContainerAppConsoleLogs_CL` tylko.
- [ ] `ACISecretLogs_CL`
- [ ] `DockerfileHistory_CL`

Answer: System logs trafiają do `ContainerAppSystemLogs_CL`.

---

Question: Azure Container Apps ma przetrwać awarię całego regionu. Jaka strategia jest właściwa?

- [x] Deploy do wielu regionów i routing przez Azure Front Door / Traffic Manager.
- [ ] Tylko zwiększyć maxReplicas w jednym regionie.
- [ ] Użyć ACI restart policy Always.
- [ ] Użyć jednego secretu.

Answer: Full region outage wymaga multi-region pattern i globalnego routingu.

---

Question: Co jest wymagane dla zone redundancy w Container Apps environment?

- [x] Environment z VNet i dostępną subnet, w regionie obsługującym tę funkcję.
- [ ] ACI restart policy Never.
- [ ] ACR Basic.
- [ ] Dockerfile bez `EXPOSE`.

Answer: Zone redundancy wymaga odpowiedniej konfiguracji environment/network i wsparcia regionu.

---

Question: Która usługa jest tylko registry i nie uruchamia containerów?

- [x] Azure Container Registry.
- [ ] Azure Container Instances.
- [ ] Azure Container Apps.
- [ ] AKS.

Answer: ACR przechowuje images; nie jest runtime dla aplikacji.

---

Question: Która usługa jest najlepsza do microservices z event-driven scale i revisions, ale bez zarządzania Kubernetes?

- [ ] ACR.
- [ ] ACI.
- [x] Azure Container Apps.
- [ ] Azure Storage Queue.

Answer: Container Apps abstrahuje Kubernetes i oferuje serverless container platform.

---

Question: Który element Dockerfile uruchamia aplikację po starcie container?

- [ ] `COPY`
- [ ] `WORKDIR`
- [x] `ENTRYPOINT`
- [ ] `FROM`

Answer: `ENTRYPOINT` definiuje komendę startową container.

---

Question: Który element Dockerfile wskazuje base image?

- [x] `FROM`
- [ ] `EXPOSE`
- [ ] `ENTRYPOINT`
- [ ] `RUN`

Answer: `FROM` wskazuje base image dla stage.

---

Question: Po co używać multi-stage Dockerfile?

- [x] Żeby final image był mniejszy i zawierał tylko runtime artifacts.
- [ ] Żeby ominąć ACR authentication.
- [ ] Żeby włączyć ACI autoscale.
- [ ] Żeby stworzyć DNS record.

Answer: Build stage ma narzędzia SDK, runtime stage zawiera tylko gotową aplikację.

---

## Źródła

- Microsoft Learn - Azure Container Registry: https://learn.microsoft.com/en-us/azure/container-registry/
- Microsoft Learn - ACR Tasks: https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tasks-overview
- Microsoft Learn - Azure Container Instances: https://learn.microsoft.com/en-us/azure/container-instances/
- Microsoft Learn - ACI restart policy: https://learn.microsoft.com/en-us/azure/container-instances/container-instances-restart-policy
- Microsoft Learn - Azure Container Apps: https://learn.microsoft.com/en-us/azure/container-apps/
- Microsoft Learn - Container Apps scaling: https://learn.microsoft.com/en-us/azure/container-apps/scale-app
- Microsoft Learn - Container Apps revisions: https://learn.microsoft.com/en-us/azure/container-apps/revisions
- Microsoft Learn - Container Apps secrets: https://learn.microsoft.com/en-us/azure/container-apps/manage-secrets
- GitHub - arvigeus/AZ-204 Topics/Containers.md: https://github.com/arvigeus/AZ-204/blob/master/Topics/Containers.md
- GitHub - arvigeus/AZ-204 Questions/Containers.md: https://github.com/arvigeus/AZ-204/blob/master/Questions/Containers.md
- GitHub - arvigeus/AZ-204 Questions/Docker.md: https://github.com/arvigeus/AZ-204/blob/master/Questions/Docker.md
