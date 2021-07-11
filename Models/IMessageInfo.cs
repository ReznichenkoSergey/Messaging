using System.Text.Json.Serialization;

namespace Models
{
    public interface IMessageInfo
    {
        string FileName { get; set; }

        string Content { get; set; }
    }
}
