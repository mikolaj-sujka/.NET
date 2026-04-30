# Cloud Orders Processor - 01. App Service i Azure Functions w praktyce

Cel tego etapu: przejść z teorii o App Service i Azure Functions do praktycznego planu wdrożenia aplikacji **Cloud Orders Processor**. To nie jest jeszcze implementacja kodu, tylko instrukcja kroków i decyzji, które później zamienimy na konfigurację, kod i pipeline.

Zakładany podział aplikacji:

- `CloudOrdersProcessor.Api` - publiczne HTTP API hostowane w Azure App Service.
- `CloudOrdersProcessor.Functions` - funkcje w tle, np. przetwarzanie zamówień, retry, synchronizacja statusów.
- `CloudOrdersProcessor.Infrastructure` - dostęp do bazy, klientów Azure SDK, konfiguracja techniczna.
- Azure SQL albo inna baza jako storage zamówień.
- Key Vault na sekrety, których nie chcemy trzymać w repo ani w plain text.
- Application Insights jako observability dla API i Functions.
- Azure Container Registry na obraz kontenera API w późniejszym wariancie deploymentu.

## 0. Zasoby, które będą potrzebne

Proponowane zasoby dla środowiska `dev`:

- Resource group: `rg-cloud-orders-dev`.
- App Service Plan: `asp-cloud-orders-dev`.
- Web App: `app-cloud-orders-api-dev`.
- Function App: `func-cloud-orders-dev`.
- Storage Account dla Functions: `stcloudordersfuncdev`.
- Application Insights: `appi-cloud-orders-dev`.
- Log Analytics Workspace: `log-cloud-orders-dev`.
- Key Vault: `kv-cloud-orders-dev`.
- Azure SQL Server + database: `sql-cloud-orders-dev` + `sqldb-cloud-orders-dev`.
- Azure Container Registry: `acrcloudordersdev`.
- Managed identity dla API i Functions: na start system-assigned identity, później ewentualnie user-assigned identity.

Warto od początku przyjąć jedną konwencję tagów:

- `app=cloud-orders-processor`
- `environment=dev`
- `owner=<twoja-nazwa>`
- `az204=true`

## 1. App Service: publish profile, connection string i konfiguracja w portalu

Pierwszy, ćwiczebny deployment API można zrobić przez publish profile, żeby zobaczyć klasyczny flow App Service. To jest dobre do nauki, ale w docelowym pipeline lepiej użyć GitHub Actions z OIDC albo managed identity, a nie trzymać publish profile jako stały sekret.

Kroki:

1. Utwórz App Service Plan i Web App dla `CloudOrdersProcessor.Api`.
2. W Azure Portal wejdź w Web App i pobierz publish profile.
3. W Visual Studio / VS Code / CLI użyj publish profile do pierwszego deploymentu.
4. Sprawdź, czy aplikacja odpowiada pod `https://app-cloud-orders-api-dev.azurewebsites.net`.
5. Włącz `HTTPS Only`.
6. Ustaw minimalną wersję TLS na co najmniej `1.2`.
7. W App Service ustaw `Always On`, jeśli tier na to pozwala.

Konfiguracja aplikacji w Azure Portal:

- App Service -> Environment variables / Configuration -> Application settings.
- Dodaj ustawienia aplikacji, np.:
  - `ASPNETCORE_ENVIRONMENT=Development` albo `Staging`.
  - `Orders__DatabaseProvider=SqlServer`.
  - `ExternalApis__Payment__BaseUrl=https://payments.example.test`.
- Connection string do bazy ustaw w sekcji Connection strings albo jako app setting zgodnie z tym, jak będzie czytać go aplikacja.

Przykładowy fikcyjny secret przez Key Vault reference:

- Secret w Key Vault: `FakePaymentApiKey`.
- Wartość przykładowa: `fake-dev-payment-key-do-not-use`.
- App setting w App Service:
  - `ExternalApis__Payment__ApiKey=@Microsoft.KeyVault(VaultName=kv-cloud-orders-dev;SecretName=FakePaymentApiKey)`

Żeby Key Vault reference działał:

1. Włącz managed identity dla Web App.
2. Nadaj tej identity dostęp do sekretów w Key Vault, np. rolę `Key Vault Secrets User`.
3. Upewnij się, że Key Vault pozwala na dostęp z tej aplikacji, zwłaszcza jeśli używasz firewall/private endpoint.
4. Zrestartuj Web App albo odśwież konfigurację.

Na co uważać:

- Zmiana app settings i connection strings zwykle restartuje aplikację.
- Publish profile daje szeroki dostęp do wdrożenia aplikacji, więc traktuj go jak sekret.
- Sekrety produkcyjne trzymaj w Key Vault, nie w `appsettings.json`, repo ani GitHub secrets, jeśli można ich uniknąć.

