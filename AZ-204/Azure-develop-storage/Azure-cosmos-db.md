# Azure Cosmos DB - notatki pod AZ-204

Stan na: 2026-05-02. Egzamin AZ-204 według Microsoft Learn obejmuje dział **Develop for Azure storage** jako 15-20% egzaminu. Dla Azure Cosmos DB trzeba umieć:

- wykonywać operacje na kontenerach i itemach przez SDK,
- dobrać odpowiedni consistency level dla operacji,
- implementować change feed notifications.

Oficjalna ścieżka Microsoft Learn dla AZ-204 skupia się na tworzeniu zasobów Cosmos DB, consistency levels i operacjach danych przez .NET SDK v3.

## Źródła

- Microsoft Learn - study guide AZ-204: https://learn.microsoft.com/en-us/credentials/certifications/resources/study-guides/az-204
- Microsoft Learn - Develop solutions that use Azure Cosmos DB: https://learn.microsoft.com/en-us/training/paths/az-204-develop-solutions-that-use-azure-cosmos-db/
- Microsoft Learn - Consistency levels: https://learn.microsoft.com/en-us/azure/cosmos-db/consistency-levels
- Microsoft Learn - Manage consistency levels: https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-manage-consistency
- Microsoft Learn - Partitioning and horizontal scaling: https://learn.microsoft.com/en-us/azure/cosmos-db/partitioning
- Microsoft Learn - Change feed: https://learn.microsoft.com/en-us/azure/cosmos-db/change-feed
- Microsoft Learn - Read change feed: https://learn.microsoft.com/en-us/azure/cosmos-db/read-change-feed
- Notatki porównawcze: https://github.com/arvigeus/AZ-204/blob/master/Topics/Cosmos%20DB.md

## Co to jest Azure Cosmos DB

Azure Cosmos DB to w pełni zarządzana, globalnie dystrybuowana baza danych dla aplikacji cloud-native. Na AZ-204 najczęściej chodzi o **API for NoSQL** i pracę przez **Azure Cosmos DB .NET SDK v3**.

Hierarchia:

- **Account** - konto Cosmos DB, endpoint np. `https://<account>.documents.azure.com:443/`.
- **Database** - namespace dla kontenerów.
- **Container** - miejsce przechowywania itemów; ma partition key, indexing policy, TTL, throughput.
- **Item** - dokument JSON w kontenerze; identyfikowany przez `id` i partition key.

API:

- **NoSQL** - natywne dokumenty JSON i SQL-like queries; najważniejsze dla AZ-204.
- **MongoDB** - kompatybilność z MongoDB.
- **Apache Cassandra** - model column-family.
- **Apache Gremlin** - grafy.
- **Table** - key-value, kompatybilność z Azure Table Storage.
- **PostgreSQL** - relacyjny wariant oparty o distributed PostgreSQL.

## Partitioning

Partition key to jedna z najważniejszych decyzji projektowych.

Pojęcia:

- **Partition key path** - np. `/tenantId`.
- **Partition key value** - wartość w itemie, np. `tenant-42`.
- **Logical partition** - wszystkie itemy z tym samym partition key value.
- **Physical partition** - wewnętrzna jednostka zarządzana przez Cosmos DB.

Ważne:

- Item jest jednoznacznie identyfikowany przez parę `id` + partition key.
- Transakcje w stored procedures/triggers działają w obrębie jednej logical partition.
- Dobry partition key ma wysoką kardynalność i równomierny rozkład.
- Unikaj hot partitions, czyli sytuacji, w której większość ruchu trafia do jednej wartości partition key.
- Jeśli jedna właściwość nie wystarcza, rozważ synthetic partition key, np. `tenantId|yyyyMM` albo suffix.

Limity, które warto pamiętać z dokumentacji:

- Pojedyncza physical partition może obsłużyć do 10 000 RU/s.
- Physical partition może przechować do 50 GB danych.
- Logical partition mapuje się do jednej physical partition, więc zły partition key może ograniczyć skalowanie.

