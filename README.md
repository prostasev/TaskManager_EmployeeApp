# EmployeeApp - WPF-приложение для распределения обязанностей работников на проекте

EmployeeApp — это настольное приложение, которое ...

Приложение EmployeeApp создано с помощью WPF , мощной платформы для создания пользовательских интерфейсов с .NET. Оно использует MySQL для хранения и извлечения данных. 

## Установка

1. Вам нужно иметь:
	- .NET
	- MySQL Workbench
 
2. Создать базу данных в MySQL:
	```SQL
	> CREATE DATABASE emploeedb;

	> USE emploeedb;

	> CREATE TABLE Positions (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name TEXT NOT NULL
	);
 	> CREATE TABLE Users (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    FullName TEXT NOT NULL,
    PositionId INT NOT NULL,
    Rate DECIMAL(10,2) NOT NULL,
    Username TEXT NOT NULL,
    PasswordHash TEXT NOT NULL,
    FOREIGN KEY (PositionId) REFERENCES Positions(Id)
	);
 	> CREATE TABLE Tasks (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name TEXT NOT NULL,
    Description TEXT
	);
  	> CREATE TABLE Duties (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    TaskId INT,
    UserId INT,
    Name TEXT NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
	);
	```

3. Создать a App.configв корне проекта и установить строку подключения базы данных, например.
   ```XML
   <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
    	<connectionStrings>
    		<add name="emploeedb" connectionString="Server=.\SQLEXPRESS;Database=emploeedb;Integrated Security=True;" providerName="System.Data.SqlClient"/>
    	</connectionStrings>
    </configuration>
   ```

## Связаться со мной

Если у вас есть какие-либо вопросы, не стесняйтесь обращаться ко мне:

<a href="[https://www.linkedin.com/in/getimad/](https://www.linkedin.com/in/%D0%B1%D0%BE%D1%80%D0%B8%D1%81-%D1%81%D1%82%D0%B0%D1%81%D0%B5%D0%B2-a416b5287/)" target="_blank">
  <img alt="Static Badge" src="https://img.shields.io/badge/LinkedIn-blue?style=for-the-badge&logo=linkedin">
</a>
