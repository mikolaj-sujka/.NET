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

## ETL i ELT

- **ETL** = **Extract, Transform, Load**
- **ELT** = **Extract, Load, Transform**
- **Co to znaczy?**
  - **Extract**: pobranie danych ze zrodla
  - **Transform**: oczyszczenie, polaczenie, zmiana formatu albo obliczenia na danych
  - **Load**: zapisanie danych do miejsca docelowego
- **Roznica:**
  - **ETL**: najpierw zmieniasz dane, potem ladujesz je do celu
  - **ELT**: najpierw ladujesz surowe dane, potem przetwarzasz je juz w systemie docelowym
- **Jak myslec na egzaminie?**
  - jesli pytanie dotyczy przenoszenia i transformacji danych miedzy systemami, mysz o **ETL / ELT**
  - w Azure czesto pojawia sie tu **Azure Data Factory** albo **Synapse Pipelines**
  - Spark i Databricks czesto realizuja czesc transformacji
- **Przyklad ETL:**
  - sklep ma dane sprzedazy w lokalnej bazie SQL
  - proces pobiera dane co noc
  - usuwa bledne rekordy i liczy nowe kolumny
  - na koncu zapisuje gotowe dane do hurtowni danych
- **Przyklad ELT:**
  - firma wrzuca surowe pliki CSV do Data Lake
  - potem Synapse albo Databricks przetwarza te dane juz w chmurze
  - wynik trafia do tabel analitycznych albo raportow
- **Latwy skrot:**
  - **ETL** = transformacja **przed** zaladowaniem
  - **ELT** = transformacja **po** zaladowaniu

## Data Pipelines

- **Co to jest?** Uporzadkowany proces, ktory przenosi i przetwarza dane od zrodla do celu.
- **Do czego sluzy?** Do automatyzacji pracy na danych, zeby wszystko dzialo sie samo i w odpowiedniej kolejnosci.
- **Jak myslec na egzaminie?**
  - data pipeline = **przeplyw danych krok po kroku**
  - pipeline moze kopiowac dane, uruchamiac transformacje, czekac na zdarzenie i zapisywac wynik
  - w Azure pipeline'y sa mocno kojarzone z **Azure Data Factory** i **Azure Synapse**
- **Co moze zawierac pipeline?**
  - pobranie danych z bazy lub plikow
  - kopiowanie danych do Data Lake
  - uruchomienie transformacji
  - zapisanie wyniku do hurtowni danych
  - uruchomienie raportu albo kolejnego procesu
- **Przyklad dzialania pipeline'u 1: dane sprzedazowe**
  - dane sa pobierane z systemu sklepowego
  - pipeline kopiuje je do Azure Data Lake
  - Databricks czysci dane i laczy je z danymi o produktach
  - wynik trafia do Azure Synapse
  - Power BI pokazuje raport sprzedazy
- **Przyklad dzialania pipeline'u 2: dane z IoT**
  - urzadzenia wysylaja telemetrie do IoT Hub
  - Stream Analytics liczy srednie i wykrywa alerty
  - dane sa zapisywane do Data Lake lub Data Explorer
  - raport albo dashboard pokazuje dane prawie w czasie rzeczywistym
- **Przyklad dzialania pipeline'u 3: nocne ladowanie danych**
  - codziennie o 1:00 pipeline startuje automatycznie
  - pobiera dane z kilku baz danych
  - wykonuje transformacje
  - laduje wynik do hurtowni
  - wysyla powiadomienie, ze proces zakonczyl sie poprawnie
- **Zapamietaj:** pipeline nie musi sam wykonywac calej analizy. Czesto tylko **orkiestruje** kolejne kroki i wywoluje inne uslugi.

## Azure Data Factory

- **Co to jest?** Chmurowa usluga Azure do integracji danych i budowania pipeline'ow.
- **Do czego sluzy?** Do przenoszenia, orkiestracji i czesciowej transformacji danych pomiedzy roznymi systemami.
- **Jak myslec na egzaminie?**
  - Data Factory = **data integration + orchestration**
  - bardzo czesto jest poprawna odpowiedzia, gdy pytanie dotyczy **pipeline**, **copy data**, **schedule**, **ETL/ELT**
  - sama usluga czesto nie wykonuje ciezkich obliczen jak Spark, tylko uruchamia i koordynuje kroki
- **Co potrafi?**
  - laczyc sie z wieloma zrodlami danych
  - kopiowac dane z miejsca do miejsca
  - uruchamiac pipeline'y wedlug harmonogramu albo zdarzenia
  - wywolywac inne uslugi, np. Databricks albo Synapse
  - wykonywac transformacje przez Mapping Data Flows albo przez integracje z innymi narzedziami
- **Typowe zastosowania:**
  - kopiowanie danych z lokalnej bazy do Azure Data Lake
  - nocne ladowanie danych do hurtowni
  - automatyczne uruchamianie krokow ETL / ELT
  - laczenie wielu zrodel w jeden przeplyw danych