## 2. Azure Functions: publish i dostęp do bazy przez managed identity

`CloudOrdersProcessor.Functions` będzie służyć do pracy asynchronicznej, np. później:

- obsługa zdarzeń z Service Bus,
- aktualizacja statusu zamówienia,
- retry i dead-letter handling,
- cykliczne sprzątanie danych testowych,
- synchronizacja z zewnętrznym systemem płatności lub magazynem.

Kroki deploymentu:

1. Utwórz Function App z odpowiednim runtime `.NET`.
2. Podepnij Storage Account wymagany przez Azure Functions.
3. Podepnij Application Insights już na etapie tworzenia albo później w konfiguracji.
4. Opublikuj `CloudOrdersProcessor.Functions` z Visual Studio / VS Code / Azure Functions Core Tools.
5. Sprawdź logi w Log stream i Application Insights.

Dostęp do bazy przez managed identity:

1. Włącz system-assigned managed identity dla Function App.
2. W Azure SQL dodaj identity Function App jako użytkownika Entra ID w bazie.
3. Nadaj minimalne role w bazie, np. osobne uprawnienia do odczytu/zapisu tabel zamówień zamiast pełnego `db_owner`.
4. W kodzie później użyj `DefaultAzureCredential`, żeby lokalnie działało konto developera, a w Azure identity Function App.
5. W konfiguracji trzymaj nazwę serwera/bazy, ale bez hasła.

Praktyczny model konfiguracji:

- Lokalnie: developer loguje się przez Azure CLI / Visual Studio / Azure Developer CLI.
- Lokalnie: ten sam developer musi mieć użytkownika/uprawnienia w bazie.
- W Azure: Function App używa swojej managed identity.
- Kod nie rozróżnia ręcznie tych przypadków, tylko używa `DefaultAzureCredential`.

Na co uważać:

- Managed identity nie daje dostępu sama z siebie. Trzeba nadać role/uprawnienia na docelowym zasobie.
- Function App też ma app settings i connection strings, ale dla identity-based access unikamy sekretów.
- Dla triggerów, które wymagają storage connection, sprawdzimy osobno, czy można użyć identity-based connection. Nie każdy binding ma te same wymagania.

## 3. Application Insights dla API i Functions

Application Insights ma dawać jeden widok na requesty API, wykonania funkcji, wyjątki, dependency calls i traces.

Kroki:

1. Utwórz workspace-based Application Insights.
2. Podepnij App Service do Application Insights.
3. Podepnij Function App do tego samego Application Insights albo do osobnej instancji, jeśli chcesz rozdzielać telemetrię.
4. Ustaw connection string Application Insights w konfiguracji:
   - App Service: `APPLICATIONINSIGHTS_CONNECTION_STRING`.
   - Function App: `APPLICATIONINSIGHTS_CONNECTION_STRING`.
5. W aplikacji API dodaj później telemetry SDK, jeśli sam runtime nie zbiera wystarczająco dużo danych.
6. W Functions zostaw konfigurację worker telemetry, która już jest w `Program.cs`, i sprawdź czy trafia do właściwego App Insights.

Co sprawdzić po wdrożeniu:

- Live Metrics pokazuje requesty.
- Failures pokazuje wyjątki.
- Dependencies pokazuje połączenia do SQL/HTTP.
- Logs pozwala pisać zapytania KQL.
- Function executions mają correlation z dependency calls.

Warto później dodać:

- health endpoint w API,
- custom telemetry dla ważnych operacji biznesowych, np. `OrderAccepted`, `OrderProcessingFailed`,
- alerty na błędy 5xx, czas odpowiedzi i nieudane wykonania funkcji.

## 4. ACR, kontener API i managed identity do pull image

Ten punkt przygotowuje wariant, w którym `CloudOrdersProcessor.Api` jest wdrażany jako kontener do App Service.

Kroki:

1. Utwórz Azure Container Registry.
2. Zbuduj obraz dla `CloudOrdersProcessor.Api`.
3. Wypchnij obraz do ACR.
4. Skonfiguruj Web App for Containers albo zmień istniejącą Web App na deployment z kontenera.
5. Włącz managed identity dla Web App.
6. Nadaj identity Web App rolę `AcrPull` na ACR.
7. Skonfiguruj App Service, żeby pobierał obraz z ACR z użyciem managed identity, bez registry password.
8. Ustaw port kontenera, jeśli aplikacja nie używa oczekiwanego domyślnego portu.
9. Sprawdź logi kontenera w App Service.

Na co uważać:

- Dla prywatnego ACR nie zapisuj admin password w konfiguracji, jeśli możesz użyć managed identity.
- Każda zmiana tagu obrazu powinna być jawna i śledzona przez pipeline.
- Na początku można używać tagu `dev`, ale docelowo lepiej tagować obraz numerem buildu albo SHA commita.

