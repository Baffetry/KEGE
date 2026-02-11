using System.Text.RegularExpressions;

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

            //if (this.Response.Contains("%noanswer", StringComparison.OrdinalIgnoreCase))
            //    return 0;

            switch (this.TaskNumber)
            {
                case "27":
                case "26":

                    int countElements = answer.Response.Split(" ").Count();

                    if (countElements > 2)
                    {
                        if (Response.Equals(@"%noAnswer"))
                            return 0;

                        string[] correctRows = answer.Response.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        string[] inputRows = Response.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        var inputRow1 = inputRows[0] + " " + inputRows[1];
                        var inputRow2 = inputRows[2] + " " + inputRows[3];

                        var correctRow1 = correctRows[0] + " " + correctRows[1];
                        var correctRow2 = correctRows[2] + " " + correctRows[3];

                        int score = 0;

                        if (inputRow1.Equals(correctRow2) && inputRow2.Equals(correctRow1)) return 1;

                        if (inputRow1.Equals(correctRow1)) score++;
                        if (inputRow2.Equals(correctRow2)) score++;

                        return score;
                    }
                    else
                    {
                        if (Response.Equals(@"%noanswer%", StringComparison.InvariantCultureIgnoreCase))
                            return 0;

                        string[] inputRow = Response.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        string[] correctRow = answer.Response.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        int score = 0;

                        int maxIdx = Math.Max(inputRow.Length, correctRow.Length);

                        for (int i = 0; i < maxIdx; i++)
                        {
                            if (inputRow[i].Equals(correctRow[i])) score++;
                            Response = $"{inputRow[0]} {inputRow[1]}";
                            answer.Response = $"{correctRow[0]} {correctRow[1]}";
                        }

                        return score;
                    }

                default:
                    inputAnswer = Response.ToLower();
                    correctAnswer = answer.Response.ToLower();

                    inputAnswer = Regex.Replace(inputAnswer, @"%no[aA]nswer%", " ").Trim();

                    return inputAnswer.Equals(correctAnswer)
                        ? 1
                        : 0;
            }
        }
    }
}
