public static class AppConfig
{

    public const int LAST_POSTS_FROM_CHANNEL_COUNT = 1;
    public static string GetTelegramConfigValue(string key) => key switch
    {
        "api_id" => Ask("Введите API ID: "),
        "api_hash" => Ask("Введите API Hash: "),
        "phone_number" => "+79371116708",
        "verification_code" => Ask("Введите код подтверждения: "),
        _ => null
    };

    public static List<string> GetChannelsToMonitor() => new()
    {
        "kazan_smi",
        "RhymesMorgen",
        "vakansii_it",
        "Golang_google"
    };

    private static string Ask(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()?.Trim();
    }
}