## 5. GitHub Actions: pipeline deploymentu

Docelowy pipeline powinien zastąpić ręczny publish profile.

Minimalny flow dla API bez kontenera:

1. Checkout kodu.
2. Restore/build/test.
3. Publish projektu API.
4. Login do Azure.
5. Deploy do App Service.
6. Smoke test endpointu `/health`.

Minimalny flow dla Functions:

1. Checkout kodu.
2. Restore/build/test.
3. Publish projektu Functions.
4. Login do Azure.
5. Deploy do Function App.
6. Sprawdzenie statusu Function App i logów.

Minimalny flow dla API jako kontener:

1. Checkout kodu.
2. Build/test.
3. Docker build.
4. Push image do ACR.
5. Deploy image tag do App Service.
6. Smoke test.

Zalecany sposób logowania:

- Na start można użyć publish profile jako sekretu GitHub, żeby zobaczyć deployment.
- Docelowo użyj GitHub Actions OIDC + federated credential w Microsoft Entra ID.
- Service principal / federated identity powinien mieć minimalne role, np. prawo do deploy na Web App/Function App i push do ACR.

Sekrety i konfiguracja w pipeline:

- Nie wkładaj connection stringów z hasłami do workflow.
- Sekrety aplikacji trzymaj w Key Vault.
- Pipeline może ustawiać app settings, ale wartości sekretne powinny być Key Vault references albo pochodzić z bezpiecznego źródła.

## 6. Microsoft Entra ID auth dla wybranych endpointów

W tym projekcie lepiej zacząć od ochrony wybranych endpointów, a nie całej aplikacji. Dzięki temu `/health`, OpenAPI w środowisku dev i publiczne endpointy testowe nie zostaną przypadkiem zablokowane.

Proponowany podział:

- Publiczne:
  - `GET /health`
  - `GET /orders/{id}/status`, jeśli status ma być publiczny po tokenie/identyfikatorze zamówienia
- Chronione Entra ID:
  - `POST /orders`
  - `GET /admin/orders`
  - `POST /admin/orders/{id}/process`
  - endpointy diagnostyczne i administracyjne

Kroki:

1. Utwórz albo użyj istniejącej App Registration w Microsoft Entra ID.
2. W App Service włącz Authentication.
3. Dodaj provider Microsoft.
4. Ustaw redirect URI dla App Service Authentication.
5. Wybierz tryb `Allow unauthenticated requests`, jeśli ochrona ma być selektywna w kodzie.
6. W kodzie później odczytuj nagłówki Easy Auth, np. `X-MS-CLIENT-PRINCIPAL`.
7. Dodaj własny mechanizm wymagania użytkownika tylko na wybranych endpointach/grupach endpointów.

Na co uważać:

- Tryb `Require authentication` chroni całą aplikację, w tym `/health`.
- Managed identity aplikacji to nie to samo co logowanie użytkownika przez Entra ID.
- Do autoryzacji administracyjnej warto później użyć ról/grup z Entra ID.

## 7. Dostęp do bazy przez managed identity lokalnie i po deployu

Chcemy jeden model dostępu do bazy:

- lokalnie developer używa swojej tożsamości Entra ID,
- w Azure App Service używa managed identity Web App,
- w Azure Functions używa managed identity Function App,
- kod używa `DefaultAzureCredential`.

Kroki dla lokalnego developmentu:

1. Developer loguje się do Azure przez Azure CLI, Visual Studio albo Azure Developer CLI.
2. Konto developera dostaje dostęp do bazy.
3. Lokalna konfiguracja zawiera nazwę serwera i bazy, ale nie hasło.
4. Kod pobiera token Entra ID do Azure SQL przez Azure Identity.

Kroki dla App Service:

1. Włącz managed identity Web App.
2. Dodaj identity jako użytkownika w bazie.
3. Nadaj minimalne uprawnienia.
4. Ustaw app settings z nazwą serwera/bazy.
5. Sprawdź połączenie po deployu.

Kroki dla Azure Functions:

1. Włącz managed identity Function App.
2. Dodaj identity jako użytkownika w bazie.
3. Nadaj minimalne uprawnienia.
4. Ustaw app settings z nazwą serwera/bazy.
5. Sprawdź wykonanie funkcji, które czyta/zapisuje dane.

Praktyczna zasada:

- API może mieć uprawnienia do tworzenia i odczytu zamówień.
- Functions mogą mieć uprawnienia do aktualizacji statusów i zapisu logów przetwarzania.
- Pipeline nie powinien potrzebować pełnego dostępu do danych aplikacji. Jeśli pipeline uruchamia migracje, nadaj mu osobną, kontrolowaną tożsamość.

## 8. Custom domain i darmowy certyfikat

