Заметка по Авторизация/аутентификация ASP.NET

Для подключения Аутентификации (проверка подлинности) и Авторизации (предоставление доступа) необходимо:

1. В классе Startup в методе ConfigureServices
использовать метод AddAuthentication(), в который передаётся схема аутентификации.
Для использования JWT*, необходимо передать схему JwtBearerDefaults.AuthenticationScheme.
Далее с помощью метода JwtBearer() настроить конфигурацию объекта JwtBearerOption, 
который описывает параметры аутентификации**.

2. В классе Startup в методе Configure
добавить компоненты AuthenticationMiddleware и AuthorizationMiddleware, 
используя методы UseAuthentication() и UseAuthorization(). 

_______
*JWT (JSON Web Token) — строка в формате header.payload.signature
При вводе логина/пароля сервер создаёт JWT и отправляет его пользователю. 
При каждом запросе пользователя к API, сервер проверяет его JWT и предоставляет или не предоставляет доступ.
**Подробнее о возможных параметрах аутентификации
https://docs.microsoft.com/ru-ru/dotnet/api/microsoft.aspnetcore.builder.jwtbeareroptions?view=aspnetcore-1.1

