using System.Text;

namespace EntityModelNameFormatter
{
    class Pascal : IFormatter
    {
        public string GetFormattedName(string originalName)
        {
            string[] chunks = originalName.Split(new char[] {'_'});
            StringBuilder result = new StringBuilder();

            foreach(string chunk in chunks)
            {
                result.Append(char.ToUpper(chunk[0]));
                result.Append(chunk.Substring(1));
            }

            return result.ToString();
        }
    }
}
