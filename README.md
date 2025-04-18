# Cumulative-Project
## Teachers MVP - ASP.NET Core Web API & MVC

## Description
This project is built using **ASP.NET Core Web API and MVC** with a **MySQL** database. It is a **Minimum Viable Product (MVP)** that focuses on the **Teachers** table of the provided **School Database**. We can use it to **READ**, **ADD**, and **DELETE** teacher details.

## Features
- **ASP.NET Core Web API** for fetching teacher data.
- **MVC Pages** for listing and displaying teacher details.
- **MySQL Integration** using `MySql.Data.MySqlClient`.
- **Structured Code** with separate **Controllers, Models, and Views**.
- **Read**: Fetch teacher data via API and display in MVC views.
-  **Add**: Create new teachers via API or form submission.
-  **Delete**: Remove teachers via API or MVC confirmation page.
-  **Update**: Update teacher information via API or MVC edit page.

## API Endpoints
| Method | Endpoint | Description | 
|--------|---------|-------------|
| **GET** | `/api/TeacherAPI/ListTeachers` | Fetch all teachers |
| **GET** | `/api/TeacherAPI/FindTeacher/{id}` | Fetch a teacher by ID |
| **GET** | `/api/TeacherAPI/ListTeachers/{SearchKey}` | Fetch a teacher by a keyword |
| **POST** | `/api/TeacherAPI/AddTeacher` | Add a new teacher |
| **DELETE** | `/api/TeacherAPI/DeleteTeacher/{TeacherID}` | Delete a teacher by ID |
| **PUT** | `/api/TeacherAPI/UpdateTeacher/{id}` | Reacive and Update teacher information |
