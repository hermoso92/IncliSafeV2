namespace IncliSafe.Client.Extensions
{
    public static class DecimalExtensions
    {
        public static string Direction(this decimal value) => value switch
        {
            > 0 => "up",
            < 0 => "down",
            _ => "neutral"
        };
    }
} 