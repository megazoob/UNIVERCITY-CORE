# Filter.Authorization #

Библиотека с набором фильтров авторизации. 
Фильтрация через атрибуты.

Сами фильтры рабочие, но вместо проверки на внешнем сервисе пока заглушка.

## [AuthorizationWithRoles("Administrator,Employee,Student")] ##

Проверка пользователя на наличие токена авторизации и на принадлежность к одной из указанных ролей.

## [IsAuthorized] ##

Проверка пользователя на наличие токена авторизации и валидность.
