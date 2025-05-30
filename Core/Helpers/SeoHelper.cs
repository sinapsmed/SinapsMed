using System.Text;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public static class SeoHelper
    {
        public static string ConverToSeo(this string url, string? lang)
        {
            switch (lang)
            {
                case "az":
                    url = SeoAz(url);
                    break;
                case "en":
                    url = SeoEn(url);
                    break;
                case "ru":
                    url = SeoRu(url);
                    break;
                case "tr":
                    url = SeoTr(url);
                    break;
                default:
                    break;
            }

            return url;
        }

        public static string SeoAz(string url)
        {
            var sb = new StringBuilder(url.Length);
            foreach (char c in url)
            {
                sb.Append(c switch
                {
                    'Ə' or 'ə' => 'e',
                    'I' or 'ı' => 'i',
                    'Ö' or 'ö' => 'o',
                    'Ş' or 'ş' => 's',
                    'Ü' or 'ü' => 'u',
                    'Ç' or 'ç' => 'c',
                    'Ğ' or 'ğ' => 'g',
                    _ => char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : c
                });
            }
            return Regex.Replace(sb.ToString(), @"[^a-z0-9]", "-").Trim('-');
        }

        public static string SeoTr(string url)
        {
            var sb = new StringBuilder(url.Length);
            foreach (char c in url)
            {
                sb.Append(c switch
                {
                    'Ç' or 'ç' => 'c',
                    'Ğ' or 'ğ' => 'g',
                    'I' or 'ı' => 'i',
                    'Ö' or 'ö' => 'o',
                    'Ş' or 'ş' => 's',
                    'Ü' or 'ü' => 'u',
                    _ => char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : c
                });
            }
            return Regex.Replace(sb.ToString(), @"[^a-z0-9]", "-").Trim('-');
        }

        public static string SeoRu(string url)
        {
            var dict = new Dictionary<char, string>
            {
                ['а'] = "a",
                ['б'] = "b",
                ['в'] = "v",
                ['г'] = "g",
                ['д'] = "d",
                ['е'] = "e",
                ['ё'] = "yo",
                ['ж'] = "zh",
                ['з'] = "z",
                ['и'] = "i",
                ['й'] = "y",
                ['к'] = "k",
                ['л'] = "l",
                ['м'] = "m",
                ['н'] = "n",
                ['о'] = "o",
                ['п'] = "p",
                ['р'] = "r",
                ['с'] = "s",
                ['т'] = "t",
                ['у'] = "u",
                ['ф'] = "f",
                ['х'] = "kh",
                ['ц'] = "ts",
                ['ч'] = "ch",
                ['ш'] = "sh",
                ['щ'] = "sch",
                ['ъ'] = "",
                ['ы'] = "y",
                ['ь'] = "",
                ['э'] = "e",
                ['ю'] = "yu",
                ['я'] = "ya"
            };

            var sb = new StringBuilder(url.Length);
            foreach (char c in url.ToLowerInvariant())
            {
                sb.Append(dict.ContainsKey(c) ? dict[c] : c);
            }

            return Regex.Replace(sb.ToString(), @"[^a-z0-9]", "-").Trim('-');
        }

        public static string SeoEn(string url)
        {
            return Regex.Replace(url.ToLowerInvariant(), @"[^a-z0-9]", "-").Trim('-');
        }
    }
}
