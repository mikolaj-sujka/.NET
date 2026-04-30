# Azure App Service Web Apps - notatki AZ-204

## 1. Czym jest Azure App Service

Azure App Service to w pełni zarządzana usługa PaaS do hostowania aplikacji webowych, REST API i backendów mobilnych. Obsługuje m.in. .NET, Java, Node.js, Python, PHP oraz custom containers.

Najważniejsze pojęcia:

- **Web App** - konkretna aplikacja hostowana w App Service.
- **App Service Plan** - plan obliczeniowy, na którym działa jedna lub wiele Web Apps. Określa region, OS, pricing tier, liczbę instancji i dostępne funkcje.
- **Pricing tier** - wpływa na CPU/RAM, scaling, custom domains, TLS/SSL, deployment slots, backups itd.
- **Windows vs Linux** - wybierane na poziomie App Service Plan. Plan nie miesza aplikacji Windows i Linux.
- **Code vs Docker Container** - aplikację można wdrożyć jako kod na obsługiwanym runtime albo jako container image.

### App Service Plan

W ramach usługi App Service aplikacja zawsze działa w planie usługi App Service. App Service Plan to zestaw zasobów compute dla App Service. Można myśleć o nim jak o "hostingu" dla jednej albo wielu aplikacji. To plan decyduje, na jakich workerach działa aplikacja, ile ma dostępnych instancji, w jakim regionie jest uruchomiona i jakie funkcje są dostępne.

Co jest przypisane do App Service Plan:

- region,
- OS: Windows albo Linux,
- pricing tier,
- liczba VM instances / workers,
- dostępne CPU/RAM,
- możliwości scale up/scale out,
- funkcje typu deployment slots, autoscale, Always On, custom domains, TLS bindings.

Ważna zależność:

- Web App jest aplikacją.
- App Service Plan jest miejscem, gdzie ta aplikacja działa.
- Kilka Web Apps w jednym planie współdzieli te same zasoby.
- Jeśli jedna aplikacja w planie zużyje dużo CPU/RAM, może wpływać na inne aplikacje w tym samym planie.

Ważne na egzamin:

- Scaling dotyczy App Service Plan, więc aplikacje w tym samym planie współdzielą zasoby.
- Scale up oznacza zmianę tier/rozmiaru planu.
- Scale out oznacza zwiększenie liczby instancji.
- Free/Shared mają ograniczone funkcje i nie nadają się do produkcji.
- Deployment slots są dostępne w Standard, Premium i Isolated tier.
- App Service Plan bez żadnych aplikacji dalej generuje koszt, bo rezerwuje skonfigurowane zasoby.
- Deployment slots, diagnostic logs, backups i WebJobs używają tych samych VM instances co aplikacje w danym planie.

Kiedy rozdzielić aplikację do osobnego App Service Plan:

- aplikacja jest resource-intensive,
- aplikacja ma skalować się niezależnie od innych aplikacji,
- aplikacja ma działać w innym regionie,
- aplikacja nie powinna konkurować o CPU/RAM z innymi aplikacjami w planie.

Przydatne progi tier:

| Funkcja | Minimalny tier |
| --- | --- |
| Dedicated workers | Basic |
| Custom DNS name | Shared |
| TLS bindings | Basic |
| Always On | Basic |
| Free managed certificate | Basic |
| Autoscale | Standard |
| Deployment slots | Standard |
| Linux apps | Basic/Standard zależnie od opcji planu |
| Automatic scaling | PremiumV2 |
| App Service Environment | Isolated |

## 2. Create an Azure App Service Web App

Minimalne zasoby potrzebne do Web App:

1. Resource group.
2. App Service Plan.
3. Web App.

Przykład Azure CLI:

```bash
az group create \
  --name rg-appservice-demo \
  --location westeurope

az appservice plan create \
  --name asp-demo \
  --resource-group rg-appservice-demo \
  --location westeurope \
  --sku B1 \
  --is-linux

az webapp create \
  --name app-demo-204 \
  --resource-group rg-appservice-demo \
  --plan asp-demo \
  --runtime "DOTNETCORE:8.0"
```

Szybka opcja dla aplikacji z lokalnego folderu:

```bash
az webapp up \
  --name app-demo-204 \
  --resource-group rg-appservice-demo \
  --location westeurope \
  --sku B1 \
  --os-type linux
```

Najważniejsze ustawienia przy tworzeniu:

- **Name** - globalnie unikalny, tworzy adres `https://<name>.azurewebsites.net`.
- **Publish** - `Code` albo `Docker Container`.
- **Runtime stack** - np. `.NET`, `Node`, `Python`, `Java`.
- **Operating System** - Windows albo Linux.
- **Region** - najlepiej blisko użytkowników albo zależnych usług.
- **App Service Plan** - decyduje o kosztach i możliwościach.

## 3. Configure settings

### App settings

App settings są przekazywane do aplikacji jako environment variables. W .NET nadpisują wartości z `appsettings.json` / `Web.config`.

Przykład:

```bash
az webapp config appsettings set \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --settings ASPNETCORE_ENVIRONMENT=Production FeatureFlags__UseCache=true
```

Ważne:

- Zmiana app settings powoduje restart aplikacji.
- App settings są encrypted at rest.
- Sekrety lepiej trzymać jako Key Vault references.
- Dla zagnieżdżonych konfiguracji w Linux/custom containers używa się `__`, np. `ApplicationInsights__ConnectionString`.
- Ustawienia mogą być **slot setting**, czyli przypięte do konkretnego deployment slot.

### Connection strings

Connection strings również są encrypted at rest i dostępne jako environment variables z prefiksem zależnym od typu.

Przykładowe prefiksy:

- `SQLCONNSTR_` - SQL Server.
- `SQLAZURECONNSTR_` - Azure SQL.
- `MYSQLCONNSTR_` - MySQL.
- `POSTGRESQLCONNSTR_` - PostgreSQL.
- `CUSTOMCONNSTR_` - Custom.

Przykład:

```bash
az webapp config connection-string set \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --connection-string-type SQLAzure \
  --settings DefaultConnection="Server=tcp:..."
```

### General settings

Typowe ustawienia:

- **Stack settings** - runtime i wersja.
- **Platform** - 32-bit/64-bit dla Windows.
- **Always On** - utrzymuje aplikację załadowaną, zmniejsza cold start. Wymagane m.in. dla ciągłych WebJobs.
- **ARR affinity** - sticky sessions; klient trafia do tej samej instancji podczas sesji. Przydatne dla aplikacji stanowych, ale zwykle lepiej projektować aplikacje stateless.
- **HTTP version** - np. HTTP/2.
- **Web sockets** - potrzebne np. dla SignalR lub socket.io.
- **FTP state** - najlepiej `FTPS only` albo disabled.
- **Startup command** - często używane dla Linux/custom containers.
- **Remote debugging** - powinno być używane tylko tymczasowo; App Service wyłącza je automatycznie po ograniczonym czasie.
- **Auto Heal** - automatyczny restart/recycle po określonych warunkach, np. memory limit, request count, slow requests, status codes.

### Handler mappings i path mappings

Handler mappings pozwalają przypisać rozszerzenie pliku do konkretnego procesora, np. obsługa `*.php` przez wskazany executable.

Path mappings pozwalają mapować URL path do katalogu aplikacji. Przykład: aplikacja może serwować `/` z `site\wwwroot\public` zamiast z głównego `site\wwwroot`.

Na egzaminie kojarz:

- `D:\home\site\wwwroot` - typowy root aplikacji Windows.
- `/home/site/wwwroot` - typowy root aplikacji Linux.
- Zmiany konfiguracji tego typu zwykle restartują aplikację.

## 4. TLS, custom domains i API settings

### TLS

App Service domyślnie obsługuje HTTPS dla domeny `azurewebsites.net`.

TLS, czyli Transport Layer Security, to protokół zabezpieczający komunikację między klientem, np. przeglądarką, a serwerem. W praktyce jest to mechanizm stojący za HTTPS.

TLS daje trzy główne rzeczy:

- **Encryption in transit** - dane lecące po sieci są zaszyfrowane.
- **Authentication** - klient może potwierdzić, że łączy się z właściwą stroną, bo serwer pokazuje certyfikat wystawiony przez zaufane CA.
- **Integrity** - dane nie powinny zostać niezauważenie zmienione po drodze.

Jak działa TLS w uproszczeniu:

1. Klient łączy się z adresem HTTPS, np. `https://api.contoso.com`.
2. Serwer odsyła certyfikat TLS/SSL dla tej domeny.
3. Klient sprawdza, czy certyfikat jest zaufany, ważny i pasuje do domeny.
4. Klient i serwer uzgadniają wspólne parametry szyfrowania.
5. Powstaje bezpieczny klucz sesji.
6. Dalsza komunikacja HTTP idzie już jako zaszyfrowany HTTPS.

W App Service:

- domena `*.azurewebsites.net` jest zabezpieczona automatycznie,
- custom domain wymaga osobnego certyfikatu i TLS binding,
- ustawienie **HTTPS Only** przekierowuje/wymusza ruch po HTTPS,
- **Minimum TLS Version** blokuje stare wersje protokołu.

Ważne ustawienia:

- **Minimum Inbound TLS Version** - minimalna wersja TLS dla ruchu do aplikacji.
- **SCM Minimum Inbound TLS Version** - minimalna wersja TLS dla SCM/Kudu (`<app>.scm.azurewebsites.net`).
- Zalecane minimum: TLS 1.2 lub nowszy.
- TLS 1.0 i 1.1 są legacy i nie powinny być używane.
- **HTTPS Only** wymusza ruch przez HTTPS.

Przykłady:

```bash
az webapp update \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --https-only true

az webapp config set \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --min-tls-version 1.2
```

Custom domain + TLS:

1. Dodać custom domain.
2. Zweryfikować domenę rekordem DNS.
3. Dodać TLS/SSL binding.
4. Użyć App Service Managed Certificate albo własnego certyfikatu z Key Vault / upload.

### DNS i custom domains

DNS, czyli Domain Name System, tłumaczy nazwę domeny na adres, pod którym działa usługa. Dzięki DNS użytkownik wpisuje `www.contoso.com`, a nie adres IP albo techniczny hostname Azure.

Jak działa DNS w uproszczeniu:

1. Użytkownik wpisuje domenę w przeglądarce.
2. System pyta DNS resolver o rekord dla tej domeny.
3. DNS zwraca rekord wskazujący, gdzie wysłać ruch.
4. Przeglądarka łączy się z docelowym adresem.

Typowe rekordy przy App Service:

- **CNAME** - alias z subdomeny do innej nazwy DNS, np. `www.contoso.com -> app-demo-204.azurewebsites.net`.
- **A record** - wskazuje domenę na adres IPv4, często używany dla root/apex domain, np. `contoso.com`.
- **TXT record** - używany do weryfikacji własności domeny, np. `asuid.contoso.com`.

Przykład:

```text
www.contoso.com CNAME app-demo-204.azurewebsites.net
asuid.www.contoso.com TXT <custom-domain-verification-id>
```

Root domain, np. `contoso.com`, zwykle używa A record, bo standard DNS nie pozwala bezpiecznie używać CNAME na apex domain. Subdomain, np. `www.contoso.com`, zwykle używa CNAME.

Na egzaminie kojarz:

- DNS pokazuje, gdzie ma iść ruch.
- TLS certificate potwierdza, że serwer jest zaufany dla tej domeny.
- Custom domain musi być dodana i zweryfikowana w App Service.
- Sam DNS nie zabezpiecza połączenia; do tego potrzebny jest TLS.

