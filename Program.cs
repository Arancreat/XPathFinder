using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XPathFinder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("USAGE:");
                Console.WriteLine("XPathFinder.exe [input] [keyword] [tabulation size]");
                Console.WriteLine();
                Console.WriteLine("[input]\t\tFilesystem path to the BaseX paths txt file");
                Console.WriteLine("[keyword]\tKeyword in desired xPath");
                Console.WriteLine("[tab size]\tNumber of whitespaces in one tab");
                return;
            }

            string inputPath = Path.GetFullPath(args[0]);
            string keyWord = args[1];
            int tabSize = int.Parse(args[2]);
            List<string> desiredXPaths = new List<string>();
            List<string> currentXPath = new List<string>();
            string? line;

            Console.WriteLine($"Input: {inputPath}");
            Console.WriteLine($"Keyword: {keyWord}");
            Console.WriteLine($"Tab size: {tabSize}");

            try
            {
                // Open stream of data from input file
                StreamReader sr = new(inputPath);

                // Read first line of input file
                line = sr.ReadLine();

                // While it's not end of file
                while (line != null)
                {
                    // Skip useless lines
                    if (line.Trim() == "" || line.Contains("text():"))
                    {
                        line = sr.ReadLine();
                        continue;
                    }

                    // Count the number of whitespaces
                    int count = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == ' ') count++;
                        else break;
                    }
                    // Divide to get a number of tabs
                    count = count / tabSize;

                    // If a number of tabs is higher than
                    // number of elements in the List
                    // then it means that a new line
                    // is going to the child element
                    if (count > currentXPath.Count - 1) {
                        currentXPath.Add(line.Substring(0, line.LastIndexOf(":")).Trim());
                    }
                    // If a number of tabs is equal to 
                    // the number of elements in the List
                    // then it means that a new line
                    // is going to the neighbouring element
                    else if (count == currentXPath.Count - 1)
                    {
                        currentXPath.RemoveAt(currentXPath.Count - 1);
                        currentXPath.Add(line.Substring(0, line.LastIndexOf(":")).Trim());
                    }
                    // If a number of tabs is lesser than
                    // number of elements in the List
                    // then it means that a new line is 
                    // going to the ancestor element
                    else {
                        while (count < currentXPath.Count)
                        {
                            currentXPath.RemoveAt(currentXPath.Count - 1);
                        }
                        currentXPath.Add(line.Substring(0, line.LastIndexOf(":")).Trim());
                    }
                    // (zachem ya pishu commenty na angliyskom?)
                    string xPath = string.Join("/", currentXPath);

                    // If XPath contains a keyword then
                    // we need to save it lol
                    if (xPath.ToLower().Contains(keyWord.ToLower())) 
                        desiredXPaths.Add(xPath);

                    // Read the next line
                    line = sr.ReadLine();
                }
                sr.Close();

                foreach (string xPath in desiredXPaths)
                {
                    Console.WriteLine(xPath);
                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine(ex.ToString());
            }

            return;
        }
    }
}