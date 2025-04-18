# Telegram Channel Parser

C# приложение для мониторинга Telegram-каналов и обработки сообщений

# Структура проекта TgParser
```bash
TgParser/
├── Configs/
│ └── AppConfig.cs         # Конфигурация приложения
├── Data/
│ └── AppDbContext.cs      # Контекст базы данных
├── Extensions/
│ └── UserExtensions.cs    # Расширения для работы с пользователями
├── Models/
│ └── RawNewsModel.cs      # Модель данных для новостей
├── Services/
│ ├── ChannelParser.cs     # Сервис парсинга каналов
│ ├── MessageLogger.cs     # Сервис логирования сообщений
│ ├── MessageProcessor.cs  # Сервис обработки сообщений
│ └── TelegramService.cs   # Основной сервис для работы с Telegram
├── UI/
│ └── UserInputProvider.cs # Взаимодействие с пользователем
├── .gitignore             # Игнорируемые файлы для Git
├── appsettings.json       # Настройки приложения
└── Program.cs             # Точка входа в приложение
```

## 📋 Основные функции
- Подключение к Telegram через официальный API
- Автоматический парсинг последних постов (Количество последних постов указывается в конфигурации)
- Логирование данных в консоль

## 🚀 Быстрый старт

### Требования
- .NET 6.0+
- PostgreSQL 12+
- API ключи Telegram (my.telegram.org)

### Установка
1. Клонируйте репозиторий:
```bash
git clone https://github.com/yourname/telegram-parser.git
cd telegram-channels-parser
```

2. Настройте базу данны:
3. Обновите конфигурацию:
## 🛠 AppConfig — Конфигурация приложения

`AppConfig` — это статический класс, отвечающий за конфигурацию Telegram-клиента и список отслеживаемых каналов для сервиса обработки Telegram-каналов.

---

### 🔑 Telegram-конфигурация

Метод:

```csharp
public static string GetTelegramConfigValue(string key)
```

Позволяет получить значения конфигурации Telegram по ключу. Используемые ключи:

| Ключ               | Описание                                      | Источник значения        |
|--------------------|-----------------------------------------------|---------------------------|
| `api_id`           | API ID Telegram (получается на [my.telegram.org](https://my.telegram.org)) | Вводится вручную         |
| `api_hash`         | API Hash Telegram                             | Вводится вручную         |
| `phone_number`     | Номер телефона, привязанный к Telegram        | Зашит в код (`+77777777777`) |
| `verification_code`| Код подтверждения из Telegram                 | Вводится вручную         |

Пример использования:

```csharp
var apiId = AppConfig.GetTelegramConfigValue("api_id");
```

> ⚠️ При запуске запрашиваются значения через консоль.

---

### 📺 Список отслеживаемых каналов

Метод:

```csharp
public static List<string> GetChannelsToMonitor()
```

Возвращает список Telegram-каналов, с которых необходимо парсить посты:

```csharp
public static List<string> GetChannelsToMonitor() => new()
{
    "channel1",
    "channel2",
    "channel3",
    "channel4"
};
```

> ❗ **Важно:** здесь указываются `username` каналов Telegram, которые приложение будет мониторить.  
> Например, если у канала ссылка `https://t.me/exampleCh`, то в список нужно добавить строку `"exampleCh"`.  
> Убедись, что каналы доступны для просмотра без подписки и не являются приватными.  
> При необходимости можно заменить или дополнить список нужными тебе каналами.

---

> Эти значения можно изменить в коде, если нужно отслеживать другие каналы.

---

### 📄 Количество последних постов

```csharp
public const int LAST_POSTS_FROM_CHANNEL_COUNT = 1;
```

Указывает, сколько последних постов загружается с каждого канала при проверке.

---

### 🧪 Пример запуска

При старте приложение может запросить следующие данные:

```
Введите API ID:
Введите API Hash:
Введите номер телефона (в формате +7...):
Введите код подтверждения:

```

Остальные параметры используются из кода по умолчанию.

### Использование
```bash
dotnet run .\TgParser.csproj
```
