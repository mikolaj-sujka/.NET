# Azure Functions - pytania AZ-204

Pytania obejmują tworzenie i konfigurację Function App, input/output bindings oraz triggers: data operations, timers i webhooks. Format jest celowo scenariuszowy, bo AZ-204 często sprawdza wybór właściwego triggera/bindingu i drobne ograniczenia runtime.

---

Question: Organizacja chce zaimplementować serverless workflow do rozwiązania problemu biznesowego. Jednym z wymagań jest designer-first, declarative development model. Która usługa spełnia wymaganie?

- [ ] Azure Functions
- [x] Azure Logic Apps
- [ ] WebJobs

Answer: Azure Logic Apps używa designer-first/declarative workflow model. Azure Functions jest code-first, a WebJobs to code-first background processing w App Service.

---

Question: Ile triggerów musi mieć pojedyncza Azure Function?

- [ ] Zero albo więcej.
- [x] Dokładnie jeden.
- [ ] Dokładnie dwa: input i output.
- [ ] Jeden trigger i jeden binding są zawsze wymagane.

Answer: Funkcja musi mieć dokładnie jeden trigger. Bindings są opcjonalne.

---

Question: Czym jest input binding?

- [x] Deklaratywnym sposobem pobrania danych z zewnętrznej usługi.
- [ ] Mechanizmem uruchamiania funkcji.
- [ ] Sposobem deployowania Function App.
- [ ] Zawsze zapisem danych do Storage.

Answer: Input binding czyta dane i przekazuje je do funkcji jako parametr.

---

Question: Czym jest output binding?

- [ ] Mechanizmem uruchamiania funkcji.
- [x] Deklaratywnym sposobem zapisu danych do zewnętrznej usługi.
- [ ] Wymaganym elementem każdej funkcji.
- [ ] Plikiem `host.json`.

Answer: Output binding pozwala zapisać dane np. do Queue, Blob, Cosmos DB albo Service Bus.

---

Question: Which of the following choices is required for a function to run?

- [ ] Binding
- [x] Trigger
- [ ] Both triggers and bindings

Answer: Trigger is required because it defines how the function is invoked. Bindings are optional.

---

Question: Which of the following choices supports both the `in` and `out` direction settings?

- [x] Bindings
- [ ] Trigger
- [ ] Connection value

Answer: Input and output bindings use `in` and `out`. A trigger is always `in`.

---

Question: Który plik zawiera globalne ustawienia runtime dla Function App?

- [x] `host.json`
- [ ] `local.settings.json`
- [ ] `function.json`
- [ ] `appsettings.json`

Answer: `host.json` konfiguruje runtime i extensions dla całej Function App.

---

Question: In the Node.js v4 programming model, how are triggers and bindings configured?

- [x] In code using the `@azure/functions` package.
- [ ] By editing `function.json` directly in the Azure portal.
- [ ] By adding entries to `host.json`.

Answer: Node.js v4 uses the code-first programming model with `@azure/functions`. `host.json` stores global runtime settings, not individual trigger definitions.

---

Question: Gdzie lokalnie przechowasz connection stringi używane podczas developmentu?

- [ ] `function.json`
- [ ] `host.json`
- [x] `local.settings.json`
- [ ] `Program.cs`

Answer: `local.settings.json` służy do lokalnych settings i sekretów, ale nie powinien być commitowany.

---

Question: Gdzie przechowasz connection strings dla Function App uruchomionej w Azure?

- [ ] `local.settings.json`
- [ ] `function.json`
- [x] Application settings Function App.
- [ ] W nazwie triggera.

Answer: W Azure configuration values są w Application settings i są dostępne jako environment variables.

---

Question: Które ustawienie określa wersję runtime Azure Functions?

- [x] `FUNCTIONS_EXTENSION_VERSION`
- [ ] `FUNCTIONS_WORKER_RUNTIME_VERSION`
- [ ] `AzureFunctionsRuntime`
- [ ] `FUNCTIONS_PLAN_VERSION`

Answer: `FUNCTIONS_EXTENSION_VERSION=~4` wskazuje Functions runtime 4.x.

---

Question: Które ustawienie wskazuje worker/język Function App?

- [ ] `FUNCTIONS_EXTENSION_VERSION`
- [x] `FUNCTIONS_WORKER_RUNTIME`
- [ ] `WEBSITE_TIME_ZONE`
- [ ] `AzureWebJobsDashboard`

