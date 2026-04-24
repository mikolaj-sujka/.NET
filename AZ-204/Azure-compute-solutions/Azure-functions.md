# Azure Functions - notatki AZ-204

## 1. Czym jest Azure Functions

Azure Functions to serverless compute w Azure. Pozwala uruchamiać małe fragmenty kodu jako reakcję na zdarzenia, bez zarządzania serwerami. Kod funkcji uruchamia się po wystąpieniu triggera, np. HTTP request, message w queue, timer, event z Event Grid albo zmiana w Cosmos DB.

Typowe zastosowania:

- serverless APIs,
- webhooks,
- background jobs,
- scheduled jobs,
- processing queue messages,
- event-driven integration,
- reagowanie na zmiany danych,
- przetwarzanie plików w Blob Storage,
- integracja z Service Bus, Event Hubs, Event Grid, Cosmos DB.

Najważniejsze pojęcia:

- **Function App** - kontener zarządzania i deploymentu dla jednej lub wielu funkcji.
- **Function** - pojedyncza jednostka kodu uruchamiana przez trigger.
- **Trigger** - mechanizm uruchomienia funkcji; każda funkcja musi mieć dokładnie jeden trigger.
- **Input binding** - deklaratywne pobranie danych z zewnętrznej usługi.
- **Output binding** - deklaratywny zapis danych do zewnętrznej usługi.
- **host.json** - globalne ustawienia runtime dla całej Function App.
- **local.settings.json** - lokalne app settings i sekrety do developmentu; nie commitować.
- **Application settings** - konfiguracja w Azure, widoczna dla funkcji jako environment variables.
- **AzureWebJobsStorage** - storage account używany przez runtime Functions.

Ważne na egzamin:

- Function App jest jednostką skalowania i deploymentu.
- Wszystkie funkcje w jednej Function App współdzielą plan hostingowy, runtime version, app settings i deployment method.
- W Functions 2.x+ funkcje w jednej Function App powinny być w tym samym języku.
- Azure Functions są stateless; stan trzymaj w zewnętrznym storage, database albo Durable Functions.
- Dla long-running HTTP request nie trzymaj połączenia otwartego; użyj async pattern albo Durable Functions.

## 2. Azure Functions vs Logic Apps vs WebJobs

### Azure Functions

- Code-first.
- Dobre, gdy chcesz pisać kod i mieć pełną kontrolę nad logiką.
- Integracja przez triggers i bindings.
- Orchestration przez Durable Functions.

### Logic Apps

- Designer-first / declarative workflow.
- Dobre do integracji systemów, workflow biznesowych i gotowych connectorów.
- Ma dużo connectorów SaaS i enterprise.
- Mniej kodu, więcej konfiguracji.

Na egzaminie: jeśli wymaganie mówi **designer-first** albo **declarative workflow**, zwykle chodzi o Azure Logic Apps, nie Azure Functions.

### WebJobs

- Działają w Azure App Service.
- Bardziej klasyczny background processing.
- Brak natywnego serverless scale jak w Functions.
- Azure Functions są zbudowane na WebJobs SDK i zwykle są lepszym wyborem dla nowych event-driven workloads.

Szybki wybór:

| Wymaganie | Najlepszy wybór |
| --- | --- |
| Code-first event processing | Azure Functions |
| Designer-first workflow | Logic Apps |
| Dużo gotowych SaaS connectorów | Logic Apps |
| Background job w istniejącym App Service | WebJobs |
| Stateful orchestration w kodzie | Durable Functions |

## 3. Create and configure an Azure Functions app

Minimalne zasoby:

1. Resource group.
2. Storage account.
3. Hosting plan.
4. Function App.

Przykład: Function App w Flex Consumption / serverless style:

```bash
az group create \
  --name rg-functions-demo \
  --location westeurope

az storage account create \
  --name stfuncdemo204 \
  --resource-group rg-functions-demo \
  --location westeurope \
  --sku Standard_LRS

az functionapp create \
  --resource-group rg-functions-demo \
  --name func-demo-204 \
  --storage-account stfuncdemo204 \
  --runtime dotnet-isolated \
  --functions-version 4 \
  --flexconsumption-location westeurope
```

Przykład: Function App na Dedicated App Service Plan:

