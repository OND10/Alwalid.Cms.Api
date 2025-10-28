# Real-Time Chat Application

A modern, scalable real-time chat application built with .NET 8, Entity Framework Core, PostgreSQL, and SignalR, featuring Google OAuth authentication and clean architecture principles.

## 🚀 Features

- **Real-time messaging** using SignalR WebSockets
- **Google OAuth authentication** with JWT tokens
- **Clean Architecture** with Domain, Application, Infrastructure, and Web layers
- **PostgreSQL database** with Entity Framework Core
- **Comprehensive validation** using FluentValidation
- **Unit and integration tests** with 80%+ code coverage
- **RESTful API** with Swagger documentation
- **Message pagination** and filtering
- **Soft delete** functionality for messages
- **Refresh token** management
- **CORS support** for frontend integration

## 🏗️ Architecture

The application follows Clean Architecture principles with the following layers:

```
├── src/
│   ├── ChatApp.Domain/           # Domain entities and interfaces
│   ├── ChatApp.Application/      # Business logic and CQRS handlers
│   ├── ChatApp.Infrastructure/   # Data access and external services
│   └── ChatApp.Web/             # API controllers and SignalR hubs
└── tests/
    ├── ChatApp.UnitTests/        # Unit tests
    └── ChatApp.IntegrationTests/ # Integration tests
```

### Domain Layer
- **Entities**: User, Message, RefreshToken
- **Interfaces**: Repository patterns and Unit of Work
- **Exceptions**: Custom domain exceptions

### Application Layer
- **CQRS**: Commands and Queries using MediatR
- **DTOs**: Data Transfer Objects
- **Services**: Authentication service
- **Validation**: FluentValidation rules

### Infrastructure Layer
- **Data Access**: EF Core DbContext and repositories
- **Migrations**: Database schema management
- **Dependency Injection**: Service registration

### Web Layer
- **Controllers**: RESTful API endpoints
- **SignalR Hubs**: Real-time communication
- **Authentication**: JWT and Google OAuth setup

## 🛠️ Technology Stack

- **.NET 8** - Latest .NET framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM for data access
- **PostgreSQL** - Primary database
- **SignalR** - Real-time communication
- **Google OAuth 2.0** - Authentication provider
- **JWT** - Token-based authentication
- **AutoMapper** - Object-to-object mapping
- **MediatR** - CQRS pattern implementation
- **FluentValidation** - Input validation
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Testing framework
- **Moq** - Mocking framework
- **FluentAssertions** - Test assertions

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 12+](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)
- Google Cloud Console account (for OAuth setup)

## ⚡ Quick Start

### 1. Clone the Repository

```bash
git clone <repository-url>
cd ChatApp
```

### 2. Database Setup

1. Install and start PostgreSQL
2. Create a database named `ChatAppDb`:

```sql
CREATE DATABASE "ChatAppDb";
```

### 3. Google OAuth Setup

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select an existing one
3. Enable the Google+ API
4. Create OAuth 2.0 credentials:
   - **Application Type**: Web application
   - **Authorized JavaScript origins**: `https://localhost:7206`
   - **Authorized redirect URIs**: `https://localhost:7206/signin-google`
5. Copy the Client ID and Client Secret

### 4. Configuration

Update `src/ChatApp.Web/appsettings.json` with your settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ChatAppDb;Username=postgres;Password=your_password;Port=5432"
  },
  "Authentication": {
    "Google": {
      "ClientId": "your-google-client-id",
      "ClientSecret": "your-google-client-secret"
    }
  },
  "Jwt": {
    "Key": "your-256-bit-secret-key-here-make-it-long-enough-for-security",
    "Issuer": "ChatApp",
    "Audience": "ChatApp",
    "ExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  }
}
```

### 5. Run the Application

```bash
# Navigate to the Web project
cd src/ChatApp.Web

# Run the application
dotnet run
```

The application will start at:
- **API**: https://localhost:7206
- **Swagger UI**: https://localhost:7206/swagger

### 6. Run Tests

```bash
# Run unit tests
dotnet test tests/ChatApp.UnitTests

# Run integration tests
dotnet test tests/ChatApp.IntegrationTests

# Run all tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### 7. Test Google OAuth Authentication

The API uses Google OAuth for authentication. Here are multiple ways to get a Google token:

#### Option 1: Use the Test Frontend (Easiest)
1. Open `test-frontend.html` in your browser
2. Sign in with Google
3. Copy the access token
4. Test the API directly from the page

#### Option 2: Use Test Scripts
```bash
# For Linux/Mac users
./test-api.sh

# For Windows users (PowerShell)
./test-api.ps1
```

