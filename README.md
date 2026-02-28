<div align="center">

# 💼 JobPortal

### A Full-Stack Job Portal Application Built with ASP.NET Core 8.0

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![EF Core](https://img.shields.io/badge/Entity%20Framework-Core%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/en-us/ef/core/)
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)
[![License](https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge)](LICENSE)

*A modern, feature-rich job portal connecting **Job Seekers** with **Employers** — powered by a robust REST API backend and a clean MVC frontend.*

[🚀 Getting Started](#-getting-started) · [📖 API Reference](#-api-endpoints) · [🏗️ Architecture](#-system-architecture) · [🤝 Contributing](#-contributing)

---

</div>

## 📋 Table of Contents

- [✨ Features](#-features)
- [🏗️ System Architecture](#-system-architecture)
- [🔄 Application Workflow](#-application-workflow)
- [🗄️ Database Schema](#-database-schema)
- [📁 Project Structure](#-project-structure)
- [🛠️ Tech Stack](#-tech-stack)
- [🚀 Getting Started](#-getting-started)
- [📖 API Endpoints](#-api-endpoints)
- [🖥️ MVC Pages & Views](#-mvc-pages--views)
- [🤝 Contributing](#-contributing)

---

## ✨ Features

<table>
<tr>
<td>

### 👤 For Job Seekers
- 🔍 Browse & search job listings
- 📝 Apply to jobs with resume upload
- 💾 Save jobs for later
- 🛠️ Manage personal skills profile
- ⭐ Review companies

</td>
<td>

### 🏢 For Employers
- 📋 Post & manage job listings
- 🏗️ Create company profiles
- 📂 Manage job categories
- 👥 View job applications
- ✏️ Edit & update job postings

</td>
</tr>
<tr>
<td colspan="2">

### ⚙️ Platform Features
- 🔐 User Authentication (Login/Register)
- 🌐 RESTful API with Swagger Documentation
- ✅ FluentValidation for data integrity
- 🗃️ Entity Framework Core with SQL Server
- 📱 Responsive MVC Frontend

</td>
</tr>
</table>

---

## 🏗️ System Architecture

```mermaid
graph TB
    subgraph "🖥️ Client Layer"
        Browser["🌐 Web Browser"]
    end

    subgraph "🎨 Presentation Layer — JobPortal_MVC"
        MVC["ASP.NET Core MVC\n(.NET 8.0)"]
        Views["Razor Views\n(Login, Registration,\nDashboards)"]
        MVCControllers["MVC Controllers\n(Login, Registration,\nJobSeeker, Employer)"]
        MVCModels["View Models\n(JobModel, CompanyModel,\nSkillModel, etc.)"]
    end

    subgraph "⚡ API Layer — JobPortal_API"
        API["ASP.NET Core Web API\n(.NET 8.0)"]
        Swagger["Swagger / OpenAPI\nDocumentation"]
        Validation["FluentValidation\nRequest Validation"]
        APIControllers["API Controllers\n(Profile, Job, Company,\nSkill, etc.)"]
    end

    subgraph "💾 Data Layer"
        EF["Entity Framework Core 8\n(ORM)"]
        DbContext["JobPortalDbContext"]
        Models["Domain Models\n(User, Job, Company,\nSkill, etc.)"]
    end

    subgraph "🗄️ Database Layer"
        SQL["Microsoft SQL Server\n(JobPortalDB)"]
    end

    Browser -->|"HTTP Requests"| MVC
    MVC --> Views
    MVC --> MVCControllers
    MVC --> MVCModels
    MVCControllers -->|"HttpClient\nREST Calls"| API
    API --> Swagger
    API --> Validation
    API --> APIControllers
    APIControllers --> EF
    EF --> DbContext
    DbContext --> Models
    EF -->|"SQL Queries"| SQL

    style Browser fill:#4FC3F7,stroke:#0288D1,color:#000
    style MVC fill:#81C784,stroke:#388E3C,color:#000
    style API fill:#FFB74D,stroke:#F57C00,color:#000
    style EF fill:#CE93D8,stroke:#7B1FA2,color:#000
    style SQL fill:#EF5350,stroke:#C62828,color:#fff
```

---

## 🔄 Application Workflow

### 🔐 User Authentication Flow

```mermaid
sequenceDiagram
    actor User
    participant MVC as 🎨 MVC Frontend
    participant API as ⚡ Web API
    participant DB as 🗄️ SQL Server

    User->>MVC: Visit Login Page
    MVC-->>User: Render LoginForm View

    User->>MVC: Submit Credentials
    MVC->>API: POST /api/Profile/Login
    API->>DB: Query Users Table
    
    alt ✅ Valid Credentials
        DB-->>API: User Record Found
        API-->>MVC: 200 OK + User Data
        MVC-->>User: Redirect to Dashboard
    else ❌ Invalid Credentials
        DB-->>API: No Match Found
        API-->>MVC: 404 Not Found
        MVC-->>User: Show Error Message
    end

    User->>MVC: Visit Registration Page
    MVC-->>User: Render RegistrationForm View
    User->>MVC: Submit Registration Data
    MVC->>API: POST /api/Profile/Register
    API->>DB: INSERT into Users
    DB-->>API: User Created
    API-->>MVC: 200 OK + New User
    MVC-->>User: Redirect to Login
```

### 💼 Job Posting & Application Flow

```mermaid
sequenceDiagram
    actor Employer
    actor JobSeeker
    participant MVC as 🎨 MVC Frontend
    participant API as ⚡ Web API
    participant DB as 🗄️ SQL Server

    Note over Employer,DB: 📝 Employer Posts a Job
    Employer->>MVC: Navigate to Post Job
    MVC->>API: GET /api/Company (Load Dropdowns)
    MVC->>API: GET /api/JobCategory/GetAllJobCategories
    MVC->>API: GET /api/Skill/GetAllSkills
    API-->>MVC: Companies, Categories, Skills
    MVC-->>Employer: Render Job Form with Dropdowns
    Employer->>MVC: Submit Job Details
    MVC->>API: POST /api/Job/AddJob
    API->>DB: INSERT into Jobs
    DB-->>API: Job Created
    API-->>MVC: 201 Created
    MVC-->>Employer: Job Listed Successfully ✅

    Note over JobSeeker,DB: 🔍 Job Seeker Applies
    JobSeeker->>MVC: Browse Jobs
    MVC->>API: GET /api/Job/GetAllJobs
    API->>DB: SELECT Jobs with Companies
    DB-->>API: Job Listings
    API-->>MVC: Job Data
    MVC-->>JobSeeker: Display Job Listings

    JobSeeker->>MVC: Apply to Job
    MVC->>API: POST /api/JobApplication/AddJobApplication
    API->>DB: INSERT into JobApplications
    DB-->>API: Application Saved
    API-->>MVC: 200 OK
    MVC-->>JobSeeker: Application Submitted ✅

    JobSeeker->>MVC: Save Job for Later
    MVC->>API: POST /api/SavedJob/AddSavedJob
    API->>DB: INSERT into SavedJobs
    API-->>MVC: 200 OK
    MVC-->>JobSeeker: Job Saved 💾
```

### 🛠️ Skills Management Flow

```mermaid
sequenceDiagram
    actor User
    participant MVC as 🎨 MVC Frontend
    participant API as ⚡ Web API
    participant DB as 🗄️ SQL Server

    User->>MVC: Navigate to Skills Page
    MVC->>API: GET /api/Skill/GetAllSkills
    MVC->>API: GET /api/UserSkill/GetAllUserSkills
    API->>DB: Query Skills & UserSkills
    DB-->>API: Skills Data
    API-->>MVC: All Skills + User's Skills
    MVC-->>User: Display Skills List

    User->>MVC: Add New Skill
    MVC->>API: GET /api/Skill/GetSkillByName/{name}
    
    alt Skill Exists
        API-->>MVC: Existing Skill ID
    else Skill Not Found
        MVC->>API: POST /api/Skill/AddSkill
        API->>DB: INSERT into Skills
        API-->>MVC: New Skill ID
    end
    
    MVC->>API: POST /api/UserSkill/AddUserSkill
    API->>DB: INSERT into UserSkills
    API-->>MVC: 200 OK
    MVC-->>User: Skill Added ✅

    User->>MVC: Remove Skill
    MVC->>API: DELETE /api/UserSkill/DeleteUserSkill/{userId}/{skillId}
    API->>DB: DELETE from UserSkills
    API-->>MVC: 204 No Content
    MVC-->>User: Skill Removed 🗑️
```

---

## 🗄️ Database Schema

```mermaid
erDiagram
    Users {
        int UserId PK
        string FirstName
        string LastName
        string Email
        string Password
        string Role
        string ImageUrl
        datetime CreatedAt
        datetime UpdatedAt
    }

    Companies {
        int CompanyId PK
        string Name
        string Website
        string Description
        string LogoUrl
        int UserId FK
        datetime CreatedAt
        datetime UpdatedAt
    }

    Jobs {
        int JobId PK
        string Title
        string Description
        string KeyResponsibilities
        string Qualifications
        bool IsActive
        string Location
        string SalaryRange
        string ExperienceRange
        string JobType
        date ExpiresAt
        int CategoryId FK
        int CompanyId FK
        int UserId FK
        int SkillId FK
        datetime CreatedAt
        datetime UpdatedAt
    }

    JobCategories {
        int CategoryId PK
        string CategoryName
        int UserId FK
        datetime CreatedAt
        datetime UpdatedAt
    }

    JobApplications {
        int JobApplicationId PK
        int JobId FK
        int UserId FK
        string ResumeUrl
        string Status
        datetime AppliedAt
        datetime UpdatedAt
    }

    SavedJobs {
        int SavedJobId PK
        int UserId FK
        int JobId FK
        datetime SavedAt
    }

    Skills {
        int SkillId PK
        string SkillName
        int UserId FK
        datetime CreatedAt
        datetime UpdatedAt
    }

    UserSkills {
        int UserSkillId PK
        int UserId FK
        int SkillId FK
        datetime CreatedAt
    }

    CompanyReviews {
        int ReviewId PK
        int CompanyId FK
        int UserId FK
        int Rating
        string Comment
        datetime CreatedAt
        datetime UpdatedAt
    }

    Users ||--o{ Companies : "creates"
    Users ||--o{ Jobs : "posts"
    Users ||--o{ JobApplications : "applies"
    Users ||--o{ SavedJobs : "saves"
    Users ||--o{ Skills : "creates"
    Users ||--o{ UserSkills : "has"
    Users ||--o{ CompanyReviews : "writes"
    Users ||--o{ JobCategories : "manages"
    Companies ||--o{ Jobs : "has"
    Companies ||--o{ CompanyReviews : "receives"
    Jobs ||--o{ JobApplications : "receives"
    Jobs ||--o{ SavedJobs : "saved_in"
    JobCategories ||--o{ Jobs : "categorizes"
    Skills ||--o{ Jobs : "required_for"
    Skills ||--o{ UserSkills : "linked_to"
```

---

## 📁 Project Structure

```
JobPortal/
│
├── 📂 JobPortal_API/                    # ⚡ ASP.NET Core Web API
│   ├── 📂 Controllers/                  # API Controllers
│   │   ├── ProfileController.cs         # 👤 User Registration, Login, Profile
│   │   ├── JobController.cs             # 💼 Job CRUD Operations
│   │   ├── CompanyController.cs         # 🏢 Company CRUD Operations
│   │   ├── JobCategoryController.cs     # 📂 Job Category Management
│   │   ├── JobApplicationController.cs  # 📝 Job Applications + Search
│   │   ├── SkillController.cs           # 🛠️ Skills CRUD Operations
│   │   ├── UserSkillController.cs       # 🔗 User-Skill Mapping
│   │   ├── SavedJobController.cs        # 💾 Saved Jobs CRUD
│   │   └── CompanyReviewController.cs   # ⭐ Company Reviews
│   │
│   ├── 📂 Models/                       # Entity Framework Models
│   │   ├── JobPortalDbContext.cs         # 🗃️ Database Context
│   │   ├── User.cs                      # User Entity
│   │   ├── Job.cs                       # Job Entity
│   │   ├── Company.cs                   # Company Entity
│   │   ├── JobCategory.cs               # Job Category Entity
│   │   ├── JobApplication.cs            # Job Application Entity
│   │   ├── Skill.cs                     # Skill Entity
│   │   ├── UserSkill.cs                 # User-Skill Entity
│   │   ├── SavedJob.cs                  # Saved Job Entity
│   │   └── CompanyReview.cs             # Company Review Entity
│   │
│   ├── Program.cs                       # 🚀 API Entry Point
│   ├── appsettings.json                 # ⚙️ Configuration
│   └── JobPortalAPI.csproj              # 📦 Project File
│
├── 📂 JobPortal_MVC/                    # 🎨 ASP.NET Core MVC Frontend
│   ├── 📂 Controllers/                  # MVC Controllers
│   │   ├── LoginController.cs           # 🔐 Login Page
│   │   ├── RegistrationController.cs    # 📝 Registration Page
│   │   ├── JobSeekerController.cs       # 👤 Job Seeker Dashboard & Features
│   │   └── EmployeerController.cs       # 🏢 Employer Dashboard & Features
│   │
│   ├── 📂 Models/                       # View Models
│   │   ├── JobModel.cs                  # Job View Model
│   │   ├── CompanyModel.cs              # Company View Model
│   │   ├── UserSkillModel.cs            # User Skill View Model
│   │   ├── CompanyReviewsModel.cs       # Reviews View Model
│   │   └── ...                          # Other View Models
│   │
│   ├── 📂 Views/                        # Razor Views
│   │   ├── Login/                       # Login Views
│   │   ├── Registration/                # Registration Views
│   │   ├── JobSeeker/                   # Job Seeker Views
│   │   ├── Employeer/                   # Employer Views
│   │   └── Shared/                      # Layout & Shared Partials
│   │
│   ├── Program.cs                       # 🚀 MVC Entry Point
│   ├── appsettings.json                 # ⚙️ Configuration
│   └── JobPortalMVC.csproj              # 📦 Project File
│
├── JobPortal_Database.ssmssln           # 🗄️ SQL Server Management Studio Solution
└── README.md                            # 📖 This File
```

---

## 🛠️ Tech Stack

| Layer | Technology | Purpose |
|:---:|:---:|:---|
| **Frontend** | ASP.NET Core MVC 8.0 | Razor Views, Server-Side Rendering |
| **Backend API** | ASP.NET Core Web API 8.0 | RESTful API Endpoints |
| **ORM** | Entity Framework Core 8.0 | Database-First Approach |
| **Database** | Microsoft SQL Server | Relational Data Storage |
| **Validation** | FluentValidation 11.3 | Server-Side Input Validation |
| **API Docs** | Swashbuckle (Swagger) 8.1 | Interactive API Documentation |
| **HTTP Client** | IHttpClientFactory | MVC-to-API Communication |
| **Serialization** | Newtonsoft.Json & System.Text.Json | JSON Data Handling |

---

## 🚀 Getting Started

### Prerequisites

| Requirement | Version |
|:---|:---|
| .NET SDK | 8.0 or later |
| SQL Server | 2019+ (or SQL Server Express) |
| Visual Studio | 2022+ (recommended) |
| SQL Server Management Studio | Latest (optional) |

### Installation

**1. Clone the repository**
```bash
git clone https://github.com/Shivam93294Valand/JobPortal.git
cd JobPortal
```

**2. Set up the Database**
- Open SQL Server Management Studio
- Open the `JobPortal_Database.ssmssln` solution file
- Execute the database scripts to create the `JobPortalDB` database

**3. Update Connection String**

Update `JobPortal_API/appsettings.json` with your SQL Server instance:
```json
{
  "ConnectionStrings": {
    "JobPortalDB": "Server=YOUR_SERVER_NAME; Database=JobPortalDB; Trusted_Connection=True; TrustServerCertificate=True;"
  }
}
```

**4. Run the API Project**
```bash
cd JobPortal_API
dotnet restore
dotnet run
```
> 📌 The API will run at `https://localhost:7172` — Swagger UI available at `/swagger`

**5. Run the MVC Project**
```bash
cd JobPortal_MVC
dotnet restore
dotnet run
```
> 📌 The MVC app will launch and connect to the API

---

## 📖 API Endpoints

### 👤 Profile / Authentication

| Method | Endpoint | Description |
|:---:|:---|:---|
| `GET` | `/api/Profile/{userId}` | Get user profile by ID |
| `POST` | `/api/Profile/Register` | Register a new user |
| `POST` | `/api/Profile/Login` | Login with email & password |
| `PUT` | `/api/Profile/Update/{userId}` | Update user profile |

### 💼 Jobs

| Method | Endpoint | Description |
|:---:|:---|:---|
| `GET` | `/api/Job/GetAllJobs` | Get all jobs (with company info) |
| `GET` | `/api/Job/GetJob/{id}` | Get job by ID |
| `POST` | `/api/Job/AddJob` | Create a new job posting |
| `PUT` | `/api/Job/UpdateJob/{id}` | Update an existing job |
| `DELETE` | `/api/Job/DeleteJob/{id}` | Delete a job posting |

### 🏢 Companies

| Method | Endpoint | Description |
|:---:|:---|:---|
| `GET` | `/api/Company` | Get all companies |
| `GET` | `/api/Company/GetCompany/{id}` | Get company by ID |
| `POST` | `/api/Company/AddCompany` | Add a new company |
| `PUT` | `/api/Company/UpdateCompany/{id}` | Update a company |
| `DELETE` | `/api/Company/DeleteCompany/{id}` | Delete a company |

### 📂 Job Categories

| Method | Endpoint | Description |
|:---:|:---|:---|
| `GET` | `/api/JobCategory/GetAllJobCategories` | Get all categories |
| `GET` | `/api/JobCategory/GetJobCategory/{id}` | Get category by ID |
| `POST` | `/api/JobCategory/AddJobCategory` | Create a category |
| `PUT` | `/api/JobCategory/UpdateJobCategory/{id}` | Update a category |
| `DELETE` | `/api/JobCategory/DeleteJobCategory/{id}` | Delete a category |

### 📝 Job Applications

| Method | Endpoint | Description |
|:---:|:---|:---|
| `GET` | `/api/JobApplication/GetAllJobApplications` | Get all applications |
| `GET` | `/api/JobApplication/GetJobApplication/{id}` | Get application by ID |
| `POST` | `/api/JobApplication/AddJobApplication` | Submit an application |
| `PUT` | `/api/JobApplication/UpdateJobApplication/{id}` | Update an application |
| `DELETE` | `/api/JobApplication/DeleteJobApplication/{id}` | Delete an application |
| `GET` | `/api/JobApplication/SearchJobApplicationByJobTitle/{title}` | 🔍 Search by title |
| `GET` | `/api/JobApplication/SearchJobApplicationByLocation/{location}` | 🔍 Search by location |

### 🛠️ Skills

| Method | Endpoint | Description |
|:---:|:---|:---|
| `GET` | `/api/Skill/GetAllSkills` | Get all skills |
| `GET` | `/api/Skill/GetSkill/{id}` | Get skill by ID |
| `POST` | `/api/Skill/AddSkill` | Add a new skill |
| `PUT` | `/api/Skill/UpdateSkill/{id}` | Update a skill |
| `DELETE` | `/api/Skill/DeleteSkill/{id}` | Delete a skill |
| `GET` | `/api/Skill/GetTopTenskills` | Get top 10 skills |

### 🔗 User Skills

| Method | Endpoint | Description |
|:---:|:---|:---|
| `GET` | `/api/UserSkill/GetAllUserSkills` | Get all user-skill mappings |
| `POST` | `/api/UserSkill/AddUserSkill` | Assign skill to user |
| `PUT` | `/api/UserSkill/UpdateUserSkill/{id}` | Update user skill |
| `DELETE` | `/api/UserSkill/DeleteUserSkill/{userId}/{skillId}` | Remove skill from user |

### 💾 Saved Jobs

| Method | Endpoint | Description |
|:---:|:---|:---|
| `GET` | `/api/SavedJob/GetAllSavedJobs` | Get all saved jobs |
| `GET` | `/api/SavedJob/GetSavedJob/{id}` | Get saved job by ID |
| `POST` | `/api/SavedJob/AddSavedJob` | Save a job |
| `PUT` | `/api/SavedJob/UpdateSavedJob/{id}` | Update saved job |
| `DELETE` | `/api/SavedJob/DeleteSavedJob/{id}` | Remove saved job |

### ⭐ Company Reviews

| Method | Endpoint | Description |
|:---:|:---|:---|
| `GET` | `/api/CompanyReview/GetAllReviews` | Get all reviews |
| `POST` | `/api/CompanyReview/AddReview` | Add a review |
| `PUT` | `/api/CompanyReview/UpdateReview/{id}` | Update a review |
| `DELETE` | `/api/CompanyReview/DeleteReview/{id}` | Delete a review |
| `GET` | `/api/CompanyReview/GetTop5Reviews` | Get top 5 reviews |

---

## 🖥️ MVC Pages & Views

```mermaid
graph LR
    subgraph "🔐 Authentication"
        Login["Login Page"]
        Register["Registration Page"]
    end

    subgraph "👤 Job Seeker"
        JSDash["Dashboard"]
        JSSkills["Skills Manager"]
        JSSkillList["Skills List"]
        JSJobs["Browse Jobs"]
    end

    subgraph "🏢 Employer"
        EMDash["Dashboard"]
        EMCompany["Company Management"]
        EMCategory["Category Management"]
        EMJobs["Job Posting"]
    end

    Login -->|"Job Seeker"| JSDash
    Login -->|"Employer"| EMDash
    Register --> Login

    JSDash --> JSSkills
    JSDash --> JSSkillList
    JSDash --> JSJobs

    EMDash --> EMCompany
    EMDash --> EMCategory
    EMDash --> EMJobs

    style Login fill:#FF7043,stroke:#D84315,color:#fff
    style Register fill:#FF7043,stroke:#D84315,color:#fff
    style JSDash fill:#42A5F5,stroke:#1565C0,color:#fff
    style EMDash fill:#66BB6A,stroke:#2E7D32,color:#fff
```

---

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. 🍴 **Fork** the repository
2. 🌿 **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. 💾 **Commit** your changes (`git commit -m 'Add AmazingFeature'`)
4. 📤 **Push** to the branch (`git push origin feature/AmazingFeature`)
5. 📩 **Open** a Pull Request

---

<div align="center">

### ⭐ If you found this project helpful, please give it a star!

Made with ❤️ by [Shivam93294Valand](https://github.com/Shivam93294Valand)

</div>
