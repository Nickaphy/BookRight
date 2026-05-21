// Data Transfer Object
// Used to move data between Facade and UI
// UI should not work directly with Domain entities
// DTOs keep the UI separated from internal business rules

/*Blazor UI
? calls Facade interfaces

Facade
? hides Application complexity

Application
? executes commands and queries

Domain
? protects business rules*/
