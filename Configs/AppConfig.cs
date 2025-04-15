using System;
using System.Collections.Generic;

public static class AppConfig
{
    public const int LAST_POSTS_FROM_CHANNEL_COUNT = 1;

    // Telegram config, по ключу — значение
    private static readonly Dictionary<string, string> _configCache = new();



    //Cписок каналов, с которых парсить данные///////////////////
    public static List<string> GetChannelsToMonitor() => new()
    {
        "exampleCh",
        "exampleCh",
        "exampleCh",
        "exampleCh"
    };


    /* 
      
     P.S:
     
     Т.е если ссылка на канал "https://t.me/exampleCh", 
        то в список нужно указывать "channel"
     
     */


    /////////////////////////////////////////////////////////////


    public static string GetTelegramConfigValue(string key)
    {
        if (_configCache.TryGetValue(key, out var cachedValue))
            return cachedValue;

        var value = key switch
        {
            "api_id" => UserInputProvider.AskWithValidation("Введите API ID: ", UserInputProvider.ValidateApiId),
            "api_hash" => UserInputProvider.AskWithValidation("Введите API Hash: ", UserInputProvider.ValidateApiHash),
            "phone_number" => UserInputProvider.AskWithValidation("Введите номер телефона (в формате +7...): ", UserInputProvider.ValidatePhoneNumber),
            "verification_code" => UserInputProvider.AskWithValidation("Введите код подтверждения из Telegram: ", UserInputProvider.ValidateVerificationCode),
            _ => null
        };

        if (value != null)
            _configCache[key] = value;

        return value;
    }


   
    
    
}