Answer: `FUNCTIONS_WORKER_RUNTIME` ma wartości typu `dotnet-isolated`, `node`, `python`.

---

Question: Do czego runtime Azure Functions używa `AzureWebJobsStorage`?

- [x] Do operacji hosta runtime, koordynacji triggerów i logów/metadanych.
- [ ] Wyłącznie do przechowywania kodu funkcji.
- [ ] Tylko do HTTP trigger.
- [ ] Do konfiguracji CORS.

Answer: `AzureWebJobsStorage` jest podstawowym storage dla runtime Functions.

---

Question: Tworzysz Function App. Który zasób jest wymagany niezależnie od planu?

- [x] Storage account.
- [ ] Azure SQL Database.
- [ ] Service Bus namespace.
- [ ] API Management.

Answer: Function App wymaga Azure Storage account dla runtime.

---

Question: Który plan jest obecnie zalecanym serverless hosting plan dla nowych Azure Functions?

- [x] Flex Consumption.
- [ ] Free App Service Plan.
- [ ] Shared App Service Plan.
- [ ] Classic Cloud Services.

Answer: Flex Consumption jest zalecanym serverless planem dla nowych Function Apps.

---

Question: What is a key benefit of the Flex Consumption plan in Azure Functions hosting options?

- [ ] It provides fully predictable billing and manual scale instances.
- [x] It offers high scalability with compute choices, virtual networking, and pay-as-you-go billing.
- [ ] It allows for the packaging of custom libraries with function code.

Answer: Flex Consumption provides serverless pay-as-you-go billing with high scalability, compute choices and virtual networking options. Predictable billing/manual scale is more aligned with Dedicated plan. Packaging custom libraries in containers points more toward Container Apps/Premium/Dedicated container scenarios.

---

Question: What is the maximum number of instances for a function app on a Consumption plan in Windows?

- [ ] 300
- [ ] 100
- [x] 200

Answer: On Windows Consumption plan, the maximum number of instances is 200. Linux Consumption is commonly listed as 100, and Container Apps can go higher depending on configuration/quota.

---

Question: Potrzebujesz prewarmed instances, VNet i mniejszego cold start. Który plan wybierzesz?

- [ ] Consumption.
- [x] Premium.
- [ ] Free.
- [ ] Shared.

Answer: Premium daje prewarmed workers, VNet i event-driven scale.

---

Question: Chcesz pełniej kontrolować manual/autoscale i płacić przewidywalnie za stale przydzielony compute. Który plan pasuje?

- [ ] Consumption.
- [ ] Flex Consumption.
- [x] Dedicated/App Service Plan.
- [ ] Event Grid.

Answer: Dedicated plan działa na App Service Plan i daje przewidywalne koszty oraz manual/autoscale.

---

Question: Jaki jest praktyczny limit czasu odpowiedzi HTTP trigger wynikający z Azure Load Balancer?

- [ ] Około 30 sekund.
- [ ] Około 60 sekund.
- [x] Około 230 sekund.
- [ ] Dokładnie 24 godziny.

Answer: HTTP-triggered function powinna odpowiedzieć w okolicach 230 sekund; dłuższa praca powinna użyć async pattern/Durable Functions.

---

Question: Który trigger wybierzesz do serverless API?

- [x] HTTP trigger.
- [ ] Timer trigger.
- [ ] Queue trigger.
- [ ] Blob trigger.

Answer: HTTP trigger uruchamia funkcję przez HTTP request i nadaje się do API/webhooks.

---

Question: Jakie są valid HTTP authorization levels?

- [x] `anonymous`
- [x] `function`
- [x] `admin`
- [ ] `owner`
- [ ] `reader`

Answer: HTTP trigger obsługuje `anonymous`, `function`, `admin`.

---

Question: Jak przekazać function key do HTTP-triggered function?

- [x] Query parameter `code`.
- [x] Header `x-functions-key`.
- [ ] Header `Authorization: Bearer` zawsze automatycznie.
- [ ] Parametr `key` w route template.

Answer: Function key można przekazać w `?code=` albo w `x-functions-key`.

---

Question: Co jest prawdą o authorization podczas lokalnego uruchamiania Functions?

- [x] Lokalnie authorization jest zwykle wyłączone niezależnie od `authLevel`.
- [ ] Lokalnie zawsze trzeba podać master key.
- [ ] Lokalnie działa tylko `anonymous`.
- [ ] Lokalnie HTTP trigger nie działa.

