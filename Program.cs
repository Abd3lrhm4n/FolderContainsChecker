using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PngChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            List<FileInfo> FinishedImages = new List<FileInfo>();
            List<FileInfo> PureImages = new List<FileInfo>();

            List<FileInfo> NotFinishedImages = new List<FileInfo>();

            Regex PathValidate = new Regex("^([a-zA-Z]:)?(\\\\[^<>:\"/\\\\|?*]+)+\\\\?$");
            if (args.Count() == 0)
            {
                System.Console.WriteLine("PngChecker [PNG Directory] [RAW Directory]");
                return;
            }
            else if(args.Count() > 2)
            {
                System.Console.WriteLine("There're much arguments");
                return;
            }

            //check if the path wrote in right syntax
            if (!PathValidate.IsMatch(args[0]) && !PathValidate.IsMatch(args[1]))
            {
                System.Console.WriteLine("Please Check the Directory Path Syntax");
                return;
            }

            var PureDirectory = new DirectoryInfo(args[1]);

            var FinishedDicretory = new DirectoryInfo(args[0]);

            //check if the directories are exsit
            if (!PureDirectory.Exists || !FinishedDicretory.Exists)
            {
                System.Console.WriteLine("There's one or both of Directory not Exist");
                return;
            }
            var FinishedFiles = FinishedDicretory.GetFiles();
            var Purefiles = PureDirectory.GetFiles();

            //add pure images into list
            foreach (var file in Purefiles)
            {
                PureImages.Add(file);
            }

            //add finished images into list
            foreach (var file in FinishedFiles)
            {                
                FinishedImages.Add(file);   
            }

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Checking...");

            for (int i = 0; i < PureImages.Count(); i++)
            {
                for (int j = 0; j < FinishedImages.Count(); j++)
                {
                    string PureName = Path.GetFileNameWithoutExtension(PureImages[i].ToString());

                    string FinishedName = Path.GetFileNameWithoutExtension(FinishedImages[j].ToString());
                    
                    //get the image name without the Extension
                    // string FinishedName = FinishedImages[j].Name
                    // .Replace(FinishedImages[j].Name
                    // .Substring(FinishedImages[j].Name.IndexOf(FinishedImages[j].Extension, FinishedImages[j].Extension.Length)), "");

                    //get the image name without the Extension
                    // string PureName = PureImages[i].Name
                    // .Replace(PureImages[i].Name
                    // .Substring(PureImages[i].Name.IndexOf(PureImages[i].Extension, PureImages[i].Extension.Length)), "");

                    if(FinishedName == PureName)
                    {
                        break;
                    }
                    else if(FinishedName != PureName && j == FinishedImages.Count() - 1)
                    {
                        NotFinishedImages.Add(PureImages[i]);

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        System.Console.WriteLine(PureImages[i]);
                    }
                }
                                    
            
            }
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"{NotFinishedImages.Count()} Images are Missing");

            if (NotFinishedImages.Count() > 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                System.Console.WriteLine("******************************************");
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Missing Files");

                foreach (var item in NotFinishedImages)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    System.Console.WriteLine(item);
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                System.Console.WriteLine("******************************************");

            }
            
            
            string targetPath = FinishedDicretory + "\\Missing\\";

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            foreach (var item in NotFinishedImages)
            {
                File.Copy(item.ToString(), targetPath + item.Name, true);
            }

            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}
