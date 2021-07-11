using System;
using System.IO;
using System.Threading.Tasks;
using MassTransit;
using Models;

namespace RabbitMQClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = "rabbitmq://localhost";
            var filePath = $"{Environment.CurrentDirectory}\\UploadStore";
            
            var directory = new DirectoryInfo(filePath);
            if(!directory.Exists)
            {
                directory.Create();
            }

            var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host(new Uri(url), c =>
                {
                    c.Username("guest");
                    c.Password("guest");
                });
            });

            try
            {
                bus.Start();
                Console.WriteLine("FileLoader Started");

                await Task.Run(async () =>
                {

                    while (true)
                    {
                        var files = directory.GetFiles("*.jpg");
                        if (files.Length > 0)
                        {
                            foreach (var file in files)
                            {
                                try
                                {
                                    var content = File.ReadAllBytes(file.FullName);

                                    var obj = new MessageInfo()
                                    {
                                        FileName = file.Name,
                                        Content = Convert.ToBase64String(content)
                                    };

                                    await bus.Publish(obj);

                                    Console.WriteLine(" [x] Sent {0}", obj.FileName);

                                    File.Delete(file.FullName);
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine($"Loading File, Error= {ex.Message}");
                                }
                            }
                        }
                        await Task.Delay(5000);
                    }
                });

            }
            finally
            {
                bus.Stop();
                Console.WriteLine("FileLoader Stopped");
            }
            Console.ReadLine();

            Console.WriteLine("Hello World!");
        }

    }
}