#### Option 3: Google OAuth Playground
1. Go to [Google OAuth Playground](https://developers.google.com/oauthplayground/)
2. Use the provided Client ID: `212127799857-ltspi8qut4fu0g94sj3t2kpdps831kgh.apps.googleusercontent.com`
3. Select userinfo scopes and get access token

#### Option 4: Manual cURL
```bash
# Replace YOUR_ACCESS_TOKEN with token from above methods
curl -X POST "https://localhost:7206/api/auth/google" \
     -H "Content-Type: application/json" \
     -H "Accept: application/json" \
     -d '{"googleToken": "YOUR_ACCESS_TOKEN"}' \
     -k
```

📖 **For detailed instructions, see `google-oauth-guide.md`**

## 📖 API Documentation

### Authentication Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/auth/login` | Initiates Google OAuth login |
| GET | `/api/auth/callback` | Handles Google OAuth callback |
| POST | `/api/auth/google` | Authenticates with Google token |
| POST | `/api/auth/refresh` | Refreshes access token |
| POST | `/api/auth/logout` | Revokes refresh token |
| GET | `/api/auth/me` | Gets current user info |

### Message Endpoints

| Method | Endpoint | Description | Authentication |
|--------|----------|-------------|----------------|
| GET | `/api/messages` | Get paginated messages | Required |
| GET | `/api/messages/latest` | Get latest messages | Required |
| POST | `/api/messages` | Create new message | Required |
| DELETE | `/api/messages/{id}` | Delete message | Required (Owner only) |

### SignalR Hub

**Endpoint**: `/chathub`

**Events**:
- `MessageReceived` - New message broadcast
- `MessageDeleted` - Message deletion broadcast
- `UserConnected` - User joined notification
- `UserDisconnected` - User left notification
- `UserTyping` - Typing indicator

## 🔐 Authentication Flow

1. **Google OAuth**: Users authenticate via Google OAuth 2.0
2. **JWT Tokens**: Access tokens for API authentication
3. **Refresh Tokens**: Long-lived tokens for token renewal
4. **SignalR**: JWT tokens passed via query parameter for WebSocket auth

## 📊 Database Schema

### Users Table
- `Id` (GUID, PK)
- `Email` (string, unique)
- `Name` (string)
- `GoogleId` (string, unique)
- `ProfilePictureUrl` (string, nullable)
- `CreatedAt` (datetime)
- `LastActiveAt` (datetime)
- `DeletedAt` (datetime, nullable)

### Messages Table
- `Id` (GUID, PK)
- `Content` (string, max 2000 chars)
- `UserId` (GUID, FK)
- `CreatedAt` (datetime)
- `EditedAt` (datetime, nullable)
- `DeletedAt` (datetime, nullable)

### RefreshTokens Table
- `Id` (GUID, PK)
- `Token` (string, unique)
- `UserId` (GUID, FK)
- `ExpiresAt` (datetime)
- `IsRevoked` (boolean)
- `RevokedAt` (datetime, nullable)
- `RevokedReason` (string, nullable)
- `CreatedAt` (datetime)
- `DeletedAt` (datetime, nullable)

## 🧪 Testing

The application includes comprehensive tests:

- **Unit Tests**: 25+ tests covering domain logic and application services
- **Integration Tests**: End-to-end API testing
- **Test Coverage**: 80%+ code coverage
- **Mocking**: Using Moq for dependency isolation
- **Assertions**: FluentAssertions for readable test code

Run specific test categories:

```bash
# Unit tests only
dotnet test tests/ChatApp.UnitTests --logger "console;verbosity=detailed"

# Integration tests only
dotnet test tests/ChatApp.IntegrationTests --logger "console;verbosity=detailed"
```

## 📝 Development Guidelines

### Code Style
- Follow C# coding conventions
- Use XML documentation for public APIs
- Implement SOLID principles
- Use async/await for I/O operations

### Git Workflow
- Use conventional commits format
- Create feature branches for new features
- Write descriptive commit messages
- Ensure tests pass before merging

### Adding New Features

1. **Domain**: Add entities and interfaces
2. **Application**: Create commands/queries and handlers
3. **Infrastructure**: Implement repositories and data access
4. **Web**: Add controllers and configure services
5. **Tests**: Write unit and integration tests

## 🚀 Deployment

### Docker Support

Create a `Dockerfile` in the root directory:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/ChatApp.Web/ChatApp.Web.csproj", "src/ChatApp.Web/"]
COPY ["src/ChatApp.Application/ChatApp.Application.csproj", "src/ChatApp.Application/"]
COPY ["src/ChatApp.Infrastructure/ChatApp.Infrastructure.csproj", "src/ChatApp.Infrastructure/"]
COPY ["src/ChatApp.Domain/ChatApp.Domain.csproj", "src/ChatApp.Domain/"]
RUN dotnet restore "src/ChatApp.Web/ChatApp.Web.csproj"
COPY . .
WORKDIR "/src/src/ChatApp.Web"
RUN dotnet build "ChatApp.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatApp.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatApp.Web.dll"]
```

### Production Considerations

- Use Azure/AWS secrets management for sensitive data
- Implement proper logging with Serilog
- Set up health checks
- Configure rate limiting
- Enable HTTPS redirect
- Use production database connection strings
- Configure CORS for production domains

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'feat: add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **Clean Architecture** pattern by Robert C. Martin
- **MediatR** for CQRS implementation
- **SignalR** for real-time communication
- **Entity Framework Core** team for excellent ORM
- **ASP.NET Core** team for the robust web framework

## 📞 Support

For support, please open an issue in the GitHub repository or contact the development team.

---

**Built with ❤️ using .NET 8 and Clean Architecture principles**