### App Service Managed Certificate

App Service Managed Certificate to darmowy, zarządzany przez App Service certyfikat TLS dla custom domain.

Co robi Azure:

- wystawia certyfikat,
- odnawia go automatycznie,
- aktualizuje binding po odnowieniu,
- obsługuje podstawowe domain validation.

Ograniczenia:

- nie można go wyeksportować,
- nie można używać poza App Service,
- nie obsługuje wildcard certificates, np. `*.contoso.com`,
- nie obsługuje private DNS,
- wymaga poprawnie skonfigurowanej i osiągalnej custom domain,
- nie jest opcją dla każdego scenariusza, np. bardziej zaawansowane certyfikaty lepiej trzymać w Key Vault albo użyć BYOC.

Typowy flow:

1. Dodać custom domain do Web App.
2. Skonfigurować DNS.
3. Poczekać na walidację domeny.
4. Utworzyć App Service Managed Certificate.
5. Dodać TLS/SSL binding dla custom domain.

### Managed identity - jak działa

Managed identity to tożsamość aplikacji w Microsoft Entra ID. Dzięki niej Web App może logować się do innych Azure resources bez hasła, connection stringa z sekretem albo client secret w konfiguracji.

Są dwa typy:

- **System-assigned managed identity** - tworzona razem z aplikacją i usuwana razem z nią.
- **User-assigned managed identity** - osobny Azure resource, który można przypisać do wielu usług.

Jak działa w praktyce:

1. Włączasz managed identity na Web App.
2. Azure tworzy service principal w Microsoft Entra ID.
3. Nadajesz tej identity rolę RBAC, np. `Key Vault Secrets User` na Key Vault.
4. Kod aplikacji używa `DefaultAzureCredential`.
5. Azure Identity library pobiera token dla identity aplikacji.
6. Aplikacja używa tokenu do dostępu do usługi, np. Key Vault.

Przykład: Web App czyta secret z Key Vault.

```bash
az webapp identity assign \
  --resource-group rg-appservice-demo \
  --name app-demo-204

az role assignment create \
  --assignee <principal-id-web-app> \
  --role "Key Vault Secrets User" \
  --scope /subscriptions/<sub-id>/resourceGroups/rg-appservice-demo/providers/Microsoft.KeyVault/vaults/kv-demo-204
```

Kod .NET:

```csharp
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var client = new SecretClient(
    new Uri("https://kv-demo-204.vault.azure.net/"),
    new DefaultAzureCredential());

KeyVaultSecret secret = await client.GetSecretAsync("DbPassword");
string password = secret.Value;
```

Ważne na egzamin:

- Managed identity usuwa potrzebę przechowywania sekretów aplikacji.
- Sama identity nic nie daje, dopóki nie dostanie uprawnień RBAC albo access policy.
- `DefaultAzureCredential` działa lokalnie przez konto developera/CLI, a w Azure przez managed identity.
- Każdy deployment slot może mieć własną managed identity.

### API settings

Dla API Apps / REST API istotne są:

- CORS - lista dozwolonych origins.
- Authentication / Authorization - Easy Auth, np. Microsoft Entra ID, GitHub, Google.
- Managed identity - bezpieczny dostęp do Azure resources bez sekretów w kodzie.
- App settings i connection strings - konfiguracja zależna od środowiska.

Przykład CORS:

```bash
az webapp cors add \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --allowed-origins https://frontend.example.com
```

## 5. Service connections

App Service może łączyć się z innymi usługami przez:

- connection strings,
- app settings,
- Managed Identity,
- Key Vault references,
- Service Connector,
- VNet Integration,
- Private Endpoint po stronie zależnej usługi.

Najbezpieczniejszy wzorzec:

1. Włączyć system-assigned albo user-assigned managed identity.
2. Nadać identity odpowiednią rolę RBAC na zależnej usłudze.
3. W kodzie używać `DefaultAzureCredential`.
4. Nie trzymać sekretów w repozytorium.

Przykład włączenia system-assigned managed identity:

```bash
az webapp identity assign \
  --resource-group rg-appservice-demo \
  --name app-demo-204
```

Key Vault reference w app setting:

```text
@Microsoft.KeyVault(SecretUri=https://<vault-name>.vault.azure.net/secrets/<secret-name>/<version>)
```

Inny zapis Key Vault reference:

```text
@Microsoft.KeyVault(VaultName=<vault-name>;SecretName=<secret-name>)
```

App Configuration reference:

```text
@Microsoft.AppConfiguration(Endpoint=https://<store-name>.azconfig.io; Key=<key>; Label=<label>)
```

## 6. Networking, access restrictions i Health Check

### Inbound access

Opcje kontroli ruchu przychodzącego:

- **Access restrictions** - allow/deny rules na podstawie IP, service tag albo Virtual Network.
- **Private Endpoint** - prywatny dostęp do Web App przez private IP z VNet; publiczny dostęp można ograniczyć.
- **Custom domain + TLS binding** - własna domena i certyfikat.
- **Client certificates** - wymaganie certyfikatu klienta dla requestów.

### Outbound access

Opcje ruchu wychodzącego:

- **VNet Integration** - aplikacja może wychodzić do zasobów w VNet albo przez VNet.
- **Hybrid Connections** - dostęp z App Service do zasobu on-premises bez pełnego VPN i bez dużych zmian firewall.
- **Service endpoints / Private endpoints** - ograniczanie dostępu do usług Azure z poziomu sieci.

Ważne:

- VNet Integration dotyczy ruchu wychodzącego z aplikacji.
- Private Endpoint dla App Service dotyczy prywatnego ruchu przychodzącego do aplikacji.
- Outbound IP addresses można sprawdzić przez `az webapp show`.
- `outboundIpAddresses` pokazuje aktualne outbound IP.
- `possibleOutboundIpAddresses` pokazuje pełną listę możliwych outbound IP po zmianach planu/skali.

Przykład:

```bash
az webapp show \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --query outboundIpAddresses
```

### Health Check

Health Check cyklicznie odpytuje wskazaną ścieżkę aplikacji. Jeśli instancja nie odpowiada poprawnie, App Service może usunąć ją z load balancer rotation, a przy dłuższym problemie zastąpić instancję.

Przykład:

```bash
az webapp config set \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --health-check-path /health
```

Ważne:

- Endpoint powinien zwracać status 200, gdy aplikacja jest gotowa obsługiwać ruch.
- Health Check pomaga przy scale out, deployment slots i wykrywaniu uszkodzonych instancji.
- Dla prywatnych endpointów i restrykcji sieciowych trzeba uważać, żeby Health Check nie był blokowany.

## 7. Diagnostics and logging

App Service ma kilka typów logów.

### Application logging

Logi generowane przez aplikację.

- Windows: filesystem albo Blob Storage.
- Linux/custom containers: filesystem.
- Poziomy: Error, Warning, Information, Verbose.
- Filesystem logging jest dobre do krótkiego debugowania.
- Dla długiego przechowywania lepiej użyć Azure Monitor / Log Analytics / Storage.

### Web server logging

Surowe logi HTTP w formacie W3C.

- Dostępne dla Windows.
- Mogą być zapisywane do filesystem albo Storage.

### Detailed error messages

Szczegółowe strony błędów HTTP.

- Windows.
- Zapisywane w filesystem.
- Przydatne do diagnozowania błędów 4xx/5xx.
- Nie powinno się wysyłać szczegółów błędów klientowi w produkcji.

### Failed request tracing

Szczegółowy tracing requestów zakończonych błędem.

- Windows.
- Przydatne przy problemach z IIS, pipeline, performance i HTTP errors.

### Deployment logs

Logi deploymentu.

- Włączone automatycznie.
- Pomagają diagnozować nieudany publish, Git deploy, custom deployment script itd.

### Log stream

Podgląd logów na żywo:

```bash
az webapp log tail \
  --resource-group rg-appservice-demo \
  --name app-demo-204
```

Włączenie application logging:

```bash
az webapp log config \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --application-logging filesystem \
  --level information
```

Pobranie logów:

```bash
az webapp log download \
  --resource-group rg-appservice-demo \
  --name app-demo-204
```

Miejsca logów:

- Windows: `D:\home\LogFiles`.
- Linux/custom containers: `/home/LogFiles`.
- Kudu/SCM: `https://<app-name>.scm.azurewebsites.net`.

Azure Monitor:

- Diagnostic settings mogą wysyłać resource logs do Log Analytics workspace, Storage account albo Event Hub.
- Application Insights służy do application performance monitoring: requests, dependencies, exceptions, traces, availability, live metrics.

## 8. Deploy code

Popularne sposoby deploymentu kodu:

- Visual Studio / VS Code publish.
- Azure CLI.
- Zip deploy.
- Local Git.
- GitHub Actions.
- Azure DevOps.
- Deployment Center.

Zip deploy:

```bash
az webapp deploy \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --src-path ./app.zip \
  --type zip
```

GitHub Actions najczęściej używa:

- `azure/login` do logowania do Azure,
- `azure/webapps-deploy` do deploymentu.

Ważne na egzamin:

- Deployment Center może wygenerować workflow dla GitHub Actions.
- Zalecane uwierzytelnianie dla GitHub Actions to OpenID Connect z managed identity/service principal, a nie publish profile.
- Deployment do slotu wymaga podania `slot-name`.
- Kudu/SCM obsługuje narzędzia deploymentu, log stream i Advanced Tools.
- Zip deploy używa Kudu.
- Dla build automation można ustawić `SCM_DO_BUILD_DURING_DEPLOYMENT=true`.
- Dla repo z wieloma projektami można wskazać projekt przez app setting `PROJECT`.
- `PRE_BUILD_COMMAND` i `POST_BUILD_COMMAND` pozwalają uruchomić komendy przed/po buildzie.

Custom deployment przez Kudu może używać pliku `.deployment`:

```text
[config]
command = deploy.cmd
```

ARM/Bicep:

- App Service da się wdrażać jako IaC.
- `az group export` może wyeksportować template z resource group.
- `az deployment group create` wdraża template do resource group.

### GitHub Actions + OpenID Connect

GitHub Actions musi zalogować się do Azure, żeby wykonać deployment. Są trzy popularne sposoby:

- **Publish profile** - pobierasz plik publish profile z App Service i zapisujesz go jako GitHub secret.
- **Service principal z client secret** - tworzysz app registration/service principal i zapisujesz secret w GitHub.
- **OpenID Connect** - GitHub dostaje krótkotrwały token i wymienia go w Microsoft Entra ID na token dostępu do Azure.

Dlaczego OIDC jest zalecane:

- nie trzymasz długowiecznego hasła ani publish profile w GitHub secrets,
- token jest krótkotrwały,
- można ograniczyć dostęp do konkretnego repo, brancha, taga albo GitHub environment,
- dostęp kontrolujesz przez Microsoft Entra ID i RBAC,
- łatwiej spełnić zasadę least privilege.

Jak działa OIDC krok po kroku:

1. Tworzysz user-assigned managed identity albo Microsoft Entra application/service principal.
2. Nadajesz tej identity rolę, np. `Website Contributor`, najlepiej tylko na konkretną Web App albo slot.
3. Konfigurujesz federated credential w Microsoft Entra ID.
4. Federated credential mówi: "ufam tokenom z GitHub repo `org/repo` dla brancha `refs/heads/main`".
5. Workflow GitHub ma permission `id-token: write`.
6. `azure/login` prosi GitHub o OIDC token.
7. Microsoft Entra ID sprawdza issuer, subject i audience tokenu.
8. Jeśli pasuje do federated credential, Azure wydaje krótkotrwały access token.
9. `azure/webapps-deploy` używa tego tokenu do deploymentu.