## Request Units i throughput

**RU/s** to waluta wydajności w Cosmos DB. Każda operacja kosztuje RU:

- point read mały dokument ok. 1 KB jest tani,
- query cross-partition kosztuje więcej,
- writes kosztują więcej niż proste reads,
- indexing zwiększa koszt zapisu, ale przyspiesza zapytania.

Modele throughput:

- **Provisioned throughput na kontenerze** - dedykowane RU/s dla jednego kontenera.
- **Provisioned throughput na bazie** - RU/s współdzielone przez kontenery w bazie.
- **Autoscale** - ustawiasz max RU/s, Cosmos skaluje zwykle między 10% a 100% max.
- **Serverless** - płacisz za zużyte RU, dobre dla sporadycznych/dev/test workloadów.

CLI autoscale:

```bash
az cosmosdb sql container create \
  --account-name cosmosaz204 \
  --resource-group rg-az204 \
  --database-name appdb \
  --name items \
  --partition-key-path "/tenantId" \
  --throughput-type autoscale \
  --max-throughput 4000
```

.NET autoscale:

```csharp
ContainerProperties properties = new("items", "/tenantId");

Container container = await database.CreateContainerIfNotExistsAsync(
    properties,
    ThroughputProperties.CreateAutoscaleThroughput(4000));
```

## Consistency levels

Cosmos DB ma pięć poziomów spójności, od najsilniejszego do najsłabszego:

1. **Strong** - zawsze najnowszy zatwierdzony zapis; większa latencja i mniejsza dostępność przy globalnej dystrybucji.
2. **Bounded staleness** - odczyty mogą być opóźnione maksymalnie o K wersji albo T czasu.
3. **Session** - domyślny i najczęściej używany; read-your-writes w ramach sesji klienta.
4. **Consistent prefix** - odczyty widzą zapisy w kolejności, bez out-of-order.
5. **Eventual** - najwyższa dostępność i najniższa latencja, ale brak gwarancji świeżości/kolejności.

Ważne na egzamin:

- Default consistency ustawia się na poziomie konta.
- SDK może tylko **osłabić** consistency dla odczytów względem ustawienia konta; nie podniesie słabszego ustawienia konta do mocniejszego.
- Session używa session tokenów.
- Strong i Bounded Staleness czytają z dwóch replik, więc mają wyższy koszt RU dla odczytów niż Session/Consistent Prefix/Eventual.
- Strong consistency nie jest używana z multiple write regions.

CLI:

```bash
az cosmosdb create \
  --name cosmosaz204 \
  --resource-group rg-az204 \
  --locations regionName=westeurope failoverPriority=0 isZoneRedundant=False \
  --default-consistency-level Session

az cosmosdb update \
  --name cosmosaz204 \
  --resource-group rg-az204 \
  --default-consistency-level BoundedStaleness \
  --max-interval 300 \
  --max-staleness-prefix 100000
```

.NET - consistency na kliencie:

```csharp
using Microsoft.Azure.Cosmos;

var options = new CosmosClientOptions
{
    ConsistencyLevel = ConsistencyLevel.Session
};

CosmosClient client = new(
    accountEndpoint: "https://cosmosaz204.documents.azure.com:443/",
    tokenCredential: new Azure.Identity.DefaultAzureCredential(),
    clientOptions: options);
```

.NET - consistency per request:

```csharp
ItemRequestOptions options = new()
{
    ConsistencyLevel = ConsistencyLevel.Eventual
};

ItemResponse<Product> response = await container.ReadItemAsync<Product>(
    id: "p1",
    partitionKey: new PartitionKey("tenant-1"),
    requestOptions: options);
```

## Operacje przez .NET SDK v3

Pakiet:

```bash
dotnet add package Microsoft.Azure.Cosmos
dotnet add package Azure.Identity
```

Klasy:

- `CosmosClient` - singleton na aplikację; nie twórz per request.
- `Database` - baza.
- `Container` - kontener.
- `ItemResponse<T>` - odpowiedź dla operacji na itemach; zawiera m.in. `RequestCharge`, `ETag`, status.
- `FeedIterator<T>` - paginowane wyniki query.

CRUD:

```csharp
using Azure.Identity;
using Microsoft.Azure.Cosmos;

CosmosClient client = new(
    accountEndpoint: "https://cosmosaz204.documents.azure.com:443/",
    tokenCredential: new DefaultAzureCredential());

Database database = await client.CreateDatabaseIfNotExistsAsync("appdb");

Container container = await database.CreateContainerIfNotExistsAsync(
    id: "products",
    partitionKeyPath: "/tenantId",
    throughput: 400);

var product = new Product
{
    id = "p1",
    tenantId = "tenant-1",
    name = "Keyboard",
    price = 199
};

ItemResponse<Product> created = await container.CreateItemAsync(
    product,
    new PartitionKey(product.tenantId));

Console.WriteLine($"RU: {created.RequestCharge}");

ItemResponse<Product> read = await container.ReadItemAsync<Product>(
    id: "p1",
    partitionKey: new PartitionKey("tenant-1"));

read.Resource.price = 179;

await container.ReplaceItemAsync(
    read.Resource,
    id: read.Resource.id,
    partitionKey: new PartitionKey(read.Resource.tenantId),
    requestOptions: new ItemRequestOptions
    {
        IfMatchEtag = read.ETag
    });

await container.DeleteItemAsync<Product>(
    id: "p1",
    partitionKey: new PartitionKey("tenant-1"));

public class Product
{
    public string id { get; set; } = default!;
    public string tenantId { get; set; } = default!;
    public string name { get; set; } = default!;
    public decimal price { get; set; }
}
```

Query:

```csharp
QueryDefinition query = new(
    "SELECT * FROM c WHERE c.tenantId = @tenantId AND c.price < @maxPrice");

query.WithParameter("@tenantId", "tenant-1");
query.WithParameter("@maxPrice", 200);

FeedIterator<Product> iterator = container.GetItemQueryIterator<Product>(
    query,
    requestOptions: new QueryRequestOptions
    {
        PartitionKey = new PartitionKey("tenant-1"),
        MaxItemCount = 50
    });

while (iterator.HasMoreResults)
{
    FeedResponse<Product> page = await iterator.ReadNextAsync();
    Console.WriteLine($"Page RU: {page.RequestCharge}");

    foreach (Product item in page)
    {
        Console.WriteLine(item.name);
    }
}
```

Best practices:

- Używaj jednego `CosmosClient` jako singleton.
- Podawaj partition key dla point reads/writes.
- Preferuj point read (`ReadItemAsync`) nad query, gdy znasz `id` i partition key.
- Ogranicz cross-partition queries.
- Mierz `RequestCharge`.
- Włącz bulk mode dla dużych zapisów.
- Ustaw `EnableContentResponseOnWrite = false`, jeśli nie potrzebujesz zwracanego dokumentu po write.
- Dostosuj indexing policy, jeśli niektórych pól nigdy nie queryujesz.

## Indexing i queries

Cosmos DB for NoSQL automatycznie indeksuje właściwości dokumentów, ale można zmieniać indexing policy.

Warto znać:

- Zapytania są case-sensitive.
- `FROM c` używa aliasu kontenera.
- JOIN działa w ramach jednego dokumentu/kontenera, typowo do rozbijania tablic, nie jako relacyjny join między kontenerami.
- `ORDER BY` po wielu polach wymaga composite index.
- UDF można wywoływać w query, ale może zwiększyć koszt RU.

Przykład composite index:

```json
{
  "indexingMode": "consistent",
  "automatic": true,
  "includedPaths": [
    { "path": "/*" }
  ],
  "excludedPaths": [
    { "path": "/description/*" }
  ],
  "compositeIndexes": [
    [
      { "path": "/tenantId", "order": "ascending" },
      { "path": "/price", "order": "descending" }
    ]
  ]
}
```

