# Telegram Channel Parser

C# приложение для мониторинга Telegram-каналов и обработки сообщений

## 📋 Основные функции
- Подключение к Telegram через официальный API
- Автоматический парсинг последних постов
- Логирование данных в консоль
- Гибкая система конфигурации

## 🚀 Быстрый старт

### Требования
- .NET 6.0+
- PostgreSQL 12+
- API ключи Telegram (my.telegram.org)

### Установка
1. Клонируйте репозиторий:
```bash
git clone https://github.com/yourname/telegram-parser.git
cd telegram-parser
```

2. Настройте базу данны:
3. Обновите конфигурацию:
```bash
// AppConfig.cs
public static string GetTelegramConfigValue(string key) => key switch
{
    "api_id" => "ваш_api_id",
    "api_hash" => "ваш_api_hash",
    "phone_number" => "+XXXXXXXXXXX",
    ...
};
```

### Использование
```bash
dotnet run --project TgParser
```
