# Demo code of JWT Authentication with C# .NET 8

Code for Article: https://github.com/ricardodemauro/Labs.JwtAuthentication

Author: Ricardo https://rmauro.dev

### Valid Http Request to Generate a JWT Token

```http
POST /connect/token HTTP/1.1
Host: localhost
Content-Type: application/x-www-form-urlencoded

grant_type=password&username=johndoe&password=A3ddj3wr
```