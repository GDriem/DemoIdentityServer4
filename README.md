# 🚀 ProtectedApi - IdentityServer con .NET 8 y PostgreSQL

ProtectedApi es una API protegida mediante **Duende IdentityServer 7**, que utiliza autenticación y autorización moderna basada en OAuth 2.0 y OpenID Connect. Este proyecto está construido con .NET 8 y PostgreSQL usando Entity Framework Core (Code-First).

---

## ✨ Características principales

- 🔒 Autenticación JWT segura mediante IdentityServer.
- 🗃️ Persistencia en PostgreSQL con Entity Framework Core.
- 🚦 Roles y políticas de autorización flexibles.
- 🛠️ Arquitectura limpia y sencilla para mantenimiento y escalabilidad.

---

## 📦 Tecnologías y herramientas

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Duende IdentityServer](https://duendesoftware.com/products/identityserver)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [PostgreSQL](https://www.postgresql.org/)
- [Swagger (OpenAPI)](https://swagger.io/)

---

## 🛠️ Instalación y configuración inicial

### 1. Clona el repositorio

```bash
git clone https://github.com/tuusuario/ProtectedApi.git


## 🔑 Cómo obtener un token JWT usando Postman

Para consumir los endpoints protegidos de la API primero debes obtener un **token JWT** desde IdentityServer usando Postman. Sigue estos pasos:

### 1. 📌 Crear la solicitud en Postman

- Método HTTP: **POST**
- URL:https://localhost:7108/connect/token

### 2. 🔧 Configurar los parámetros del request:

En la pestaña **Body**, selecciona **x-www-form-urlencoded** y agrega los siguientes valores:

| Key             | Value                  |
|-----------------|------------------------|
| `client_id`     | `client`               |
| `client_secret` | `api-secret`           |
| `grant_type`    | `client_credentials`   |
| `scope`         | `api1`                 |

### 3. 🚀 Realizar la solicitud:

Haz clic en **Send**. Recibirás una respuesta similar a:

```json
{
  "access_token": "<tu_token>",
  "expires_in": 3600,
  "token_type": "Bearer",
  "scope": "api1"
}
