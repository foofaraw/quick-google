namespace QuickGoogleWpf
{
    public static class SearchHelper
    {
        public static bool RunSearch(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                string encodedInput = System.Web.HttpUtility.UrlEncode(input);
                return System.Diagnostics.Process.Start($@"https://www.google.com/search?q={encodedInput}") != null;
            }
            return false;
        }
    }
}
