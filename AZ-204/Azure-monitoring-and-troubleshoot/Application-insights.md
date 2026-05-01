# Azure Application Insights - notatki AZ-204

## 1. Miejsce w AZ-204

Application Insights jest częścią Azure Monitor i jest głównym narzędziem APM dla aplikacji w Azure. W AZ-204 temat pojawia się w obszarze **Monitorowanie, rozwiązywanie problemów i optymalizacja rozwiązań Azure**, który według aktualnego study guide zajmuje **5-10% egzaminu**. Egzamin AZ-204 ma zostać wycofany **31 lipca 2026**, więc do tego czasu warto trzymać się aktualnych materiałów Microsoft Learn.

Na egzaminie kojarz przede wszystkim:

- konfigurowanie aplikacji i usług do użycia Application Insights,
- metryki, logi, traces i KQL,
- alerts i availability tests,
- telemetry: requests, dependencies, exceptions, traces, custom events, custom metrics,
- sampling i jego wpływ na dane,
- Application Map, Live Metrics i transaction search,
- instrumentację przez connection string, SDK, auto-instrumentation albo OpenTelemetry.

## 2. Czym jest Application Insights

Application Insights zbiera telemetry z aplikacji i pozwala analizować jej zachowanie: wydajność, błędy, zależności, użycie i dostępność. Dane trafiają do Azure Monitor / Log Analytics i można je analizować przez portal, metryki, alerty oraz zapytania KQL.

Typowe pytanie egzaminacyjne nie brzmi "co to jest monitoring", tylko raczej:

- użytkownicy widzą sporadyczne błędy - gdzie sprawdzisz wyjątki i failed requests,
- API działa wolno - jak znaleźć wolne dependencies,
- chcesz wykrywać awarię endpointu z kilku regionów - jak skonfigurować availability test,
- chcesz ograniczyć koszt telemetry - co robi sampling,
- masz distributed application - jak zobaczyć przepływ requestu między komponentami.

Najważniejszy podział:

- **Azure Monitor Metrics** - liczbowe szeregi czasowe, dobre do szybkich wykresów i alertów.
- **Azure Monitor Logs / Log Analytics** - szczegółowe dane telemetryczne analizowane KQL.
- **Application Insights** - APM layer dla aplikacji, z widokami typu Failures, Performance, Application Map, Live Metrics i Usage.

## 3. Workspace-based Application Insights

Obecnie zalecanym modelem jest **workspace-based Application Insights**, czyli zasób Application Insights zapisujący dane w Log Analytics workspace. Starszy model classic Application Insights istnieje, ale w nowych scenariuszach warto kojarzyć workspace-based.

Co daje workspace-based:

- KQL w Log Analytics,
- integracja z Azure Monitor Logs,
- łatwiejsze zarządzanie retention i access control,
- wspólne miejsce na logi aplikacji, platformy i infrastruktury,
- możliwość używania Azure Monitor features na danych aplikacyjnych.

Na egzaminie:

- jeśli pytanie mówi o zapytaniach KQL, zwykle chodzi o Logs / Log Analytics,
- jeśli pytanie mówi o szybkim alarmie na liczbie błędów albo czasie odpowiedzi, często wystarczy metric alert,
- jeśli pytanie mówi o analizie konkretnych requestów, dependencies i exceptions, szukaj w Application Insights Logs / Transaction Search.

## 4. Instrumentacja aplikacji

### Auto-instrumentation

Auto-instrumentation dodaje telemetry bez zmian albo z minimalnymi zmianami w kodzie. Jest wygodne dla typowych aplikacji hostowanych w Azure, np. App Service.

Dobre, gdy:

- chcesz szybko włączyć monitoring istniejącej aplikacji,
- nie chcesz zmieniać kodu,
- wystarczą standardowe requests, dependencies, exceptions i performance counters.

### Manual instrumentation

Manual instrumentation oznacza użycie SDK albo OpenTelemetry w kodzie. Daje kontrolę nad custom events, custom metrics, custom dependencies i właściwościami telemetry.

Dobre, gdy:

- potrzebujesz własnych business events,
- chcesz dodać correlation id, tenant id, order id itp.,
- chcesz mierzyć konkretny fragment kodu,
- auto-instrumentation nie obsługuje danego scenariusza.