Minimalny fragment workflow:

```yaml
permissions:
  id-token: write
  contents: read

steps:
  - uses: actions/checkout@v4

  - uses: azure/login@v2
    with:
      client-id: ${{ secrets.AZURE_CLIENT_ID }}
      tenant-id: ${{ secrets.AZURE_TENANT_ID }}
      subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}

  - uses: azure/webapps-deploy@v3
    with:
      app-name: app-demo-204
      package: ./publish
```

Publish profile jest prostszy, ale mniej bezpieczny:

- jest app-level credential,
- zwykle daje szeroki dostęp do deploymentu aplikacji,
- trzeba go rotować,
- jeśli wycieknie, ktoś może publikować do aplikacji,
- dla publish profile trzeba mieć włączone basic authentication/publishing credentials.

Ważne na egzamin:

- OIDC nie wymaga sekretu typu password/client secret w GitHub.
- GitHub secret nadal może przechowywać `client-id`, `tenant-id`, `subscription-id`, ale to nie są hasła.
- Identity używana przez workflow musi mieć RBAC do App Service i slotu, jeśli deployment idzie do slotu.
- Deployment do slotu w `azure/webapps-deploy` wymaga `slot-name`.

## 9. Deploy containerized solutions

App Service może uruchamiać custom container z registry, np. Azure Container Registry albo Docker Hub.

Tworzenie Web App dla containera:

```bash
az webapp create \
  --resource-group rg-appservice-demo \
  --plan asp-demo \
  --name app-container-204 \
  --deployment-container-image-name myregistry.azurecr.io/myapp:v1
```

Konfiguracja obrazu:

```bash
az webapp config container set \
  --resource-group rg-appservice-demo \
  --name app-container-204 \
  --docker-custom-image-name myregistry.azurecr.io/myapp:v1 \
  --docker-registry-server-url https://myregistry.azurecr.io
```

Ważne:

- Dla prywatnego ACR najlepiej użyć managed identity do pull image.
- App settings są przekazywane do containera jako environment variables.
- Container powinien nasłuchiwać na porcie oczekiwanym przez App Service.
- Dla Linux custom container często ustawia się `WEBSITES_PORT`, jeśli aplikacja nie używa domyślnego portu.
- Logi containera trafiają do `/home/LogFiles`.
- Dla ACR z managed identity trzeba nadać roli `AcrPull` identity aplikacji.
- App Service może uruchamiać też multi-container apps z Docker Compose, ale to jest bardziej ograniczony scenariusz niż AKS/Container Apps.

Przykład ACR pull przez managed identity:

```bash
az webapp identity assign \
  --resource-group rg-appservice-demo \
  --name app-container-204

az role assignment create \
  --assignee <principal-id> \
  --scope <acr-resource-id> \
  --role AcrPull

az webapp config set \
  --resource-group rg-appservice-demo \
  --name app-container-204 \
  --generic-configurations '{"acrUseManagedIdentityCreds": true}'
```

### Persistent storage w custom containers

W Linux custom containers katalog `/home` może być persistent i współdzielony między instancjami.

Ważne:

- `/home/LogFiles` utrzymuje logi, gdy logging jest włączony.
- Persistent storage można wyłączyć przez `WEBSITES_ENABLE_APP_SERVICE_STORAGE=false`.
- Storage mount changes restartują aplikację.
- Mount Azure Storage nie jest dobrym miejscem na lokalną bazę danych ani pliki wymagające file locks.
- Azure Files mount obsługuje read/write; Blob mount dla Linux jest typowo read-only.

GitHub Actions dla containera:

1. Login do Azure.
2. Build image.
3. Push image do registry.
4. Deploy image do App Service przez `azure/webapps-deploy` z parametrem `images`.

## 10. Autoscaling

Są trzy główne sposoby zwiększania mocy:

### Scale up

Zmiana App Service Plan na mocniejszy tier/rozmiar.

Przykład:

```bash
az appservice plan update \
  --resource-group rg-appservice-demo \
  --name asp-demo \
  --sku P1V3
```

### Manual scale out

Ręczne ustawienie liczby instancji:

```bash
az appservice plan update \
  --resource-group rg-appservice-demo \
  --name asp-demo \
  --number-of-workers 3
```

### Autoscale

Reguły skalowania oparte o metryki albo harmonogram, np.:

- CPU percentage,
- Memory percentage,
- HTTP queue length,
- Data in/out,
- schedule, np. godziny pracy.

Autoscale to rozwiązanie do automatycznego zwiększania i zmniejszania liczby instancji bez aktywnego monitorowania obciążenia przez administratora. Najlepiej pasuje do obciążeń, które zmieniają się według znanego wzorca albo dają się opisać metrykami.

Typowe dobre scenariusze:

- liczba użytkowników zmienia się według regularnego harmonogramu, np. godziny pracy,
- aplikacja ma przewidywalne piki ruchu i można wcześniej ustawić profil harmonogramu,
- obciążenie rośnie lub maleje stopniowo i można reagować na metryki, np. CPU albo HTTP queue length.

Słabszy scenariusz:

- nagły napływ ruchu, który już zatrzymał system; autoscale może dodać instancje, ale nie zastępuje projektowania pod odporność, limitów, kolejek, cache i wcześniejszego ustawienia sensownych min/max.

Typowy przykład:

- scale out, gdy CPU > 70% przez 10 minut,
- scale in, gdy CPU < 30% przez 10 minut,
- min instances = 2,
- max instances = 5.

