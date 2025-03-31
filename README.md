# EmployeeApp

**EmployeeApp** — это настольное приложение, предназначенное для распределения обязанностей сотрудников на проекте. Оно обеспечивает эффективный контроль за работой сотрудников и управления проектами.

Приложение разработано с использованием **WPF** (Windows Presentation Foundation) и использует **MySQL** для хранения и извлечения данных.

## Установка

### Необходимые требования

Перед началом установки убедитесь, что у вас установлены следующие компоненты:

- [.NET](https://dotnet.microsoft.com/download/dotnet)
- [MySQL](https://dev.mysql.com/downloads/mysql/)

### Шаги по установке

1. Создайте базу данных в MySQL:

   ```sql
   CREATE DATABASE emploeedb;

    Создайте файл App.config в корне проекта и установите строку подключения к базе данных:
    xml

    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
        <connectionStrings>
            <add name="emploeedb" 
                 connectionString="Server=localhost;Database=emploeedb;User Id=ваш_пользователь;Password=ваш_пароль;" 
                 providerName="MySql.Data.MySqlClient"/>
        </connectionStrings>
    </configuration>

    Замените ваш_пользователь и ваш_пароль на ваши учетные данные для подключения к MySQL.

Запуск приложения

После установки всех необходимых компонентов и настройки конфигурации, вы можете запустить приложение с помощью Visual Studio или командной строки.
Контактная информация

Если у вас есть вопросы или предложения, не стесняйтесь обращаться ко мне:

<a href="https://www.linkedin.com/in/%D0%B1%D0%BE%D1%80%D0%B8%D1%81-%D1%81%D1%82%D0%B0%D1%81%D0%B5%D0%B2-a416b5287/" target="_blank">

<img alt="LinkedIn Badge" src="https://img.shields.io/badge/LinkedIn-blue?style=for-the-badge&logo=linkedin">

</a>

```