Ważne rozróżnienie: Azure App Service daje darmową domyślną domenę `*.azurewebsites.net`. Dla własnej domeny, np. `orders.example.com`, musisz mieć domenę kupioną albo zarządzaną poza App Service. Darmowy może być certyfikat TLS przez App Service Managed Certificate, ale nie sama własna domena.

Kroki:

1. Wybierz subdomenę, np. `orders.example.com`.
2. W DNS dodaj CNAME:
   - `orders.example.com -> app-cloud-orders-api-dev.azurewebsites.net`
3. Dodaj TXT record do weryfikacji domeny, jeśli App Service tego wymaga.
4. W App Service dodaj custom domain.
5. Utwórz App Service Managed Certificate dla tej domeny.
6. Dodaj TLS binding do custom domain.
7. Włącz `HTTPS Only`.
8. Sprawdź, czy `https://orders.example.com` działa i pokazuje prawidłowy certyfikat.

Na co uważać:

- App Service Managed Certificate nie obsługuje wildcard certificates.
- Certyfikatu zarządzanego App Service nie można wyeksportować.
- Dla root/apex domain zwykle używa się A record, dla subdomeny zwykle CNAME.

## 9. Dodatkowe praktyczne elementy z teorii App Service i Functions

Te rzeczy warto dodać w tym samym etapie albo zaraz po nim:

### Health Check

Dodaj endpoint `/health` w API i skonfiguruj App Service Health Check.

Cel:

- App Service może odsunąć uszkodzoną instancję od ruchu.
- Pipeline może wykonać smoke test po deployu.
- Monitoring może używać endpointu do availability tests.

### Deployment slots

Dla App Service dodaj slot `staging`.

Flow:

1. Pipeline deployuje do `staging`.
2. Aplikacja robi warm-up.
3. Smoke test sprawdza `staging`.
4. Swap przenosi wersję na production.
5. W razie problemu robisz swap back.

Pamiętaj:

- Nowy slot ma domyślnie 0% ruchu.
- Connection strings i app settings środowiskowe oznaczaj jako slot settings.
- WebJobs content zwykle jest swapped, ale WebJob schedulers nie.

### Autoscale

Dla App Service Plan ustaw sensowny autoscale:

- minimum 1 albo 2 instancje,
- maksimum zależne od budżetu,
- scale out np. przy CPU albo HTTP queue length,
- scale in z niższym progiem i cooldown, żeby uniknąć flapping.

Na start w `dev` może wystarczyć manual scale. Autoscale ma większy sens w środowisku testowym/prod albo przy ćwiczeniu egzaminacyjnym.

### Access restrictions

Jeśli dodasz endpointy administracyjne:

- ogranicz dostęp do nich autoryzacją Entra ID,
- rozważ Access restrictions dla środowiska dev,
- nie zostawiaj publicznych endpointów diagnostycznych bez ochrony.

### Logi i diagnostyka

Włącz:

- App Service application logs do krótkiego debugowania,
- Application Insights dla trwałej obserwowalności,
- diagnostic settings do Log Analytics, jeśli chcesz analizować resource logs.

### Konfiguracja środowisk

Rozdziel:

- `dev`,
- `test`,
- `prod`.

Dla każdego środowiska osobne:

- App Service / Function App,
- Key Vault,
- database,
- app settings,
- managed identities,
- Application Insights albo przynajmniej osobny `cloud_RoleName`.

## 10. Kolejne etapy projektu

Ten plik jest pierwszym etapem. Następne praktyczne notatki mogą wyglądać tak:

1. `02-container-apps.md` - wariant z Azure Container Apps, revisions, ingress, scale rules.
2. `03-storage.md` - Blob Storage, Queue Storage, lifecycle, managed identity.
3. `04-messaging.md` - Service Bus vs Event Grid/Event Hubs, queues, topics, DLQ, retry.
4. `05-api-management.md` - wystawienie API przez APIM, policies, rate limits, auth, versioning.
5. `06-observability-and-resilience.md` - App Insights, KQL, alerts, retries, circuit breaker, dashboards.

## Szybka checklista tego etapu

- App Service utworzony i pierwszy publish wykonany.
- Connection string albo konfiguracja bazy ustawiona w Azure Portal.
- Fikcyjny secret w Key Vault podpięty przez Key Vault reference.
- Web App ma managed identity i dostęp do Key Vault.
- Function App opublikowana.
- Function App ma managed identity i dostęp do bazy.
- API i Functions wysyłają telemetry do Application Insights.
- ACR utworzony.
- Web App potrafi pobrać obraz z ACR przez managed identity.
- GitHub Actions ma plan deployu API i Functions.
- Wybrane endpointy są chronione przez Microsoft Entra ID.
- Lokalny development i deployment używają identity-based access do bazy.
- Custom domain i darmowy managed certificate są zaplanowane.
- Health Check, deployment slot, logs i autoscale są uwzględnione w planie.
