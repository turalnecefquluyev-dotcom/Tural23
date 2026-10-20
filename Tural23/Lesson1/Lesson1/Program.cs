using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkProgramming1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("FILE SERVER");

            using (var server = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp))
            {
                server.Bind(new IPEndPoint(IPAddress.Any, 27001));
                server.Listen(10);

                Console.WriteLine("Server ise salindi...");

                while (true)
                {
                    var client = await server.AcceptAsync();

                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            byte[] buffer = new byte[8192];

                            string folder = "ReceivedFiles";
                            Directory.CreateDirectory(folder);

                            using (var file = File.Create(
                                Path.Combine(folder, "file.dat")))
                            {
                                int length;

                                while ((length = await client.ReceiveAsync(
                                    buffer, SocketFlags.None)) > 0)
                                {
                                    await file.WriteAsync(buffer, 0, length);
                                }
                            }

                            Console.WriteLine("Fayl qebul edildi!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally
                        {
                            client.Dispose();
                        }
                    });
                }
            }
        }
    }
}