# EF Core - ladowanie encji powiazanych (related entities)

Poniższy opis jest oparty o dokumentacje Microsoft Learn (Entity Framework Core).
Link do glownego rozdzialu: https://learn.microsoft.com/ef/core/querying/related-data/

EF Core opisuje trzy najczesciej spotykane wzorce ORM do ladowania danych
powiazanych:
- eager loading
- explicit loading
- lazy loading

## 1) Eager loading

**Na czym polega (wg MS docs)**
Powiazane dane sa ladowane z bazy juz w trakcie wykonywania zapytania glownego.
Najczesciej robi sie to przez `Include()` i `ThenInclude()`.

**Przyklad (z dokumentacji)**
```csharp
var blogs = await context.Blogs
	.Include(blog => blog.Posts)
	.ToListAsync();
```

**Plusy**
- Jedno zapytanie (albo mala liczba zapytan), przewidywalne zachowanie.
- Dobre, gdy wiesz, ze relacje sa zawsze potrzebne.

**Minusy / uwagi z MS docs**
- Eager loading kolekcji w pojedynczym zapytaniu moze powodowac problemy
  wydajnosciowe. Microsoft odsyla do tematu single vs split queries:
  https://learn.microsoft.com/ef/core/querying/single-split-queries
  
	**Dokladniej (wg MS docs):**
	- Gdy ladowana jest kolekcja przez JOIN w jednym zapytaniu, wiersze w wyniku
		ulegaja duplikacji (ten sam obiekt glownej encji powtarza sie dla kazdego
		elementu kolekcji). To zwieksza rozmiar danych przesylanych i koszt
		materializacji w EF Core.
	- Przy wielu kolekcjach w `Include()` problem moze sie nasilac, bo zapytanie
		zaczyna tworzyc kombinacje wielu relacji (tzw. cartesian explosion).
	- MS docs sugeruje rozwazenie zapytan dzielonych (split query), gdzie EF Core
		wykonuje kilka zapytan SQL zamiast jednego JOIN. Mozesz to wymusic np. przez
		`AsSplitQuery()` albo `AsSingleQuery()` w zaleznosci od scenariusza.
  
	**Krotki obraz problemu:**
	- `Blog` ma 1 000 postow. `Include(blog => blog.Posts)` w trybie single query
		zwroci 1 000 wierszy z powtorzonymi danymi bloga, co zwieksza transfer i
		pamiec.

	**Przyklad SQL (single query, uproszczony):**
	```sql
	SELECT b.Id, b.Name, p.Id, p.Title, p.BlogId
	FROM Blogs AS b
	LEFT JOIN Posts AS p ON b.Id = p.BlogId
	WHERE b.Id = 1;
	```
	W tym wyniku dane `Blogs` powtarzaja sie dla kazdego wiersza z `Posts`.

	**Przyklad SQL (split query, uproszczony):**
	```sql
	SELECT b.Id, b.Name
	FROM Blogs AS b
	WHERE b.Id = 1;

	SELECT p.Id, p.Title, p.BlogId
	FROM Posts AS p
	WHERE p.BlogId = 1;
	```
	EF Core scala wyniki po stronie klienta, bez duplikacji danych glownej encji.

	**Kiedy realnie uzywac `AsSplitQuery()` (wg MS docs i praktyki):**
	- Gdy `Include()` laduje duze kolekcje i widzisz duza duplikacje danych.
	- Gdy masz wiele `Include()` na kolekcjach i pojawia sie „cartesian explosion”.
	- Gdy wolniejsze pojedyncze zapytanie jest gorsze niz kilka mniejszych.

	**Kiedy zostac przy `AsSingleQuery()`**
	- Gdy laczysz tylko kilka malych relacji i chcesz minimalnej liczby rund.
	- Gdy transakcja i spojnosc odczytu w jednym zapytaniu jest priorytetem.

	**Przyklad uzycia:**
	```csharp
	var blogs = await context.Blogs
			.Include(b => b.Posts)
			.AsSplitQuery()
			.ToListAsync();
	```

**Dokumentacja**
- https://learn.microsoft.com/ef/core/querying/related-data/eager