Ważne:

- Autoscale działa na App Service Plan.
- Scale out zwiększa liczbę instancji dla wszystkich apps w planie.
- Trzeba ustawić minimum, maximum i default instance count.
- Zbyt agresywne reguły mogą powodować flapping.
- Scale in powinien mieć spokojniejsze progi niż scale out.
- Scale out odpala się, gdy spełniona jest dowolna reguła scale out.
- Scale in zwykle wymaga, żeby wszystkie reguły scale in były spełnione.
- Metryki takie jak CPU Percentage i Memory Percentage dotyczą zasobów planu/instancji, a nie tylko jednej aplikacji w izolacji.
- `CPU Time` jest istotne szczególnie dla Free/Shared, bo tam obowiązują limity CPU quota.

### Flapping

Flapping to sytuacja, w której autoscale wpada w pętlę przeciwstawnych akcji: najpierw scale out, zaraz potem scale in, potem znowu scale out itd.

Przykład:

- scale out, gdy CPU > 50%,
- scale in, gdy CPU < 50%.

Jeśli masz 2 instancje i CPU wynosi 60%, autoscale doda trzecią instancję. Po dodaniu instancji ten sam ruch rozłoży się na 3 instancje i CPU może spaść np. do 40%. Wtedy reguła scale in od razu próbuje usunąć instancję. Po usunięciu CPU znowu rośnie i cykl zaczyna się od nowa.

Jak unikać flapping:

- zostaw odstęp między progami, np. scale out przy CPU > 70%, scale in przy CPU < 30%,
- ustaw cooldown period,
- używaj dłuższego okna obserwacji metryki,
- nie skaluj naraz o zbyt wiele instancji, jeśli obciążenie szybko się zmienia,
- dla scale in używaj ostrożniejszych warunków niż dla scale out.

Azure Autoscale może pominąć albo zmniejszyć planowany scale in, jeśli przewidzi, że spowodowałby natychmiastowy scale out. Scale out nie jest zwykle blokowany przez mechanizm anti-flapping, bo platforma priorytetowo traktuje dostępność.

### Automatic scaling

Automatic scaling to osobna opcja App Service, w której platforma podejmuje decyzje o scale out i prewarmed instances. Różni się od klasycznego Autoscale, gdzie sam definiujesz reguły.

Ważne:

- Działa dla Windows, Linux, code i container.
- Nie jest wspierane dla deployment slot traffic.
- Prewarmed instances są płatne.

## 11. Deployment slots

Deployment slot to osobne, działające środowisko tej samej Web App, np. `staging`.

Slot ma własny hostname:

```text
https://<app-name>-staging.azurewebsites.net
```

Zastosowania:

- testowanie zmian przed produkcją,
- zero-downtime deployment,
- szybki rollback przez swap,
- warm-up aplikacji przed przełączeniem ruchu.

Tworzenie slotu:

```bash
az webapp deployment slot create \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --slot staging
```

Deployment do slotu:

```bash
az webapp deploy \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --slot staging \
  --src-path ./app.zip \
  --type zip
```

Swap:

```bash
az webapp deployment slot swap \
  --resource-group rg-appservice-demo \
  --name app-demo-204 \
  --slot staging \
  --target-slot production
```

Slot settings:

- Niektóre ustawienia powinny zostać w slocie i nie przechodzić podczas swap.
- Przykłady: connection string do testowej bazy, feature flags, external API endpoint, Application Insights connection string.
- Takie ustawienia oznacza się jako **Deployment slot setting**.

Przykłady ustawień, które zwykle są swapped:

- framework/runtime, 32/64-bit, WebSockets,
- app settings, jeśli nie są oznaczone jako slot setting,
- connection strings, jeśli nie są oznaczone jako slot setting,
- handler mappings,
- public certificates,
- WebJobs content,
- hybrid connections,
- path mappings.

Przykłady ustawień, które zwykle nie są swapped:

- publishing endpoints,
- custom domain names,
- non-public certificates i TLS/SSL settings,
- scale settings,
- WebJobs schedulers,
- IP restrictions,
- Always On,
- diagnostic log settings,
- CORS,
- VNet Integration,
- managed identities,
- ustawienia kończące się na `_EXTENSION_VERSION`.

Jeśli chcesz zmienić domyślną sticky behavior części ustawień, można użyć app setting `WEBSITE_OVERRIDE_PRESERVE_DEFAULT_STICKY_SLOT_SETTINGS`, ale managed identities nigdy nie są swapped.

Co dzieje się przy swap:

- App Service przygotowuje target slot.
- Następuje warm-up.
- Hostnames/routing zostają przełączone.
- Jeśli coś pójdzie źle po swap, można wykonać swap z powrotem.

### Traffic routing do slotów

Można ręcznie kierować część produkcyjnego ruchu do slotu, np. 10% do `staging`, 90% do `production`.

Parametr `x-ms-routing-name` może wymusić slot dla konkretnego requestu:

```text
https://<app-name>.azurewebsites.net/?x-ms-routing-name=staging
https://<app-name>.azurewebsites.net/?x-ms-routing-name=self
```

Ważne:

- `self` oznacza production slot.
- Nowy slot zaczyna od 0% ruchu.
- Domyślna reguła routingu dla nowego deployment slot to 0%, więc cały ruch z produkcyjnego URL nadal idzie do production slot, dopóki ręcznie nie ustawisz procentu dla innego slotu.
- Traffic splitting jest przydatny do testów canary.

### Warm-up przed swap

Jeśli aplikacja wymaga przygotowania przed swap, można użyć warm-up endpoint albo konfiguracji `applicationInitialization` w `web.config` dla Windows/IIS.

Na egzaminie kojarz:

