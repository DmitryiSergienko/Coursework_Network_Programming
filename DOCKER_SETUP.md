# Docker Compose Setup

## Запуск проекта

### 1. Запуск всех сервисов

```bash
docker-compose up -d
```

Эта команда запустит:
- **MSSQL Server 2019** (порт 1433)
- **Examen Server** (порт 5000)

### 2. Проверка статуса контейнеров

```bash
docker-compose ps
```

### 3. Просмотр логов

Все логи:
```bash
docker-compose logs -f
```

Только MSSQL Server:
```bash
docker-compose logs -f mssql
```

Только Examen Server:
```bash
docker-compose logs -f examen-server
```

### 4. Подключение к базе данных

**Из хоста (вашего компьютера):**
- Server: `localhost,1433`
- Database: `Examen_ModelFirst`
- User: `sa`
- Password: `YourStrong!Passw0rd`

**Из другого контейнера в той же сети:**
- Server: `mssql`
- Database: `Examen_ModelFirst`
- User: `sa`
- Password: `YourStrong!Passw0rd`

### 5. Остановка сервисов

```bash
docker-compose down
```

Остановка и удаление томов (удаляет данные БД):
```bash
docker-compose down -v
```

## Компоненты

### MSSQL Server 2019

- **Образ**: `mcr.microsoft.com/mssql/server:2019-latest`
- **Порт**: 1433
- **База данных**: `Examen_ModelFirst` (создается автоматически при первом запуске)
- **Пароль SA**: `YourStrong!Passw0rd` (⚠️ измените в production!)

### Examen Server

- **Порт**: 5000 (маппится на внутренний 8080)
- **Окружение**: Docker
- **Конфигурация**: `appsettings.Docker.json`

## Примечания

1. При первом запуске MSSQL Server может потребоваться до 30 секунд для инициализации
2. Examen Server будет ждать полной готовности MSSQL Server благодаря healthcheck
3. Данные БД сохраняются в Docker volume `mssql-data`
4. SQL скрипты из папки `Examen/SQL` монтируются в контейнер MSSQL

## Troubleshooting

Если сервисы не запускаются:

1. Проверьте, что порты 1433 и 5000 свободны
2. Проверьте логи: `docker-compose logs`
3. Убедитесь, что Docker Desktop запущен
4. Попробуйте пересобрать: `docker-compose up --build`
