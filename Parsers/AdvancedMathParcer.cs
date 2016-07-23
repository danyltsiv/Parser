using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Parser.Contracts;

namespace Parser.Parsers
{
    public class AdvancedMathParcer : MathParser, IParser
    {
        protected readonly Regex _mathFinderReg = new Regex(String.Concat("((\\(\\s*)?\\-?\\d+(\\s*,\\d*)?)",
            "(\\s*[+\\-\\*/\\^]\\s*(\\(\\s*)?\\-?\\d+(\\s*,\\d*)?(\\s*\\))?)+"));

        /// <summary>
        /// Returns a parsed string in which every command mathematical expression is
        /// replaced by result of solved expression.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        new public string Parse(string text)
        {
            var matches = _mathFinderReg.Matches(text);

            if (matches.Count == 0)
            {
                return text;
            }

            string replacement;

            foreach (var matched in matches)
            {
                try
                {
                    var RPNList = ConvertToRPN(matched.ToString());
                    replacement = CalculateTheRPNExpression(RPNList);
                }
                catch (Exception)
                {
                    continue;
                }
                text = _mathFinderReg.Replace(text, replacement, 1);
            }
            return text;
        }

        public List<string> FindExpressions(string text)
        {
            var matches = _mathFinderReg.Matches(text)
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();
            return matches;   
        }

    }
}