- **Przyklad 1:**
  - Data Factory pobiera dane z on-prem SQL Server
  - kopiuje je do Azure Data Lake Storage Gen2
  - uruchamia Databricks do oczyszczenia danych
  - zapisuje wynik do Azure Synapse
- **Przyklad 2:**
  - codziennie o 2:00 w nocy pipeline w Data Factory startuje automatycznie
  - pobiera dane z CRM i ERP
  - laczy je i laduje do hurtowni danych
  - rano Power BI ma juz swieze dane
- **Jak odroznic od innych uslug:**
  - **Azure Data Factory**: integruje dane i orkiestruje pipeline'y
  - **Azure Databricks**: przetwarza dane przy pomocy Spark
  - **Azure Synapse Analytics**: platforma analityczna do SQL, Spark i hurtowni danych
  - **Azure Stream Analytics**: przetwarza dane strumieniowe na biezaco
  - **Azure IoT Hub**: zbiera dane z urzadzen IoT
- **Prosty sposob zapamietania:**
  - jesli pytanie brzmi "jak **przeniesc** dane?"
  - albo "jak **zautomatyzowac pipeline**?"
  - albo "jak **uruchamiac procesy wedlug harmonogramu**?"
  - to bardzo czesto chodzi o **Azure Data Factory**

## Azure Cosmos DB

- **Co to jest?** Globalnie dystrybuowana, nierelacyjna baza danych Azure.
- **Do czego sluzy?** Do przechowywania danych, ktore musza byc bardzo szybko odczytywane i zapisywane przy duzej skali.
- **Jak myslec na egzaminie?**
  - Cosmos DB = **NoSQL / high scale / low latency**
  - dobrze nadaje sie do aplikacji internetowych, mobilnych i systemow z duza liczba operacji
  - nie jest klasyczna relacyjna baza danych jak Azure SQL Database
- **Najwazniejsze cechy:**
  - bardzo szybki odczyt i zapis
  - globalna replikacja
  - elastyczny model danych
  - obsluga wielu API
  - skalowanie throughput przez **RU/s**
- **Jakie API warto znac pod DP-900?**
  - **SQL API**: dokumenty JSON
  - **MongoDB API**: zgodnosc z MongoDB
  - **Cassandra API**: model column-family
  - **Gremlin API**: dane grafowe
  - **Table API**: key/value
- **Typowe zastosowania:**
  - aplikacja webowa z bardzo duza liczba uzytkownikow
  - katalog produktow zapisany jako dokumenty JSON
  - dane globalnie dostepne z wielu regionow
  - graf relacji, np. znajomosci albo polaczenia miedzy obiektami
- **Przyklad 1:**
  - sklep internetowy trzyma katalog produktow w JSON
  - aplikacja musi szybko odczytywac dane produktow dla uzytkownikow z wielu krajow
  - Cosmos DB dobrze pasuje, bo zapewnia niski czas odpowiedzi i globalna replikacje
- **Przyklad 2:**
  - aplikacja mobilna zapisuje profile uzytkownikow i ich preferencje
  - struktura danych moze sie zmieniac
  - Cosmos DB pasuje lepiej niz sztywna relacyjna tabela
- **Jak odroznic od innych uslug:**
  - **Azure Cosmos DB**: nierelacyjna baza danych do szybkich operacji i duzej skali
  - **Azure SQL Database**: relacyjna baza danych z tabelami, kluczami i SQL
  - **Data Lake**: magazyn plikow, a nie baza do szybkich operacji aplikacyjnych
  - **Azure Data Explorer**: analiza logow i telemetrii, a nie glowna baza operacyjna aplikacji
- **Na co uwazac na egzaminie?**
  - jesli pytanie dotyczy **JSON**, **NoSQL**, **global distribution**, **low latency**, **high throughput**, to czesto chodzi o **Cosmos DB**
  - jesli pytanie dotyczy **graph database**, poprawna odpowiedz moze byc **Cosmos DB for Gremlin**
  - jesli pytanie dotyczy **column-family**, poprawna odpowiedz moze byc **Cosmos DB for Apache Cassandra**
- **Prosty sposob zapamietania:**
  - **relacyjne dane i klasyczne SQL** -> **Azure SQL Database**
  - **elastyczne dane NoSQL i duza skala** -> **Azure Cosmos DB**

## Azure HDInsight

- **Co to jest?** Chmurowa usluga Azure do uruchamiania popularnych frameworkow open-source do big data.
- **Do czego sluzy?** Do pracy z technologiami takimi jak Hadoop, Spark, Hive, HBase i Kafka w modelu zarzadzanym przez Azure.
- **Jak myslec na egzaminie?**
  - HDInsight = **open-source big data clusters**
  - czesto pojawia sie, gdy pytanie wprost wspomina **Hadoop** albo ekosystem Hadoop
  - jest bardziej "klastrowe" i infrastrukturalne niz Databricks czy Data Factory
- **Typowe zastosowania:**
  - przetwarzanie danych przy pomocy Hadoop
  - uruchamianie Spark w klastrze
  - analiza duzych zbiorow przez Hive
  - obsluga rozproszonych workloadow big data
