using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotSkypeTCPServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) { Console.WriteLine("No arguments passed!"); Console.ReadLine(); Environment.Exit(1); }
            Console.WriteLine("NotSkype TCP Server");
            Console.WriteLine("Loggin");
        }
    }
}