```bash
az appservice plan create \
  --name asp-functions-demo \
  --resource-group rg-functions-demo \
  --location westeurope \
  --sku S1

az functionapp create \
  --resource-group rg-functions-demo \
  --name func-dedicated-204 \
  --storage-account stfuncdemo204 \
  --plan asp-functions-demo \
  --runtime dotnet-isolated \
  --functions-version 4
```

Najważniejsze ustawienia:

- **Runtime stack** - np. .NET isolated, Node.js, Python, Java, PowerShell.
- **Functions runtime version** - obecnie najważniejsza wersja to `~4`.
- **Region** - powinien być blisko zależnych usług.
- **Storage account** - wymagany przez runtime.
- **Hosting plan** - decyduje o kosztach, skalowaniu, timeoutach i networkingu.
- **Application Insights** - monitoring, logs, traces, exceptions, dependencies.

App settings:

```bash
az functionapp config appsettings set \
  --resource-group rg-functions-demo \
  --name func-demo-204 \
  --settings MySetting=Value FUNCTIONS_EXTENSION_VERSION=~4
```

Ważne app settings:

- `FUNCTIONS_EXTENSION_VERSION` - wersja runtime, np. `~4`.
- `FUNCTIONS_WORKER_RUNTIME` - język/worker, np. `dotnet-isolated`, `node`, `python`.
- `AzureWebJobsStorage` - storage runtime.
- `APPLICATIONINSIGHTS_CONNECTION_STRING` - Application Insights.
- `WEBSITE_RUN_FROM_PACKAGE` - deployment package jako read-only zip.
- `WEBSITE_TIME_ZONE` - timezone dla timer trigger, ale nie używać w Linux Consumption/Flex Consumption, gdzie może powodować problemy.

## 4. Hosting plans

Hosting plan wpływa na:

- scaling,
- billing,
- cold start,
- timeout,
- VNet support,
- Always On,
- container support,
- dostępne CPU/RAM.

| Plan | Kiedy użyć |
| --- | --- |
| Flex Consumption | Zalecany nowy serverless plan; dynamic scale, pay-per-use, lepsza kontrola i VNet możliwości niż klasyczny Consumption. |
| Consumption | Legacy serverless plan; pay-per-execution, automatyczne skalowanie, cold start. Linux Consumption jest wycofywany dla nowych scenariuszy. |
| Premium | Event-driven scale, prewarmed instances, VNet, dłuższe wykonania, mniej cold start. |
| Dedicated/App Service Plan | Predictable billing, manual/autoscale, Always On, wspólne zasoby z Web Apps. |
| Container Apps | Functions w kontenerach obok innych microservices, skalowanie KEDA, cloud-native containers. |

Ważne:

- Flex Consumption jest zalecanym serverless hosting plan dla nowych Function Apps.
- Consumption plan ma limit timeout 5 minut domyślnie i 10 minut maksymalnie.
- HTTP trigger może mieć praktyczny limit odpowiedzi około 230 sekund przez Azure Load Balancer.
- Premium i Dedicated mogą mieć dłuższe albo unbounded timeout, ale Dedicated wymaga Always On dla niezawodnego działania.
- Dedicated plan dla Functions nie wspiera Free/Shared tier.
- Premium używa prewarmed workers, więc zmniejsza cold start.
- Container support: Premium, Dedicated i Container Apps; Flex Consumption nie wspiera custom containers.

Przykładowe limity scale out:

| Plan | Przykładowy limit |
| --- | --- |
| Consumption Windows | Do 200 instances na Function App. |
| Consumption Linux | Do 100 instances na Function App; Linux Consumption jest legacy/retired dla nowych kierunków. |
| Container Apps | Do setek replicas zależnie od konfiguracji i quota. |

Na egzaminie: pytanie o **maximum number of instances for Consumption plan on Windows** najczęściej celuje w odpowiedź **200**.

## 5. Project files

Typowy projekt Functions:

```text
host.json
local.settings.json
Function1/
  function.json
  run.csx / index.js / __init__.py
```

Dla .NET isolated/class library struktura wygląda inaczej, bo bindings są zwykle w atrybutach C#.

### host.json

Globalna konfiguracja runtime dla całej Function App.

Przykłady ustawień:

- logging,
- Application Insights sampling,
- function timeout,
- queue trigger batch size,
- retry/concurrency,
- extension-specific settings.

Przykład:

