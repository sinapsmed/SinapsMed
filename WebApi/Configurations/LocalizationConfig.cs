namespace WebApi.Configurations
{
    public static class LocalizationConfig
    {
        public static readonly List<string> supportedLanguages = new List<string>
        {
            "az",
            "en",
            "ru"
        };

        public static string[] GetSupportedCultures()
        {
            return supportedLanguages.ToArray();
        }

        public static string GetDefaultCulture()
        {
            return supportedLanguages[0];
        }
    }
}