# Скрипт остановки Docker Compose
# Использование: .\docker-stop.ps1

Write-Host "🛑 Остановка Docker Compose..." -ForegroundColor Yellow

# Остановка сервисов
docker-compose down

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n✅ Все сервисы остановлены" -ForegroundColor Green
} else {
    Write-Host "`n❌ Ошибка при остановке сервисов" -ForegroundColor Red
    exit 1
}

# Опция удаления volumes
$response = Read-Host "`nУдалить данные БД (volumes)? (y/N)"
if ($response -eq 'y' -or $response -eq 'Y') {
    Write-Host "`n🗑️  Удаление volumes..." -ForegroundColor Yellow
    docker-compose down -v
    Write-Host "✅ Volumes удалены" -ForegroundColor Green
}
