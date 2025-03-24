# ContactManagement.WebAPI

## Overview

ContactManagement.WebAPI is a .NET 8 Minimal API project designed for Contact and Fund management. The API supports authentication and authorization using JWT and provides role-based access control.

## Requirements

- .NET 8
- SQL Server

## Setup Instructions

1. Clone the repository.
2. Update the ContactManagementDb database connection in the appsettings.Development.json file to point to an available SQL Server.
3. If using an IDE (Rider or Visual Studio), set ContactManagement.WebAPI as the startup project and run it from there. Alternatively, use a CLI to run `dotnet run` command from ../src/ContactManagement.WebAPI.

## Testing

- Example unit tests are provided for key functionalities.
- API can be manually tested using **Swagger**. (path: {root-url}/swagger/).


## Architecture

The project follows a **Vertical Slicing Architecture**, organizing features by modules. It consists of three main modules:

- **AuthModule**: Handles authentication and JWT issuance.
- **ContactModule**: Manages Contacts with role-based access control.
- **FundModule**: Manages Funds and Contact assignments.

### Project Structure

Each module contains the following folders:

- **Endpoints**: Defines the API endpoints.
- **Repo**: Contains repository services for `Commands` and `Queries` to handle database interactions.
- **Configurations**: Contains configuration-related classes.

A top-level `Endpoints.cs` file consolidates all available API endpoints for easy reference.

## API Endpoints

### AuthModule

- `/api/login` - Issues a JWT token for authentication and authorization.
  - Hardcoded users for demo purposes:
    - username: **admin** / password: `password`
    - username: **fundmanager** / password: `password`

### ContactModule

- `CreateContact` - Create a new Contact. *(Admin only)*
- `UpdateContact` - Update Contact details. *(Admin only)*
- `DeleteContactById` - Delete a Contact by ID. *(Admin only)*
- `GetContactById` - Retrieve Contact details by ID. *(Admin & Fund Manager)*
- `GetContacts (Paginated)` - Retrieve a paginated list of all Contacts. *(Admin & Fund Manager)*

### FundModule

- `CreateFund` - Create a new Fund. *(Fund Manager only)*
- `AssignContact` - Assign a Contact to a Fund. *(Fund Manager only)*
- `RemoveContact` - Remove a Contact from a Fund. *(Fund Manager only)*
- `GetFundContacts` - Retrieve all Contacts assigned to a specific Fund. *(Fund Manager only)*
- `GetFunds` - Retrieve all Funds. *(Public access, no authentication required)*

## Database

- Uses SQL Server as the database.
- Funds are pre-seeded on database creation.

## Validation

FluentValidation is used to validate API requests.

## Exception Handling

A global exception handler catches unhandled exceptions and returns a `500` error message without exposing internal details.
