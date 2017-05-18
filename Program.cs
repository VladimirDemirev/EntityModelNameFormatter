using System;
using System.Collections.Generic;
using System.IO;

namespace EntityModelNameFormatter
{
    class Program
    {
        const string FILE_EXT = ".cs";
        static List<MyFile> files;
        static IFormatter formatter;


        static void processFile(MyFile f, IFormatter formatter)
        {
            string ext = Path.GetExtension(f.OldName);
            string srcDir = Directory.GetParent(f.OldName).FullName;

            string oldStrippedName = Path.GetFileNameWithoutExtension((f.OldName));
            string newStrippedName = formatter.GetFormattedName(oldStrippedName);

            f.NewName = Path.Combine( f.NewName /* it contained new path, until here */, newStrippedName+FILE_EXT );

            // update current class name
            f.Content = RegexFuncs.replaceClassName(f.Content, formatter, oldStrippedName, newStrippedName);

            // update properties, calls, other usages
            f.Content = RegexFuncs.regexPropNames(f.Content, formatter);
            f.Content = RegexFuncs.regexVirtualProps(f.Content, formatter);
            f.Content = RegexFuncs.regexInTriangleBrakets(f.Content, formatter);
            f.Content = RegexFuncs.regexHashSet(f.Content, formatter);
            f.Content = RegexFuncs.regexLambda1(f.Content, formatter);
        }


        static void processFiles()
        {
            // update files contents
            foreach(MyFile f in files)
            {
                processFile(f, formatter);
            }

            // save the new files
            foreach (MyFile f in files)
            {
                System.Console.Write(String.Format("* {0}", f.OldName));
                System.Console.WriteLine(String.Format(" ---> {0}", f.NewName));
                File.WriteAllText(f.NewName, f.Content);
            }
        }


        static List<MyFile> readAllFiles(string srcPath, string trgPath)
        {
            List<MyFile> result = new List<MyFile>();

            Directory.CreateDirectory(trgPath);

            // add all files into collection List<MyFile>
            string[] fileEntries = Directory.GetFiles(srcPath, "*"+ FILE_EXT);
            foreach (string fileFullName in fileEntries)
            {
                MyFile f = new MyFile(fileFullName, trgPath, File.ReadAllText(fileFullName));
                result.Add(f);
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(srcPath);
            foreach (string subdirectory in subdirectoryEntries)
            {
                string trgSubFolder = Path.Combine(trgPath, Path.GetFileName(subdirectory));
                result.AddRange( readAllFiles(subdirectory, trgSubFolder) );
            }

            return result;
        }

        static void Main(string[] args)
        {
            formatter = new Pascal();   // I want Pascal format:  some_database_table -> SomeDatabaseTable

            // delete target folder
            if (Directory.Exists(args[1]))
                Directory.Delete(args[1], true);

            files = readAllFiles(args[0], args[1]);
            processFiles();

            Console.WriteLine("\nALL DONE!");
        }
    }
}