```json
{
  "version": "2.0",
  "functionTimeout": "00:10:00",
  "logging": {
    "applicationInsights": {
      "samplingSettings": {
        "isEnabled": true
      }
    }
  }
}
```

### local.settings.json

Ustawienia lokalne. Nie powinny trafiać do repozytorium, bo często zawierają sekrety.

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "MyConnection": "<connection-string>"
  }
}
```

W Azure odpowiednikiem `Values` są Application settings w Function App.

### function.json

Dla języków skryptowych i niektórych modeli konfiguracja triggerów/bindings jest w `function.json`.

```json
{
  "bindings": [
    {
      "type": "httpTrigger",
      "direction": "in",
      "authLevel": "function",
      "name": "req",
      "methods": [ "get", "post" ]
    },
    {
      "type": "http",
      "direction": "out",
      "name": "$return"
    }
  ]
}
```

Kierunki:

- Trigger zawsze ma `direction: "in"`.
- Input binding ma `direction: "in"`.
- Output binding ma `direction: "out"`.
- Niektóre bindings mogą mieć `inout`.

### Node.js v4 programming model

W Node.js v4 triggers i bindings są konfigurowane w kodzie przez package `@azure/functions`, a nie przez ręczne edytowanie `function.json`.

Przykład:

```javascript
const { app, output } = require('@azure/functions');

const queueOutput = output.storageQueue({
  queueName: 'outqueue',
  connection: 'StorageConnection'
});