- Swap może poczekać na warm-up.
- Auto swap nie zawsze jest właściwy, jeśli przed przełączeniem muszą wykonać się skrypty albo walidacja.
- Health Check i endpoint `/health` pomagają upewnić się, że instancja jest gotowa.

Ważne na egzamin:

- Sloty są live apps, nie tylko statyczną kopią.
- Sloty mają własne konfiguracje, ale część konfiguracji może być wymieniana przy swap.
- Produkcyjny slot nazywa się `production`.
- Deployment slots wymagają odpowiedniego pricing tier.
- Auto swap może automatycznie przełączać slot po deployment, ale nie zawsze jest dobry, jeśli potrzebujesz ręcznej walidacji.

## 12. Najczęstsze pytania egzaminacyjne - szybka powtórka

- Chcesz bezpiecznie przechowywać sekret: użyj Key Vault reference albo managed identity, nie wpisuj sekretu w kodzie.
- Chcesz debugować aplikację na żywo: włącz application logging i użyj Log stream.
- Chcesz trwałe logi i zapytania KQL: wyślij logs/metrics do Log Analytics.
- Chcesz APM dla kodu: użyj Application Insights.
- Chcesz usunąć z ruchu uszkodzoną instancję: skonfiguruj Health Check.
- Chcesz, żeby klient trafiał do tej samej instancji: ARR affinity, ale preferuj aplikacje stateless.
- Chcesz zero-downtime deployment: użyj deployment slot i swap.
- Chcesz przetestować produkcyjny deployment przed przełączeniem: deploy do staging slot.
- Chcesz test canary: użyj traffic routing do deployment slot.
- Chcesz rollback po błędnym release: swap staging/production z powrotem.
- Chcesz zwiększyć moc maszyny: scale up.
- Chcesz więcej instancji: scale out.
- Chcesz automatyczne skalowanie według CPU: Autoscale rule na App Service Plan.
- Chcesz uniknąć cold start przy platform-managed scale: Automatic scaling z prewarmed instances.
- Chcesz wymusić HTTPS: ustaw HTTPS Only.
- Chcesz ograniczyć stare protokoły: ustaw minimum TLS version na 1.2 lub wyżej.
- Chcesz wdrożyć container: użyj Web App for Containers i image z registry.
- Chcesz pull image z prywatnego ACR bez hasła: managed identity + rola `AcrPull`.
- Chcesz, aby container dostał konfigurację: użyj App settings jako environment variables.
- Chcesz mieć trwały katalog w Linux custom container: `/home`, ewentualnie Azure Files mount.
- Chcesz dostęp do Azure SQL/Storage/Key Vault bez sekretu: użyj managed identity i RBAC.
- Chcesz ograniczyć ruch przychodzący: Access restrictions albo Private Endpoint.
- Chcesz dostęp wychodzący do zasobów w VNet: VNet Integration.

## 13. App Service Authentication z Microsoft Entra ID

App Service moze obsluzyc logowanie przez Microsoft Entra ID na poziomie platformy. Ten mechanizm jest czesto nazywany **Easy Auth**.

Najwazniejszy endpoint logowania:

```text
https://<app-name>.azurewebsites.net/.auth/login/aad
```

Po zalogowaniu App Service dodaje do requestu naglowki z informacjami o uzytkowniku, np.:

```text
X-MS-CLIENT-PRINCIPAL
X-MS-CLIENT-PRINCIPAL-ID
X-MS-CLIENT-PRINCIPAL-NAME
X-MS-CLIENT-PRINCIPAL-IDP
```

Aplikacja nie musi sama wykonywac redirectu do Entra ID. Platforma robi to przed wejsciem requestu do kodu.

Wazny wybor konfiguracyjny:

- Jesli ustawisz **Require authentication**, to App Service bedzie chronil cala aplikacje, lacznie z `/health`.
- Jesli `/health` ma zostac publiczne dla Health Check, ustaw **Allow unauthenticated requests** i sprawdzaj Easy Auth w kodzie tylko dla wybranych endpointow.

### Redirect URI w App Registration

Dla App Service dodaj w Microsoft Entra ID App Registration redirect URI:

```text
https://<app-name>.azurewebsites.net/.auth/login/aad/callback
```

Dla lokalnego developmentu Easy Auth nie dziala automatycznie, bo jest funkcja platformy App Service. Lokalnie mozna testowac logike aplikacji mockujac naglowki albo uzyc normalnego JWT Bearer auth w kodzie.

### Przykladowy kod odczytu uzytkownika z Easy Auth

```csharp
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed record AppServiceAuthenticatedUser(
    string? Id,
    string? Name,
    string? IdentityProvider,
    IReadOnlyDictionary<string, string[]> Claims);

public sealed class AppServiceClientPrincipal
{
    [JsonPropertyName("auth_typ")]
    public string? AuthenticationType { get; init; }

    [JsonPropertyName("name_typ")]
    public string? NameClaimType { get; init; }

    [JsonPropertyName("role_typ")]
    public string? RoleClaimType { get; init; }

    [JsonPropertyName("claims")]
    public List<AppServiceClientPrincipalClaim> Claims { get; init; } = [];
}

public sealed class AppServiceClientPrincipalClaim
{
    [JsonPropertyName("typ")]
    public string Type { get; init; } = string.Empty;

    [JsonPropertyName("val")]
    public string Value { get; init; } = string.Empty;
}

public static class AppServiceAuthenticationExtensions
{
    public static AppServiceAuthenticatedUser? GetAppServiceUser(this HttpContext context)
    {
        var principalHeader = context.Request.Headers["X-MS-CLIENT-PRINCIPAL"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(principalHeader))
        {
            return null;
        }

        var json = Encoding.UTF8.GetString(Convert.FromBase64String(principalHeader));
        var principal = JsonSerializer.Deserialize<AppServiceClientPrincipal>(json);

        if (principal is null)
        {
            return null;
        }

        var claims = principal.Claims
            .GroupBy(x => x.Type)
            .ToDictionary(
                x => x.Key,
                x => x.Select(c => c.Value).ToArray());

        return new AppServiceAuthenticatedUser(
            Id: context.Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].FirstOrDefault(),
            Name: context.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"].FirstOrDefault(),
            IdentityProvider: context.Request.Headers["X-MS-CLIENT-PRINCIPAL-IDP"].FirstOrDefault(),
            Claims: claims);
    }
}
```

