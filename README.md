
# Team Task Management API

A lightweight, highly decoupled REST API built with .NET Minimal APIs. This project demonstrates clean coding architecture principles, focusing heavily on the **Open/Closed Principle (OCP)**, **Interface Segregation**, and the **Repository Pattern** to keep the core business logic completely independent of concrete storage implementations.
> See SOLID principles 

## Features & Architecture

*   **Decoupled Interface Segregation:** The core `TeamTask` entity implements fine-grained interfaces (`IAssignable`, `ITransitionable`, `ISchedulable`). API mutation handlers depend entirely on these contracts rather than the concrete class itself.
*   **Repository Pattern:** Data access is abstracted using `ITaskRepository`. Swapping the in-memory data store for a persistent SQL Database requires changing only a single line in the dependency injection configuration.
*   **Event-Driven Notifications:** Uses standard C# events (`StatusChanged`) to handle background logging, notifications, and auditing without polluting the core task-transition logic.



## API Endpoints Reference

### Tasks Management
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/tasks` | Returns 200 OK with the full list of tracked tasks. |
| `GET` | `/api/tasks/{id}` | Returns 200 OK with a specific task, or 404 Not Found. |
| `GET` | `/api/tasks/overdue` | Returns a list of tasks where `IsOverdue` evaluated to true using LINQ. |
| `POST` | `/api/tasks` | Creates a new task with an automatically incremented, immutable ID. |

### Domain Operations (Interface-Driven)
*   `PATCH /api/tasks/{id}/assign` — Mutates assignments via `IAssignable`.
*   `PATCH /api/tasks/{id}/transition` — Moves tasks between states (`Backlog`, `Todo`, `InProgress`, `Done`) via `ITransitionable`.



## Getting Started Locally

### Prerequisites
Ensure you have the latest .NET SDK installed on your machine:
```bash
dotnet --version
```

### Run the Application
Execute the following command in the root folder containing your project file:

```Bash
dotnet run
```

### Access the Application
   Once the console outputs the active hosting parameters, navigate to the following links in your browser:
   * **Interactive API Reference:** `https://localhost:7001/scalar/v1` (or your configured local port)
   * **Raw OpenAPI Specification Spec:** `https://localhost:7001/openapi/v1.json`


