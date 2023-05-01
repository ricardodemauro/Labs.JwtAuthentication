# Demo code of JWT Authentication with C# .NET 8

Code for Article: https://rmauro.dev/jwt-authentication-with-csharp-dotnet/

Author: Ricardo https://rmauro.dev

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

### Phase-2

- [ ] Check on some Datasource the username and password
- [ ] Load the user claims from an external Datasource
- [ ] Add samples with Asp.NET MVC
- [ ] Add Authorization Policies
- [ ] Create a OpenAI/Swagger documentation
