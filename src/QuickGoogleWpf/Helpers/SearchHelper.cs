namespace QuickGoogleWpf
{
    public static class SearchHelper
    {
        public static bool RunSearch(string input)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(input))
            {
                System.Diagnostics.Process.Start($@"https://www.google.com/search?q={input}");
                result = true;
            }
            return result;
        }
    }
}
