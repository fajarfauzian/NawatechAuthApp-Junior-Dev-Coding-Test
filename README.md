# NawatechAuthApp - User Registration & Product Management App (.NET Core)

## 📝 Deskripsi
Proyek ini merupakan implementasi dari technical test **Nawatech Junior Fullstack Developer**, dengan fitur utama **User Registration**, **Login**, dan **Manajemen Produk & Kategori** menggunakan **ASP.NET Core (.NET 6/7/8)** dan **Entity Framework Core**. Fitur opsional konfirmasi email juga tersedia.

## 📸 Screenshots

### Halaman Utama
![Halaman Utama](/wwwroot/image/home.png)

---

## 🚀 Fitur Utama
1. **User Registration**  
   User dapat melakukan registrasi akun dengan email dan password.  
   *(Opsional) Email konfirmasi untuk verifikasi akun.*

2. **User Login**  
   User dapat masuk/login menggunakan email dan password yang sudah terdaftar.

3. **Edit Profil User**  
   User dapat mengedit profilnya termasuk mengubah foto profil.

4. **Manajemen Kategori Produk**  
   User dapat membuat, mengubah, dan melakukan soft delete pada data kategori produk.

5. **Manajemen Produk**  
   User dapat membuat, mengubah, dan melakukan soft delete pada data produk.

---

## 🛠 Teknologi yang Digunakan
- ASP.NET Core Web App (Razor Pages / MVC)
- ASP.NET Core Identity
- Entity Framework Core
- MySQL
- C#
- .NET 8 SDK
- (Opsional) SMTP / MailKit untuk Email Confirmation

---

## ⚙️ Cara Setup & Menjalankan Project

### ✅ Prasyarat
Pastikan kamu sudah install:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) (opsional, untuk GUI)

---

### 🔧 Langkah-langkah Setup

#### 1. **Clone Repository**
```bash
git clone https://github.com/fajarfauzian/NawatechAuthApp-Junior-Dev-Coding-Test.git
cd NawatechAuthApp-Junior-Dev-Coding-Test
```

#### 2. **Setup Database MySQL**
Buat database baru di MySQL:
```sql
CREATE DATABASE nawatech_auth_db;
```

#### 3. **Konfigurasi Connection String**
Buka file `appsettings.json` dan sesuaikan connection string MySQL:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=nawatech_auth_db;Uid=root;Pwd=password-kamu;"
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

**Catatan:** Ganti `your_password` dengan password MySQL kamu.

#### 4. **Install Dependencies**
Restore semua package yang diperlukan:
```bash
dotnet restore
```

#### 5. **Menjalankan Migrasi Database**

##### a. **Install Entity Framework Tools** (jika belum ada)
```bash
dotnet tool install --global dotnet-ef
```

##### b. **Membuat Migrasi Pertama**
```bash
dotnet ef migrations add InitialCreate
```

##### c. **Update Database**
Jalankan migrasi untuk membuat tabel-tabel di database:
```bash
dotnet ef database update
```

#### 6. **Build Project**
```bash
dotnet build
```

#### 7. **Menjalankan Aplikasi**
```bash
dotnet run
```

Aplikasi akan berjalan di:
- **HTTP:** `http://localhost:5264`

---

## 🗃️ Struktur Database

Setelah migrasi berhasil, tabel-tabel berikut akan dibuat:

### Tabel Identity (ASP.NET Core Identity)
- `AspNetUsers` - Menyimpan data pengguna (nama, email, password, dll).
- `AspNetRoles` - Menyimpan data role (Admin, User, dsb).
- `AspNetUserRoles` - Relasi antara user dan role.
- `AspNetUserClaims` - Klaim tambahan milik user.
- `AspNetRoleClaims` - Klaim tambahan milik role.
- `AspNetUserLogins` - Informasi login eksternal (misal Google, Facebook).
- `AspNetUserTokens` - Token yang digunakan user (misal untuk reset password).

### Tabel Custom
- `ProductCategories` - Menyimpan data kategori produk (nama kategori, deskripsi, dll).
- `Products` - Menyimpan data produk (nama produk, harga, stok, gambar, dll).

### Tabel Migration (Entity Framework Core)
- `__EFMigrationsHistory` - Menyimpan riwayat migrasi database EF Core.

---

## 📋 Perintah Migration Tambahan

### Membuat Migration Baru
Jika ada perubahan model:
```bash
dotnet ef migrations add InitialCreate
```

### Update Database ke Migration Tertentu
```bash
dotnet ef database update NamaMigration
```

### Rollback Migration
```bash
dotnet ef database update NamaMigrationSebelumnya
```

### Hapus Migration Terakhir
```bash
dotnet ef migrations remove
```

### Reset Database (Drop & Recreate)
```bash
dotnet ef database drop
dotnet ef database update
```

---

## 🔐 Konfigurasi Email Service (Opsional)

Jika ingin mengaktifkan fitur email confirmation, tambahkan konfigurasi SMTP di `appsettings.json`:

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

### Error Connection String
Jika terjadi error koneksi database:
1. Pastikan MySQL Server berjalan
2. Cek username, password, dan nama database
3. Pastikan port MySQL (default 3306) tidak terblokir

### Error Migration
```bash
# Jika ada error saat migration, coba:
dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Port Already in Use
Jika port sudah digunakan, ubah di `Properties/launchSettings.json`:
```json
{
  "applicationUrl": "https://localhost:7219;http://localhost:5264"
}
```

---

## 📝 Default Login

Setelah setup selesai, buat akun pertama melalui halaman register atau seed data admin jika tersedia.

---

## 📞 Kontak

Jika ada pertanyaan atau kendala, silakan hubungi:
- **Email:** fajarfauzian53@gmail.com
- **GitHub:** [fajarfauzian](https://github.com/fajarfauzian)
