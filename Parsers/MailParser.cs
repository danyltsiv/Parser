using System;
using System.Text.RegularExpressions;
using Parser.Contracts;

namespace Parser.Parsers
{
    public class MailParser : IParser
    {
        private Regex _mailReg = new Regex(String.Concat("(\\.?\\w+)+@(\\w+.)+(com|"
            ,"com.ua|ru|if.ua|net|org|biz|edu|gov|info|int|net|pro)"));

        public string Parse(string text)
        {
            var matches = _mailReg.Matches(text);

            if (matches.Count == 0)
            {
                return text;
            }

            string parsed = "";
            int currentStartIndex = 0;

            foreach (var matched in matches)
            {
                //I can't use replace method here coz of bug with identic mails in one string.
                int indexOfUrl = text.IndexOf(matched.ToString());
                parsed = String.Concat(parsed, text.Substring(currentStartIndex, indexOfUrl - currentStartIndex));
                text = text.Remove(indexOfUrl, matched.ToString().Length);
                string replacement = String.Format("<a href='mailto:{0}'>{1}</a>", matched.ToString(), 
                    matched.ToString());
                parsed = String.Concat(parsed, replacement);
                currentStartIndex = indexOfUrl;
            }
            parsed = String.Concat(parsed, text.Substring(currentStartIndex, text.Length - currentStartIndex));
            return parsed;
        }
    }
}
