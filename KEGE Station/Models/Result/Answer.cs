namespace Participant_Result
{
    public class Answer
    {
        public string TaskNumber { get; set; }
        public string Response { get; set; }

        public Answer(string taskNumber, string response)
        {
            TaskNumber = taskNumber;
            Response = response;
        }

        public int Equals(Answer answer)
        {
            string taskNumber = answer.TaskNumber;

            string inputAnswer = string.Empty, correctAnswer = string.Empty;

            switch (taskNumber)
            {
                case "27":
                case "26":

                    int countElements = answer.Response.Split(" ", 
                        StringSplitOptions.RemoveEmptyEntries).Count();

                    if (countElements > 2)
                    {
                        string[] inputRows = answer.Response.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        string[] correctRows = Response.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        var inputRow1 = inputRows[0] + " " + inputRows[1];
                        var inputRow2 = inputRows[2] + " " + inputRows[3];

                        var correctRow1 = correctRows[0] + " " + correctRows[1];
                        var correctRow2 = correctRows[2] + " " + correctRows[3];

                        int score = 0;

                        if (inputRow1.Equals(correctRow1)) score++;
                        if (inputRow2.Equals(correctRow2)) score++;
                        
                        return score;
                    }
                    else
                    {
                        string[] inputRow = Response.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        string[] correctRow = answer.Response.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        int score = 0;

                        if (inputRow[0].Equals(correctRow[0])) score++;
                        if (inputRow[1].Equals(correctRow[1])) score++;

                        return score;
                    }

                default:

                    inputAnswer = answer.Response.ToLower();
                    correctAnswer = Response.ToLower();

                    return inputAnswer.Equals(correctAnswer) 
                        ? 1 
                        : 0;
            }
        }
    }
}
