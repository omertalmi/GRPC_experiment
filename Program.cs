// See https://aka.ms/new-console-template for more information

/*
using System;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace BumpTests
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            Stopwatch sw_all = new Stopwatch();
            var numberOfDies = 10;
            var bumpsInDie = 694444;
            var totalBumps = numberOfDies * bumpsInDie;
            Console.WriteLine(numberOfDies.GetType());
            Console.WriteLine(bumpsInDie.GetType());
            Console.WriteLine(totalBumps.GetType());
            Console.WriteLine($"***** Bumps Experiment - Total bumps to write: {totalBumps} ******");
            
            //Clean the existing files
            System.IO.DirectoryInfo di = new DirectoryInfo(@"D:\bumpsData\");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete(); 
            }
            
            for (int i = 0; i < numberOfDies; i++)
            {
                var csv = new StringBuilder();
                var firstLine = "bumpID,dieX,dieY,waferX,waferY,Type,Height,Cop,Die_ID,WaferID";
                csv.AppendLine(firstLine);
                for (int j = 0; j < bumpsInDie; j++)
                {
                    Guid dieId = Guid.NewGuid();
                    Guid waferId = Guid.NewGuid();
                    var newLine = string.Format("{0},12.1,12.2,123.1,124.25,1,8.422,9.3223,{1},{2}", j, dieId, waferId);
                    csv.AppendLine(newLine);
                }
                sw.Start();
                string filePath = @"D:\bumpsData\Die" + i + ".csv";
                File.WriteAllText(filePath, csv.ToString());
                sw.Stop();
                Console.WriteLine($"***** Done writing die [{i}] ******");
            }
            Console.WriteLine("Elapsed={0}",sw.Elapsed.TotalSeconds);
            var speed = 81.272 * numberOfDies / sw.Elapsed.TotalSeconds;
            Console.WriteLine("Total Size={0} GB",81.272 * numberOfDies / 1000);
            Console.WriteLine("Speed={0} MB/s",speed);
        }
    }
} */

using System;
using Grpc.Core;
namespace BumpsData
{
    class Program
    {
        const int Port = 50051;
        public static void Main(string[] args)
        {
            try
            {    
                Server server = new Server
                {
                    Services = { AccountService.BindService(new AccountsImpl()) },
                    Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
                };
                server.Start();
                Console.WriteLine("Accounts server listening on port " + Port);
                Console.WriteLine("Press any key to stop the server...");
                Console.ReadKey();
                server.ShutdownAsync().Wait();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception encountered: {ex}");
            }
        }
    }    
}