Answer: Przy lokalnym uruchomieniu auth requirements są zwykle wyłączone; w Azure są egzekwowane.

---

Question: Który trigger wybierzesz do zadania co 5 minut?

- [ ] HTTP trigger.
- [x] Timer trigger.
- [ ] Cosmos DB trigger.
- [ ] Event Hub trigger.

Answer: Timer trigger uruchamia funkcję według schedule.

---

Question: Która NCRONTAB expression uruchamia funkcję co 5 minut?

- [x] `0 */5 * * * *`
- [ ] `* 5 * * * *`
- [ ] `0 5 * * * *`
- [ ] `5 0 * * * *`

Answer: W Azure Functions NCRONTAB ma sekundy jako pierwsze pole. `0 */5 * * * *` oznacza co 5 minut w sekundzie 0.

---

Question: Jaki timezone domyślnie używa Timer trigger?

- [x] UTC.
- [ ] Timezone resource group.
- [ ] Timezone przeglądarki użytkownika.
- [ ] Timezone regionu Azure.

Answer: Timer trigger domyślnie używa UTC.

---

Question: Dlaczego `runOnStartup=true` w Timer trigger jest ryzykowne w produkcji?

- [x] Funkcja może uruchamiać się przy restartach, wake-up i scale-out w trudnych do przewidzenia momentach.
- [ ] Wyłącza harmonogram.
- [ ] Zmienia timezone na lokalny.
- [ ] Usuwa `AzureWebJobsStorage`.

Answer: `runOnStartup` może powodować dodatkowe wykonania i koszty, szczególnie w planach dynamicznych.

---

Question: Function App skaluje się do wielu instancji. Co jest prawdą dla Timer trigger?

- [x] Runtime używa storage lock, aby uruchomić tylko jedną instancję timera.
- [ ] Timer uruchamia się na każdej instancji jednocześnie.
- [ ] Timer wymaga HTTP request.
- [ ] Timer działa tylko w Dedicated plan.

Answer: Timer trigger używa storage lock do koordynacji.

---

Question: Który trigger wybierzesz, gdy wiadomość pojawia się w Azure Storage Queue?

- [x] Queue Storage trigger.
- [ ] Blob input binding.
- [ ] Timer trigger.
- [ ] Table input binding.

Answer: Queue Storage trigger uruchamia funkcję po pojawieniu się message w Storage Queue.

---

Question: Queue-triggered function wielokrotnie failuje dla tej samej wiadomości. Co zwykle stanie się po przekroczeniu `maxDequeueCount`?

- [x] Message trafi do poison queue.
- [ ] Message zostanie automatycznie zapisany do Cosmos DB.
- [ ] Function App zmieni plan.
- [ ] Message zostanie przetworzony jako Timer trigger.

Answer: Storage Queue trigger używa poison queue dla wiadomości, których nie da się przetworzyć.

---

Question: Chcesz ograniczyć równoległość Queue trigger, bo baza danych ma za mały connection pool. Co możesz zmienić?

- [x] `batchSize` w `host.json`.
- [ ] `authLevel`.
- [ ] `WEBSITE_TIME_ZONE`.
- [ ] DNS CNAME.

Answer: `batchSize` wpływa na liczbę messages pobieranych/przetwarzanych równolegle.

---

Question: Który trigger wybierzesz do enterprise messaging z topics, subscriptions i DLQ?

- [ ] Storage Queue trigger.
- [x] Service Bus trigger.
- [ ] Timer trigger.
- [ ] HTTP trigger.

Answer: Service Bus jest właściwy dla bardziej zaawansowanych scenariuszy messaging.

---

Question: Co oznacza PeekLock w Service Bus trigger?

- [x] Message jest zablokowany dla innych receiverów, a po sukcesie funkcji zostaje completed.
- [ ] Message jest usuwany przed wykonaniem funkcji.
- [ ] Message jest zawsze przetwarzany tylko raz.
- [ ] Lock dotyczy wyłącznie Blob Storage.

Answer: Functions runtime używa PeekLock; sukces kończy message, błąd go porzuca/odnawia według ustawień Service Bus.

---

Question: Który trigger wybierzesz, gdy plik pojawia się w Blob Storage i chcesz niską latencję?

- [ ] Klasyczny polling Blob trigger zawsze.
- [x] Event-based Blob trigger / Event Grid.
- [ ] Timer co 24h.
- [ ] HTTP trigger bez event subscription.