---

## 2) Lazy loading

**Na czym polega (wg MS docs)**
Powiazane dane sa ladowane automatycznie dopiero wtedy, gdy nastapi odwolanie
do wlasciwosci nawigacyjnej.

**Jak sie to robi (wg MS docs)**
Najprostsza opcja to pakiet `Microsoft.EntityFrameworkCore.Proxies` i
`UseLazyLoadingProxies()`. Wlasciwosci nawigacyjne musza byc `virtual`.

**Przyklad (z dokumentacji)**
```csharp
optionsBuilder
	.UseLazyLoadingProxies()
	.UseSqlServer(myConnectionString);

public class Blog
{
	public int Id { get; set; }
	public string Name { get; set; }
	public virtual ICollection<Post> Posts { get; set; }
}
```

**Plusy**
- Pobierasz tylko to, do czego faktycznie sie odwolales.
- Proste API w kodzie domenowym.

**Minusy / ostrzezenie z MS docs**
- Lazy loading moze powodowac dodatkowe rundy do bazy (problem N+1).
  Microsoft ostrzega przed tym w sekcji performance:
  https://learn.microsoft.com/ef/core/performance/efficient-querying#beware-of-lazy-loading
  
	**Dokladniej (wg MS docs):**
	- N+1 oznacza, ze najpierw pobierasz liste encji glownej (1 zapytanie),
		a potem dla kazdej z nich EF Core wykonuje kolejne zapytanie po relacji.
	- Przy wiekszej liczbie rekordow moze to oznaczac dziesiatki lub setki
		dodatkowych rund do bazy, co znacznie zwalnia aplikacje.
	- Problem jest trudny do zauwazenia w kodzie, bo wywolanie relacji wyglada
		jak zwykle odwolanie do wlasciwosci.
  
	**Krotki obraz problemu:**
	- `var blogs = context.Blogs.ToList();` (1 zapytanie)
	- petla po `blogs` i odwolanie do `blog.Posts` generuje osobne zapytanie
		dla kazdego bloga (N zapytan)

**Dokumentacja**
- https://learn.microsoft.com/ef/core/querying/related-data/lazy

---

## 3) Explicit loading

**Na czym polega (wg MS docs)**
Powiazane dane sa ladowane jawnie, po pobraniu encji glownej. Uzywa sie
`DbContext.Entry(...)` i metod `Reference(...).LoadAsync()` albo
`Collection(...).LoadAsync()`.

**Przyklad (z dokumentacji)**
```csharp
var blog = await context.Blogs
	.SingleAsync(b => b.BlogId == 1);

await context.Entry(blog)
	.Collection(b => b.Posts)
	.LoadAsync();

await context.Entry(blog)
	.Reference(b => b.Owner)
	.LoadAsync();
```

**Plusy**
- Masz pelna kontrole nad tym, kiedy i co jest ladowane.
- Dobre, gdy decyzja o ladowaniu zalezy od logiki biznesowej.

**Minusy**
- Wiecej kodu i wiecej miejsc, gdzie latwo zapomniec o doladowaniu.
- Latwo wygenerowac duzo zapytan, jesli robisz to w petlach.

**Dokumentacja**
- https://learn.microsoft.com/ef/core/querying/related-data/explicit

---

## Krotkie porownanie (na podstawie MS docs)

| Strategia        | Kiedy laduje?                          | Kontrola | Ryzyko N+1 |
|------------------|----------------------------------------|----------|------------|
| Eager loading    | W trakcie zapytania glownego           | Srednia  | Niskie     |
| Lazy loading     | Przy odwolaniu do relacji              | Niska    | Wysokie    |
| Explicit loading | Gdy jawnie wywolasz `Load()`            | Wysoka   | Srednie    |

## Zrodla (Microsoft Learn)

- https://learn.microsoft.com/ef/core/querying/related-data/
- https://learn.microsoft.com/ef/core/querying/related-data/eager
- https://learn.microsoft.com/ef/core/querying/related-data/lazy
- https://learn.microsoft.com/ef/core/querying/related-data/explicit
