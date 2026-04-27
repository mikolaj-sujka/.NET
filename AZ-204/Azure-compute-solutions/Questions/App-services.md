# Azure App Service - pytania AZ-204

Pytania są przygotowane pod styl AZ-204: scenariusz, wybór najlepszej konfiguracji, rozróżnianie podobnych usług i pułapki typu slots, TLS, logging, scaling, managed identity.

---

Question: Masz Web App w App Service Plan `B1`. Chcesz użyć deployment slots do wdrażania przez staging i swap. Co musisz zrobić?

- [ ] Włączyć Always On.
- [x] Przejść na co najmniej Standard tier.
- [ ] Dodać custom domain.
- [ ] Włączyć Application Insights.

Answer: Deployment slots wymagają Standard tier lub wyższego. Basic nie wystarczy.

---

Question: Aplikacja ma używać własnej domeny `www.contoso.com`. Który rekord DNS najczęściej ustawisz dla subdomeny?

- [ ] A record do `127.0.0.1`.
- [x] CNAME do `<app-name>.azurewebsites.net`.
- [ ] MX record do App Service.
- [ ] SRV record do App Service Plan.

Answer: Dla subdomeny zwykle używa się CNAME wskazującego na domyślny hostname App Service.

---

Question: Masz root domain `contoso.com`. Który rekord DNS jest typowo używany do wskazania App Service?

- [x] A record.
- [ ] MX record.
- [ ] CNAME record.
- [ ] NS record.

Answer: Apex/root domain zwykle używa A record. CNAME jest typowy dla subdomen, np. `www`.

---

Question: Co daje TLS w Azure App Service?

- [x] Szyfrowanie danych w tranzycie.
- [x] Potwierdzenie tożsamości serwera przez certyfikat.
- [x] Ochronę integralności transmisji.
- [ ] Automatyczne skalowanie aplikacji.

Answer: TLS zabezpiecza komunikację HTTPS. Nie odpowiada za scaling.

---

Question: Chcesz wymusić, aby wszystkie requesty do Web App używały HTTPS. Co ustawisz?

- [x] HTTPS Only.
- [ ] ARR Affinity.
- [ ] WEBSITE_RUN_FROM_PACKAGE.
- [ ] Always On.

Answer: HTTPS Only wymusza ruch przez HTTPS.

---

Question: Kiedy App Service Managed Certificate jest dobrym wyborem?

- [x] Chcesz darmowy, automatycznie odnawiany certyfikat dla public custom domain.
- [ ] Potrzebujesz wildcard certificate.
- [ ] Potrzebujesz wyeksportować certyfikat poza App Service.
- [ ] Używasz private DNS bez publicznej walidacji domeny.

Answer: Free managed certificate jest wygodny dla prostych publicznych custom domains, ale nie wspiera wildcard i nie jest exportable.

---

Question: Który certyfikat można eksportować i użyć poza App Service?

- [ ] App Service Managed Certificate.
- [x] App Service Certificate.
- [x] Certyfikat zarządzany w Key Vault, jeśli spełnia wymagania.
- [ ] Domyślny certyfikat `azurewebsites.net`.

Answer: Free managed certificate nie jest exportable. App Service Certificate i własne certyfikaty w Key Vault dają większą kontrolę.

---

Question: Web App musi dostać się do Key Vault bez przechowywania client secret. Co wybierzesz?

- [ ] Publish profile.
- [x] Managed identity + RBAC/access policy.
- [ ] FTP credentials.
- [ ] Deployment slot setting.

Answer: Managed identity pozwala aplikacji uzyskać token z Microsoft Entra ID bez sekretu w konfiguracji.

---

Question: Włączyłeś system-assigned managed identity dla Web App. Aplikacja nadal nie może czytać sekretów z Key Vault. Co prawdopodobnie brakuje?

- [x] Uprawnień RBAC albo access policy do Key Vault.
- [ ] Deployment slot.
- [ ] ARR Affinity.
- [ ] CNAME record.

Answer: Samo włączenie identity nie daje dostępu do zasobu. Trzeba nadać uprawnienia.

---

Question: GitHub Actions ma wdrażać Web App bez przechowywania publish profile ani client secret. Co jest zalecane?