### Endpoint `/me`

Ten endpoint pokazuje, czy App Service przekazal zalogowanego uzytkownika do aplikacji.

```csharp
app.MapGet("/me", (HttpContext context) =>
{
    var user = context.GetAppServiceUser();

    return user is null
        ? Results.Unauthorized()
        : Results.Ok(user);
});
```

### Ochrona wybranych endpointow

Mozesz zrobic prosty helper, ktory sprawdza, czy request zawiera uzytkownika z Easy Auth.

```csharp
static IResult RequireAppServiceUser(HttpContext context, out AppServiceAuthenticatedUser? user)
{
    user = context.GetAppServiceUser();

    return user is null
        ? Results.Unauthorized()
        : Results.Empty;
}
```

Przyklad z chronionym endpointem:

```csharp
app.MapGet("/secure-orders", async (
    HttpContext context,
    OrdersDbContext db,
    CancellationToken ct) =>
{
    var authResult = RequireAppServiceUser(context, out var user);

    if (user is null)
    {
        return authResult;
    }

    var orders = await db.Orders
        .AsNoTracking()
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync(ct);

    return Results.Ok(new
    {
        requestedBy = user.Name,
        orders
    });
});
```

### Konfiguracja w Azure Portal

1. Wejdz w App Service.
2. Otworz **Authentication**.
3. Wybierz **Add identity provider**.
4. Wybierz **Microsoft**.
5. Uzyj istniejacej App Registration albo utworz nowa.
6. Dodaj redirect URI:

```text
https://<app-name>.azurewebsites.net/.auth/login/aad/callback
```

7. Dla aplikacji z publicznym `/health` wybierz **Allow unauthenticated requests**.
8. Dla aplikacji w pelni chronionej wybierz **Require authentication**.

### Testowanie

Wejdz w przegladarce na:

```text
https://<app-name>.azurewebsites.net/.auth/login/aad?post_login_redirect_uri=/me
```

Po zalogowaniu powinienes zostac przekierowany na:

```text
https://<app-name>.azurewebsites.net/me
```

I zobaczyc dane uzytkownika oraz claims przekazane przez App Service.

Wylogowanie:

```text
https://<app-name>.azurewebsites.net/.auth/logout?post_logout_redirect_uri=/
```

Test endpointow:

```text
https://<app-name>.azurewebsites.net/health
https://<app-name>.azurewebsites.net/me
https://<app-name>.azurewebsites.net/secure-orders
```

Na egzaminie kojarz:

- `/.auth/login/aad` uruchamia logowanie Microsoft Entra ID w App Service Authentication.
- `/.auth/login/aad/callback` musi byc redirect URI w App Registration.
- Easy Auth moze chronic cala aplikacje bez kodu autoryzacji w aplikacji.
- Kod moze odczytywac zalogowanego uzytkownika z naglowkow `X-MS-CLIENT-PRINCIPAL*`.
- Dla publicznego `/health` uwazaj na tryb **Require authentication**, bo moze zablokowac health checki.
- Managed identity sluzy aplikacji do dostepu do Azure resources, np. Key Vault albo Storage. Logowanie uzytkownika do aplikacji przez Entra ID to osobny mechanizm.

## 14. Źródła

- Microsoft Learn - Azure App Service overview: https://learn.microsoft.com/en-us/azure/app-service/
- Microsoft Learn - Azure App Service plans: https://learn.microsoft.com/en-us/azure/app-service/overview-hosting-plans
- Microsoft Learn - Configure common settings: https://learn.microsoft.com/en-us/azure/app-service/configure-common
- Microsoft Learn - What is TLS/SSL in Azure App Service: https://learn.microsoft.com/en-us/azure/app-service/overview-tls
- Microsoft Learn - Add and manage TLS/SSL certificates: https://learn.microsoft.com/en-us/azure/app-service/configure-ssl-certificate
- Microsoft Learn - Set up custom domain in App Service: https://learn.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-custom-domain
- Microsoft Learn - Overview: custom domain names: https://learn.microsoft.com/en-us/azure/app-service/overview-custom-domains
- Microsoft Learn - App Service managed identity: https://learn.microsoft.com/en-us/azure/app-service/overview-managed-identity
- Microsoft Learn - Key Vault references in App Service: https://learn.microsoft.com/en-us/azure/app-service/app-service-key-vault-references
- Microsoft Learn - Deploy to App Service by using GitHub Actions: https://learn.microsoft.com/en-us/azure/app-service/deploy-github-actions
- Microsoft Learn - Diagnostics logging: https://learn.microsoft.com/en-us/azure/app-service/troubleshoot-diagnostic-logs
- Microsoft Learn - Deployment slots: https://learn.microsoft.com/en-us/azure/app-service/deploy-staging-slots
- Microsoft Learn - Health Check: https://learn.microsoft.com/en-us/azure/app-service/monitor-instances-health-check
- Microsoft Learn - Automatic scaling: https://learn.microsoft.com/en-us/azure/app-service/manage-automatic-scaling
- Microsoft Learn - Autoscale settings: https://learn.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-understanding-settings
- Microsoft Learn - Autoscale flapping: https://learn.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-flapping
- GitHub - arvigeus/AZ-204 App Service notes: https://github.com/arvigeus/AZ-204/blob/master/Topics/App%20Service.md
