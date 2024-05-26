# Library Management System

## Overview

This project is a library management system with a backend API for managing books and authors, and an simple non database authentication API which generate JWT tokens for only test purposes.

1. **LibraryManagementAPI**: Handles CRUD operations for books and authors.
2. **IdentityAPI**: Manages user authentication and authorization, generating JWT tokens for authentication testing purposes. Note that this IdentityAPI implementation is intended for development and testing purposes and may contain vulnerabilities.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [PostgreSQL](https://www.postgresql.org/) (or any other configured database)
- Docker (for containerized database setup)

## Getting Started
- Clone the repository to your local machine
- For Quick setup you should use Docker and docker-compose.yml

### For quick setup

You can run full project using Docker with the provided `docker-compose.yml` file:

```sh
docker-compose up -d
```

Default Endpoints for Docker
- LibraryManagementAPI: http://localhost:5000/swagger
- IdentityAPI: http://localhost:5001/swagger