app.http('CreateMessage', {
  methods: ['POST'],
  authLevel: 'function',
  extraOutputs: [queueOutput],
  handler: async (request, context) => {
    context.extraOutputs.set(queueOutput, 'message');
    return { status: 202 };
  }
});
```

Na egzaminie:

- Node.js v4: konfiguracja w code-first model przez `@azure/functions`.
- Starsze modele / niektóre języki skryptowe: konfiguracja przez `function.json`.
- `host.json` nie definiuje pojedynczego triggera; zawiera globalne runtime settings.

## 6. Triggers and bindings

Trigger uruchamia funkcję. Binding upraszcza połączenie funkcji z zewnętrzną usługą.

Zasady:

- Każda funkcja musi mieć dokładnie jeden trigger.
- Bindings są opcjonalne.
- Funkcja może mieć wiele input bindings i wiele output bindings.
- Trigger jest specjalnym typem input binding.
- Bindings pozwalają unikać ręcznego pisania kodu SDK dla prostych operacji.
- Connection w bindingu wskazuje nazwę app setting, a nie sam connection string.

Przykład scenariusza:

| Scenariusz | Trigger | Input binding | Output binding |
| --- | --- | --- | --- |
| API zapisuje message do queue | HTTP | brak | Queue Storage |
| Queue message tworzy dokument | Queue Storage | brak | Cosmos DB |
| Timer czyta plik i zapisuje wynik | Timer | Blob Storage | Blob Storage |
| Blob upload wysyła event | Blob Storage/Event Grid | Blob Storage | Event Grid |
| Zmiana w Cosmos DB publikuje message | Cosmos DB trigger | brak | Service Bus |

## 7. HTTP trigger and webhooks

HTTP trigger uruchamia funkcję przez HTTP request. Nadaje się do:

- serverless API,
- webhook receivers,
- lightweight endpoints,
- integracji z systemami zewnętrznymi.

Przykład C# isolated:

```csharp
[Function("GetOrder")]
public static HttpResponseData Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "orders/{id}")]
    HttpRequestData req,
    string id)
{
    var response = req.CreateResponse(HttpStatusCode.OK);
    response.WriteString($"Order: {id}");
    return response;
}
```

Authorization levels:

| Level | Znaczenie |
| --- | --- |
| `anonymous` | Bez function key. |
| `function` | Wymaga function key. To typowy default. |
| `admin` | Wymaga master key; używać ostrożnie. |

Key można przekazać:

```text
https://<app>.azurewebsites.net/api/<function>?code=<function-key>
```

albo headerem:

```text
x-functions-key: <function-key>
```

Ważne:

- Lokalnie auth jest zwykle wyłączone, ale w Azure `authLevel` jest egzekwowany.
- HTTP trigger w Functions 2.x+ bez body zwraca domyślnie `204 No Content`.
- Dla long-running HTTP unikaj odpowiedzi po wielu minutach; zwróć `202 Accepted` i status URL albo użyj Durable Functions.
- Webhooks w runtime 2.x+ są zwykle zwykłym HTTP triggerem.
- Specjalny `webHookType` był funkcją runtime 1.x.

## 8. Timer trigger

Timer trigger uruchamia funkcję według harmonogramu.

Przykład:

```csharp
[Function("Cleanup")]
public static void Run(
    [TimerTrigger("0 */5 * * * *")] TimerInfo timer,
    FunctionContext context)
{
    var logger = context.GetLogger("Cleanup");
    logger.LogInformation("Cleanup started");
}
```

Azure Functions używa NCRONTAB z 6 polami:

```text
{second} {minute} {hour} {day} {month} {day-of-week}
```

Przykłady:

| Schedule | Znaczenie |
| --- | --- |
| `0 */5 * * * *` | Co 5 minut. |
| `0 0 * * * *` | Co godzinę. |
| `0 0 9 * * *` | Codziennie o 09:00 UTC. |
| `0 30 8 * * 1-5` | W dni robocze o 08:30 UTC. |

Ważne właściwości:

- `schedule` - NCRONTAB albo TimeSpan.
- `runOnStartup` - uruchamia przy starcie runtime; prawie nigdy nie używać w produkcji.
- `useMonitor` - zapisuje stan harmonogramu w storage, pomaga utrzymać schedule po restartach.

Ważne na egzamin:

- Domyślny timezone to UTC.
- `TimeSpan` zamiast NCRONTAB można użyć tylko na App Service Plan.
- Timer trigger używa storage lock, żeby przy scale out uruchomiła się tylko jedna instancja danego timera.
- Timer trigger nie retry po błędzie; kolejne uruchomienie nastąpi przy następnym schedule.
- Jeśli kilka Function Apps współdzieli storage i host ID, timer może działać tylko w jednej z nich; ustaw unikalny host ID.

## 9. Data operation triggers

### Queue Storage trigger

Uruchamia funkcję, gdy wiadomość trafia do Azure Storage Queue.

```csharp
[Function("ProcessQueueMessage")]
public static void Run(
    [QueueTrigger("orders", Connection = "StorageConnection")]
    string message,
    FunctionContext context)
{
    var logger = context.GetLogger("ProcessQueueMessage");
    logger.LogInformation(message);
}
```

Ważne:

- Dobry dla prostych kolejek.
- Wiadomość po niepowodzeniu może zostać przetworzona ponownie.
- Po przekroczeniu `maxDequeueCount` trafia do poison queue.
- Nazwa poison queue zwykle ma suffix `-poison`.
- `batchSize` w `host.json` kontroluje, ile messages przetwarzać równolegle.
- `visibilityTimeout` kontroluje, po jakim czasie failed message wróci do kolejki.

### Service Bus trigger

Uruchamia funkcję dla message z Service Bus queue albo topic subscription.

```csharp
[Function("ProcessServiceBusOrder")]
public static void Run(
    [ServiceBusTrigger("orders", Connection = "ServiceBusConnection")]
    string message,
    FunctionContext context)
{
}
```

Ważne:

- Service Bus obsługuje bardziej enterprise messaging: topics, subscriptions, sessions, dead-letter queue.
- Runtime używa PeekLock.
- Gdy funkcja kończy się sukcesem, message jest completed.
- Gdy funkcja kończy się błędem, message jest abandoned.
- Poison/dead-letter handling jest po stronie Service Bus.
- Jeśli funkcja działa dłużej niż lock duration, runtime może odnawiać lock do `maxAutoRenewDuration`.

### Blob Storage trigger

Uruchamia funkcję po utworzeniu albo aktualizacji bloba.

```csharp
[Function("ProcessBlob")]
public static void Run(
    [BlobTrigger("incoming/{name}", Connection = "StorageConnection")]
    Stream blob,
    string name,
    FunctionContext context)
{
}
```

Ważne:

- Klasyczny Blob trigger może działać przez polling/logs scan.
- Event-based Blob trigger przez Event Grid ma niższą latencję i jest zalecany dla nowych scenariuszy.
- Flex Consumption wspiera event-based Blob trigger.
- Blob trigger utrzymuje blob receipts w `azure-webjobs-hosts`, żeby nie przetwarzać tego samego blob version wiele razy.
- Po kilku nieudanych próbach blob może trafić jako poison blob message do `webjobs-blobtrigger-poison`.
- Dla dużych blobów używaj `Stream`, żeby nie ładować całego pliku do pamięci.

### Cosmos DB trigger

Uruchamia funkcję na podstawie change feed w Azure Cosmos DB.

```csharp
[Function("CosmosChanges")]
public static void Run(
    [CosmosDBTrigger(
        databaseName: "shop",
        containerName: "orders",
        Connection = "CosmosConnection",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)]
    IReadOnlyList<Order> changes,
    FunctionContext context)
{
}
```

Ważne:

- Cosmos DB trigger używa change feed.
- Change feed pokazuje inserts i updates, nie deletions.
- Potrzebny jest lease container do koordynacji przetwarzania partycji.
- Jeśli kilka funkcji czyta ten sam container, użyj oddzielnych lease containers albo `LeaseContainerPrefix`.
- Trigger nie mówi bezpośrednio, czy dokument został dodany czy zaktualizowany; trzeba to rozróżniać w danych aplikacji.

### Event Grid trigger

Uruchamia funkcję na podstawie eventów z Event Grid.

```csharp
[Function("HandleEventGrid")]
public static void Run(
    [EventGridTrigger] EventGridEvent eventGridEvent,
    FunctionContext context)
{
}
```

Ważne:

- Event Grid jest dobry do event-driven integration.
- Wymaga event subscription.
- Często używany z Blob Storage events, Azure resources events i custom topics.
- Event Grid trigger jest technicznie dostarczany jako webhook HTTP request.

### Event Hubs trigger

Uruchamia funkcję dla strumienia eventów.

```csharp
[Function("ProcessEvents")]
public static void Run(
    [EventHubTrigger("telemetry", Connection = "EventHubConnection")]
    string[] events,
    FunctionContext context)
{
}
```

Ważne:

- Dobry dla telemetry, IoT, clickstream, high-throughput event ingestion.
- Przetwarzanie jest partycjonowane.
- Do integracji z IoT Hub często używa się Event Hubs-compatible endpoint.

## 10. Input and output bindings

### Input binding

Input binding pobiera dane z zewnętrznej usługi bez ręcznego tworzenia klienta SDK.

Przykład: HTTP request z `id`, funkcja czyta dokument z Cosmos DB.

```csharp
[Function("GetOrder")]
public static HttpResponseData Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "orders/{id}")]
    HttpRequestData req,
    [CosmosDBInput(
        databaseName: "shop",
        containerName: "orders",
        Id = "{id}",
        PartitionKey = "{id}",
        Connection = "CosmosConnection")]
    Order order)
{
}
```

### Output binding

Output binding zapisuje dane do zewnętrznej usługi.

Przykład: HTTP request zapisuje message do queue.

```csharp
[Function("CreateOrder")]
[QueueOutput("orders", Connection = "StorageConnection")]
public static string Run(
    [HttpTrigger(AuthorizationLevel.Function, "post")]
    HttpRequestData req)
{
    return "{ \"orderId\": \"123\" }";
}
```

### Multiple output bindings

Funkcja może zapisać dane do kilku miejsc. W .NET isolated często robi się to przez custom response object.

```csharp
public class MultiOutput
{
    [QueueOutput("audit", Connection = "StorageConnection")]
    public string AuditMessage { get; set; }