- [x] OpenID Connect z federated credential i RBAC.
- [ ] FTP deployment credentials.
- [ ] Basic authentication.
- [ ] Connection string w GitHub secret.

Answer: OIDC używa krótkotrwałego tokenu z GitHub i wymienia go w Microsoft Entra ID na token do Azure.

---

Question: W workflow GitHub Actions używasz OIDC. Które permission jest potrzebne, żeby `azure/login` mogło pobrać token?

- [x] `id-token: write`
- [ ] `packages: write`
- [ ] `actions: admin`
- [ ] `secrets: read`

Answer: `id-token: write` pozwala workflow uzyskać OIDC token z GitHub.

---

Question: Potrzebujesz debugować błędy wygenerowane przez kod aplikacji. Który logging App Service włączysz?

- [x] Application logging.
- [ ] Web server logging.
- [ ] Failed request tracing.
- [ ] Deployment logging.

Answer: Application logging zbiera logi z aplikacji. Web server logging dotyczy surowych requestów HTTP.

---

Question: Użytkownicy widzą dużo HTTP 404 i chcesz przeanalizować requesty HTTP. Co będzie najbardziej trafne?

- [ ] Application logging.
- [x] Web server logging.
- [ ] App settings.
- [ ] Managed identity.

Answer: Web server logging zapisuje surowe dane HTTP i pomaga przy 404/5xx.

---

Question: Chcesz podglądać logi Web App na żywo przez Azure CLI. Której komendy użyjesz?

- [x] `az webapp log tail`
- [ ] `az webapp log download`
- [ ] `az monitor metrics list`
- [ ] `az webapp show`

Answer: `az webapp log tail` pokazuje log stream.

---

Question: Dla Linux custom container gdzie typowo znajdziesz logi aplikacji?

- [ ] `C:\home\LogFiles`
- [x] `/home/LogFiles`
- [ ] `/var/app/logs-only`
- [ ] `D:\LogFilesOnly`

Answer: Dla Linux/custom containers typową lokalizacją jest `/home/LogFiles`.

---

Question: Chcesz zero-downtime deployment i możliwość szybkiego rollbacku. Co wybierzesz?

- [x] Deployment slot i swap.
- [ ] FTP deployment bez slotu.
- [ ] Zmianę pricing tier.
- [ ] CORS.

Answer: Slot staging + swap pozwala przetestować release i szybko wrócić przez swap back.

---

Question: Które ustawienie powinno być slot setting?

- [x] Connection string do testowej bazy w staging.
- [ ] Publiczny runtime stack.
- [ ] Nazwa production custom domain.
- [ ] Liczba instancji planu.

Answer: Connection string zależny od środowiska powinien zostać przypięty do slotu.

---

Question: Które ustawienie zwykle nie jest swapped między slots?

- [x] Managed identity.
- [x] Custom domain names.
- [x] CORS.
- [ ] WebJobs content.

Answer: Managed identities, custom domains i CORS są zwykle slot-specific. WebJobs content jest swapped.

---

Question: Co robi parametr `x-ms-routing-name=staging`?

- [x] Kieruje request do slotu `staging`.
- [ ] Włącza Always On.
- [ ] Wymusza HTTPS.
- [ ] Tworzy deployment slot.

Answer: `x-ms-routing-name` pozwala ręcznie kierować request do określonego slotu.

---

Question: Co oznacza `x-ms-routing-name=self`?

- [x] Kierowanie do production slot.
- [ ] Kierowanie do ostatnio utworzonego slotu.
- [ ] Kierowanie do slotu o nazwie `self`.
- [ ] Wyłączenie traffic routing.

Answer: `self` oznacza production slot.

---

Question: Chcesz, aby Web App wychodziła do zasobów w VNet. Która funkcja jest właściwa?

- [ ] Private Endpoint.
- [x] VNet Integration.
- [ ] Access restrictions.
- [ ] App Service Managed Certificate.

Answer: VNet Integration dotyczy ruchu wychodzącego z aplikacji do VNet.

---

Question: Chcesz, aby Web App była dostępna prywatnym IP z VNet. Która funkcja jest właściwa?

- [x] Private Endpoint.
- [ ] Hybrid Connections.
- [ ] ARR Affinity.
- [ ] Web server logging.

Answer: Private Endpoint dotyczy prywatnego ruchu przychodzącego do aplikacji.

---

