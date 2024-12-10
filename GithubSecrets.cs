namespace Skal_vi_videre
{
    public static class GithubSecrets
    {
        // Hvis miljøvariablen "GITHUB_ACTIONS" er sat, så er vi i GitHub Actions, ellers er vi i Visual Studio.
        public static string ConnectionString =
            Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == null ? Secrets.ConnectionString : "";

        public static string APIKey =
            Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == null ? Secrets.APIKey : "";
    }
}