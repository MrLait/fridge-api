namespace Fridge.Api.Constants;

public static class ApiConstants
{
    public static class Swagger
    {
        public const string DocName = "v1";
        public const string Title = "Fridge API";
        public const string Version = "v1";

        public const string BearerSchemeName = "Bearer";
        public const string BearerScheme = "bearer";
        public const string BearerFormat = "JWT";
        public const string BearerDescription = "Enter: Bearer {your JWT token}";
    }

    public static class Jwt
    {
        public const string SectionName = "Jwt";
    }
}