Question: Web App musi połączyć się z lokalnym serwerem on-premises bez pełnego VPN. Co rozważysz?

- [ ] App-assigned address.
- [x] Hybrid Connections.
- [ ] TLS binding.
- [ ] Deployment Center.

Answer: Hybrid Connections pozwala aplikacji w Azure dotrzeć do zasobu on-premises.

---

Question: Masz wiele aplikacji w jednym App Service Plan. Co stanie się przy scale out planu?

- [x] Wszystkie aplikacje w planie dostaną dodatkowe instances/workers.
- [ ] Tylko jedna wybrana aplikacja zostanie przeskalowana.
- [ ] Plan zmieni region.
- [ ] Deployment slots zostaną usunięte.

Answer: Scaling dotyczy App Service Plan, nie pojedynczej Web App.

---

Question: Co oznacza scale up?

- [x] Zmianę tier/rozmiaru planu na mocniejszy lub słabszy.
- [ ] Zwiększenie liczby instances.
- [ ] Utworzenie deployment slot.
- [ ] Dodanie CNAME.

Answer: Scale up/down to pionowa zmiana planu. Scale out/in to liczba instancji.

---

Question: Autoscale ma regułę scale out przy CPU > 70% i scale in przy CPU < 68%. Jaki problem może wystąpić?

- [x] Flapping.
- [ ] TLS downgrade.
- [ ] Poison queue.
- [ ] DNS propagation.

Answer: Progi są zbyt blisko, więc scaling może oscylować między scale out i scale in.

---

Question: Jak ograniczyć flapping w Autoscale?

- [x] Zostawić większy odstęp między scale out i scale in thresholds.
- [x] Ustawić cooldown period.
- [x] Użyć dłuższego okna obserwacji metryki.
- [ ] Użyć tego samego progu dla scale out i scale in.

Answer: Anti-flapping wymaga histerezy, cooldown i spokojniejszych reguł.

---

Question: Co oznacza ARR Affinity?

- [x] Sticky sessions, czyli kierowanie klienta do tej samej instancji.
- [ ] Automatyczne odnawianie certyfikatu.
- [ ] Przekierowanie HTTP na HTTPS.
- [ ] Deployment przez GitHub Actions.

Answer: ARR Affinity trzyma klienta na tej samej instancji, ale lepiej projektować aplikacje stateless.

---

Question: Chcesz uruchomić custom container z prywatnego ACR bez hasła registry w konfiguracji. Co wybierzesz?

- [x] Managed identity + rola `AcrPull`.
- [ ] Publiczny Blob container.
- [ ] App Service Managed Certificate.
- [ ] CORS.

Answer: App Service może używać managed identity do pull z ACR po nadaniu `AcrPull`.

---

Question: Custom Linux container nasłuchuje na porcie 8080. Co często trzeba ustawić?

- [x] `WEBSITES_PORT=8080`
- [ ] `FUNCTIONS_WORKER_RUNTIME=8080`
- [ ] `SCM_DO_BUILD_DURING_DEPLOYMENT=false`
- [ ] `ARR_PORT=8080`

Answer: `WEBSITES_PORT` informuje App Service, na którym porcie działa container.

---

Question: Który katalog w Linux custom container może służyć jako persistent shared storage?

- [x] `/home`
- [ ] `/tmp`
- [ ] `/var/cache`
- [ ] `/root`

Answer: `/home` może być persistent i współdzielony między instancjami.

---

Question: Health Check w App Service pinguje endpoint, a instancja stale zwraca błędy. Co może zrobić platforma?

- [x] Usunąć instancję z load balancer rotation.
- [x] Przy dłuższym problemie zastąpić instancję.
- [ ] Automatycznie utworzyć deployment slot.
- [ ] Zmienić TLS certificate.

Answer: Health Check pomaga odsunąć uszkodzoną instancję od ruchu.

---

Question: Który endpoint Kudu/SCM jest typowy dla Advanced Tools?

- [x] `https://<app-name>.scm.azurewebsites.net`
- [ ] `https://<app-name>.kudu.local`
- [ ] `https://portal.azure.com/<app-name>/logs`
- [ ] `https://<app-name>.azurewebsites.net/admin`

Answer: SCM/Kudu działa pod domeną `.scm.azurewebsites.net`.

---

