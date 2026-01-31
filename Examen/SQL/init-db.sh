#!/bin/bash

# Ожидание запуска SQL Server
echo "Waiting for SQL Server to start..."
sleep 30s

# Создание базы данных
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong!Passw0rd -Q "CREATE DATABASE Examen_ModelFirst"

echo "Database Examen_ModelFirst created successfully"
