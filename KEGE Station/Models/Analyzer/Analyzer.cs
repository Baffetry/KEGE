using Participant_Result;
using System.IO;
using System.Text.Json;
using KEGE_Station.Models.Option;
using System.Data;
using KEGE_Station;

namespace Result_Analyzer
{
    public class Analyzer
    {
        private readonly string answersPath;
        private readonly string scorePath;

        private static Analyzer _analyzer;
        private Dictionary<int, int> _scoreDict;

        private Analyzer()
        {
            answersPath = App.GetResourceString("SaveAnswersPath");
            scorePath = App.GetResourceString("ScoreTable");

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
                    var currentAnswer = resultList[i];
                    var correctAnswer = responseList.First(x => x.TaskNumber.Equals(currentAnswer.TaskNumber));

                    var currentTaskScore = currentAnswer.Equals(correctAnswer);
                    var color = SetColor(currentTaskScore, currentAnswer.TaskNumber);

                    score += currentTaskScore;

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
                var file = Directory.GetFiles(answersPath, $"{id}*.json");
                var json = File.ReadAllText(file[0]);
                return JsonSerializer.Deserialize<ResponseOption>(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetScoreDictionary()
        {
            _scoreDict = new Dictionary<int, int>();
            string line;

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
    }
}