Question: Zmiana którego ustawienia zwykle powoduje restart Web App?

- [x] App settings.
- [x] Connection strings.
- [x] Storage mounts.
- [ ] Odczyt outbound IP przez `az webapp show`.

Answer: Zmiany konfiguracji aplikacji i mountów zwykle restartują aplikację.

---

Question: Które z poniższych kategorii planów App Service zapewniają maksymalne możliwości skalowania w poziomie?

- [ ] Dedykowane obliczenia.
- [x] Izolowana.
- [ ] Współużytkowane zasoby obliczeniowe.

Answer: Kategoria Isolated daje największe możliwości scale out, bo działa w dedykowanym App Service Environment.

---

Question: Które z następujących funkcji sieciowych usługi App Service mogą służyć do kontrolowania ruchu wychodzącego w sieci?

- [ ] Adres przypisany do aplikacji.
- [x] Połączenia hybrydowe.
- [ ] Punkty końcowe usługi.

Answer: Hybrid Connections dotyczą ruchu wychodzącego z App Service do wskazanych zasobów, np. on-premises. App-assigned address, service endpoints i private endpoints są kojarzone głównie z kontrolą ruchu przychodzącego.

---

Question: Jaki jest cel funkcji środowiska Azure App Service Environment?

- [ ] Zapewnia udostępnioną infrastrukturę do uruchamiania aplikacji usługi App Service.
- [ ] Umożliwia wdrażanie i uruchamianie konteneryzowanych aplikacji internetowych w systemach Windows i Linux.
- [x] Zapewnia w pełni izolowane i dedykowane środowisko do uruchamiania aplikacji usługi App Service z ulepszonymi zabezpieczeniami na dużą skalę.

Answer: App Service Environment zapewnia izolowane, dedykowane środowisko dla App Service, używane przy wymaganiach dużej skali, izolacji sieciowej i wyższych wymaganiach bezpieczeństwa.

---

Question: Co określa zestaw zasobów obliczeniowych dla aplikacji internetowej do uruchomienia w usłudze Azure App Service?

- [ ] Region geograficzny, w którym jest wdrażana aplikacja.
- [ ] Warstwa cenowa aplikacji.
- [x] Plan usługi App Service.

Answer: App Service Plan definiuje zasoby compute, na których działa aplikacja, w tym region, tier, rozmiar i liczbę instancji.

---

Question: Jaki jest cel korzystania z miejsc wdrożenia w usłudze Azure App Service?

- [x] Aby wdrożyć aplikację w środowisku przejściowym, a następnie zamienić miejsca przejściowe i produkcyjne, eliminując przestój.
- [ ] Aby zwiększyć pojemność magazynu aplikacji.
- [ ] Aby dodać do dziewięciu kontenerów pomocniczych dla każdej niestandardowej aplikacji kontenera z obsługą sidecar.

Answer: Deployment slots pozwalają wdrożyć aplikację do staging, rozgrzać i sprawdzić wersję, a potem wykonać swap z produkcją bez przestoju.

---

## Źródła

- Microsoft Learn - Azure App Service: https://learn.microsoft.com/en-us/azure/app-service/
- Microsoft Learn - Configure App Service: https://learn.microsoft.com/en-us/azure/app-service/configure-common
- Microsoft Learn - TLS/SSL in App Service: https://learn.microsoft.com/en-us/azure/app-service/overview-tls
- Microsoft Learn - Custom domains: https://learn.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-custom-domain
- Microsoft Learn - Deployment slots: https://learn.microsoft.com/en-us/azure/app-service/deploy-staging-slots
- Microsoft Learn - Diagnostics logging: https://learn.microsoft.com/en-us/azure/app-service/troubleshoot-diagnostic-logs
- Microsoft Learn - GitHub Actions deployment: https://learn.microsoft.com/en-us/azure/app-service/deploy-github-actions
- Microsoft Learn - Autoscale settings and flapping: https://learn.microsoft.com/en-us/azure/azure-monitor/autoscale/autoscale-understanding-settings
- GitHub - arvigeus/AZ-204 Questions/App Service.md: https://github.com/arvigeus/AZ-204/blob/master/Questions/App%20Service.md
- Reddit / AzureCertification discussions about AZ-204 question style and hands-on focus: https://www.reddit.com/r/AzureCertification/