- **Przyklad 1:**
  - firma ma rozwiazanie oparte o Hadoop
  - chce przeniesc je do Azure bez porzucania znanych narzedzi
  - HDInsight jest naturalnym wyborem
- **Przyklad 2:**
  - zespol pracuje na Spark i Hive
  - potrzebuje klastra do przetwarzania duzych zbiorow danych z Data Lake
  - HDInsight moze zapewnic takie srodowisko
- **Jak odroznic od innych uslug:**
  - **Azure HDInsight**: zarzadzane klastry dla Hadoop, Spark, Hive, Kafka
  - **Azure Databricks**: wygodniejsza platforma Spark z notebookami i naciskiem na data engineering / data science
  - **Azure Data Factory**: orkiestracja i pipeline'y, a nie klaster obliczeniowy
  - **Azure Synapse Analytics**: szersza platforma analityczna z SQL, Spark i integracja danych
- **Na co uwazac na egzaminie?**
  - jesli pytanie brzmi "co sluzy do przetwarzania duzych ilosci danych przy uzyciu **Apache Hadoop**?" -> bardzo czesto **Azure HDInsight**
  - jesli pytanie mocno akcentuje **Spark notebooks** i wygodna analityke w Azure -> czesto **Azure Databricks**
- **Prosty sposob zapamietania:**
  - **Hadoop / Hive / HBase / Kafka cluster** -> **HDInsight**
  - **Spark notebooks i transformacje danych** -> **Databricks**

## Szybkie rozroznienie do DP-900

- **Apache Spark**: silnik do przetwarzania danych
- **Azure Databricks**: zarzadzana platforma oparta o Spark
- **Azure Synapse Analytics**: szeroka platforma analityczna z SQL, Spark i pipeline'ami
- **Data Lake**: magazyn plikow dla duzych zbiorow danych
- **Azure Data Explorer**: analiza logow, telemetrii i danych czasowych
- **Azure Stream Analytics**: przetwarzanie danych strumieniowych w czasie rzeczywistym
- **Azure IoT Hub**: laczenie i obsluga urzadzen IoT oraz odbior telemetrii
- **ETL / ELT**: wzorzec przenoszenia i transformacji danych
- **Data Pipelines**: automatyczny przeplyw danych i orkiestracja krokow
- **Azure Data Factory**: integracja danych, kopiowanie danych i orkiestracja pipeline'ow
- **Azure Cosmos DB**: nierelacyjna baza danych NoSQL o duzej skali i niskich opoznieniach
- **Azure HDInsight**: zarzadzane klastry open-source do Hadoop, Spark, Hive i Kafka

## Prosty sposob zapamietania

- **Gdzie leza dane?** -> **Data Lake**
- **Czym je przetwarzasz?** -> **Spark**
- **Gdzie wygodnie robisz Spark w Azure?** -> **Databricks**
- **Gdzie laczysz SQL, analityke i big data w jednym miejscu?** -> **Synapse**
- **Czym analizujesz logi i dane czasowe?** -> **Data Explorer**
- **Czym liczysz zdarzenia na biezaco w oknach czasowych?** -> **Stream Analytics**
- **Czym podlaczasz urzadzenia i zbierasz z nich telemetrie?** -> **IoT Hub**
- **Czym automatyzujesz przeplyw danych miedzy uslugami?** -> **Data Pipelines**
- **Jak nazywa sie proces pobrania, zmiany i zaladowania danych?** -> **ETL / ELT**
- **Czym glownie kopiujesz dane i uruchamiasz pipeline'y?** -> **Data Factory**
- **Jaka usluge wybierasz do NoSQL, JSON i globalnej skali?** -> **Cosmos DB**
- **Czego uzyjesz, gdy pytanie wprost mowi o Hadoop?** -> **HDInsight**

## Pytania pomocnicze do zapamietania

- **Czy pytanie dotyczy przechowywania plikow?** -> mysli o **Data Lake**
- **Czy pytanie dotyczy samego przetwarzania danych?** -> mysli o **Spark**
- **Czy pytanie dotyczy notebookow Spark i transformacji danych?** -> mysli o **Databricks**
- **Czy pytanie dotyczy SQL, hurtowni danych i szerokiej analityki?** -> mysli o **Synapse**
- **Czy pytanie dotyczy kopiowania danych i orkiestracji pipeline'ow?** -> mysli o **Data Factory**
- **Czy pytanie dotyczy danych strumieniowych i okien czasowych?** -> mysli o **Stream Analytics**
- **Czy pytanie dotyczy urzadzen IoT i telemetrii z urzadzen?** -> mysli o **IoT Hub**
- **Czy pytanie dotyczy logow, telemetrii i danych time-series?** -> mysli o **Data Explorer**
- **Czy pytanie dotyczy NoSQL, JSON i bardzo szybkich odczytow / zapisow?** -> mysli o **Cosmos DB**
- **Czy pytanie wprost mowi o Hadoop, Hive albo HBase?** -> mysli o **HDInsight**
