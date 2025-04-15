using System;
using System.Text.RegularExpressions;

public static class UserInputProvider
{
    public static string AskWithValidation(string prompt, Func<string, bool> validator)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(prompt);
            Console.ResetColor();

            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                PrintError("Поле не может быть пустым!");
                continue;
            }

            if (validator(input))
                return input;

            PrintError("Некорректный формат. Попробуйте еще раз.");
        }
    }



    //////////Методы для валидации входных данных/////////
    public static bool ValidateApiId(string input) =>
        input.Length >= 5 && int.TryParse(input, out _);

    public static bool ValidateApiHash(string input) =>
        input.Length == 32 && Regex.IsMatch(input, @"^[a-f0-9]+$");

    public static bool ValidateVerificationCode(string input) =>
        input.Length == 5 && int.TryParse(input, out _);

    public static bool ValidatePhoneNumber(string input) =>
        Regex.IsMatch(input, @"^\+7\d{10}$");

    //////////////////////////////////////////////////////


    //////////Вывод со стилямия///////////////////////////
    private static void PrintError(string message)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"⛔ {message}");
        Console.ForegroundColor = originalColor;
    }

    public static void PrintSuccess(string message)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✓ {message}");
        Console.ForegroundColor = originalColor;
    }
}
