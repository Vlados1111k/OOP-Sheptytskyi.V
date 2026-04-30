# CinemaPeak

**CinemaPeak** — система управління кінотеатром (Capstone-проєкт).

## Статус проекту
- **Lab 34 (Ітерація 1):** Завершено. Створено архітектурний каркас (N-Tier) та базову модель фільму.
- **Lab 35 (Ітерація 2):** Завершено. Додано JSON-збереження та Strategy Pattern.

## Структура
- `Cinema.Domain` — Сутності.
- `Cinema.Application` — Бізнес-логіка.
- `Cinema.Infrastructure` — Робота з даними.
- `Cinema.Console` — Інтерфейс користувача.

## Запуск
```bash
dotnet run --project src/Cinema.Console