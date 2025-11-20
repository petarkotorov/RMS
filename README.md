# RMS – Retail Management System
Technologies: .NET 9, Blazor WebAssembly, Entity Framework Core, Outbox Pattern, Background Hosted Services  

## Overview

Този solution съдържа пълна двупосочна синхронизация между Central API и множество Store API инстанции.  
Системата имплементира Outbox Pattern за гарантирано доставяне на събития, издръжливост и комуникация между системите.

## Projects

### Central.API
- Управлява главния каталог с продукти.
- Генерира SyncEvent-и при създаване, редактиране и изтриване на продукти.
- Записва събития в таблица (`SyncWorkers`).
- Фонов Hosted Service изпраща pending събития към съответните Store API.

### Store.API
- Получава SyncEvent-и от Central.
- Приема CRUD операции локално и генерира SyncEvent-и към Central.
- Също използва Outbox + HostedService.

### Shared
- Съдържа общите модели (`ProductModel`, `SyncEvent`) използвани от всички проекти.

### Central.Client / Store.Client
- Blazor WebAssembly фронтове.
- Central Client управлява продукти на централно ниво.
- Store Client управлява локален Store.

### Central.Tests / Store.Tests
- xUnit тестове.
- Използват InMemory EF Core.

---

## Synchronization Flow

1. Central създава/редактира/изтрива продукт → генерира SyncEvent, ако се отнася за определен Store → записва го в Outbox.
2. background HostedService изпраща събитието към конкретния Store (по DestinationStore).
3. Store получава SyncEvent.
4. Store обновява локалната база.
5. Store създава/редактира/изтрива продукт → генерира SyncEvent за Central → записва го в Outbox.
6. background HostedService изпраща събитието към Central.
7. Central получава SyncEvent.
8. Central обновява локалната база.



