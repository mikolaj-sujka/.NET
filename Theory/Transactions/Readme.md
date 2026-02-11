
# C# Ambient Transactions: What They Are and Why They Matter

## Czym są transakcje ambientowe?
Transakcja ambientowa to „aktualna” transakcja, do której automatycznie dołączają się zasoby w obrębie wykonywanego kodu. W .NET jest ona dostępna jako `Transaction.Current` i zarządzana najczęściej przez `TransactionScope`, które tworzy lub dołącza do istniejącej transakcji bez konieczności jawnego przekazywania obiektu transakcji między metodami.

Najważniejsza idea: otaczasz blok kodu zakresem (`TransactionScope`), a infrastruktura `System.Transactions` sama zarządza kontekstem transakcji (tworzenie, dołączanie, commit/rollback). Dzięki temu kod aplikacji jest prostszy, a spójność danych jest zapewniona w jednym, logicznym „unit of work”.

## Dlaczego to ma znaczenie?
1. **Mniej boilerplate** – nie trzeba ręcznie przekazywać obiektu transakcji przez wiele warstw ani ręcznie enlistować zasobów.
2. **Spójność i bezpieczeństwo** – jeśli coś się nie powiedzie, cały zakres jest wycofywany (rollback), o ile nie wywołasz `Complete()`.
3. **Jednolity model programowania** – `System.Transactions` wspiera zarówno model jawny, jak i implicit, ale Microsoft rekomenduje używanie `TransactionScope` jako prostszego i bardziej efektywnego podejścia.
4. **Automatyczne dołączanie zasobów** – zasoby zgodne z `System.Transactions` (np. SQL Server) automatycznie wykrywają i dołączają się do transakcji ambientowej.

## Jak to działa w praktyce?
- Tworzysz `TransactionScope`.
- Kod wewnątrz scope działa w ramach transakcji ambientowej (`Transaction.Current`).
- Wywołujesz `Complete()` aby „zagłosować” za commit.
- Brak `Complete()` lub wyjątek oznacza rollback.

## Uwagi praktyczne
- `Transaction.Current` jest ustawiane per wątek (thread-static), więc przy programowaniu asynchronicznym warto używać odpowiednich opcji przepływu transakcji w `TransactionScope`.
- Zagnieżdżone scope’y mogą dołączać do transakcji ambientowej lub tworzyć nowe w zależności od `TransactionScopeOption` (`Required`, `RequiresNew`, `Suppress`).

## Oficjalne źródła (Microsoft Learn)
- `TransactionScope` – opis, uwagi i rekomendacje: https://learn.microsoft.com/en-us/dotnet/api/system.transactions.transactionscope
- `Transaction.Current` – ambient transaction property: https://learn.microsoft.com/en-us/dotnet/api/system.transactions.transaction.current
- „Implementing an Implicit Transaction using TransactionScope” – pełne omówienie modelu implicit: https://learn.microsoft.com/en-us/dotnet/framework/data/transactions/implementing-an-implicit-transaction-using-transaction-scope
- „Writing a Transactional Application” – przegląd modeli transakcyjnych w `System.Transactions`: https://learn.microsoft.com/en-us/dotnet/framework/data/transactions/writing-a-transactional-application

