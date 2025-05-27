# NawatechAuthApp - User Registration & Product Management App (.NET Core)

## 📝 Description
This project is an implementation of **Nawatech Junior Fullstack Developer** technical test, featuring **User Registration**, **Login**, and **Product & Category Management** using **ASP.NET Core (.NET 6/7/8)** and **Entity Framework Core**. Optional email confirmation feature is also available.

---

## 🚀 Main Features
1. **User Registration**  
   Users can register an account with email and password.  
   *(Optional) Email confirmation for account verification.*

2. **User Login**  
   Users can login using registered email and password.

3. **User Profile Edit**  
   Users can edit their profile including changing profile photo.

4. **Product Category Management**  
   Users can create, update, and soft delete product category data.

5. **Product Management**  
   Users can create, update, and soft delete product data.

---

## 🛠 Technologies Used
- ASP.NET Core Web App (Razor Pages / MVC)
- ASP.NET Core Identity
- Entity Framework Core
- MySQL
- C#
- .NET 6 / .NET 7 / .NET 8 SDK
- (Optional) SMTP / MailKit for Email Confirmation

---

## ⚙️ Setup & Running Project

### ✅ Prerequisites
Make sure you have installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) (optional, for GUI)

---

### 🔧 Setup Steps

#### 1. **Clone Repository**
```bash
git clone https://github.com/fajarfauzian/NawatechAuthApp-Junior-Dev-Coding-Test.git
cd NawatechAuthApp-Junior-Dev-Coding-Test
```

#### 2. **Setup MySQL Database**
Create new database in MySQL:
```sql
CREATE DATABASE nawatech_auth_db;
```

#### 3. **Configure Connection String**
Open `appsettings.json` file and adjust MySQL connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=nawatech_auth_db;Uid=root;Pwd=your_password;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Note:** Replace `your_password` with your MySQL password.

#### 4. **Install Dependencies**
Restore all required packages:
```bash
dotnet restore
```

#### 5. **Run Database Migration**

##### a. **Install Entity Framework Tools** (if not already installed)
```bash
dotnet tool install --global dotnet-ef
```

##### b. **Create First Migration**
```bash
dotnet ef migrations add InitialCreate
```

##### c. **Update Database**
Run migration to create tables in database:
```bash
dotnet ef database update
```

#### 6. **Build Project**
```bash
dotnet build
```

#### 7. **Run Application**
```bash
dotnet run
```

Application will run at:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:5001`

---

## 🗃️ Database Structure

After successful migration, the following tables will be created:

### Identity Tables (ASP.NET Core Identity)
- `AspNetUsers` - User data
- `AspNetRoles` - User roles
- `AspNetUserRoles` - User and role relationships
- `AspNetUserClaims` - User claims
- `AspNetUserLogins` - External logins
- `AspNetUserTokens` - User tokens
- `AspNetRoleClaims` - Role claims

### Custom Tables
- `Categories` - Product category data
- `Products` - Product data

---

## 📋 Additional Migration Commands

### Create New Migration
If there are model changes:
```bash
dotnet ef migrations add MigrationName
```

### Update Database to Specific Migration
```bash
dotnet ef database update MigrationName
```

### Rollback Migration
```bash
dotnet ef database update PreviousMigrationName
```

### Remove Last Migration
```bash
dotnet ef migrations remove
```

### Reset Database (Drop & Recreate)
```bash
dotnet ef database drop
dotnet ef database update
```

---

## 🔐 Email Service Configuration (Optional)

If you want to activate email confirmation feature, add SMTP configuration in `appsettings.json`:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "NawatechAuthApp"
  }
}
```

---

## 🚨 Troubleshooting

### Connection String Error
If database connection error occurs:
1. Make sure MySQL Server is running
2. Check username, password, and database name
3. Make sure MySQL port (default 3306) is not blocked

### Migration Error
```bash
# If migration error occurs, try:
dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Port Already in Use
If port is already in use, change in `Properties/launchSettings.json`:
```json
{
  "applicationUrl": "https://localhost:5002;http://localhost:5001"
}
```

---

## 📝 Default Login

After setup completion, create first account through register page or seed admin data if available.

---

## 🤝 Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Create Pull Request

---

## 📄 License

This project was created for Nawatech Junior Fullstack Developer technical test purposes.

---

## 📞 Contact

If you have questions or issues, please contact:
- **Email:** fajarfauzian@example.com
- **GitHub:** [fajarfauzian](https://github.com/fajarfauzian)
