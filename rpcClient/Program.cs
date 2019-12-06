using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace rpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                Console.WriteLine("Connectiong...");
                tcpClient.Connect("192.168.1.52", 8000);
                Console.WriteLine("Connected");
                Console.WriteLine("Ente the string you want to send");
                string str = Console.ReadLine();
                Stream stm = tcpClient.GetStream();
                ASCIIEncoding ascnd = new ASCIIEncoding();
                byte[] ba = ascnd.GetBytes(str);
                Console.WriteLine("Sending");
                stm.Write(ba, 0, ba.Length);    
                byte[] bb = new byte[100];
                int k = stm.Read(bb,0,100);
                for(int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }

                tcpClient.Close();
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error" + ex.StackTrace);
            }
        }
    }
}
