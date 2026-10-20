using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("FILE CLIENT");
            Console.Write("Faylin yolunu yaz: ");

            string path = Console.ReadLine();

            if (!File.Exists(path))
            {
                Console.WriteLine("Fayl tapilmadi!");
                return;
            }

            using (var client = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp))
            {
                try
                {
                    await client.ConnectAsync(
                        new IPEndPoint(IPAddress.Loopback, 27001));

                    byte[] data = await File.ReadAllBytesAsync(path);

                    int sent = 0;

                    while (sent < data.Length)
                    {
                        sent += await client.SendAsync(
                            data.AsMemory(sent),
                            SocketFlags.None
                        );
                    }

                    client.Shutdown(SocketShutdown.Send);

                    Console.WriteLine("Fayl gonderildi!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}