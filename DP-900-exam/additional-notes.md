# Additional notes - DP-900

## Apache Spark

> Uwaga: chodzi o **Apache Spark**. Czasem przez literowke pojawia sie "Apache Stark".

- **Co to jest?** Open-source'owy silnik do przetwarzania duzych ilosci danych.
- **Do czego sluzy?** Do szybkiego liczenia, filtrowania, laczenia i analizowania danych w wielu maszynach naraz.
- **Jak myslec na egzaminie?**
  - Spark = **compute / processing**
  - Nie jest glownie magazynem danych
  - Czesto dziala nad danymi zapisanymi w Data Lake
- **Typowe zastosowania:**
  - ETL / ELT
  - analiza duzych plikow
  - streaming
  - machine learning
- **Przyklad:** Masz miliony rekordow sprzedazy w plikach CSV w Data Lake. Spark moze je wczytac, przefiltrowac, policzyc przychod i zapisac wynik do tabel lub nowych plikow.
- **Zapamietaj:** Spark najczesciej pojawia sie w uslugach takich jak **Azure Databricks** i **Azure Synapse Analytics (Spark pools)**.

## Azure Synapse Analytics

- **Co to jest?** Platforma analityczna Azure do pracy z duzymi zbiorami danych.
- **Do czego sluzy?** Laczy w jednym miejscu:
  - zapytania SQL
  - przetwarzanie big data
  - pipeline'y danych
  - analityke na danych z Data Lake
- **Jak myslec na egzaminie?**
  - Synapse = **analytics platform**
  - nadaje sie do hurtowni danych i duzej analityki
  - potrafi pracowac z plikami w Data Lake
  - moze korzystac z SQL i Spark
- **Typowe zastosowania:**
  - hurtownia danych
  - analiza duzych zbiorow
  - laczenie danych z roznych zrodel
  - near real-time analytics
- **Przyklad:** Firma ma dane sprzedazowe w Data Lake i chce je analizowac przez SQL oraz budowac raporty BI. Synapse jest dobrym wyborem.
- **Skrot do zapamietania:** jesli w pytaniu masz **SQL + big data + pipeline + Data Lake**, czesto dobra odpowiedzia jest **Azure Synapse Analytics**.

## Azure Databricks

- **Co to jest?** Zarzadzana usluga Azure oparta na **Apache Spark**.
- **Do czego sluzy?** Do przetwarzania i analizy danych, szczegolnie gdy zespol pracuje w notebookach i uzywa Spark.
- **Jak myslec na egzaminie?**
  - Databricks = **Spark as a managed service**
  - bardzo dobry do transformacji danych, data engineering i data science
  - czesto wspolpracuje z Data Lake
- **Typowe zastosowania:**
  - czyszczenie danych
  - laczenie wielu zrodel danych
  - przygotowanie danych do analizy
  - praca w Pythonie, SQL, Scala
- **Przyklad:** Dane z aplikacji trafiaja do Data Lake, a zespol uruchamia notebook Databricks, zeby je oczyscic i przygotowac do raportow.
- **Latwe porownanie z Synapse:**
  - **Databricks**: bardziej kojarz z **Spark/notebooks/processing**
  - **Synapse**: bardziej kojarz z **pelna platforma analityczna i SQL**

## Data Lake

- **Co to jest?** Duzy magazyn plikow dla danych surowych i przetworzonych.
- **Do czego sluzy?** Do przechowywania danych w roznych formatach:
  - structured
  - semi-structured
  - unstructured
- **Jak myslec na egzaminie?**
  - Data Lake = **storage**
  - trzyma dane jako pliki
  - nie jest tym samym co hurtownia danych
  - dobrze nadaje sie do big data i analizy na danych surowych
- **Typowe formaty danych:**
  - CSV
  - JSON
  - Parquet
  - logi
  - obrazy / pliki
- **Przyklad:** Firma zbiera logi aplikacji, pliki CSV ze sprzedaza i dane z IoT. Wszystko trafia najpierw do Data Lake.
- **W Azure na DP-900:** najczesciej chodzi o **Azure Data Lake Storage Gen2**.
- **Zapamietaj roznice:**
  - **Data Lake** = pliki, elastyczne dane, duza skala
  - **Data Warehouse** = dane bardziej uporzadkowane, zwykle do raportowania SQL

## Azure Data Explorer

- **Co to jest?** Usluga do bardzo szybkiej analizy duzych ilosci danych telemetrycznych i logow.
- **Do czego sluzy?** Do analizowania:
  - logow aplikacji
  - danych z IoT
  - zdarzen
  - danych time-series / near real-time
