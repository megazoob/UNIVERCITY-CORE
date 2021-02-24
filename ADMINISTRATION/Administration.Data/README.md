# Administration.Data #

Слой данных (модели, контекст) сервиса управления административным корпусом.

## DBContextSQLServer ##

Контекст SQL Server. При создании контекста передаем в него строку подключения.

```c#
   string connectionNews = Configuration.GetConnectionString("AdministrationConnection");
   services.AddDbContext<DBContextSQLServer>(options => options.UseSqlServer(connectionNews, p => p.MigrationsAssembly("Administration.API")));
```