## TTL

Time to live automatycznie usuwa itemy po czasie liczonym od ostatniej modyfikacji.

- TTL można ustawić na kontenerze.
- Item może nadpisać TTL własną wartością.
- `-1` na kontenerze oznacza, że TTL jest włączone, ale itemy nie wygasają domyślnie, chyba że item ma własne `ttl`.
- Usuwanie zużywa spare RU; przy małym throughput fizyczne usunięcie może się opóźnić.

.NET:

```csharp
ContainerProperties props = new("sessions", "/tenantId")
{
    DefaultTimeToLive = 3600
};

Container container = await database.CreateContainerIfNotExistsAsync(props);
```

Item z własnym TTL:

```json
{
  "id": "session-1",
  "tenantId": "tenant-1",
  "ttl": 300
}
```

## Change feed

Change feed to trwały zapis zmian w kontenerze. Jest włączony domyślnie i może być konsumowany asynchronicznie.

Najważniejsze:

- Rejestruje inserty i update'y.
- W trybie latest version nie pokazuje delete jako zdarzeń.
- Tryb all versions and deletes potrafi uwzględniać deletes i TTL expirations, ale wymaga continuous backup i jest traktowany inaczej niż podstawowy latest version mode.
- Kolejność jest gwarantowana w obrębie partition key; nie ma globalnej kolejności między różnymi partition key values.
- Odczyt change feed kosztuje RU.

Modele:

- **Push model** - Azure Functions trigger albo change feed processor. Najczęściej rekomendowany.
- **Pull model** - aplikacja sama pobiera zmiany i zarządza continuation tokenami.

Change feed processor:

- Monitorowany kontener - źródło zmian.
- Lease container - przechowuje stan/checkpointy i koordynuje wielu workerów.
- Delegate - kod biznesowy obsługujący zmiany.
- Delivery semantics: at least once, więc kod powinien być idempotentny.

.NET:

```csharp
Container monitored = client.GetContainer("appdb", "products");
Container leases = client.GetContainer("appdb", "leases");

ChangeFeedProcessor processor = monitored
    .GetChangeFeedProcessorBuilder<Product>(
        processorName: "products-processor",
        onChangesDelegate: HandleChangesAsync)
    .WithInstanceName(Environment.MachineName)
    .WithLeaseContainer(leases)
    .Build();

await processor.StartAsync();

static async Task HandleChangesAsync(
    ChangeFeedProcessorContext context,
    IReadOnlyCollection<Product> changes,
    CancellationToken cancellationToken)
{
    Console.WriteLine($"Lease: {context.LeaseToken}");
    Console.WriteLine($"RU: {context.Headers.RequestCharge}");

    foreach (Product product in changes)
    {
        // Idempotent processing: e.g. upsert projection, send event with deduplication key.
        Console.WriteLine(product.id);
    }

    await Task.CompletedTask;
}
```

Azure Functions trigger - idea:

```csharp
[FunctionName("ProductsChanged")]
public static void Run(
    [CosmosDBTrigger(
        databaseName: "appdb",
        containerName: "products",
        Connection = "CosmosConnection",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)]
    IReadOnlyList<Product> input,
    ILogger log)
{
    foreach (var item in input)
    {
        log.LogInformation("Changed item: {Id}", item.id);
    }
}
```

## Stored procedures, triggers, UDF

Cosmos DB for NoSQL wspiera JavaScript wykonywany po stronie serwera:

- **Stored procedures** - transakcyjne operacje w obrębie jednej logical partition.
- **Pre-triggers** - uruchamiane przed operacją create/replace/delete, ale muszą być jawnie wskazane w request options.
- **Post-triggers** - po operacji, w tej samej transakcji.
- **UDF** - funkcje używane w zapytaniach SQL.

