# Azure Application Insights - pytania AZ-204

Pytania są przygotowane pod styl AZ-204: scenariusze, wybór właściwego narzędzia i pułapki typu sampling, connection string, availability tests, KQL, Application Map i różnica między metrics a logs. Porównałem zakres z `arvigeus/AZ-204` i uzupełniłem go o aktualne informacje Microsoft Learn oraz typowe wątki z forów AZ-204.

---

Question: Chcesz skonfigurować nową aplikację do wysyłania telemetry do Application Insights. Które ustawienie jest obecnie zalecane?

- [x] `APPLICATIONINSIGHTS_CONNECTION_STRING`
- [ ] `APPINSIGHTS_INSTRUMENTATIONKEY` jako jedyne ustawienie.
- [ ] `AzureWebJobsStorage`
- [ ] `WEBSITE_RUN_FROM_PACKAGE`

Answer: Connection string jest zalecanym sposobem wskazywania zasobu Application Insights. Instrumentation key jest legacy.

---

Question: Dlaczego connection string jest lepszym wyborem niż sam instrumentation key?

- [x] Może zawierać ingestion endpoint.
- [x] Lepiej wspiera sovereign clouds i regional endpoints.
- [ ] Automatycznie tworzy Log Analytics workspace.
- [ ] Wyłącza sampling.

Answer: Connection string może zawierać więcej informacji niż sam instrumentation key, np. endpointy. Nie tworzy automatycznie workspace i nie wyłącza sampling.

---

Question: Microsoft zakończył wsparcie dla ingestion opartego tylko o instrumentation key. Która data jest poprawna?

- [x] 31 marca 2025.
- [ ] 31 sierpnia 2024.
- [ ] 30 września 2026.
- [ ] 31 lipca 2026.

Answer: Wsparcie dla instrumentation key ingestion zakończyło się 31 marca 2025. Dane mogą nadal działać, ale zalecany jest connection string.

---

Question: Chcesz szybko dodać monitoring do istniejącej aplikacji App Service bez zmian w kodzie. Co wybierzesz w pierwszej kolejności?

- [x] Auto-instrumentation Application Insights.
- [ ] Snapshot Debugger.
- [ ] Azure Traffic Manager.
- [ ] Storage Queue trigger.

Answer: Auto-instrumentation jest właściwe, gdy chcesz dodać standardową telemetry bez zmian albo z minimalnymi zmianami w kodzie.

---

Question: Chcesz wysłać zdarzenie biznesowe `OrderSubmitted` z dodatkowymi właściwościami, np. `tenantId`. Który typ telemetry wybierzesz?

- [ ] Dependency.
- [x] Custom event.
- [ ] Availability result.
- [ ] Performance counter.

Answer: Custom events służą do zdarzeń biznesowych i analizy usage.

---

Question: Chcesz mierzyć liczbową wartość `QueueDepth`. Który typ telemetry pasuje najlepiej?

- [ ] Trace.
- [ ] Page view.
- [x] Custom metric.
- [ ] Request.

Answer: Custom metric służy do wartości liczbowych. W .NET warto używać `GetMetric`, gdy metryka jest wysyłana często.

---

Question: Aplikacja wykonuje wolne zapytania do SQL Database. Gdzie najpierw szukać tych wywołań w Application Insights?

- [ ] `requests`
- [x] `dependencies`
- [ ] `availabilityResults`
- [ ] `pageViews`

Answer: Wywołania do zewnętrznych usług, np. SQL albo HTTP, są dependency telemetry.

---

Question: Który typ telemetry opisuje request przychodzący do API?

- [x] Request.
- [ ] Dependency.
- [ ] Custom metric.
- [ ] Availability result.

Answer: Request telemetry opisuje wejście do aplikacji, np. HTTP request do endpointu API.

---

Question: Chcesz znaleźć wyjątki aplikacji z ostatnich 24 godzin. Które zapytanie jest najlepszym punktem startowym?

- [ ] `dependencies | where success == false`
- [x] `exceptions | where timestamp > ago(24h)`
- [ ] `availabilityResults | where success == true`
- [ ] `customMetrics | where value > 0`

Answer: Exceptions są zapisywane w tabeli `exceptions` albo `AppExceptions` w workspace-based Application Insights.

---

Question: W workspace-based Application Insights widzisz tabele z prefiksem `App`. Która tabela odpowiada requestom?

- [x] `AppRequests`
- [ ] `AzureRequests`
- [ ] `InsightsHttp`
- [ ] `MonitorRequests`

