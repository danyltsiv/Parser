using System;
using System.Text.RegularExpressions;
using System.Net;
using Parser.Contracts;

namespace Parser.Parsers
{
    public class URLParser : IParser
    {
        private readonly Regex _urlReg = new Regex(String.Concat("(https?://)?(\\w+\\.)+(com|",
            "com.ua|ru|if.ua|net|org|biz|edu|gov|info|int|net|pro)((/.+)*)?"));
        private readonly Regex _titleReg = new Regex("\\w*<title>(\\w*)</title>\\w*", RegexOptions.IgnoreCase);
        private readonly WebClient client = new WebClient();

        /// <summary>
        /// Returns a parsed string in which every link is anchored with the corresponding caption.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Parse(string text)
        {
            var matches = _urlReg.Matches(text);

            if (matches.Count == 0)
            {
                return text;
            }

            string parsed = "";
            int currentStartIndex = 0;
            foreach (var matched in matches)
            {
                string matchedUrl = matched.ToString();
                string caption;

                if (!matchedUrl.Contains("http://") && !matchedUrl.Contains("https://"))
                {
                    matchedUrl = String.Concat("https://", matchedUrl);
                }

                try
                {
                    string source = client.DownloadString(matchedUrl);
                    caption = _titleReg.Match(source).Groups[1].ToString();
                }
                catch
                {
                    caption = matched.ToString();
                }
                //I can't use replace method here coz of bug with identic urls in one string.
                int indexOfUrl = text.IndexOf(matched.ToString());
                parsed = String.Concat(parsed, text.Substring(currentStartIndex, indexOfUrl - currentStartIndex));
                text = text.Remove(indexOfUrl, matched.ToString().Length);
                string replacement = String.Format("<a href='{0}'>{1}</a>", matchedUrl, caption);
                parsed = String.Concat(parsed, replacement);
                currentStartIndex = indexOfUrl;
            }
            parsed = String.Concat(parsed, text.Substring(currentStartIndex, text.Length - currentStartIndex));
            return parsed;
        }
    }
}
