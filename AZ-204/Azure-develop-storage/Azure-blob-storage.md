# Azure Blob Storage - notatki pod AZ-204

Stan na: 2026-05-02. Egzamin AZ-204 według Microsoft Learn obejmuje dział **Develop for Azure storage** jako 15-20% egzaminu. Dla Blob Storage trzeba umieć:

- ustawiać i odczytywać properties oraz metadata,
- wykonywać operacje na danych przez odpowiedni SDK,
- implementować storage policies i lifecycle management.

Oficjalna ścieżka Microsoft Learn dla AZ-204 kładzie nacisk na tworzenie zasobów Blob Storage, zarządzanie lifecycle danych i pracę z kontenerami oraz blobami przez Azure Blob Storage client library v12 dla .NET.

## Źródła

- Microsoft Learn - study guide AZ-204: https://learn.microsoft.com/en-us/credentials/certifications/resources/study-guides/az-204
- Microsoft Learn - Develop solutions that use Blob storage: https://learn.microsoft.com/en-us/training/paths/develop-solutions-that-use-blob-storage/
- Microsoft Learn - Access tiers for blob data: https://learn.microsoft.com/en-us/azure/storage/blobs/access-tiers-overview
- Microsoft Learn - Manage blob properties and metadata with .NET: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-properties-metadata
- Microsoft Learn - Blob lifecycle management overview: https://learn.microsoft.com/en-us/azure/storage/blobs/lifecycle-management-overview
- Microsoft Learn - Lifecycle policy structure: https://learn.microsoft.com/en-us/azure/storage/blobs/lifecycle-management-policy-structure
- Microsoft Learn - Soft delete for blobs: https://learn.microsoft.com/en-us/azure/storage/blobs/soft-delete-blob-overview
- Notatki porównawcze: https://github.com/arvigeus/AZ-204/blob/master/Topics/Blob%20Storage.md

## Co to jest Blob Storage

Azure Blob Storage to object storage dla danych nieustrukturyzowanych: plików, obrazów, backupów, logów, dokumentów, streamingu audio/wideo i danych do analizy.

Hierarchia:

- **Storage account** - konto Azure Storage, endpoint np. `https://<account>.blob.core.windows.net`.
- **Container** - logiczny zbiór blobów, podobny do katalogu.
- **Blob** - obiekt danych w kontenerze.

Typy blobów:

- **Block blob** - najczęstszy typ; pliki tekstowe i binarne. Ważny dla upload/download i access tiers.
- **Append blob** - dopisywanie danych na końcu; dobry dla logów.
- **Page blob** - losowy dostęp do stron; używany m.in. przez dyski/VHD.

Na egzaminie najczęściej zakładaj **general-purpose v2 storage account** i **block blobs**, bo to jest standard dla nowych rozwiązań i lifecycle/access tiers.

## Redundancja

Warto znać skróty, bo często pojawiają się w pytaniach projektowych:

- **LRS** - 3 kopie w jednym datacenter/regionie.
- **ZRS** - kopie między availability zones w regionie.
- **GRS** - LRS + asynchroniczna replikacja do regionu parowanego.
- **RA-GRS** - GRS z read access do regionu secondary.
- **GZRS** - ZRS w regionie primary + replikacja do secondary.
- **RA-GZRS** - GZRS z read access do secondary.

Archive tier nie wspiera ZRS/GZRS/RA-GZRS. Dla archive używaj LRS, GRS albo RA-GRS.

## Authorization i access

Sposoby autoryzacji:

- **Microsoft Entra ID + RBAC** - preferowane dla aplikacji produkcyjnych. W .NET zwykle `DefaultAzureCredential`.
- **Shared Key** - klucz storage account; bardzo szeroki dostęp, używać ostrożnie.
- **SAS** - delegated access z ograniczeniem czasu, uprawnień i zakresu.
- **Anonymous public access** - domyślnie blokowany na poziomie konta; nawet jeśli kontener ma publiczny dostęp, konto może go zabronić.

Role RBAC do danych:

- `Storage Blob Data Reader` - odczyt danych blob.
- `Storage Blob Data Contributor` - odczyt/zapis/usuwanie blobów.
- `Storage Blob Data Owner` - pełne prawa do danych i ACL.
- `Storage Blob Delegator` - potrzebne do generowania user delegation SAS.

W pytaniach egzaminacyjnych: jeśli aplikacja działa na Azure i ma Managed Identity, wybieraj Entra ID/RBAC zamiast connection stringów i kluczy.

## Properties i metadata

