# CinemaPeak

**CinemaPeak** — система управління кінотеатром (Capstone-проєкт).

## Статус проекту
- **Lab 34 (Ітерація 1):** Завершено. Створено архітектурний каркас (N-Tier) та базову модель фільму.
- **Lab 35 (Ітерація 2):** Завершено. Додано JSON-збереження та Strategy Pattern.
- **Lab 36 (Ітерація 3):** Завершено. Впроваджено автоматизоване тестування (xUnit) та перевірку якості коду (Quality Gate).

## Структура
- `Cinema.Domain` — Сутності та інтерфейси.
- `Cinema.Application` — Бізнес-логіка та сервіси.
- `Cinema.Infrastructure` — Робота з JSON-сховищем.
- `Cinema.Console` — Інтерфейс користувача.
- `Cinema.Tests` — Юніт-тести для перевірки логіки.

## Запуск
```bash
dotnet run --project src/Cinema.Console