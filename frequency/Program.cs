using System.Text;

public class Frequency
{
    public static void Main(string[] args)
    {
        try
        {
            // get input file
            if (args.Length == 0)
            {
                Console.WriteLine("Error: Please provide an input file name.");
                 Console.WriteLine("Usage: frequency.exe <input_file> [--ignore-case]");
                return;
            }

            string inputFile = args[0];
            bool ignoreCase = args.Length > 1 && args[1].ToLower() == "--ignore-case";

            int bufferSize = 4096;

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Error: Input file '{inputFile}' not found.");
                return;
            }

            // dictionary key lookup cost O(1)
            Dictionary<char, long> charCounts = [];

            long fileSize = new FileInfo(inputFile).Length;
            long bytesRead = 0;

            // chunk the file incase we get passed a large file
            using (StreamReader reader = new StreamReader(inputFile, Encoding.UTF8))
            {
                char[] buffer = new char[bufferSize];
                int charsRead;

                while ((charsRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    bytesRead += charsRead;

                    for (int i = 0; i < charsRead; i++)
                    {
                        char chr = buffer[i];
                        if (char.IsPunctuation(chr)
                        || char.IsSymbol(chr)
                        || char.IsControl(chr)
                        || char.IsWhiteSpace(chr))
                            // Skip these
                            continue;

                        // if we don't care about case then just make everything lower
                        if (ignoreCase)
                            chr = char.ToLower(chr);

                        if (charCounts.ContainsKey(chr))
                            charCounts[chr]++;
                        else
                            charCounts[chr] = 1;
                    }

                    Console.Write($"\rBytes read: {bytesRead}/{fileSize}");
                }
            }

            // dictionary sort cost O(n log n) but should be relatively small number of lines
            var sortedChars = charCounts.OrderByDescending(kvp => kvp.Value).ToArray();
            Console.WriteLine($"\n\nTotal characters: {sortedChars.Length}");

            for (int i = 0; i < sortedChars.Length; i++)
            {
                var pair = sortedChars[i];
                Console.WriteLine($"{(i < 10 ? $"Rank: {i + 1}" : "")}: {pair.Key} ({pair.Value})");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured counting characters: {ex.Message}");
        }
    }

}