Answer: Event-based Blob trigger przez Event Grid ma niższą latencję i jest zalecany dla nowych scenariuszy.

---

Question: Dlaczego dla dużych blobów warto używać `Stream`?

- [x] Żeby nie ładować całego blob do pamięci.
- [ ] Bo tylko `Stream` pozwala używać Timer trigger.
- [ ] Bo `byte[]` nie jest wspierane nigdy.
- [ ] Bo `Stream` automatycznie tworzy Cosmos DB lease.

Answer: `Stream` ogranicza memory pressure przy dużych plikach.

---

Question: Do czego służą blob receipts?

- [x] Do śledzenia, że konkretna wersja blob została już przetworzona.
- [ ] Do wystawiania TLS certificate.
- [ ] Do ustawiania function key.
- [ ] Do zapisywania deployment package.

Answer: Runtime zapisuje blob receipts w host storage, aby nie przetwarzać tej samej wersji wielokrotnie.

---

Question: Który trigger używa Cosmos DB change feed?

- [x] Cosmos DB trigger.
- [ ] Timer trigger.
- [ ] HTTP trigger.
- [ ] Queue output binding.

Answer: Cosmos DB trigger reaguje na change feed.

---

Question: Co publikuje Cosmos DB change feed dla triggera?

- [x] Inserts i updates.
- [ ] Deletes jako osobne delete events w podstawowym trybie.
- [ ] Tylko reads.
- [ ] Tylko schema changes.

Answer: Change feed dla triggera publikuje nowe i zmienione itemy; deletions nie są standardowo emitowane jako dokumenty.

---

Question: Po co Cosmos DB trigger potrzebuje lease container?

- [x] Do koordynacji przetwarzania zmian i partycji.
- [ ] Do przechowywania HTTP keys.
- [ ] Do hostowania Function App.
- [ ] Do szyfrowania TLS.

Answer: Lease container przechowuje stan przetwarzania change feed.

---

Question: Dwie funkcje czytają ten sam Cosmos DB container przez trigger. Jak uniknąć sytuacji, że jedna blokuje drugą?

- [x] Użyć osobnych lease containers albo różnych `LeaseContainerPrefix`.
- [ ] Użyć tego samego host ID i tego samego lease container.
- [ ] Wyłączyć `AzureWebJobsStorage`.
- [ ] Zmienić HTTP authLevel na anonymous.

Answer: Oddzielne lease state pozwala obu funkcjom niezależnie przetwarzać change feed.

---

Question: Który trigger wybierzesz do reagowania na zmiany zasobów Azure, np. utworzenie blob albo zmiana resource state?

- [x] Event Grid trigger.
- [ ] Timer trigger.
- [ ] Table input binding.
- [ ] HTTP output binding.

Answer: Event Grid jest przeznaczony do event-driven reakcji na zdarzenia.

---

Question: Który trigger najlepiej pasuje do telemetry stream/high-throughput events?

- [ ] Timer trigger.
- [ ] Blob input binding.
- [x] Event Hubs trigger.
- [ ] HTTP output binding.

Answer: Event Hubs trigger jest właściwy dla strumieni telemetry, IoT i dużego wolumenu eventów.

---

Question: Które z poniższych są triggerami?

- [x] HTTP.
- [x] Timer.
- [x] Queue Storage.
- [x] Cosmos DB.
- [ ] SendGrid.
- [ ] Table Storage.

Answer: SendGrid jest output binding; Table Storage jest input/output binding, nie triggerem.

---

Question: Które z poniższych są typowymi output bindings?

- [x] Queue Storage.
- [x] Blob Storage.
- [x] Cosmos DB.
- [x] SendGrid.
- [ ] Timer.

Answer: Timer jest triggerem, nie output binding.

---

Question: Masz HTTP-triggered function, która po request ma wrzucić message do Storage Queue. Co zastosujesz?

- [x] HTTP trigger + Queue output binding.
- [ ] Queue trigger + HTTP output binding.
- [ ] Timer trigger + Table trigger.
- [ ] Cosmos DB trigger + Timer output.

Answer: HTTP uruchamia funkcję, Queue output zapisuje message.

---

Question: Masz Queue message z orderId i chcesz pobrać dodatkowe dane zamówienia z Blob Storage. Co zastosujesz?