### OpenTelemetry

Microsoft coraz mocniej promuje Azure Monitor OpenTelemetry Distro. W pytaniach egzaminacyjnych możesz zobaczyć zarówno starsze SDK Application Insights, jak i OpenTelemetry.

Kojarz:

- OpenTelemetry używa pojęć takich jak traces, metrics, logs, spans, service name,
- dane można wysyłać do Azure Monitor przez Azure Monitor OpenTelemetry Distro/exporter,
- dla nowych rozwiązań OpenTelemetry jest kierunkiem strategicznym,
- starsze Application Insights SDK nadal pojawia się w materiałach i pytaniach.

### ASP.NET Core - typowy przykład

Pakiet:

```bash
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

Minimalna konfiguracja:

```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

Connection string w konfiguracji:

```json
{
  "ApplicationInsights": {
    "ConnectionString": "InstrumentationKey=...;IngestionEndpoint=https://..."
  }
}
```

W App Service typowo ustawisz app setting:

```text
APPLICATIONINSIGHTS_CONNECTION_STRING=InstrumentationKey=...;IngestionEndpoint=https://...
```

### Instrumentation key vs connection string

Instrumentation key był starszym sposobem wskazywania zasobu Application Insights. Microsoft zakończył wsparcie dla ingestion opartego tylko o instrumentation key **31 marca 2025**. Nadal może działać, ale nie dostaje dalszych aktualizacji i poprawek. W nowych konfiguracjach używaj **connection string**.

Na egzaminie:

- preferuj `APPLICATIONINSIGHTS_CONNECTION_STRING`,
- `APPINSIGHTS_INSTRUMENTATIONKEY` / instrumentation key traktuj jako legacy,
- connection string może zawierać ingestion endpoint i jest lepszy dla sovereign clouds / regional endpoints.

## 5. Typy telemetry

Najczęstsze typy danych:

| Typ | Co oznacza | Typowa tabela |
| --- | --- | --- |
| Request | Request HTTP do aplikacji | `requests` / `AppRequests` |
| Dependency | Wywołanie zewnętrzne, np. SQL, HTTP, Storage | `dependencies` / `AppDependencies` |
| Exception | Wyjątek aplikacji | `exceptions` / `AppExceptions` |
| Trace | Log aplikacyjny | `traces` / `AppTraces` |
| Custom event | Zdarzenie biznesowe | `customEvents` / `AppEvents` |
| Custom metric | Własna metryka liczbowa | `customMetrics` / `AppMetrics` |
| Page view | Widok strony w aplikacji frontendowej | `pageViews` / `AppPageViews` |
| Availability | Wynik availability test | `availabilityResults` / `AppAvailabilityResults` |

Nazwy tabel zależą od kontekstu: w klasycznym widoku Application Insights często spotkasz krótsze nazwy, np. `requests`, a w workspace-based Log Analytics często nazwy z prefiksem `App`, np. `AppRequests`.

## 6. Requests, dependencies i exceptions

### Requests

Request opisuje wejście do aplikacji, np. wywołanie endpointu HTTP.

Typowe pola:

- `timestamp` / `TimeGenerated`,
- `name`,
- `url`,
- `resultCode`,
- `success`,
- `duration`,
- `operation_Id`.

Przykład KQL:

```kql
requests
| where timestamp > ago(1h)
| where success == false
| summarize failedRequests = count() by resultCode
| order by failedRequests desc
```

### Dependencies

Dependency opisuje wywołania wychodzące z aplikacji, np. SQL, HTTP, Azure Storage, Service Bus albo Cosmos DB.

Przykład KQL:

```kql
dependencies
| where timestamp > ago(1h)
| summarize avgDuration = avg(duration), failures = countif(success == false) by type, target
| order by avgDuration desc
```

### Exceptions

Exceptions pomagają znaleźć błędy w kodzie i powiązać je z konkretnym requestem.

Przykład KQL:

```kql
exceptions
| where timestamp > ago(24h)
| summarize count() by type, outerMessage
| order by count_ desc
```

## 7. Correlation i distributed tracing

Application Insights łączy requesty, dependencies, traces i exceptions przez correlation context.

Najważniejsze pola:

- `operation_Id` - wspólny identyfikator operacji / transakcji,
- `operation_ParentId` - rodzic danego elementu telemetry,
- `id` - identyfikator konkretnego requestu / dependency,
- `cloud_RoleName` - nazwa komponentu / usługi,
- `cloud_RoleInstance` - instancja komponentu.

Przykład: znajdź telemetry dla jednej transakcji.

```kql
union requests, dependencies, exceptions, traces
| where operation_Id == "<operation-id>"
| order by timestamp asc
```

Na egzaminie:

- Application Map i Transaction Search bazują na poprawnie skorelowanej telemetry,
- przy wielu mikroserwisach ustaw sensowny `cloud_RoleName` albo OpenTelemetry `service.name`,
- bez correlation trudniej połączyć frontend, API, dependencies i exceptions w jeden przepływ.

## 8. Application Map

Application Map pokazuje topologię aplikacji: komponenty, zależności, failed calls i latency. Jest szczególnie przydatna przy distributed applications.

Co zobaczysz:

- usługi / role aplikacji,
- wywołania HTTP między komponentami,
- dependencies do baz danych i usług,
- failure rate,
- średni czas odpowiedzi,
- hot spots problemów.

Na egzaminie:

- chcesz zobaczyć zależności między komponentami - **Application Map**,
- chcesz znaleźć konkretny request i powiązane dependencies - **Transaction Search / End-to-end transaction details**,
- chcesz realtime telemetry podczas deploymentu - **Live Metrics**.

## 9. Live Metrics

Live Metrics Stream pokazuje prawie na żywo:

- incoming requests,
- failed requests,
- dependency calls,
- exceptions,
- CPU/memory,
- sample telemetry.

Dobre zastosowania:

- sprawdzenie, czy aplikacja wysyła telemetry,
- obserwacja aplikacji zaraz po deployment,
- szybka diagnoza, czy błędy nadal występują.

Live Metrics nie zastępuje KQL i długoterminowych logów. To widok operacyjny "teraz", a nie pełna historia.

## 10. Metrics: standard vs log-based

W Application Insights spotkasz dwa typy metryk.

### Standard metrics / pre-aggregated metrics

Są wstępnie agregowane i nadają się do dashboardów oraz alertów. Przykłady:

- Server requests,
- Failed requests,
- Server response time,
- Dependency calls,
- Exceptions.

Zalety:

- szybkie,
- dobre do alertów,
- mniejszy koszt zapytań niż pełne logi.

### Log-based metrics

Są wyliczane z telemetry w Logs. Dają więcej szczegółów, ale zależą od zebranych danych i mogą być dotknięte samplingiem.

Na egzaminie:

- alert na prosty próg, np. failed requests > X - metric alert,
- analiza przyczyn i filtrowanie po polach - Logs/KQL,
- szczegółowy drill-down do requestów - Application Insights Logs / Transaction Search.

## 11. Custom telemetry

### Custom events

Używaj do zdarzeń biznesowych, np. `OrderSubmitted`, `CheckoutStarted`, `PaymentFailed`.

```csharp
telemetryClient.TrackEvent("OrderSubmitted", new Dictionary<string, string>
{
    ["tenantId"] = tenantId,
    ["paymentMethod"] = "Card"
});
```

### Custom metrics

Używaj do wartości liczbowych. W .NET preferuj agregowane metryki przez `GetMetric`, gdy wysyłasz często powtarzaną wartość.

```csharp
telemetryClient.GetMetric("QueueDepth").TrackValue(queueDepth);
```

### TrackException

Używaj do wyjątków, które złapałeś samodzielnie i chcesz jawnie wysłać.

```csharp
telemetryClient.TrackException(ex);
```

### TrackDependency

Używaj, gdy automatyczne śledzenie dependencies nie obejmuje Twojego scenariusza.

### Flush

`Flush()` wymusza wysłanie buforowanej telemetry, ale nie powinien być wywoływany po każdym request. Typowe użycie: shutdown aplikacji albo krótko żyjący proces konsolowy.

## 12. Sampling

Sampling ogranicza ilość wysyłanej telemetry. Pomaga zmniejszyć koszt i wolumen danych, ale może sprawić, że nie zobaczysz każdego pojedynczego eventu.

