## AUTHORIZATION ##

Сервис авторизации.

Microsoft.Identity авторизация и аутентификация по токенам JWT:

запросы должны иметь заголовок с ключом Authorization и со значением в формате "Bearer: токен авторизации".

Имеет Web Api для управления пользователями и их профилями, а так же Web Api для аутентификации и авторизации со стороны внешних приложений.

Получение токена:

```
  api/login
```
  
В ответе поле Token содержит строковое представление JWT токена авторизации.

Поле RefreshToken содержит ключ для восстановления токена.


Восстановление токена:

```
  api/refresh-token
 ```
  
В ответе строка с токеном авторизации.

После получения токена все запросы должны содержать заголовок с ключом Authorization и со значением в формате "Bearer: токен авторизации".

Все запросы к сервису авторизации локализуются праметром "culture", например, "culture=ru".

Все функции Api доступны к тестированию в Swagger.