Answer: W Log Analytics dla workspace-based Application Insights często używa się tabel typu `AppRequests`, `AppDependencies`, `AppExceptions`.

---

Question: Chcesz prześledzić jedną transakcję przez request, dependencies, traces i exceptions. Które pole jest najważniejsze?

- [x] `operation_Id`
- [ ] `resultCode`
- [ ] `itemCount`
- [ ] `client_City`

Answer: `operation_Id` łączy telemetry należącą do tej samej operacji lub transakcji.

---

Question: W mikroserwisach kilka komponentów raportuje telemetry do jednego Application Insights. Co pomaga rozróżnić komponenty w Application Map?

- [x] `cloud_RoleName`
- [ ] `resultCode`
- [ ] `duration`
- [ ] `User-Agent`

Answer: `cloud_RoleName` identyfikuje rolę/komponent aplikacji. W OpenTelemetry podobną rolę pełni `service.name`.

---

Question: Chcesz zobaczyć graficzną mapę komponentów aplikacji, zależności i failure rate. Którego widoku użyjesz?

- [x] Application Map.
- [ ] Funnels.
- [ ] Retention.
- [ ] Access restrictions.

Answer: Application Map pokazuje topologię aplikacji, dependencies, błędy i opóźnienia między komponentami.

---

Question: Chcesz zobaczyć telemetry prawie w czasie rzeczywistym po deployment. Co wybierzesz?

- [x] Live Metrics.
- [ ] Retention.
- [ ] Snapshot Debugger.
- [ ] Azure Policy.

Answer: Live Metrics Stream pokazuje bieżące requesty, failures, dependencies i sample telemetry.

---

Question: Chcesz analizować historyczne dane telemetry i filtrować po własnych polach. Co wybierzesz?

- [ ] Live Metrics.
- [x] Logs / KQL.
- [ ] App Service TLS binding.
- [ ] Deployment Center.

Answer: Logs i KQL są najlepsze do szczegółowej, historycznej analizy telemetry.

---

Question: Chcesz stworzyć szybki alert, gdy liczba failed requests przekroczy próg. Jaki typ alertu zwykle wystarczy?

- [x] Metric alert.
- [ ] DNS alert.
- [ ] ARM template validation alert.
- [ ] User flow alert.

Answer: Failed requests są dostępne jako metryka, więc metric alert jest prostym i właściwym wyborem.

---

Question: Chcesz alertować na wystąpienie konkretnego typu wyjątku według zapytania KQL. Co wybierzesz?

- [ ] Metric alert na CPU.
- [x] Log alert.
- [ ] Deployment slot swap.
- [ ] App Service certificate.

Answer: Gdy warunek jest wyrażony zapytaniem KQL, użyj log alert.

---

Question: Co definiuje odbiorców i akcje wykonywane po wyzwoleniu alertu?

- [ ] Application Map.
- [x] Action group.
- [ ] Sampling rule.
- [ ] Deployment slot.

Answer: Action group określa akcje, np. email, webhook, Azure Function albo Logic App.

---

Question: Chcesz monitorować publiczny endpoint API z kilku lokalizacji i dostać alert, gdy jest niedostępny. Co wybierzesz?

- [x] Standard availability test + alert.
- [ ] Snapshot Debugger.
- [ ] Application Map bez testu.
- [ ] Custom metric bez harmonogramu.

Answer: Standard availability test sprawdza endpoint z zewnętrznych lokalizacji i może tworzyć alerty.

---

Question: Classic URL ping availability tests mają zaplanowaną datę wycofania. Która data jest poprawna?

- [ ] 31 marca 2025.
- [ ] 31 sierpnia 2024.
- [x] 30 września 2026.
- [ ] 1 stycznia 2030.

Answer: Classic URL ping tests mają zostać wycofane 30 września 2026. Zalecane są Standard tests albo custom TrackAvailability.

---

Question: Multi-step web tests w Application Insights zostały wycofane. Która data jest poprawna?

- [ ] 31 marca 2025.
- [x] 31 sierpnia 2024.
- [ ] 30 września 2026.
- [ ] 31 lipca 2026.

Answer: Multi-step web tests zostały wycofane 31 sierpnia 2024.

---

Question: Masz złożony test dostępności wymagający logowania i kilku kroków. Co jest najlepszą alternatywą dla multi-step web test?

- [x] Własny test w kodzie i `TrackAvailability`.
- [ ] Wyłączenie availability monitoring.
- [ ] Tylko Application Map.
- [ ] Sampling fixed-rate.