    public HttpResponseData HttpResponse { get; set; }
}
```

Ważne na egzamin:

- Bindings są wygodne, ale przy skomplikowanej logice można używać SDK.
- Nie wszystkie usługi mają trigger, input i output.
- Timer nie ma output binding jako trigger; może być triggerem, ale nie miejscem zapisu.
- SendGrid jest output binding, nie trigger.
- Table Storage jest input/output binding, nie trigger.
- Queue Storage jest trigger/output, ale nie typowym input binding.

## 11. Connections and security

Bindings używają `connection`, które wskazuje nazwę app setting.

Przykład:

```json
{
  "type": "queueTrigger",
  "name": "msg",
  "queueName": "orders",
  "connection": "OrdersStorage",
  "direction": "in"
}
```

W app settings musi istnieć:

```text
OrdersStorage=<connection-string albo identity-based connection settings>
```

Sekrety:

- lokalnie: `local.settings.json`, ale nie commitować,
- w Azure: Application settings,
- bezpieczniej: Key Vault references,
- najlepiej dla usług wspierających: managed identity / identity-based connection.

Managed identity:

1. Włącz identity dla Function App.
2. Nadaj RBAC do target service.
3. Skonfiguruj binding albo kod tak, żeby używał identity.
4. Nie przechowuj secretów.

Ważne:

- Host storage `AzureWebJobsStorage` jest szczególnie ważny dla runtime.
- Niektóre plany i funkcje nadal wymagają Azure Files settings, np. `WEBSITE_CONTENTSHARE`.
- Azure Files nie zawsze wspiera managed identity w tym samym sensie co inne usługi.
- Dla host storage identity-based connection wymaga odpowiednich ról na Storage, np. Storage Blob Data Owner w scenariuszach runtime.

## 12. Retry, errors and poison messages

Retry zależy od triggera.

Queue Storage:

- failed message wraca do kolejki po visibility timeout,
- po `maxDequeueCount` trafia do poison queue,
- concurrency można ograniczyć przez `batchSize`.

Service Bus:

- runtime używa PeekLock,
- sukces: complete,
- błąd: abandon,
- dead-letter handling jest po stronie Service Bus.

Blob trigger:

- domyślnie kilka retry,
- po niepowodzeniach może powstać poison blob message.

Timer:

- brak retry natychmiastowego,
- następne wywołanie przy kolejnym schedule.

HTTP:

- klient decyduje, czy retry,
- dla retry używać idempotency i correlation IDs.

Ważne:

- Projektuj funkcje idempotentnie.
- Zakładaj at-least-once delivery dla event/message triggers.
- Uważaj na duplicate processing.
- Dla efektów ubocznych zapisuj operation id / message id.

## 13. Scaling and concurrency

Azure Functions skaluje się zależnie od planu i triggera.

Ogólnie:

- HTTP triggers mogą szybciej dostawać nowe instances.
- Non-HTTP triggers skalują się według liczby eventów/messages i mechanizmu triggera.
- Flex Consumption ma bardziej deterministyczne per-function scaling.
- Premium zmniejsza cold start przez prewarmed instances.
- Dedicated używa manual scale albo App Service autoscale.

Concurrency:

- Queue trigger ma `batchSize` i `newBatchThreshold`.
- Service Bus ma ustawienia concurrency i lock renewal.
- HTTP concurrency zależy od planu, workerów i konfiguracji.
- Zbyt duża concurrency może wyczerpać DB connection pool albo limit downstream service.

Praktyczna zasada:

- Skalowanie Functions nie rozwiąże limitów bazy, API albo storage.
- Gdy downstream service nie wyrabia, ogranicz concurrency albo dodaj queue/buffering.

## 14. Monitoring and diagnostics

Najważniejsze narzędzia:

- Application Insights,
- Log stream,
- Azure Monitor metrics,
- Function invocation logs,
- Live metrics,
- distributed tracing,
- alerts.

Co monitorować:

- function execution count,
- failures,
- duration,
- dependency failures,
- cold start / latency,
- queue length,
- dead-letter / poison queues,
- throttling downstream services.

W kodzie:

```csharp
var logger = context.GetLogger("FunctionName");
logger.LogInformation("Processed message {MessageId}", messageId);
```

Ważne:

- Logging settings są w `host.json`.
- Connection string do Application Insights jest w app settings.
- Sampling może ograniczać ilość telemetry.
- Dla problemów produkcyjnych patrz na exceptions i dependencies, nie tylko requests.

## 15. Deployment

Popularne sposoby:

- Visual Studio / VS Code publish,
- Azure Functions Core Tools,
- Azure CLI,
- Zip deploy,
- GitHub Actions,
- Azure DevOps.

Przykład zip deploy:

```bash
az functionapp deployment source config-zip \
  --resource-group rg-functions-demo \
  --name func-demo-204 \
  --src functionapp.zip
