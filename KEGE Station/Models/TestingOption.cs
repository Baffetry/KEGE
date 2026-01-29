using System.Text.Json.Serialization;
using Task_Data;

namespace Testing_Option
{
    [Serializable]
    public class TestingOption
    {
        [JsonInclude]
        public string OptionID { get; private set; }
        public List<TaskData> TaskList { get; set; } = new();

        [JsonIgnore]
        public List<string> ResponsesList { get; private set; } = new();

        public TestingOption() { }
 
        public TestingOption(string id)
        {
            OptionID = id;
        }

        public void AddTask(TaskData data)
        {
            TaskList.Add(data);
        }
    }
}
