using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace rpcServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPAddress ipaddress = IPAddress.Parse("192.168.3.32");
                TcpListener mylist = new TcpListener(ipaddress, 8000);
                mylist.Start();
                Console.WriteLine("Server is Runing on Port:8000");

            }
            catch(Exception ex)
            {
                while (ex.InnerException != null)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine(ex.Message);
            }
        }
    }
}