Answer: Dla złożonych scenariuszy użyj custom availability test i wyślij wynik przez `TrackAvailability`.

---

Question: Availability test nie może dotrzeć do aplikacji chronionej access restrictions. Co możesz zrobić?

- [x] Dopuścić ruch z lokalizacji testowych albo service tag `ApplicationInsightsAvailability`.
- [ ] Zmienić metric namespace na `AppRequests`.
- [ ] Wyłączyć Application Insights.
- [ ] Użyć `WEBSITE_RUN_FROM_PACKAGE`.

Answer: Test musi mieć dostęp do endpointu. Przy firewall/access restrictions trzeba dopuścić odpowiedni ruch albo użyć własnego testu.

---

Question: Jaka jest różnica między App Service Health Check a Application Insights availability test?

- [x] Health Check pomaga App Service wycofać z ruchu niezdrową instancję, a availability test monitoruje endpoint z zewnątrz i alertuje.
- [ ] Oba mechanizmy są dokładnie tym samym.
- [ ] Availability test automatycznie skaluje App Service Plan.
- [ ] Health Check wysyła custom events do Funnels.

Answer: Health Check działa na poziomie App Service i instancji, availability test jest zewnętrznym monitoringiem dostępności.

---

Question: Chcesz zmniejszyć koszt i wolumen telemetry bez całkowitego wyłączenia monitoringu. Co zastosujesz?

- [ ] Deployment slot.
- [x] Sampling.
- [ ] CNAME.
- [ ] Managed certificate.

Answer: Sampling ogranicza liczbę wysyłanych albo przyjmowanych elementów telemetry.

---

Question: Które stwierdzenie o sampling jest prawdziwe?

- [x] Może wpłynąć na wyniki zapytań KQL i log-based metrics.
- [ ] Zawsze zachowuje każdy pojedynczy event audytowy.
- [ ] Działa tylko dla availability tests.
- [ ] Tworzy automatycznie action group.

Answer: Sampling usuwa część telemetry, więc szczegółowe zapytania i log-based metrics mogą widzieć próbkę, a nie komplet danych.

---

Question: Czym różni się SDK sampling od ingestion sampling?

- [x] SDK sampling działa przed wysłaniem danych do Azure, ingestion sampling po stronie usługi.
- [ ] Ingestion sampling działa w kodzie aplikacji.
- [ ] SDK sampling działa tylko dla DNS.
- [ ] Nie ma żadnej różnicy.

Answer: SDK sampling ogranicza dane przed wysyłką, ingestion sampling działa dopiero po dotarciu telemetry do Azure Monitor.

---

Question: Chcesz mieć każdą pojedynczą transakcję do audytu. Co jest ryzykiem?

- [x] Sampling może usunąć część telemetry.
- [ ] Application Map zawsze usuwa dane.
- [ ] Connection string wyłącza exceptions.
- [ ] Live Metrics blokuje requesty.

Answer: Sampling jest dobry kosztowo, ale przy wymaganiu kompletnego audytu może być niewłaściwy.

---

Question: Które narzędzie służy do analizy performance bottlenecks w kodzie aplikacji?

- [ ] Availability test.
- [x] Profiler.
- [ ] User flows.
- [ ] Action group.

Answer: Profiler pomaga analizować wydajność i wolne ścieżki wykonania.

---

Question: Chcesz zebrać stan aplikacji przy wyjątku w produkcji bez klasycznego remote debugging. Co pasuje najlepiej?

- [ ] Funnels.
- [x] Snapshot Debugger.
- [ ] CORS.
- [ ] Deployment Center.

Answer: Snapshot Debugger jest narzędziem do snapshotów przy exceptions.

---

Question: Chcesz sprawdzić, na którym kroku procesu zakupowego użytkownicy odpadają. Co wybierzesz?

- [x] Funnels.
- [ ] Application Map.
- [ ] Dependencies.
- [ ] Snapshot Debugger.

Answer: Funnels pokazują konwersję między kolejnymi krokami procesu.

---

Question: Chcesz zobaczyć ścieżki użytkowników między stronami i zdarzeniami. Co wybierzesz?

- [ ] Profiler.
- [x] User flows.
- [ ] Action groups.
- [ ] Availability results.

Answer: User flows pokazują, jak użytkownicy przechodzą przez aplikację.

---

Question: Chcesz analizować powracających użytkowników po wykonaniu konkretnej akcji. Co wybierzesz?

