using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RunCmd
{
    public class RunBATWithOutput
    {
        public static void ToDo()
        {
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "YOURBATCHFILE.bat";
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            Console.WriteLine("The OutPut is:");
            Console.WriteLine(output);

        }

    }
}
