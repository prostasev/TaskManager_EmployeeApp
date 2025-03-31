# EmployeeApp - WPF-приложение для распределения обязанностей работников на проекте

EmployeeApp — это настольное приложение, которое предназначено для контроля сотрудников.

Приложение EmployeeApp создано с помощью WPF , мощной платформы для создания пользовательских интерфейсов с .NET. Оно использует MySQL для хранения и извлечения данных. 

## Установка

1. Вам нужно иметь:
	- .NET
	- MySQL
 
2. Создать базу данных в MySQL:
	```
 #!/bin/bash

# Параметры базы данных
DB_USER="ваш_пользователь"
DB_PASSWORD="ваш_пароль"
DB_NAME="ваша_база_данных"
BACKUP_FILE="путь_к_вашему_файлу_бэкапа.sql"

# Восстановление базы данных
mysql -u $DB_USER -p$DB_PASSWORD $DB_NAME < $BACKUP_FILE

echo "Восстановление завершено."
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
