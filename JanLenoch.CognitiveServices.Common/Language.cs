using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JanLenoch.CognitiveServices.Common
{
    public static class Globalization
    {
        public static Dictionary<string, string> Languages { get; private set; }

        static Globalization()
        {
            string input = @"unk(AutoDetect),
zh-hans(ChineseSimplified),
zh-hant(ChineseTraditional),
cs(Czech),
da(Danish),
nl(Dutch),
en(English),
fi(Finnish),
fr(French),
de(German),
el(Greek),
hu(Hungarian),
it(Italian),
ja(Japanese),
ko(Korean),
nb(Norwegian),
pl(Polish),
pt(Portuguese),
ru(Russian),
es(Spanish),
sv(Swedish),
tr(Turkish)";

            string[] pairs = input.Split(",".ToCharArray());
            string pattern = @"^([^\(]+)([^\)])$";

            foreach (string pair in pairs)
            {
                var key = Regex.Matches(pair, pattern)[0];
                var value = Regex.Matches(pair, pattern)[1];
                Languages.Add(key.Value, value.Value);
            }
        }
    }
}
