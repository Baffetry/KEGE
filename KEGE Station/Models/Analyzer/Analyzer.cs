using System.IO;
using Exceptions;
using System.Text;
using System.Data;
using KEGE_Station;
using System.Text.Json;
using Participant_Result;
using KEGE_Station.Models.Option;
using KEGE_Station.Models.Exceptions;

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


        public (int, List<AnswerStatistic>, string) GetScore(Result result)
        {
            try
            {
                StringBuilder sb = new StringBuilder($"{result.SecondName} {result.Name} {result.MiddleName}: ");
                List<AnswerStatistic> statistic = new List<AnswerStatistic>();
                List<string> perTaskScores = new List<string>();

                int primaryScore = 0;

                var response = GetResponses(result.OptionID);
                var responseList = response.ResponsesList;
                var resultList = result.Answers;

                for (int i = 1; i <= responseList.Count; i++)
                {
                    string taskNum = i.ToString();
                    var currentAnswer = resultList.FirstOrDefault(r => r.TaskNumber == taskNum);
                    var correctAnswer = responseList.FirstOrDefault(x => x.TaskNumber == taskNum);

                    int taskScore = 0;
                    string participantResponse = "%noAnswer%";

                    if (currentAnswer is not null)
                    {
                        if (correctAnswer is not null)
                        {
                            participantResponse = currentAnswer.Response;
                            taskScore = currentAnswer.Equals(correctAnswer);
                        }

                        primaryScore += taskScore;
                        perTaskScores.Add(taskScore.ToString());

                        var color = SetColor(taskScore, currentAnswer.TaskNumber);

                        statistic.Add(new AnswerStatistic(
                        currentAnswer.TaskNumber,
                        currentAnswer.Response,
                        correctAnswer.Response,
                        color));
                    }
                    else
                        perTaskScores.Add("0");
                }
                    int finalScore = _scoreDict.ContainsKey(primaryScore) ? _scoreDict[primaryScore] : primaryScore;

                    string tasksPart = string.Join("\t", perTaskScores);
                    string extractionString = $"{result.SecondName} {result.Name} {result.MiddleName}\t{tasksPart}";

                    return (finalScore, statistic, extractionString);
            }
            catch (Exception ex)
            {
                throw new OptionNotFoundException(
                    $"Не удалось найти вариант участника: " +
                    $"{result.SecondName} {result.Name} {result.MiddleName}\n\n" +
                    $"ID варианта: {result.OptionID}"
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

                var file = Directory.GetFiles(answersPath, $"{id}*.json"); // Получаем файл с соответствующим id
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
                throw new ConfigurationException("Проверьте путь к файлу с баллами.");
            }
        }
    }
}