Ważne:

- Nie uruchamiają się automatycznie bez rejestracji i wskazania.
- Mają limity czasu wykonania.
- Są ograniczone partition key scope.

## Global distribution i conflict resolution

Cosmos DB może działać globalnie:

- wiele regionów odczytu,
- opcjonalnie wiele regionów zapisu,
- manual/automatic failover,
- dodawanie/usuwanie regionów bez downtime.

Przy multiple write regions mogą pojawić się konflikty. Strategie:

- **Last Writer Wins** - automatyczna, na podstawie wskazanej właściwości/system timestamp.
- **Custom** - stored procedure do rozwiązywania konfliktów.

Na AZ-204 pamiętaj: global distribution wpływa na consistency, latency i availability.

## Security

Sposoby dostępu:

- Microsoft Entra ID + RBAC - preferowane dla aplikacji z Managed Identity.
- Account keys - pełny dostęp, ostrożnie.
- Resource tokens - ograniczony dostęp do zasobów dla użytkowników/aplikacji.

Dane:

- szyfrowanie at rest domyślnie,
- TLS in transit,
- możliwość użycia customer-managed keys,
- private endpoints i firewall/network rules dla ograniczenia sieci.

## Backup i restore

Modele:

- **Periodic backup** - domyślny; backupi wykonywane okresowo, restore przez support/portal zależnie od scenariusza.
- **Continuous backup** - point-in-time restore w okresie retencji, np. 7 albo 30 dni; wymagany dla all versions and deletes change feed.

## Monitoring

Monitoruj:

- RU consumption,
- throttling / HTTP 429,
- latency,
- availability,
- storage,
- normalized RU consumption per partition,
- query metrics,
- diagnostic logs do Log Analytics/Event Hubs/Storage.

Najczęstsza diagnoza:

- dużo 429 = za mało RU albo hot partition,
- drogie query = cross-partition query, brak filtra po partition key, zła indexing policy,
- duża latencja = region aplikacji daleko od regionu Cosmos DB albo gateway/direct mode/firewall.

## Tworzenie i zarządzanie - Azure Portal

Account:

1. Create a resource -> Azure Cosmos DB.
2. Wybierz API, najczęściej Azure Cosmos DB for NoSQL.
3. Ustaw account name, region, capacity mode: provisioned throughput albo serverless.
4. Ustaw apply free tier, jeśli dostępne i potrzebne.
5. Networking: public, selected networks albo private endpoint.
6. Backup policy: periodic albo continuous.
7. Review + create.

Database i container:

1. Account -> Data Explorer.
2. New Database.
3. New Container.
4. Podaj database id, container id, partition key path, np. `/tenantId`.
5. Ustaw throughput: manual, autoscale albo shared na database.
6. Opcjonalnie TTL, indexing policy, unique keys.

Consistency:

1. Account -> Settings -> Default consistency.
2. Wybierz Strong, Bounded Staleness, Session, Consistent Prefix albo Eventual.
3. Dla Bounded Staleness ustaw K/T.

Change feed:

1. Utwórz kontener monitorowany.
2. Utwórz lease container, np. `leases`, z partition key `/id`.
3. Skonfiguruj Azure Function Cosmos DB Trigger albo aplikację z change feed processor.

## Tworzenie i zarządzanie - Azure CLI

```bash
az group create \
  --name rg-az204 \
  --location westeurope

az cosmosdb create \
  --name cosmosaz204 \
  --resource-group rg-az204 \
  --locations regionName=westeurope failoverPriority=0 isZoneRedundant=False \
  --default-consistency-level Session

az cosmosdb sql database create \
  --account-name cosmosaz204 \
  --resource-group rg-az204 \
  --name appdb

az cosmosdb sql container create \
  --account-name cosmosaz204 \
  --resource-group rg-az204 \
  --database-name appdb \
  --name products \
  --partition-key-path "/tenantId" \
  --throughput 400

az cosmosdb sql container create \
  --account-name cosmosaz204 \
  --resource-group rg-az204 \
  --database-name appdb \
  --name leases \
  --partition-key-path "/id" \
  --throughput 400

az cosmosdb sql container throughput update \
  --account-name cosmosaz204 \
  --resource-group rg-az204 \
  --database-name appdb \
  --name products \
  --throughput 1000
```

