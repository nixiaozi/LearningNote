using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace RunCmd
{
    public class RunCMDWithOutput
    {
        public static void ToDo()
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.Arguments = "echo ToDo The CMD Output";
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true; // 输入也要监视
            info.UseShellExecute = false;

            p.StartInfo = info;
            p.Start();

            // 命令 echo
            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    /*
                    sw.WriteLine(string.Format("git config --global user.name {0}", args[0]));
                    sw.WriteLine(string.Format("git config --global user.email {0}", args[1]));
                    sw.WriteLine("call start-ssh-agent");
                    sw.WriteLine(string.Format("ssh-add {0}", args[2]));
                    */
                    sw.WriteLine("echo ToDo The CMD Output1"); // 输入命令
                    System.Threading.Thread.Sleep(1000);
                    sw.WriteLine(Environment.NewLine); // 提交命令
                    System.Threading.Thread.Sleep(1000);

                    // 查看命令输出
                    //Console.WriteLine(p.StandardOutput.ReadLine());
                    //Console.WriteLine(p.StandardOutput.ReadLine());
                    var outputStr = p.StandardOutput.ReadLine();
                    var falge = true;
                    while (outputStr!=null&&falge)
                    {
                        Console.WriteLine(outputStr);
                        if (outputStr== "ToDo The CMD Output1")
                        {
                            // 表示已经执行完毕
                            falge = false;
                            Console.WriteLine("命令 echo ToDo The CMD Output1  已执行完毕");
                        }


                        outputStr = p.StandardOutput.ReadLine();
                    }


                    // 不过可以在一个inputstream 中输入不同的命令
                    sw.WriteLine("echo ToDo The CMD Output2"); // 输入命令
                    System.Threading.Thread.Sleep(1000);
                    sw.WriteLine(Environment.NewLine); // 提交命令

                    falge = true;
                    while (outputStr != null && falge)
                    {
                        Console.WriteLine(outputStr);
                        if (outputStr == "ToDo The CMD Output2")
                        {
                            // 表示已经执行完毕
                            falge = false;
                            Console.WriteLine("命令 echo ToDo The CMD Output2  已执行完毕");
                        }


                        outputStr = p.StandardOutput.ReadLine();
                    }

                }
            }

            // 测试发现只能输入一次内容 现在会发现 
            //using (StreamWriter sw = p.StandardInput)
            //{
            //    if (sw.BaseStream.CanWrite)
            //    {
            //       
            //    }
            //}


            Console.WriteLine("Done");

        }

    }
}
