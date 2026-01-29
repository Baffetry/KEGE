using System.Text.Json.Serialization;

namespace Task_Data
{
    [Serializable]
    public class TaskData
    {
        public string TaskNumber { get; set; }
        public byte[] Image { get; set; }
        public int TaskWeight { get; set; } = 1;

        public List<FileData> Files { get; set; }

        [JsonIgnore]
        public List<byte[]> Answers { get; set; }

        public TaskData() { }

    }
}
