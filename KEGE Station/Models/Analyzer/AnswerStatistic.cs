namespace Result_Analyzer
{
    public enum StatisticColor
    { 
        Green, Yellow, Red
    }

    public class AnswerStatistic
    {
        public string TaskNumber { get; set; }
        public string ParticipantAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public StatisticColor Color { get; set; }

        public AnswerStatistic(string taskNumber, string participantAnswer, 
            string correctAnswer, StatisticColor color)
        {
            TaskNumber = taskNumber;
            ParticipantAnswer = participantAnswer;
            CorrectAnswer = correctAnswer;
            Color = color;
        }
    }
}
