using Participant_Result;
using System.IO;
using System.Text.Json;
using KEGE_Station.Models.Option;
using System.Data;
using KEGE_Station;
using KEGE_Station.Models.Exceptions;
using Exceptions;
using System.ComponentModel;

namespace Result_Analyzer
{
    public class Analyzer
    {
        private static Analyzer _analyzer;
        private Dictionary<int, int> _scoreDict;

        private Analyzer()
        {
            SetScoreDictionary();
        }

        public static Analyzer Instance()
        {
            if (_analyzer is null)
                _analyzer = new Analyzer();
            return _analyzer;
        }


        public (int, List<AnswerStatistic>) GetScore(Result result)
        {
            try
            {
                List<AnswerStatistic> statistic = new List<AnswerStatistic>();
                int score = 0;

                var response = GetResponses(result.OptionID);

                var responseList = response.ResponsesList;
                var resultList = result.Answers;

                for (int i = 0; i < resultList.Count; i++)
                {
                    if (resultList[i].Response.Equals("%noanswer%") || resultList[i].Response.Equals("%noAnswer%"))
                        continue;

                    var currentAnswer = resultList[i];
                    var correctAnswer = responseList.First(x => x.TaskNumber.Equals(currentAnswer.TaskNumber));

                    var currentTaskScore = currentAnswer.Equals(correctAnswer);
                    var color = SetColor(currentTaskScore, currentAnswer.TaskNumber);

                    score += currentTaskScore;

                    //var temp = correctAnswer.Response.Split(new[] { ' ' },
                    //        StringSplitOptions.RemoveEmptyEntries);

                    //correctAnswer.Response = $"{temp[0]} {temp[1]}";

                    statistic.Add(new AnswerStatistic(
                        currentAnswer.TaskNumber,
                        currentAnswer.Response, 
                        correctAnswer.Response, 
                        color)
                    );
                }

                return (_scoreDict[score], statistic);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    $"{result.SecondName} " +
                    $"{result.Name} " +
                    $"{result.MiddleName}\n"
                );
            }
        }

        private StatisticColor SetColor(int score, string taskNumber)
        {
            switch (taskNumber)
            {
                case "26":
                case "27":

                    return score == 0
                        ? StatisticColor.Red
                        : score == 1
                            ? StatisticColor.Yellow
                            : StatisticColor.Green;
                default:

                    return score == 0
                        ? StatisticColor.Red
                        : StatisticColor.Green;
            }
        }

        private ResponseOption GetResponses(string id)
        {
            try
            {
                string answersPath = App.GetResourceString("SaveAnswersPath");

                var file = Directory.GetFiles(answersPath, $"{id}*.json");
                var json = File.ReadAllText(file[0]);
                return JsonSerializer.Deserialize<ResponseOption>(json);
            }
            catch (Exception ex)
            {
                throw new OptionNotFoundException($"Не удалось найти вариант. ID варианта: {id}");
            }
        }
        private void SetScoreDictionary()
        {
            try
            {
                _scoreDict = new Dictionary<int, int>();
                string line;

                string scorePath = App.GetResourceString("ScoreTable");

                using (var sr = new StreamReader(scorePath))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        var row = line.Trim().Split().Select(x => int.Parse(x)).ToList();
                        var (key, value) = (row[0], row[1]);

                        _scoreDict[key] = value;
                    }
                }
            }
            catch (Exception cfgEx)
            {
                throw new ConfigurationException("Проверьте, указан ли путь к файлу с баллами");
            }
        }
    }
}
