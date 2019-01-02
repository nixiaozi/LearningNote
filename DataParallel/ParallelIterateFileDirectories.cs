using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataParallel
{
    public class ParallelIterateFileDirectories
    {
        public static void MainAction()
        {
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            Console.WriteLine("请输入要搜索的文件夹路径：");
            var SearchDirectory = Console.ReadLine().ToString();
            try
            {
                ostrm = new FileStream("./Redirect.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
                Console.SetOut(writer);
          
                TraverseTreeParallelForEach(SearchDirectory, (f) =>
                {
                    // Exceptions are no-ops.
                    try
                    {
                        // Do nothing with the data except read it.
                        byte[] data = File.ReadAllBytes(f);
                    }
                    catch (FileNotFoundException) { }
                    catch (IOException) { }
                    catch (UnauthorizedAccessException) { }
                    catch (SecurityException) { }
                        // Display the filename.
                        Console.WriteLine(f);
                });
                Console.SetOut (oldOut);
                writer.Close();
                ostrm.Close();
                Console.WriteLine ("文件夹搜索已完成！");
            }
            catch(ArgumentException)
            {
                Console.WriteLine(@"The directory '"+SearchDirectory+"' does not exist.");
            } 
            // Keep the console window open.
            Console.ReadKey();


        }

        public static void TraverseTreeParallelForEach(string root,Action<string> action)
        {
            //Count of files traversed and timer for diagnostic output
            int fileCount = 0;
            var sw = Stopwatch.StartNew();

            //Determine whether to parallelize file processing on each folder based on processor count.
            int procCount = System.Environment.ProcessorCount;

            //Data structure to hold names of subflolders to be examined for files.
            Stack<string> dirs = new Stack<string>();

            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
            }

            dirs.Push(root);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs = { };
                string[] files = { };

                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                //Thrown if we bo not have discovery permission on the directory.
                catch(UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                //Thrown if another process has deleted the directory after we retrieved ites anme.
                catch(DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                try
                {
                    files = Directory.GetFiles(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                //Execute in parallel if there are enough files in the directory.
                //Otherwise, execute sequentially.Files are opened and processed
                //synchronously but this could be modified to perform async I/O.
                try
                {
                    if (files.Length < procCount)
                    {
                        foreach(var file in files)
                        {
                            action(file);
                            fileCount++;
                        }
                    }
                    else
                    {
                        Parallel.ForEach(files, () => 0, (file, loopState, localCount) =>
                                                    {
                                                        action(file);
                                                        return (int)++localCount;
                                                    },
                                            (c) =>
                                            {
                                                Interlocked.Add(ref fileCount, c);
                                            });
                    }
                }
                catch(AggregateException ae)
                {
                    ae.Handle((ex) =>
                    {
                        if (ex is UnauthorizedAccessException)
                        {
                            //Here we just output a message and go on.
                            Console.WriteLine(ex.Message);
                            return true;
                        }
                        //Hand other exceptions here if necessxary...
                        return false;
                    });
                }

                //Push the subdirectories onto the stack for traversal.
                //This could also be done before handing the files.
                foreach(string str in subDirs)
                {
                    dirs.Push(str);
                }
            }

            //For diagnositic purposes.
            Console.WriteLine("Processed {0} files in {1} milliseconds", fileCount, sw.ElapsedMilliseconds);
        }

    }
}
