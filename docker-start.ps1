# Скрипт быстрого запуска Docker Compose
# Использование: .\docker-start.ps1

Write-Host "🚀 Запуск Docker Compose..." -ForegroundColor Cyan

# Проверка Docker
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Docker не найден. Установите Docker Desktop." -ForegroundColor Red
    exit 1
}

# Проверка, что Docker запущен
try {
    docker ps | Out-Null
} catch {
    Write-Host "❌ Docker не запущен. Запустите Docker Desktop." -ForegroundColor Red
    exit 1
}

Write-Host "✓ Docker доступен" -ForegroundColor Green

# Остановка и удаление старых контейнеров
Write-Host "`n🧹 Очистка старых контейнеров..." -ForegroundColor Yellow
docker-compose down 2>$null

# Запуск сервисов
Write-Host "`n🐳 Запуск сервисов..." -ForegroundColor Cyan
docker-compose up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n✅ Сервисы успешно запущены!" -ForegroundColor Green
    Write-Host "`n📊 Статус контейнеров:" -ForegroundColor Cyan
    docker-compose ps
    
    Write-Host "`n🔗 Доступные сервисы:" -ForegroundColor Cyan
    Write-Host "  • MSSQL Server: localhost:1433" -ForegroundColor White
    Write-Host "    - User: sa" -ForegroundColor Gray
    Write-Host "    - Password: YourStrong!Passw0rd" -ForegroundColor Gray
    Write-Host "    - Database: Examen_ModelFirst" -ForegroundColor Gray
    Write-Host "`n  • Examen Server: http://localhost:5000" -ForegroundColor White
    
    Write-Host "`n📝 Полезные команды:" -ForegroundColor Cyan
    Write-Host "  docker-compose logs -f          # Просмотр всех логов" -ForegroundColor Gray
    Write-Host "  docker-compose logs -f mssql    # Логи MSSQL Server" -ForegroundColor Gray
    Write-Host "  docker-compose down             # Остановка сервисов" -ForegroundColor Gray
    Write-Host "  docker-compose restart          # Перезапуск сервисов" -ForegroundColor Gray
} else {
    Write-Host "`n❌ Ошибка при запуске сервисов" -ForegroundColor Red
    Write-Host "Проверьте логи: docker-compose logs" -ForegroundColor Yellow
    exit 1
}
