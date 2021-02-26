# Библиотеки #

## Authorization.Gateway.Services ##

Реализует интерфейс <b>IAuthorizationGateway</b>: отправка приложением запроса на авторизацию в сервис авторизации.

## Filter.AuthOperation ##

Предоставляет фильтр, необходимый Swagger для обработки атрибутов авторизации: <b>AuthOperationFilter : IOperationFilter</b>.

## Filter.Authorization ##

Предоставляет фильтры, реализующие атрибуты авторизации в контроллерах приложений: <b>IsAuthorizedAttribute</b> и <b>AuthorizationWithRolesAttribute</b>.
Фильтры авторизуют пользователей токенами, обращаясь к интерфейсу <b>IAuthorizationGateway</b>.

## Localization.Services ##

Библиотека локализации, реализующая интерфейсы <b>IStringLocalizer</b> и <b>IStringLocalizerFactory</b>.
В настоящий момент просто возвращает переданное значение [ключ].

## Users.Roles ##

Библиотека с классом ролей пользователей.