- [ ] Application Map.
- [ ] Live Metrics.
- [x] Retention.
- [ ] Dependency tracking.

Answer: Retention pomaga analizować, czy użytkownicy wracają po wykonaniu akcji.

---

Question: Chcesz utworzyć dynamiczną grupę użytkowników spełniających warunki, aby potem analizować ich zachowanie. Co wybierzesz?

- [x] Cohorts.
- [ ] Health Check.
- [ ] TLS binding.
- [ ] Kudu.

Answer: Cohorts tworzą dynamiczne grupy użytkowników, sesji lub zdarzeń spełniające warunki.

---

Question: Które zapytanie KQL znajdzie failed requests w klasycznych tabelach Application Insights?

- [x] `requests | where success == false`
- [ ] `dependencies | where type == "SQL"`
- [ ] `exceptions | where success == true`
- [ ] `availabilityResults | where duration == 0`

Answer: Failed incoming requests znajdziesz w `requests` z `success == false`.

---

Question: Które zapytanie znajdzie wolne dependencies według percentyla 95?

- [ ] `requests | summarize count() by resultCode`
- [x] `dependencies | summarize p95 = percentile(duration, 95) by target`
- [ ] `traces | where message == "OK"`
- [ ] `customEvents | summarize users = dcount(resultCode)`

Answer: Wolne wywołania zewnętrzne analizuje się w `dependencies`, np. przez `percentile(duration, 95)`.

---

Question: Chcesz powiązać exceptions z requestami w KQL. Po czym najczęściej połączysz tabele?

- [ ] `resultCode`
- [x] `operation_Id`
- [ ] `client_CountryOrRegion`
- [ ] `severityLevel`

Answer: `operation_Id` jest wspólnym identyfikatorem korelacji dla telemetry z tej samej operacji.

---

Question: Function App ma wysyłać telemetry do Application Insights. Gdzie w Azure trzymasz connection string?

- [ ] W `function.json`.
- [x] W Application settings Function App.
- [ ] W nazwie triggera.
- [ ] W route template HTTP trigger.

Answer: W Azure connection string trzymasz w Application settings, jako zmienną środowiskową.

---

Question: Gdzie w Azure Functions najczęściej konfiguruje się logging i sampling Application Insights?

- [ ] `local.settings.json` tylko w Azure.
- [x] `host.json`.
- [ ] DNS zone.
- [ ] App Service Plan name.

Answer: Runtime Functions używa `host.json` do globalnych ustawień logowania, w tym Application Insights sampling.

---

Question: Chcesz monitorować execution time, failures i retries Azure Functions. Które narzędzie jest najbardziej naturalne?

- [x] Application Insights.
- [ ] Azure DNS.
- [ ] App Service Managed Certificate.
- [ ] CORS.

Answer: Application Insights jest standardowym narzędziem monitoringu Function App.

---

Question: Kiedy lepiej użyć manual instrumentation zamiast samej auto-instrumentation?

- [x] Gdy potrzebujesz własnych business events albo custom properties.
- [ ] Gdy chcesz tylko domyślne HTTP request telemetry.
- [ ] Gdy chcesz dodać CNAME do App Service.
- [ ] Gdy chcesz włączyć HTTPS Only.

Answer: Manual instrumentation daje kontrolę nad custom telemetry i dodatkowymi właściwościami.

---

Question: Które z poniższych są typowymi typami telemetry Application Insights?

- [x] Requests.
- [x] Dependencies.
- [x] Exceptions.
- [x] Traces.
- [ ] DNS zones.

Answer: Requests, dependencies, exceptions i traces są podstawowymi typami telemetry. DNS zones nie są typem telemetry Application Insights.

---

Question: Który scenariusz najlepiej pasuje do Transaction Search / end-to-end transaction details?

- [x] Chcesz przeanalizować jeden konkretny request i powiązane dependencies oraz exceptions.
- [ ] Chcesz kupić App Service Certificate.
- [ ] Chcesz ustawić root domain.
- [ ] Chcesz utworzyć Service Bus queue.

Answer: Transaction Search pomaga wejść w szczegóły jednej transakcji i powiązanej telemetry.

---

Question: Które stwierdzenie najlepiej opisuje różnicę między metrics i logs?

- [x] Metrics są dobre do szybkich liczbowych alertów, a logs/KQL do szczegółowej analizy.
- [ ] Logs są zawsze szybsze i tańsze niż metrics.
- [ ] Metrics zawierają pełny stack trace każdego wyjątku.
- [ ] Logs nie mogą być używane w alertach.