- [x] Queue trigger + Blob input binding.
- [ ] Blob trigger + Queue input binding.
- [ ] Timer trigger + HTTP admin key.
- [ ] SendGrid trigger.

Answer: Queue trigger uruchamia funkcję, Blob input binding czyta dane.

---

Question: Chcesz zapisać wynik funkcji do kilku miejsc. Co jest prawdą?

- [x] Funkcja może mieć wiele output bindings.
- [ ] Funkcja może mieć tylko jeden output binding.
- [ ] Output binding zawsze musi być HTTP.
- [ ] Output binding zastępuje trigger.

Answer: Funkcja może mieć wiele bindings, ale tylko jeden trigger.

---

Question: Kiedy lepiej użyć SDK zamiast bindingu?

- [x] Gdy logika dostępu jest złożona i binding nie daje wystarczającej kontroli.
- [ ] Gdy funkcja ma Timer trigger.
- [ ] Zawsze, bo bindings nie są wspierane.
- [ ] Nigdy, bo SDK jest zabronione w Functions.

Answer: Bindings są wygodne, ale SDK daje większą kontrolę.

---

Question: Jak projektować functions obsługujące messages/events?

- [x] Idempotentnie, zakładając at-least-once delivery.
- [ ] Zakładając exactly-once delivery zawsze.
- [ ] Z globalnym stanem w pamięci.
- [ ] Bez retry i bez correlation id.

Answer: Event/message processing może wykonać się więcej niż raz, więc funkcje powinny być idempotentne.

---

Question: Co jest dobrą praktyką przy sekretach dla bindings?

- [ ] Wpisać connection string bezpośrednio w `function.json`.
- [x] W `function.json` podać nazwę app setting, a sekret trzymać w Application settings/Key Vault.
- [ ] Commitować `local.settings.json`.
- [ ] Wpisać secret w nazwę kolejki.

Answer: Binding `connection` wskazuje nazwę settingu, nie powinien zawierać sekretu.

---

Question: Co najlepiej wybrać, jeśli Function App ma czytać Key Vault bez sekretu?

- [x] Managed identity + RBAC.
- [ ] Function key.
- [ ] Timer trigger.
- [ ] Publish profile.

Answer: Managed identity pozwala uwierzytelnić aplikację do Azure resources bez client secret.

---

Question: Gdzie skonfigurujesz logging settings dla Functions?

- [x] `host.json`.
- [ ] `function.json` zawsze.
- [ ] DNS zone.
- [ ] Deployment slot route.

Answer: Logging runtime i Application Insights sampling konfiguruje się w `host.json`.

---

Question: Jaki jest najważniejszy monitoring tool dla Azure Functions?

- [x] Application Insights.
- [ ] Azure DNS.
- [ ] App Service Managed Certificate.
- [ ] Azure Policy only.

Answer: Application Insights zbiera requests, traces, exceptions, dependencies i metrics.

---

Question: Który deployment approach często ustawia aplikację jako read-only package?

- [x] Run from package / `WEBSITE_RUN_FROM_PACKAGE`.
- [ ] ARR Affinity.
- [ ] CORS.
- [ ] `WEBSITE_TIME_ZONE`.

Answer: Run from package uruchamia Function App z paczki zip.

---

## Źródła

- Microsoft Learn - Azure Functions overview: https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview
- Microsoft Learn - Triggers and bindings: https://learn.microsoft.com/en-us/azure/azure-functions/functions-triggers-bindings
- Microsoft Learn - Hosting options: https://learn.microsoft.com/en-us/azure/azure-functions/functions-scale
- Microsoft Learn - HTTP trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-trigger
- Microsoft Learn - Timer trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer
- Microsoft Learn - Queue trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue-trigger
- Microsoft Learn - Blob trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob-trigger
- Microsoft Learn - Cosmos DB trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-cosmosdb-v2-trigger
- Microsoft Learn - Event Grid trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-event-grid-trigger
- Microsoft Learn - Service Bus trigger: https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus-trigger
- GitHub - arvigeus/AZ-204 Learning Path/Functions.md: https://github.com/arvigeus/AZ-204/blob/master/Learning%20Path/Functions.md
- GitHub - arvigeus/AZ-204 Questions/Functions.md: https://github.com/arvigeus/AZ-204/blob/master/Questions/Functions.md
- Reddit / AzureCertification discussions about AZ-204 hands-on and scenario question style: https://www.reddit.com/r/AzureCertification/