Item przez CLI:

```bash
az cosmosdb sql container item create \
  --account-name cosmosaz204 \
  --resource-group rg-az204 \
  --database-name appdb \
  --container-name products \
  --partition-key-value "tenant-1" \
  --body '{
    "id": "p1",
    "tenantId": "tenant-1",
    "name": "Keyboard",
    "price": 199
  }'
```

Consistency update:

```bash
az cosmosdb update \
  --name cosmosaz204 \
  --resource-group rg-az204 \
  --default-consistency-level Eventual
```

## Tworzenie i zarządzanie - kod .NET

Minimalny przykład z Managed Identity/Entra ID:

```csharp
using Azure.Identity;
using Microsoft.Azure.Cosmos;

CosmosClientOptions options = new()
{
    ApplicationName = "az204-notes",
    ConnectionMode = ConnectionMode.Direct,
    EnableContentResponseOnWrite = false
};

CosmosClient client = new(
    accountEndpoint: "https://cosmosaz204.documents.azure.com:443/",
    tokenCredential: new DefaultAzureCredential(),
    clientOptions: options);

Database database = await client.CreateDatabaseIfNotExistsAsync("appdb");

ContainerProperties properties = new("products", "/tenantId")
{
    DefaultTimeToLive = -1
};

Container container = await database.CreateContainerIfNotExistsAsync(
    properties,
    throughput: 400);

var item = new Product
{
    id = "p1",
    tenantId = "tenant-1",
    name = "Keyboard",
    price = 199
};

await container.UpsertItemAsync(item, new PartitionKey(item.tenantId));

Product product = (await container.ReadItemAsync<Product>(
    item.id,
    new PartitionKey(item.tenantId))).Resource;

Console.WriteLine(product.name);
```

## Porównanie z arvigeus/AZ-204

Repozytorium `arvigeus/AZ-204` zawiera bardzo bogate notatki. Do powtórki pod AZ-204 szczególnie przydatne są tam:

- account/database/container/item,
- TTL,
- CLI i .NET SDK przykłady CRUD,
- consistency levels z przykładami zastosowań,
- partitioning i synthetic partition keys,
- RU/s, provisioned/shared/autoscale/serverless,
- API models,
- stored procedures, triggers i UDF,
- change feed processor,
- query syntax, composite indexes,
- connectivity modes,
- global distribution, conflict resolution, security, backup, monitoring.

Różnice względem oficjalnego zakresu:

- Oficjalny study guide dla AZ-204 wymienia Cosmos DB wężej: SDK operations na containers/items, consistency i change feed.
- Notatki `arvigeus` rozszerzają temat o elementy architektoniczne i administracyjne. To jest dobre do zrozumienia usługi, ale do egzaminu w pierwszej kolejności opanuj .NET SDK, partition key, RU, consistency i change feed.

## Najczęstsze pułapki egzaminacyjne

- Bez `id` + partition key nie masz efektywnego point read.
- Zły partition key powoduje hot partitions i throttling mimo wysokiego RU/s.
- Session consistency wymaga session tokenów; po odtworzeniu klienta token cache znika.
- SDK może osłabić consistency na odczycie, ale nie wzmocnić ponad default konta.
- Change feed latest version mode nie daje klasycznych delete events.
- Change feed processor ma at-least-once delivery, więc handler musi być idempotentny.
- Stored procedures/triggers są transakcyjne tylko w jednej logical partition.
- Cross-partition query kosztuje więcej RU niż query zawężone partition key.
- Jeden `CosmosClient` powinien być singletonem w aplikacji.
