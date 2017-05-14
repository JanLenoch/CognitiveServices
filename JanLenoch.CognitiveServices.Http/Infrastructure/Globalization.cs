using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JanLenoch.CognitiveServices.Http
{
    public static class Globalization
    {
        public static Dictionary<string, string> Languages = new Dictionary<string, string>();

        static Globalization()
        {
            InitiLanguages();
        }

        private static void InitiLanguages()
        {
            string input = @"unk (AutoDetect)
zh-Hans (ChineseSimplified)
zh-Hant (ChineseTraditional)
cs (Czech)
da (Danish)
nl (Dutch)
en (English)
fi (Finnish)
fr (French)
de (German)
el (Greek)
hu (Hungarian)
it (Italian)
Ja (Japanese)
ko (Korean)
nb (Norwegian)
pl (Polish)
pt (Portuguese)
ru (Russian)
es (Spanish)
sv (Swedish)
tr (Turkish)";

            string[] pairs = input.Split("\n".ToCharArray());
            string pattern = @"([^\s]+)(\s)\(([^)]+)";

            foreach (string pair in pairs)
            {
                // May be substituted to also populate values with friendly names.
                var match = Regex.Match(pair, pattern);
                var key = match.Groups[1];
                var value = match.Groups[3];
                Languages.Add(key.Value, value.Value);
            }
        }
    }
}
