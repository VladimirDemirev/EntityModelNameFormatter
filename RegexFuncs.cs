using System.Text.RegularExpressions;

namespace EntityModelNameFormatter
{
    class RegexFuncs
    {
        /// <summary>
        /// Insert [Column("real_database_name")] and [Key]  - before property
        /// </summary>
        private static string getPropAttributes(string propName)
        {
            string r = "        [Column(\"" + propName + "\")]" + System.Environment.NewLine;

            if (propName.ToLower().Equals("id"))
                r = "        [Key]" + System.Environment.NewLine + r;
            
            return r;
        }

        public static string replaceClassName(string s, IFormatter formatter, string oldName, string newName)
        {
            string result = s;
            result = result.Replace(" class " + oldName, " class " + newName);
            result = result.Replace("public " + oldName + "(", "public " + newName + "(");
            return result;
        }

        //REGEX: XXXX { get; set; }
        public static string regexPropNames(string s, IFormatter formatter)
        {
            var result = Regex.Replace(s, @"\r\n.*\s[a-z_0-9]+\s{\sget;\sset;", delegate (Match match)
            {
                string matched = match.ToString();

                string origName = Regex.Match(matched, @"\s[a-z_0-9]+\s{").Value;
                origName = origName.Replace(" {", "").TrimStart();
                string newName = formatter.GetFormattedName(origName);

                string newLine = matched.Replace(origName + " {", newName + " {").Trim();

                string r = System.Environment.NewLine + (matched.Contains(" virtual ") ? "" : getPropAttributes(origName)) + "        " + newLine;
                return r;
            });


            return result;
        }

        //REGEX: public virtual XXXXX
        public static string regexVirtualProps(string s, IFormatter formatter)
        {
            var result = Regex.Replace(s, @"\spublic+\svirtual+\s[a-z_0-9]+\s", delegate (Match match)
            {
                string v = match.ToString();

                string[] a = v.TrimEnd().Split(new char[] { ' ' });
                string n = formatter.GetFormattedName(a[a.Length - 1].Trim());

                return " public virtual " + n + " ";
            });

            return result;
        }

        //REGEX: <XXXXX>
        public static string regexInTriangleBrakets(string s, IFormatter formatter)
        {
            var result = Regex.Replace(s, @"<+[a-z_0-9]+>", delegate (Match match)
            {
                string v = match.ToString();

                string a = v.Trim(new char[] { '<', '>', ' ' });
                string n = formatter.GetFormattedName(a);

                return "<" + n + ">";
            });

            return result;
        }

        //REGEX: XXXX = new HashSet
        public static string regexHashSet(string s, IFormatter formatter)
        {
            var result = Regex.Replace(s, @"\s[a-z_0-9]+\s=\snew+\s", delegate (Match match)
            {
                string v = match.ToString();

                string[] a = v.TrimStart().Split(new char[] { ' ' });
                string n = formatter.GetFormattedName(a[0]);

                return " " + n + " = new ";
            });

            return result;
        }

        //REGEX: (e => e.XXXXX)
        public static string regexLambda1(string s, IFormatter formatter)
        {
            var result = Regex.Replace(s, @"\(e\s=>\se\.[a-z_0-9]+\)", delegate (Match match)
            {
                string v = match.ToString();

                string[] a = v.TrimStart().Split(new char[] { '.' });
                string n = formatter.GetFormattedName(a[a.Length-1]);

                return "(e => e." + n;
            });

            return result;
        }

    }
}
