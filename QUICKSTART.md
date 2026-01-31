# Быстрый старт Docker Compose

## 🚀 Запуск

```powershell
# Запуск всех сервисов
docker compose up -d

# Или используйте PowerShell скрипт
.\docker-start.ps1
```

## 📊 Проверка статуса

```powershell
docker compose ps
```

## 📝 Просмотр логов

```powershell
# Все сервисы
docker compose logs -f

# Только MSSQL
docker compose logs -f mssql

# Только Examen Server
docker compose logs -f examen-server
```

## 🔌 Подключение к MSSQL

**Server**: localhost,1433  
**Database**: Examen_ModelFirst  
**User**: sa  
**Password**: YourStrong!Passw0rd

## 🛑 Остановка

```powershell
# Остановка сервисов
docker compose down

# Остановка и удаление данных
docker compose down -v

# Или используйте PowerShell скрипт
.\docker-stop.ps1
```

## 🔄 Перезапуск

```powershell
docker compose restart
```

## 🏗️ Пересборка

```powershell
docker compose up --build -d
```

---

📚 Подробная документация: [DOCKER_SETUP.md](DOCKER_SETUP.md)