- **Jak myslec na egzaminie?**
  - Data Explorer = **logs + telemetry + time-series analytics**
  - dobry do szybkiego odpytania duzych strumieni danych
  - nie jest klasyczna relacyjna baza danych
- **Typowe zastosowania:**
  - analiza logow z serwerow
  - monitorowanie urzadzen IoT
  - wykrywanie anomalii w danych czasowych
- **Przyklad:** Tysiace czujnikow wysyla pomiary co sekunde. Azure Data Explorer pozwala szybko sprawdzic, ktore urzadzenia zaczely dzialac nietypowo.
- **Zapamietaj:** jesli pytanie brzmi jak **duzo logow, telemetry, time-series, szybka analiza**, bardzo czesto chodzi o **Azure Data Explorer**.

## Azure Stream Analytics

- **Co to jest?** Usluga Azure do przetwarzania danych strumieniowych w czasie rzeczywistym lub prawie rzeczywistym.
- **Do czego sluzy?** Do analizowania danych, ktore naplywaja bez przerwy, np. z czujnikow, aplikacji albo zdarzen systemowych.
- **Jak myslec na egzaminie?**
  - Stream Analytics = **stream processing**
  - dziala na danych "w ruchu", a nie dopiero po zapisaniu calego zbioru
  - umozliwia filtrowanie, agregacje i liczenie w oknach czasowych
- **Typowe zastosowania:**
  - analiza danych z IoT
  - liczenie zdarzen w przedzialach czasu
  - wykrywanie anomalii w strumieniu danych
  - przekazywanie wynikow do Data Lake, Power BI albo bazy danych
- **Przyklad:** Czujniki temperatury wysylaja dane co sekunde. Azure Stream Analytics moze policzyc srednia temperature z ostatnich 5 minut i zapisac wynik do Power BI albo Azure Data Lake.
- **Zapamietaj:** jesli pytanie brzmi jak **real-time**, **stream**, **windowing**, **event processing**, to czesto chodzi o **Azure Stream Analytics**.

## Azure IoT Hub

- **Co to jest?** Usluga Azure do bezpiecznego laczenia, zarzadzania i komunikacji z urzadzeniami IoT.
- **Do czego sluzy?** Do zbierania danych z urzadzen oraz wysylania polecen do tych urzadzen.
- **Jak myslec na egzaminie?**
  - IoT Hub = **device connectivity**
  - sluzy do komunikacji miedzy chmura a urzadzeniami
  - sam w sobie nie sluzy glownie do zaawansowanej analizy danych
- **Typowe zastosowania:**
  - odbieranie telemetrii z czujnikow
  - zarzadzanie wieloma urzadzeniami IoT
  - wysylanie komend z chmury do urzadzen
  - przekazywanie danych dalej do Stream Analytics, Data Lake albo Data Explorer
- **Przyklad:** Tysiace licznikow energii wysyla dane do Azure IoT Hub. Potem te dane trafiaja do Azure Stream Analytics, gdzie liczone sa srednie i alerty.
- **Jak odroznic od innych uslug:**
  - **IoT Hub** zbiera dane z urzadzen i obsluguje komunikacje
  - **Stream Analytics** przetwarza naplywajace zdarzenia
  - **Data Explorer** analizuje duze ilosci telemetrii i logow
- **Zapamietaj:** jesli w pytaniu kluczowe sa slowa **devices**, **telemetry ingestion**, **cloud-to-device**, **device-to-cloud**, to czesto chodzi o **Azure IoT Hub**.

## Szybkie rozroznienie do DP-900

- **Apache Spark**: silnik do przetwarzania danych
- **Azure Databricks**: zarzadzana platforma oparta o Spark
- **Azure Synapse Analytics**: szeroka platforma analityczna z SQL, Spark i pipeline'ami
- **Data Lake**: magazyn plikow dla duzych zbiorow danych
- **Azure Data Explorer**: analiza logow, telemetrii i danych czasowych
- **Azure Stream Analytics**: przetwarzanie danych strumieniowych w czasie rzeczywistym
- **Azure IoT Hub**: laczenie i obsluga urzadzen IoT oraz odbior telemetrii

## Prosty sposob zapamietania

- **Gdzie leza dane?** -> **Data Lake**
- **Czym je przetwarzasz?** -> **Spark**
- **Gdzie wygodnie robisz Spark w Azure?** -> **Databricks**
- **Gdzie laczysz SQL, analityke i big data w jednym miejscu?** -> **Synapse**
- **Czym analizujesz logi i dane czasowe?** -> **Data Explorer**
- **Czym liczysz zdarzenia na biezaco w oknach czasowych?** -> **Stream Analytics**
- **Czym podlaczasz urzadzenia i zbierasz z nich telemetrie?** -> **IoT Hub**
