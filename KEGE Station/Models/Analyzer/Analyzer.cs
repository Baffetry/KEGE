using Participant_Result;
using System.IO;
using System.Text.Json;
using KEGE_Station.Models.Option;

namespace Result_Analyzer
{
    public class Analyzer
    {
        private string answersPath = @"D:\Temp\Answers";

        public int GetScore(Result result)
        {
            try
            {
                int score = 0;

                var response = GetResponses(result.OptionID);

                var responseList = response.ResponsesList;
                var resultList = result.Answers;

                for (int i = 0; i < resultList.Count; i++)
                {
                    var currentAnswer = resultList[i];
                    var responseAnswer = responseList.First(x => x.TaskNumber.Equals(currentAnswer.TaskNumber));

                    if (currentAnswer.Equals(responseAnswer))
                        score++;

                }

                return score;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Не получилось найти вариант.\n\n" +
                    $"ID: {result.OptionID}\n" +
                    $"Фамилия: {result.SecondName}\n" +
                    $"Имя: {result.Name}\n" +
                    $"Отчество: {result.MiddleName}");
            }
        }

        public ResponseOption GetResponses(string id)
        {
            var files = Directory.GetFiles(answersPath);

            foreach (var file in files)
            {
                string json = File.ReadAllText(file);
                
                var responses = JsonSerializer.Deserialize<ResponseOption>(json);

                if (responses.OptionID.Equals(id))
                    return responses;
            }

            throw new ArgumentException();
        }
    }
}
