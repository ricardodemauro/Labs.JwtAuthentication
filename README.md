# Demo code of JWT Authentication with C# .NET 8

Code for Article: https://rmauro.dev/jwt-authentication-with-csharp-dotnet/
Second Article: https://rmauro.dev/csharp-get-jwt-token-request/

Author: @rmauro.dev

### Valid Http Request to Generate a JWT Token

```http
POST /connect/token HTTP/1.1
Host: localhost
Content-Type: application/x-www-form-urlencoded

grant_type=password&username=johndoe&password=A3ddj3wr
```


## TO-DO

### MVP

- [X] Add authentication / authorization control over the routes
- [X] Add method to read jwt token from incoming request

### Phase-2

- [ ] Add Authorization Policies
- [ ] Check the `username` and `password` against some database or external datasource
- [ ] Load the user claims from a database
- [ ] Add samples with Asp.NET MVC
- [ ] Create a OpenAI/Swagger documentation
