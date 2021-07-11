using MassTransit;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Messaging.Infrastructure.Consumers
{
    public class FileCollectorConsumer : IConsumer<IMessageInfo>
    {
        private readonly ILogger<FileCollectorConsumer> _logger;
        private readonly string _fileStore = Path.Combine(Directory.GetCurrentDirectory(), "FileStore");

        public FileCollectorConsumer(ILogger<FileCollectorConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<IMessageInfo> context)
        {
            try
            {
                var message = context.Message;

                File.WriteAllBytes($"{_fileStore}\\{message.FileName}", Convert.FromBase64String(message.Content));

                _logger.LogInformation($"Consumer, Received file: {message.FileName}");

            }
            catch(Exception ex)
            {
                _logger.LogError($"Consumer, Error= {ex.Message}");
            }
        }

        
    }
}