Answer: Metrics są liczbowe i dobre do alertów. Logs/KQL dają szczegółowe rekordy i elastyczne filtrowanie.

---

Question: Co robi `TelemetryClient.Flush()`?

- [x] Próbuje wysłać buforowaną telemetry.
- [ ] Tworzy availability test.
- [ ] Wyłącza sampling.
- [ ] Resetuje Log Analytics workspace.

Answer: `Flush()` wymusza wysyłkę bufora, ale nie powinien być wołany po każdym request.

---

Question: Chcesz sprawdzić, czy aplikacja w ogóle wysyła telemetry tuż po wdrożeniu. Co jest szybkim narzędziem w portalu?

- [x] Live Metrics.
- [ ] Retention.
- [ ] Key Vault access policy.
- [ ] Deployment slot route.

Answer: Live Metrics szybko pokazuje, czy requesty i telemetry dochodzą do Application Insights.

---

Question: Który typ testu dostępności jest zalecany dla prostego HTTP endpointu w nowych scenariuszach?

- [ ] Classic URL ping test.
- [x] Standard test.
- [ ] Multi-step web test.
- [ ] Storage lifecycle rule.

Answer: Standard test jest zalecanym typem availability test dla prostych endpointów HTTP/HTTPS.

---

Question: Który z poniższych testów dostępności jest zalecany w przypadku testów uwierzytelniania?

- [ ] Polecenie ping adresu URL.
- [ ] Standard.
- [x] Niestandardowy test `TrackAvailability`.

Answer: Niestandardowy test `TrackAvailability` jest zalecany dla scenariuszy wymagających uwierzytelniania, wielu requestów albo niestandardowej logiki testowej. URL ping jest starszym prostym testem endpointu, a Standard test jest dobry dla pojedynczego requestu HTTP/HTTPS.

---

Question: Które z poniższych typów kolekcji metryk udostępniają zapytania niemal w czasie rzeczywistym, alerty dotyczące wymiarów metryk i bardziej dynamiczne pulpity nawigacyjne?

- [ ] Oparte na dzienniku.
- [x] Wstępnie zagregowane.
- [ ] Azure Service Bus.

Answer: Wstępnie zagregowane metryki są przechowywane jako time series z kluczowymi wymiarami, dlatego obsługują szybsze zapytania, near real-time alerting na wymiarach i bardziej responsywne dashboardy. Metryki log-based są agregowane dopiero w czasie zapytania, a Azure Service Bus jest usługą messaging, nie typem kolekcji metryk Application Insights.

---

Question: Według aktualnego study guide, jaki udział ma obszar monitorowania i troubleshootingu w AZ-204?

- [ ] 25-30%.
- [ ] 15-20%.
- [x] 5-10%.
- [ ] 1-2%.

Answer: Aktualny study guide AZ-204 wskazuje 5-10% dla monitorowania, troubleshootingu i optymalizacji.

---

Question: Która data jest ważna dla samego egzaminu AZ-204 według aktualnego Microsoft Learn?

- [ ] 31 marca 2025.
- [ ] 30 września 2026.
- [x] 31 lipca 2026.
- [ ] 31 sierpnia 2024.

Answer: Microsoft Learn wskazuje, że AZ-204 ma zostać wycofany 31 lipca 2026.

---

Question: W Azure Functions widzisz, że nie wszystkie invocation logs trafiają do Application Insights. Która konfiguracja może być przyczyną?

- [x] Sampling w `host.json`.
- [ ] CNAME custom domain.
- [ ] TLS binding.
- [ ] ARR Affinity.

Answer: Application Insights sampling w Functions może odfiltrować część telemetry. Jeśli potrzebujesz pełnych invocation logs, sprawdź `logging.applicationInsights.samplingSettings` w `host.json`.

---

Question: Chcesz tymczasowo wyłączyć sampling dla requestów w Azure Functions, ale zostawić sampling dla innych typów telemetry. Które ustawienie jest najbliższe temu celowi?

- [x] `excludedTypes: "Request"` w `samplingSettings`.
- [ ] `WEBSITE_RUN_FROM_PACKAGE=1`.
- [ ] `FUNCTIONS_WORKER_RUNTIME=dotnet-isolated`.
- [ ] `x-ms-routing-name=self`.

Answer: `excludedTypes` pozwala wyłączyć sampling dla wskazanych typów telemetry, np. Request.