Typy:

- **Adaptive sampling** - SDK dynamicznie dopasowuje próbkowanie do wolumenu telemetry.
- **Fixed-rate sampling** - stały procent telemetry, np. 10%.
- **Ingestion sampling** - próbkowanie po stronie usługi, po dotarciu danych do Azure Monitor.

Ważne:

- SDK sampling działa przed wysłaniem danych do Azure,
- ingestion sampling działa dopiero po dotarciu danych do usługi,
- powiązana telemetry powinna być próbkowana spójnie, żeby zachować correlation,
- log-based metrics i KQL mogą być dotknięte samplingiem,
- standard/pre-aggregated metrics są zwykle lepsze do alertów na podstawowe wskaźniki.

Na egzaminie:

- chcesz obniżyć koszt bez całkowitego wyłączenia telemetry - sampling,
- chcesz mieć każdą pojedynczą transakcję do audytu - sampling może być problemem,
- po zmianie sampling w konfiguracji App Service aplikacja może wymagać restartu.

## 13. Availability tests

Availability tests sprawdzają, czy endpoint aplikacji jest dostępny z zewnętrznych lokalizacji.

Typy, które warto kojarzyć:

- **Standard test** - zalecany prosty test HTTP/HTTPS endpointu.
- **Custom TrackAvailability** - własny test dostępności wykonywany przez kod, np. z Azure Function.
- **Classic URL ping test** - starszy typ, wycofywany; ma zostać usunięty **30 września 2026**.
- **Multi-step web test** - starszy typ, wycofany **31 sierpnia 2024**.

Co można ustawić w standard availability test:

- URL,
- częstotliwość,
- lokalizacje testowe,
- timeout,
- HTTP method,
- expected status code,
- request headers,
- SSL certificate validation,
- alert rule.

Na egzaminie:

- endpoint musi być publicznie dostępny dla testów platformy,
- jeśli aplikacja ma firewall/access restrictions, dopuść ruch z lokalizacji testowych albo użyj service tag `ApplicationInsightsAvailability`,
- dla złożonych scenariuszy logowania albo kilku kroków użyj custom availability test i `TrackAvailability`,
- availability test może automatycznie tworzyć alert.

## 14. Alerts

Najczęstsze typy alertów:

- **Metric alert** - szybki alert na metrykę, np. Failed requests, Server response time.
- **Log alert** - alert na wynik zapytania KQL.
- **Availability alert** - alert tworzony wokół availability test.

Action group definiuje, co się stanie po alercie:

- email,
- SMS,
- webhook,
- Azure Function,
- Logic App,
- ITSM,
- push notification.

Na egzaminie:

- chcesz powiadomić zespół, gdy endpoint padnie - availability test + alert + action group,
- chcesz alertować na zapytanie KQL, np. konkretny wyjątek - log alert,
- chcesz alertować na podstawową metrykę - metric alert.

## 15. Snapshot Debugger i Profiler

### Profiler

Profiler pomaga analizować wydajność aplikacji i znajdować wolne ścieżki wykonania. Kojarz go z performance investigation, a nie z prostym logowaniem błędów.

### Snapshot Debugger

Snapshot Debugger pozwala zebrać snapshot stanu aplikacji przy wyjątku w produkcji bez klasycznego remote debugging. Na egzaminie może pojawić się jako narzędzie do diagnozowania wyjątków przy minimalnym wpływie na produkcję.

W praktyce te funkcje mają ograniczenia platformowe i runtime'owe, więc na egzaminie najczęściej wystarczy znać różnicę:

- **Profiler** - performance bottlenecks,
- **Snapshot Debugger** - stan aplikacji przy exceptions,
- **Live Metrics** - realtime monitoring,
- **Logs/KQL** - analiza historyczna.

## 16. Usage analysis

Application Insights ma też widoki analizy użycia.

Najważniejsze:

- **Users** - liczba unikalnych użytkowników / przeglądarek / urządzeń według telemetry frontendowej.
- **Sessions** - sesje użytkowników; typowo kończą się po czasie bezczynności albo bardzo długim czasie trwania.
- **Events** - page views i custom events.
- **Funnels** - liniowy lejek, np. `ViewedProduct -> AddedToCart -> Checkout`.
- **User flows** - ścieżki użytkowników między stronami i eventami.
- **Cohorts** - dynamiczne grupy użytkowników / sesji / eventów spełniające warunki.
- **Impact** - co wpływa na konwersję albo wydajność.
- **Retention** - czy użytkownicy wracają po wykonaniu akcji.

