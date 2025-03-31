# EmployeeApp

**EmployeeApp** — это настольное приложение, предназначенное для распределения обязанностей сотрудников на проекте. Оно обеспечивает эффективный контроль за работой сотрудников и управление проектами.

Приложение разработано с использованием **WPF** (Windows Presentation Foundation) и использует **MySQL** для хранения и извлечения данных.

## Установка

### Необходимые требования

Перед началом установки убедитесь, что у вас установлены следующие компоненты:

- [.NET](https://dotnet.microsoft.com/download/dotnet)
- [MySQL](https://dev.mysql.com/downloads/mysql/)

### Шаги по установке

1. Восстановите базу данных в MySQL. Бэкап лежит в файлах проекта и называется `backup_employeedb.sql`. Для восстановления базы данных выполните следующие действия:

   ```bash
   #!/bin/bash

   # Параметры базы данных
   DB_USER="ваш_пользователь"
   DB_PASSWORD="ваш_пароль"
   DB_NAME="EmployeeDB"
   BACKUP_FILE="путь_к_вашему_файлу_бэкапа.sql"  # Замените на путь к backup_employeedb.sql

   # Восстановление базы данных
   mysql -u $DB_USER -p$DB_PASSWORD $DB_NAME < $BACKUP_FILE




Измените строку подключения к базе данных в файле EmployeeDbContext.cs:
   
    C#

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(
            "Server=localhost;Database=EmployeeDB;User=ваш_пользователь;Password=ваш_пароль;",
            ServerVersion.AutoDetect("Server=localhost;Database=EmployeeDB;User=ваш_пользователь;Password=ваш_пароль;"));
    }

    Замените ваш_пользователь и ваш_пароль на ваши учетные данные для подключения к MySQL.

### Запуск приложения

После установки всех необходимых компонентов и настройки конфигурации, вы можете запустить приложение с помощью Visual Studio или командной строки.

### Контактная информация

Если у вас есть вопросы или предложения, не стесняйтесь обращаться ко мне:

<a href="https://www.linkedin.com/in/%D0%B1%D0%BE%D1%80%D0%B8%D1%81-%D1%81%D1%82%D0%B0%D1%81%D0%B5%D0%B2-a416b5287/" target="_blank">

<img alt="LinkedIn Badge" src="https://img.shields.io/badge/LinkedIn-blue?style=for-the-badge&logo=linkedin">

</a>

