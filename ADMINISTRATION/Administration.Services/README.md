# Administration.Servies #

Бизнес слой сервиса управления административным корпусом.

## IDepartmentsManagement ##

Интерфейс управления отделами (создание, редактирование, вывод списков).

### DepartmentsManagementSQLServer ###

Реализация интерфейса IDepartmentsManagement для взаимодействия с DBContextSQLServer.

```c#
services.AddScoped<IDepartmentsManagement, DepartmentsManagementSQLServer>();
```