Na egzaminie:

- chcesz sprawdzić, gdzie użytkownicy odpadają w procesie - **Funnels**,
- chcesz zobaczyć ścieżki użytkowników - **User flows**,
- chcesz porównać grupę użytkowników spełniającą warunki - **Cohorts**,
- chcesz sprawdzić powracających użytkowników - **Retention**.

## 17. KQL - szybkie wzorce

### Failed requests

```kql
requests
| where timestamp > ago(24h)
| where success == false
| summarize count() by resultCode, name
| order by count_ desc
```

### Slow requests

```kql
requests
| where timestamp > ago(24h)
| summarize p95 = percentile(duration, 95), avgDuration = avg(duration) by name
| order by p95 desc
```

### Slow dependencies

```kql
dependencies
| where timestamp > ago(24h)
| summarize p95 = percentile(duration, 95), failures = countif(success == false) by type, target
| order by p95 desc
```

### Exceptions connected with requests

```kql
exceptions
| join kind=leftouter requests on operation_Id
| project timestamp, type, outerMessage, requestName = name, resultCode
| order by timestamp desc
```

### Availability failures

```kql
availabilityResults
| where timestamp > ago(24h)
| where success == false
| summarize failures = count() by name, location
| order by failures desc
```

### Workspace-based table example

```kql
AppRequests
| where TimeGenerated > ago(24h)
| where Success == false
| summarize FailedRequests = count() by ResultCode, Name
| order by FailedRequests desc
```

## 18. Application Insights w Azure Functions

Azure Functions integruje się z Application Insights bardzo często automatycznie, ale konfiguracja nadal ma znaczenie.

Ważne:

- Function App wysyła logs, requests, dependencies i exceptions do Application Insights,
- logging i sampling można konfigurować w `host.json`,
- connection string powinien być w Application settings,
- telemetry Functions pomaga diagnozować trigger failures, retries, exceptions i execution time.

Przykład `host.json`:

```json
{
  "logging": {
    "applicationInsights": {
      "samplingSettings": {
        "isEnabled": true,
        "excludedTypes": "Request"
      }
    }
  }
}
```

Na egzaminie:

- chcesz monitorować Function App - Application Insights,
- chcesz zmienić poziom logowania runtime Functions - `host.json`,
- chcesz trzymać connection string w Azure - Application settings,
- chcesz analizować wykonania funkcji - Application Insights Logs / Monitor tab.

## 19. Pułapki egzaminacyjne

- Application Insights nie jest tym samym co Log Analytics, ale może używać Log Analytics workspace.
- Metrics są dobre do alertów, Logs/KQL do szczegółowej analizy.
- Availability test sprawdza dostępność endpointu z zewnątrz, a nie health każdej instancji App Service.
- Health Check w App Service usuwa instancję z load balancer rotation; availability test tylko monitoruje endpoint i alertuje.
- Sampling obniża wolumen danych, ale może utrudnić audyt każdej pojedynczej transakcji.
- Connection string jest preferowany względem instrumentation key.
- Application Map wymaga dobrze skorelowanej telemetry i rozróżnienia ról aplikacji.
- Live Metrics jest prawie realtime, ale nie zastępuje historycznych logów.
- URL ping test to starszy availability test i ma datę wycofania 30 września 2026.
- Multi-step web tests są już wycofane; dla złożonych scenariuszy użyj custom availability test.
- Custom events są do zdarzeń biznesowych, custom metrics do wartości liczbowych.
- `Flush()` nie jest metodą do wywoływania po każdym request.

## 20. Porównanie z repo arvigeus/AZ-204

W pliku `Topics/Application Insights.md` z repo arvigeus są dobre podstawy: Application Insights jako APM, telemetry types, sampling, availability tests, Application Map, usage tools, alerts i instrumentacja. Do tych notatek dopisałem i uporządkowałem rzeczy, które są ważne na 2026:

- aktualny udział monitoringu w AZ-204: 5-10%,
- data wycofania AZ-204: 31 lipca 2026,
- preferowanie connection string zamiast instrumentation key,
- zakończenie wsparcia dla instrumentation key ingestion 31 marca 2025,
- wycofanie classic URL ping tests 30 września 2026,
- wycofanie multi-step web tests 31 sierpnia 2024,
- workspace-based Application Insights i tabele `AppRequests`, `AppDependencies` itd.,
- większy nacisk na OpenTelemetry,
- rozróżnienie Health Check App Service vs Availability tests,
- dodatkowe wzorce KQL i pytania scenariuszowe.

## 21. Najczęstsze pytania egzaminacyjne - szybka powtórka

- Chcesz monitorować aplikację .NET w App Service bez dużych zmian w kodzie: auto-instrumentation albo `AddApplicationInsightsTelemetry`.
- Chcesz wskazać zasób Application Insights w aplikacji: `APPLICATIONINSIGHTS_CONNECTION_STRING`.
- Chcesz zobaczyć błędy requestów: Failures albo `requests | where success == false`.
- Chcesz znaleźć wolne wywołania SQL/HTTP: `dependencies`.
- Chcesz prześledzić jedną transakcję przez kilka usług: `operation_Id`, Transaction Search, Application Map.
- Chcesz realtime podgląd po deployment: Live Metrics.
- Chcesz alert, gdy endpoint jest niedostępny: Standard availability test + alert + action group.
- Chcesz złożony test logowania albo kilku kroków: custom availability test z `TrackAvailability`.
- Chcesz zmniejszyć koszt telemetry: sampling.
- Chcesz mieć każde zdarzenie audytowe: uważaj na sampling.
- Chcesz mierzyć zdarzenie biznesowe: custom event.
- Chcesz mierzyć wartość liczbową: custom metric.
- Chcesz analizować ścieżkę użytkownika: User flows.
- Chcesz mierzyć konwersję krok po kroku: Funnels.
- Chcesz znaleźć performance bottleneck w kodzie: Profiler.
- Chcesz stan aplikacji przy wyjątku: Snapshot Debugger.

## 22. Źródła

- Microsoft Learn - AZ-204 study guide: https://learn.microsoft.com/en-us/credentials/certifications/resources/study-guides/az-204
- Microsoft Learn - Application Insights overview: https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview
- Microsoft Learn - Workspace-based Application Insights: https://learn.microsoft.com/en-us/azure/azure-monitor/app/create-workspace-resource
- Microsoft Learn - Connection strings: https://learn.microsoft.com/en-us/azure/azure-monitor/app/connection-strings
- Microsoft Learn - Application Insights for ASP.NET Core: https://learn.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core
- Microsoft Learn - Azure Monitor OpenTelemetry: https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-enable
- Microsoft Learn - Application Insights data model: https://learn.microsoft.com/en-us/azure/azure-monitor/app/data-model-complete
- Microsoft Learn - Application Insights sampling: https://learn.microsoft.com/en-us/azure/azure-monitor/app/sampling-classic-api
- Microsoft Learn - Availability tests: https://learn.microsoft.com/en-us/azure/azure-monitor/app/availability-overview
- Microsoft Learn - Availability standard tests: https://learn.microsoft.com/en-us/azure/azure-monitor/app/availability-standard-tests
- Microsoft Learn - Application Map: https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-map
- Microsoft Learn - Live Metrics: https://learn.microsoft.com/en-us/azure/azure-monitor/app/live-stream
- Microsoft Learn - Usage analysis: https://learn.microsoft.com/en-us/azure/azure-monitor/app/usage-overview
- Microsoft Learn - Azure Functions monitoring with Application Insights: https://learn.microsoft.com/en-us/azure/azure-functions/functions-monitoring
- GitHub - arvigeus/AZ-204 Topics/Application Insights.md: https://github.com/arvigeus/AZ-204/blob/master/Topics/Application%20Insights.md
- GitHub - arvigeus/AZ-204 Questions/Application Insights.md: https://github.com/arvigeus/AZ-204/blob/master/Questions/Application%20Insights.md
- Reddit / AzureCertification discussions about AZ-204 scenario style and monitoring topics: https://www.reddit.com/r/AzureCertification/
