using Participant_Result;

namespace KEGE_Station.Models.Option
{
    public class ResponseOption
    {
        public string OptionID { get; set; }
        public List<Answer> ResponsesList { get; set; } = new();

        public ResponseOption() { }
        public ResponseOption(string id)
        {
            OptionID = id;
        }

        public void AddAnswer(Answer answer)
        {
            ResponsesList.Add(answer);
        }
    }
}
