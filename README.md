# 🧩 UserTaskAPI – Task & User Management System

A simple **.NET 9 Web API** for managing **users** and **tasks** with **role-based access control (RBAC)**.  
Built for the Technical Assessment — implements CRUD operations, access control, and clean architecture principles.

---

## 🚀 Project Overview

The **UserTaskAPI** allows administrators and users to manage tasks.  
- **Admins** can create, read, update, and delete all tasks and users.  
- **Users** can only view and update the status of their assigned tasks.

---

## 🏗️ Architecture

Follows a **Clean Architecture** / **Layered Architecture** approach:

### 🔧 Design Patterns Used
- **Repository Pattern** for data access abstraction.  
- **Dependency Injection (DI)** for services and repositories.  
- **Middleware** for enforcing role-based access control (RBAC).
- **Service Layer** for business logic and communicate between controllers and repositories
- **Unit of Work** for database multiple transactions

---

## ⚙️ Technologies Used

| Technology | Purpose |
|-------------|----------|
| .NET 9 | API Framework |
| Entity Framework Core | ORM with In-Memory Database |
| Swagger / Swashbuckle / Open API | API Documentation |
| Dependency Injection | Service Management |

---

## ✅ Features Implemented

### 👤 User Management

| Operation | Endpoint | Access |
|------------|-----------|--------|
| Create User | `POST /api/users` | Admin |
| Get User by ID | `GET /api/users/{id}` | Admin |
| List All Users | `GET /api/users` | Admin |
| Update User | `PUT /api/users/{id}` | Admin |
| Delete User | `DELETE /api/users/{id}` | Admin |

---

### 📋 Task Management

| Operation | Endpoint | Access |
|------------|-----------|--------|
| Create Task | `POST /api/tasks` | Admin |
| Get Task by ID | `GET /api/tasks/{id}` | Admin / Assigned User |
| List All Tasks | `GET /api/tasks` | Admin |
| Update Task | `PUT /api/tasks/{id}` | Admin (all fields) / User (status only) |
| Delete Task | `DELETE /api/tasks/{id}` | Admin |

---

## 🔐 Role-Based Access Control (RBAC)

- Implemented using a **custom middleware**.  
- **Admins** have full access to all endpoints.  
- **Users** can:
  - View only their assigned tasks.
  - Update only the **status** of their assigned tasks.

- **NOTE: I DID NOT USE JWT, IDENTITY SERVER, OR ANY AUTHENTICATION METHOD BECAUSE IT WAS NOT MENTIONED IN THE TASK REQUIREMENTS. I DEVELOPED THE PROJECT WITHOUT THEM ACCORDINGLY.**

---

## 🧱 Data & Seeding

- Uses **EF Core In-Memory Database**.  
- **Seeded Data Includes:**
  - 👤 2 Users:
    - `Admin` (Role: Admin)
    - `User` (Role: User)
  - ✅ 3 Tasks assigned to different users.

---

## 📘 API Documentation

Swagger is enabled by default.

After running the API, open your browser at:
https://localhost:portnmumber/swagger

This provides interactive API documentation for all endpoints.

---

## 🧪 Testing

### 🔜 Remaining Work
- [ ] Write **unit test** for one **Service Layer** method (e.g., `UserService.CreateUserAsync`).
- [ ] Write **unit test** for one **Controller Action** (e.g., `TasksController.GetTaskById`).

---

## ⚠️ IMPORTANT NOTES

1️⃣ **Note:** I did not use JWT, Identity Server, or any authentication method because it was not mentioned in the task requirements. I developed the project without them accordingly.

2️⃣ **Note:** I used lazy loading instead of eager loading because the project is small. However, if we need to use eager loading in a larger project, we can implement it using the `Include()` method in Entity Framework.

