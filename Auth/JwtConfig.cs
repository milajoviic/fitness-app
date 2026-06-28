namespace FitnessApp.Auth
{
    public static class JwtConfig
    {
        public const string Key = "aca5696e3ff4514c5adafe68b860c177";
        public const string Issuer = "FitnessApp";
        public const int AccessTokenMinutes = 15;
        public const int RefreshTokenDays = 7;
    }
}
