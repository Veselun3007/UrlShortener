# 🔗 URL Shortener

A full-stack web application for shortening URLs, managing links, and tracking creations. Built with **.NET 8** and **Angular** .

## Tech Stack
* **Backend:** .NET 8, ASP.NET Core Web API, Entity Framework Core, SQL Server, ASP.NET Core Identity with JWT.
* **Frontend:** Angular, Bootstrap 5.
* **Security:** JWT Authentication, Role-Based Access Control (Admin / User).
* **Infrastructure:** Docker (SQL Server container).

## Prerequisites
Before you begin, ensure you have the following installed:
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Node.js](https://nodejs.org/) (v18+)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)

---

## How to Run Locally

### 1. Start the Database
The project uses SQL Server hosted in a Docker container. 
In the root directory of the project (where `docker-compose.yml` is located), run:
```bash
docker-compose up -d
```

### 2. Run the Backend API

Navigate to the API project folder:

```bash
cd Api
dotnet run
```

- The API will start, apply EF Core migrations, and seed the database automatically.
- Swagger UI is available at: http://localhost:5288/swagger


### 3. Run the Frontend Client

Open a new terminal window, navigate to the client project folder:

```bash
cd Client
npm install
npm start
```

- The application will be accessible at: http://localhost:4200 (port is strictly configured for CORS compatibility).

## Default Credentials

The database is automatically seeded with two test users on the first startup:

| Role | Username | Password | Permissions |
| :--- | :--- | :--- | :--- |
| **Admin** | `admin` | `Admin123!` | Can view all URLs, add new URLs, and delete **any** short URL. Can edit the About page. |
| **User** | `user` | `User123!` | Can view all URLs, add new URLs, and delete **only their own** short URLs. |
