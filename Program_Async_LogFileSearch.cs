using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace MatchDump
{
    
    partial class Program
    {
        public struct InputArg{
            public string FileName;
            public string Pattern;
            public int Delay;
        };

        static void Main(string[] args)
        {
            #region [ App Setup Code ]

            string rootPath = @"c:\src\C#_Questions_PL\Interview_Questions\TestData\";
            string outputFileName = "Output.txt";

            CleanUp(rootPath, outputFileName);

            List<string> patternList = new List<string>() {
                "number", "search", "function", "destination", "files", "lines", "and" };
            List<int> delayList = new List<int>() { 1000, 3000, 5000, 7000, 9000, 11000 };
            //
            List<string> inputFileList = FetchTestFiles(rootPath);

            // Randomaly assign a delay and search pattern to the inputFileList
            List<InputArg> inputArgs = BuildArgs(inputFileList, patternList, delayList);
            
            #endregion

            MatchCountFolderAsync(rootPath, inputArgs, outputFileName);

            Console.ReadKey();
        } // END_Main()

        public async static void MatchCountFolderAsync(string rootPath, List<InputArg> inputArgList, string outputFileName)
        {
            // Launch Tasks
            List<Task<Tuple<string, string, int, int>>> taskList = new List<Task<Tuple<string, string, int, int>>>();

            foreach (InputArg inputArg in inputArgList)
            {
                taskList.Add(
                    Task<Tuple<string, string, int, int>>.Run(() =>
                    MatchCountFileAsync(inputArg.Pattern, rootPath, inputArg.FileName, inputArg.Delay)));

                Console.WriteLine(
                    "From MatchCountFolderAsync: MatchCountFileAsync Called "+
                    "--- File Name: {0} --- Delay: {1}",
                    inputArg.FileName, inputArg.Delay);
            }
            
            // Wait
            Tuple<string, string, int, int>[] resultsArray = await Task.WhenAll(taskList);
            Console.WriteLine("\nTask.WhenAll: Complete --- Outputing Results from each MatchCountFolderAsync to: Output.txt");

            // Write data to filesystem
            FsWriteMatches(rootPath, outputFileName, resultsArray);

            // Output results of processing
            int totalMatchLineCount = 0;
            int totalMatchCount = 0;
            foreach (Tuple<string, string, int, int> result in resultsArray)
            {
                totalMatchLineCount += result.Item3;
                totalMatchCount += result.Item4;
            }
            //
            Console.WriteLine("\nFile Count: " + resultsArray.Length);
            Console.WriteLine("Total Match Line Count: " + totalMatchLineCount);
            Console.WriteLine("Total Match Count: " + totalMatchCount);
        }

        private async static Task<Tuple<string, string, int, int>> MatchCountFileAsync(string pattern, string rootPath, string fileName, int delay)
        {
            Console.WriteLine(
                "\nFrom MatchCountFileAsync: Enter "+
                "--- File Name: {0} --- Delay: {1}", fileName, delay);
            Thread.Sleep(delay);
            Console.WriteLine(
                "\nFrom MatchCountFileAsync: Sleep Completed "+
                "--- Start MatchCountFileAsync: File Name: {0} --- Delay: {1}", fileName, delay);

            int totalMatchLineCount = 0;
            int totalMatchCount = 0;
            string line;
            var sb = new StringBuilder();
            var sr = new StreamReader(rootPath + fileName);

            sb.Append(string.Format("Input File: {0}\nPattern: {1}\n", fileName, pattern));

            // read one line at a time from the source file
            while ((line = sr.ReadLine()) != null)
            {
                // Perform matches on lowercase values
                string lineLower = line.ToLower();
                string patternLower = pattern.ToLower();
                //
                MatchCollection rxMatches = Regex.Matches(lineLower, patternLower);

                int matchCount = rxMatches.Count;

                if (matchCount > 0)
                {
                    totalMatchLineCount++;
                    totalMatchCount += matchCount;

                    string output = string.Format("\nMatch Count: {0} --- Match Line: {1}\n", matchCount, line);
                    sb.Append(output);
                }

            } // END_WHILE

            sr.Close();

            Console.WriteLine("From MatchCountFileAsync: Done --- File Name: {0} --- Delay: {1}", fileName, delay);
            return new Tuple<string, string, int, int>(sb.ToString(), pattern, totalMatchLineCount, totalMatchCount);
        } // END_Simple()

        private static void FsWriteMatches(string rootPath, string outputFileName, Tuple<string, string, int, int>[] resultsArray)
        {
            var fs = new FileStream(rootPath + outputFileName, FileMode.Create);
            var sw = new StreamWriter(fs, Encoding.Default);

            foreach (Tuple<string, string, int, int> result in resultsArray)
                sw.WriteLine(result.Item1);

            sw.Close();
            fs.Close();
        }


        public static List<string> FetchTestFiles(string rootPath)
        {
            List<string> fileList = new List<string>();
            string[] fileArray = Directory.GetFiles(rootPath);
            foreach (string file in fileArray)
            {
                fileList.Add(Path.GetFileName(file));
            }
            return fileList;
        } // END_FetchTestFiles()

        public static void CleanUp(string rootPath, string filename)
        {
            if (File.Exists(rootPath + filename))
            {
                File.Delete(rootPath + filename);
            }

        } // END_CleanUp()
        
        public static List<InputArg> BuildArgs(List<string> inputFileList, List<string> patternList, List<int> delayList)
        {
            Random random = new Random();
            //
            List<InputArg> inputArgList = new List<InputArg>();

            foreach (string inputFile in inputFileList)
            {
                InputArg inputArg;
                inputArg.Delay = delayList[random.Next(0, delayList.Count)];
                inputArg.Pattern = patternList[random.Next(0, patternList.Count)];
                inputArg.FileName = inputFile;
                inputArgList.Add(inputArg);
            }
            return inputArgList;
        } // END_BuildArgs()
    }
}