**System properties** to właściwości HTTP/Blob Storage, np. `Content-Type`, `Content-Encoding`, `Cache-Control`, `Content-Length`, `ETag`, `Last-Modified`.

**Metadata** to własne pary key-value przypięte do kontenera lub bloba.

Ważne zasady:

- Metadata są nagłówkami HTTP, więc nazwy powinny być ASCII i zgodne z zasadami nagłówków.
- Całkowity rozmiar metadata jest ograniczony.
- `SetMetadata` nadpisuje cały zestaw metadata, nie robi partial update.
- `SetHttpHeaders` może wyczyścić properties, których nie przekażesz, więc przed zmianą jednej property najpierw pobierz istniejące properties.
- `ETag` służy do optimistic concurrency: operacja może wykonać się tylko, jeśli blob nadal ma oczekiwany ETag.

Przykład .NET - metadata i properties:

```csharp
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

var accountName = "mystorageaccount";
var service = new BlobServiceClient(
    new Uri($"https://{accountName}.blob.core.windows.net"),
    new DefaultAzureCredential());

BlobContainerClient container = service.GetBlobContainerClient("docs");
BlobClient blob = container.GetBlobClient("readme.txt");

BlobProperties current = await blob.GetPropertiesAsync();

var headers = new BlobHttpHeaders
{
    ContentType = "text/plain",
    ContentLanguage = current.ContentLanguage,
    CacheControl = current.CacheControl,
    ContentDisposition = current.ContentDisposition,
    ContentEncoding = current.ContentEncoding,
    ContentHash = current.ContentHash
};

await blob.SetHttpHeadersAsync(headers);

await blob.SetMetadataAsync(new Dictionary<string, string>
{
    ["docType"] = "notes",
    ["exam"] = "AZ204"
});

BlobProperties updated = await blob.GetPropertiesAsync();
Console.WriteLine(updated.ETag);
```

Przykład z warunkiem ETag:

```csharp
BlobProperties props = await blob.GetPropertiesAsync();

await blob.UploadAsync(
    BinaryData.FromString("new content"),
    new BlobUploadOptions
    {
        Conditions = new BlobRequestConditions
        {
            IfMatch = props.ETag
        }
    });
```

## Operacje na danych przez SDK

Najważniejsze klasy w Azure Storage Blobs v12:

- `BlobServiceClient` - poziom storage account/blob service.
- `BlobContainerClient` - kontener.
- `BlobClient` - ogólny blob.
- `BlockBlobClient`, `AppendBlobClient`, `PageBlobClient` - typy specjalizowane.

Podstawowy CRUD:

```csharp
using Azure.Identity;
using Azure.Storage.Blobs;

var service = new BlobServiceClient(
    new Uri("https://mystorageaccount.blob.core.windows.net"),
    new DefaultAzureCredential());

BlobContainerClient container =
    await service.CreateBlobContainerAsync("images");

BlobClient blob = container.GetBlobClient("logo.png");

await blob.UploadAsync("logo.png", overwrite: true);

await foreach (var item in container.GetBlobsAsync())
{
    Console.WriteLine(item.Name);
}

await blob.DownloadToAsync("logo-downloaded.png");

await blob.DeleteIfExistsAsync();
```

Append blob:

```csharp
AppendBlobClient append = container.GetAppendBlobClient("app.log");
await append.CreateIfNotExistsAsync();

using var stream = BinaryData.FromString("new log line\n").ToStream();
await append.AppendBlockAsync(stream);
```

Retry options:

```csharp
using Azure.Core;
using Azure.Storage.Blobs;

var options = new BlobClientOptions
{
    Retry =
    {
        Mode = RetryMode.Exponential,
        MaxRetries = 5,
        Delay = TimeSpan.FromSeconds(2),
        MaxDelay = TimeSpan.FromSeconds(30)
    }
};
```

## Access tiers

Access tier wpływa na koszt przechowywania, koszt dostępu i latency.

- **Hot** - często używane/zmieniane dane; wyższy koszt przechowywania, niższy koszt operacji.
- **Cool** - rzadziej używane dane; minimum zalecane 30 dni.
- **Cold** - rzadko używane, ale nadal online; minimum zalecane 90 dni.
- **Archive** - offline, najtańsze przechowywanie, odczyt dopiero po rehydration; minimum zalecane 180 dni.
- **Smart tier** - automatyczne przenoszenie między hot/cool/cold na podstawie użycia.

Ważne:

- Access tiers dotyczą **block blobs**.
- Archive jest offline; nie można bezpośrednio czytać danych, trzeba rehydrate do hot/cool/cold.
- Rehydration z archive może potrwać do kilkunastu godzin, zależnie od priorytetu.
- Archive nie może być domyślnym tierem storage account.
- Lifecycle policy nie służy do rehydratacji archive do online tier.

CLI:

```bash
az storage account create \
  --name mystorage204 \
  --resource-group rg-az204 \
  --location westeurope \
  --sku Standard_LRS \
  --kind StorageV2 \
  --access-tier Hot

az storage blob set-tier \
  --account-name mystorage204 \
  --container-name docs \
  --name archive/report.pdf \
  --tier Archive \
  --auth-mode login

az storage blob set-tier \
  --account-name mystorage204 \
  --container-name docs \
  --name archive/report.pdf \
  --tier Hot \
  --rehydrate-priority Standard \
  --auth-mode login
```

## Lifecycle management

Lifecycle management to rule-based policy na storage account. Służy do:

- przenoszenia blobów do tańszych tierów,
- usuwania blobów, snapshots albo previous versions,
- filtrowania po prefixach i blob index tags,
- automatyzacji kosztów.

Nie działa na systemowych kontenerach typu `$logs` i `$web`.

Przykład policy JSON:

```json
{
  "rules": [
    {
      "enabled": true,
      "name": "move-old-logs",
      "type": "Lifecycle",
      "definition": {
        "filters": {
          "blobTypes": ["blockBlob"],
          "prefixMatch": ["logs/"]
        },
        "actions": {
          "baseBlob": {
            "tierToCool": {
              "daysAfterModificationGreaterThan": 30
            },
            "tierToArchive": {
              "daysAfterModificationGreaterThan": 180
            },
            "delete": {
              "daysAfterModificationGreaterThan": 365
            }
          }
        }
      }
    }
  ]
}
```

CLI:

```bash
az storage account management-policy create \
  --account-name mystorage204 \
  --resource-group rg-az204 \
  --policy @policy.json
```

Portal:

1. Storage account.
2. Data management -> Lifecycle management.
3. Add rule.
4. Wybierz scope, blob types, prefix/tag filters.
5. Ustaw akcje: move to cool/cold/archive albo delete.

## Data protection

Funkcje, które warto znać:

- **Blob soft delete** - chroni blob/snapshot/version przed przypadkowym usunięciem lub nadpisaniem przez ustawiony retention period.
- **Container soft delete** - odzyskiwanie usuniętego kontenera.
- **Blob versioning** - automatyczne poprzednie wersje po nadpisaniu/usunięciu.
- **Snapshots** - ręczny point-in-time read-only stan bloba.
- **Immutable storage / legal hold** - WORM, ochrona przed modyfikacją/usunięciem przez czas retencji.

Microsoft rekomenduje łączyć container soft delete, blob versioning i blob soft delete jako bazową ochronę danych blob.

Przykład snapshot:

```csharp
BlobSnapshotInfo snapshot = await blob.CreateSnapshotAsync();

await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
```

## Leases

Lease daje tymczasową wyłączność zapisu/usuwania dla bloba. Przydatne do blokady konkurencyjnych writerów.

- Czas trwania: skończony albo infinite.
- Operacje: acquire, renew, change, release, break.
- Operacje modyfikujące leased blob muszą podać lease ID.

CLI:

```bash
lease_id=$(az storage blob lease acquire \
  --account-name mystorage204 \
  --container-name docs \
  --blob-name locked.txt \
  --lease-duration 60 \
  --auth-mode login \
  --query leaseId \
  --output tsv)

az storage blob lease release \
  --account-name mystorage204 \
  --container-name docs \
  --blob-name locked.txt \
  --lease-id "$lease_id" \
  --auth-mode login
```

## Tworzenie i zarządzanie - Azure Portal

Storage account:

1. Create a resource -> Storage account.
2. Wybierz subscription, resource group, name, region.
3. Performance: Standard/Premium.
4. Redundancy: LRS/ZRS/GRS/GZRS itd.
5. Networking: public access, selected networks albo private endpoint.
6. Data protection: soft delete, versioning, change feed, point-in-time restore według potrzeb.
7. Review + create.

Container i blob:

1. Storage account -> Data storage -> Containers.
2. Create container.
3. Ustaw public access level, zwykle Private.
4. Wejdź do kontenera -> Upload.
5. W properties bloba możesz zmieniać tier, metadata i HTTP headers.

Lifecycle:

1. Storage account -> Data management -> Lifecycle management.
2. Dodaj regułę.
3. Ustaw warunki wieku, prefixy, blob index tags i akcje.

## Tworzenie i zarządzanie - Azure CLI

```bash
az group create \
  --name rg-az204 \
  --location westeurope

az storage account create \
  --name mystorage204 \
  --resource-group rg-az204 \
  --location westeurope \
  --sku Standard_LRS \
  --kind StorageV2 \
  --access-tier Hot \
  --allow-blob-public-access false

az storage container create \
  --account-name mystorage204 \
  --name docs \
  --public-access off \
  --auth-mode login

az storage blob upload \
  --account-name mystorage204 \
  --container-name docs \
  --name notes/readme.txt \
  --file ./readme.txt \
  --overwrite \
  --auth-mode login

az storage blob list \
  --account-name mystorage204 \
  --container-name docs \
  --output table \
  --auth-mode login

az storage blob metadata update \
  --account-name mystorage204 \
  --container-name docs \
  --name notes/readme.txt \
  --metadata exam=AZ204 type=notes \
  --auth-mode login

az storage blob download \
  --account-name mystorage204 \
  --container-name docs \
  --name notes/readme.txt \
  --file ./downloaded-readme.txt \
  --auth-mode login
```

RBAC dla użytkownika lub managed identity:

```bash
az role assignment create \
  --assignee <principal-id-or-user-upn> \
  --role "Storage Blob Data Contributor" \
  --scope "/subscriptions/<sub-id>/resourceGroups/rg-az204/providers/Microsoft.Storage/storageAccounts/mystorage204"
```

## Tworzenie i zarządzanie - kod .NET

Pakiety:

```bash
dotnet add package Azure.Storage.Blobs
dotnet add package Azure.Identity
```

Przykład:

```csharp
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

var accountName = "mystorage204";
var service = new BlobServiceClient(
    new Uri($"https://{accountName}.blob.core.windows.net"),
    new DefaultAzureCredential());

BlobContainerClient container =
    service.GetBlobContainerClient("docs");

await container.CreateIfNotExistsAsync(PublicAccessType.None);

BlobClient blob = container.GetBlobClient("notes/readme.txt");

await blob.UploadAsync(
    BinaryData.FromString("AZ-204 Blob Storage notes"),
    overwrite: true);

await blob.SetMetadataAsync(new Dictionary<string, string>
{
    ["exam"] = "AZ204",
    ["topic"] = "blob-storage"
});

await foreach (BlobItem item in container.GetBlobsAsync())
{
    Console.WriteLine($"{item.Name} {item.Properties.ContentLength}");
}

string content = (await blob.DownloadContentAsync()).Value.Content.ToString();
Console.WriteLine(content);
```

## Porównanie z arvigeus/AZ-204

Repozytorium `arvigeus/AZ-204` ma bardzo szerokie notatki, często bardziej szczegółowe niż minimum z oficjalnego study guide. Do moich notatek warto było przenieść lub zaakcentować:

- hierarchię storage account -> container -> blob,
- typy blobów: block, append, page,
- access tiers, rehydration i lifecycle policies,
- properties, metadata, ETag i access conditions,
- authorization: Entra ID, Shared Key, SAS, public access,
- leases,
- data protection: snapshots, versioning, soft delete,
- podstawowe operacje CLI, SDK i HTTP.

Różnice względem oficjalnego zakresu:

- Oficjalny AZ-204 wymienia dla Blob Storage głównie SDK operations, properties/metadata i lifecycle. 
- Notatki `arvigeus` dodają dużo materiału architektonicznego: redundancję, encryption scopes, object replication, AzCopy, network access rules. To jest przydatne praktycznie, ale na egzaminie dla Blob Storage priorytetem są operacje developerskie i lifecycle.

## Najczęstsze pułapki egzaminacyjne

- `SetMetadata` nadpisuje metadata, nie aktualizuje pojedynczego pola.
- Przy ustawianiu HTTP headers przez SDK nie pomijaj istniejących properties, jeśli chcesz je zachować.
- Archive tier jest offline i wymaga rehydration.
- Lifecycle management może przenosić do chłodniejszych tierów i usuwać, ale nie rehydratuje archive do online.
- Anonymous public access może być zablokowany na poziomie storage account.
- Do aplikacji Azure preferuj Managed Identity + RBAC, nie account key.
- `ETag` + `IfMatch` to optimistic concurrency.
- Lease wymaga `LeaseId` przy modyfikacji leased blob.