---

Question: Chcesz widzieć metryki i przykładową telemetry prawie w czasie rzeczywistym, bez pisania zapytań KQL. Co wybierzesz?

- [x] Live Metrics.
- [ ] Log alert.
- [ ] Retention.
- [ ] Cohorts.

Answer: Live Metrics pokazuje bieżące requesty, failed requests, dependencies, exceptions i sample telemetry prawie na żywo.

---

Question: Chcesz wysyłać telemetry z nowej aplikacji zgodnie z kierunkiem OpenTelemetry. Który komponent Azure jest najbardziej pasujący?

- [x] Azure Monitor OpenTelemetry Distro albo exporter.
- [ ] App Service Managed Certificate.
- [ ] Azure DNS private zone.
- [ ] Storage lifecycle policy.

Answer: Azure Monitor OpenTelemetry Distro/exporter pozwala wysyłać traces, metrics i logs do Azure Monitor/Application Insights w modelu OpenTelemetry.

---

Question: Które pole najczęściej ustawisz, żeby wiele usług raportujących do jednego Application Insights miało czytelne nazwy w Application Map?

- [x] `cloud_RoleName` albo OpenTelemetry `service.name`.
- [ ] `resultCode`.
- [ ] `client_IP`.
- [ ] `itemCount`.

Answer: `cloud_RoleName` i `service.name` opisują komponent/usługę, dzięki czemu Application Map nie zlewa wszystkich telemetry w jedną rolę.

---

Question: Chcesz wykryć, że availability test zgłasza awarię tylko z jednej lokalizacji testowej. Którą tabelę/widok sprawdzisz?

- [ ] `dependencies` bez filtrów.
- [x] `availabilityResults` / `AppAvailabilityResults` z grupowaniem po lokalizacji.
- [ ] `customMetrics` tylko po nazwie metryki.
- [ ] `pageViews` po przeglądarce.

Answer: Wyniki availability tests trafiają do `availabilityResults` albo `AppAvailabilityResults`; można grupować je po nazwie testu i lokalizacji.

---

## Źródła

- Microsoft Learn - AZ-204 study guide: https://learn.microsoft.com/en-us/credentials/certifications/resources/study-guides/az-204
- Microsoft Learn - Application Insights overview: https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview
- Microsoft Learn - Workspace-based Application Insights: https://learn.microsoft.com/en-us/azure/azure-monitor/app/create-workspace-resource
- Microsoft Learn - Connection strings: https://learn.microsoft.com/en-us/azure/azure-monitor/app/connection-strings
- Microsoft Learn - Azure Monitor OpenTelemetry: https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-enable
- Microsoft Learn - Application Insights data model: https://learn.microsoft.com/en-us/azure/azure-monitor/app/data-model-complete
- Microsoft Learn - Sampling in Application Insights: https://learn.microsoft.com/en-us/azure/azure-monitor/app/sampling-classic-api
- Microsoft Learn - Availability tests: https://learn.microsoft.com/en-us/azure/azure-monitor/app/availability-overview
- Microsoft Learn - Standard availability tests: https://learn.microsoft.com/en-us/azure/azure-monitor/app/availability-standard-tests
- Microsoft Learn - Application Map: https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-map
- Microsoft Learn - Live Metrics: https://learn.microsoft.com/en-us/azure/azure-monitor/app/live-stream
- Microsoft Learn - Usage analysis: https://learn.microsoft.com/en-us/azure/azure-monitor/app/usage-overview
- Microsoft Learn - Monitor Azure Functions: https://learn.microsoft.com/en-us/azure/azure-functions/functions-monitoring
- GitHub - arvigeus/AZ-204 Topics/Application Insights.md: https://github.com/arvigeus/AZ-204/blob/master/Topics/Application%20Insights.md
- GitHub - arvigeus/AZ-204 Questions/Application Insights.md: https://github.com/arvigeus/AZ-204/blob/master/Questions/Application%20Insights.md
- Reddit / AzureCertification - AZ-204 App Service, Functions and App Insights topics reported by learners: https://www.reddit.com/r/AzureCertification/comments/1j9oyuo
- Reddit / AzureCertification - broad AZ-204 service topic reports: https://www.reddit.com/r/AzureCertification/comments/1j42s5c
- Microsoft Q&A - Application Insights not showing all Function invocations due sampling discussion: https://learn.microsoft.com/en-us/answers/questions/401811/application-insights-is-not-showing-all-function-i
