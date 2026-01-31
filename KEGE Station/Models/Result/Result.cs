namespace Participant_Result
{
    public class Result
    {
        public string OptionID { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public List<Answer> Answers { get; set; } = new();

        public Result() { }

        public Result(string id) 
        {
            OptionID = id;
        }

        public void AddAnswer(Answer answer)
        {
            Answers.Add(answer);
        }
    }
}