```

Run from package:

- aplikacja działa z paczki zip,
- pliki są read-only,
- często stabilniejsze deploymenty,
- ogranicza problemy z blokadą plików.

GitHub Actions:

- `azure/login` do logowania,
- `Azure/functions-action` do deploymentu,
- zalecane OIDC zamiast publish profile/client secret.

## 16. Najczęstsze pytania egzaminacyjne - szybka powtórka

- Funkcja musi mieć dokładnie jeden trigger.
- Bindings są opcjonalne.
- Trigger ma zawsze kierunek `in`.
- Input binding czyta dane, output binding zapisuje dane.
- `host.json` zawiera globalne runtime settings.
- `local.settings.json` jest tylko lokalnie i nie powinien iść do repo.
- W Azure sekrety trzymaj w Application settings, Key Vault references albo managed identity.
- `AzureWebJobsStorage` jest wymagany przez runtime.
- HTTP auth levels: `anonymous`, `function`, `admin`.
- Function key można przekazać jako `code` query parameter albo `x-functions-key`.
- Timer trigger używa NCRONTAB z 6 polami, pierwsze pole to sekundy.
- Timer domyślnie używa UTC.
- Nie ustawiaj `runOnStartup=true` w produkcji bez bardzo dobrego powodu.
- Queue trigger po wielu błędach używa poison queue.
- Service Bus używa PeekLock i dead-letter queue po stronie Service Bus.
- Cosmos DB trigger używa change feed i lease container.
- Cosmos DB change feed nie pokazuje deletions.
- Blob trigger event-based/Event Grid jest szybszy i bardziej niezawodny niż polling.
- Dla dużych blobów używaj `Stream`.
- Consumption ma cold start i krótszy timeout.
- Premium ma prewarmed instances i VNet.
- Dedicated wymaga Always On dla niezawodnych long-running funkcji.
- HTTP response ma praktyczny limit około 230 sekund.

## 17. Porównanie z notatkami arvigeus/AZ-204

Po porównaniu z `Learning Path/Functions.md` z repo `arvigeus/AZ-204` najważniejsze punkty, które trzeba znać i które zostały uwzględnione:

- Azure Functions vs Logic Apps vs WebJobs.
- Hosting plans: Consumption, Flex Consumption, Premium, Dedicated, Container Apps.
- Timeouty i praktyczny limit HTTP przez Azure Load Balancer.
- Wymóg storage account dla Function App.
- Rola Function App jako jednostki deploymentu i skalowania.
- Pliki projektu: `host.json`, `local.settings.json`, `function.json`.
- Triggers i bindings: dokładnie jeden trigger, wiele input/output bindings.
- Kierunki bindings: `in`, `out`, czasem `inout`.
- Różnice konfiguracji bindings między .NET/Java attributes a `function.json`.
- App settings jako miejsce na connection strings i sekrety.
- Identity-based connections i managed identity.

Dodatkowo dopisałem rzeczy, które są mocno egzaminacyjne, a w porównywanym materiale są mniej rozwinięte albo warto mieć je pod ręką:

- HTTP auth levels i function keys.
- Timer NCRONTAB z przykładami.
- Queue poison messages i `visibilityTimeout`.
- Service Bus PeekLock i dead-letter behavior.
- Blob trigger receipts, poison blobs i event-based trigger.
- Cosmos DB change feed, lease container i brak delete events.
- Retry/idempotency.
- Monitoring i deployment.

## 18. Źródła

- Microsoft Learn - Azure Functions overview: https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview
- Microsoft Learn - Azure Functions triggers and bindings: https://learn.microsoft.com/en-us/azure/azure-functions/functions-triggers-bindings
- Microsoft Learn - Azure Functions hosting options: https://learn.microsoft.com/en-us/azure/azure-functions/functions-scale
- Microsoft Learn - Flex Consumption plan: https://learn.microsoft.com/en-us/azure/azure-functions/flex-consumption-plan
- Microsoft Learn - App settings reference: https://learn.microsoft.com/en-us/azure/azure-functions/functions-app-settings
- Microsoft Learn - HTTP trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-trigger
- Microsoft Learn - Timer trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer
- Microsoft Learn - Queue Storage trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue-trigger
- Microsoft Learn - Blob Storage trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob-trigger
- Microsoft Learn - Event Grid trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-event-grid-trigger
- Microsoft Learn - Cosmos DB trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-cosmosdb-v2-trigger
- Microsoft Learn - Service Bus trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus-trigger
- GitHub - arvigeus/AZ-204 Functions learning path: https://github.com/arvigeus/AZ-204/blob/master/Learning%20Path/Functions.md
