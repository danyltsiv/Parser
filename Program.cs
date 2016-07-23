using System;
using Parser.Parsers;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            MathParser mathParser = new MathParser();
            MailParser mailParser = new MailParser();
            URLParser urlParser = new URLParser();
            AdvancedMathParcer advancedMathParser = new AdvancedMathParcer();
            var urlParsed = urlParser.Parse("some www.google.com text goolgle.com here");
            var mailParsed = mailParser.Parse("want to send letter to somemail@gmail.com now!");
            var mathParsed = mathParser.Parse("calculate it =math(2+5*(2^(3+1))/5) now!");
            var advMParsed = advancedMathParser.Parse("advanced 2+4/3 calculation 2^(5+1) it's cool!");
            Console.WriteLine(String.Format("{0}\n{1}\n{2}\n{3}",urlParsed,
                mailParsed, mathParsed, advMParsed));

            Console.Read();
        }
    }
}
