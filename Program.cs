using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Renamer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine("Usage: Renamer <pattern_old> <pattern_new> [\"directory\"]");
                return;
            }

            string patternOld = args[0];
            string patternNew = args[1];
            string directory = args.Length == 3 ? args[2] : Directory.GetCurrentDirectory();

            RenameFiles(patternOld, patternNew, directory);
        }

        static void RenameFiles(string patternOld, string patternNew, string directory)
        {
            try
            {
                string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

                int count = 0;
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    if (Regex.IsMatch(fileName, patternOld))
                    {
                        count++;
                        string newPattern = Regex.Replace(fileName, patternOld, patternNew);
                        string [] newPatternSplit = newPattern.Split('.');
                        string newFileName = newPatternSplit[0] + count.ToString()+"."+ newPatternSplit[1];
                        string newPath = Path.Combine(Path.GetDirectoryName(file), newFileName);
                        File.Move(file, newPath);
                        Console.WriteLine($"Renamed: {fileName} -> {newFileName}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }
    }
}
