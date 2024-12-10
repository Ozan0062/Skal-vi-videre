namespace Skal_vi_videre
{
    public static class GithubSecrets
    {
        // Hvis vi er i GitHub Actions (miljøvariablen GITHUB_ACTIONS er til stede),
        // skal vi bruge tomme strenge, ellers bruger vi Secrets fra Visual Studio.
        public static string ConnectionString =
            Environment.GetEnvironmentVariable("GITHUB_ACTIONS") != null ? "" : Secrets.ConnectionString;

        public static string APIKey =
            Environment.GetEnvironmentVariable("GITHUB_ACTIONS") != null ? "" : Secrets.APIKey;
    }
}