# 🧪 Nexle-Dotnet-Evaluation

A **.NET 8 backend system** built following **Clean Architecture** principles. This project supports secure user authentication, token-based session management (JWT and refresh tokens), and modular development using well-structured service abstractions and repositories.

---

## 🗂️ Project Structure

```bash
NexleEvaluation/
│
├── NexleEvaluation.Application     # Application layer (Services, DTOs, Requests, Responses, Interfaces)
├── NexleEvaluation.Domain          # Domain layer (Entities, Interfaces)
├── NexleEvaluation.Infrastructure  # Infrastructure layer (EF Core, DB context, repository implementations)
├── NexleEvaluation.API             # ASP.NET Core Web API (Entry point)
├── NexleEvaluation.Tests           # Unit tests (XUnit, Moq, Shouldly/FluentAssertions)
```

---

## ⚙️ Technologies Used

- [.NET 8](https://dotnet.microsoft.com/)
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- BCrypt for password hashing
- XUnit, Moq, Shouldly (Unit Testing)
- Clean Architecture
- Dependency Injection

---

## ✅ Features

- 🔐 **User Registration** (SignUp)
- 🔑 **User Login** (SignIn)
- 🔁 **JWT Access Token & Refresh Token** generation
- 🔄 **Token Refresh** and **SignOut** support
- 🔒 **Secure Password Hashing** with BCrypt
- 🧠 **Token Claims & Expiration** logic
- ✅ **Unit-tested service logic** using mocking

---

## 🧪 Running Tests

> Run all unit tests to ensure core services are working as expected:

```bash
dotnet test NexleEvaluation.Tests
```

- Tests are located under the `NexleEvaluation.Tests` project.
- Services tested include:
  - User sign-up
  - User login
  - Token generation
  - Sign-out logic

---

## 🔧 Configuration

Update the following values in the `appsettings.json` file in the **API project**:

### 🐘 Database Connection

```json
"ConnectionStrings": {
  "DefaultConnection": "YOUR_CONNECTION"
}
```

---

## 📬 Swagger

The API includes Swagger for testing and documentation.

- Access Swagger UI at: `https://localhost:<port>/swagger/index.html` 
- Example: https://localhost:7202/swagger/index.html

- Configured via:

```json
"SwaggerDocOptions": {
  "Title": "Nexle Evaluation API",
  "Description": "Web API .NET 8",
  "Organization": "Hoang Le",
  "Email": "hoangle14702@gmail.com"
}
```

---

## 👤 Author

**Hoang Le**  
📧 hoangle14702@gmail.com
