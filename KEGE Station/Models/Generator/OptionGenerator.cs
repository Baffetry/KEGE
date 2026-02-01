using KEGE_Station.Models.Option;
using Participant_Result;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Task_Data;
using Testing_Option;

namespace Option_Generator
{
    public class OptionGenerator
    {
        private readonly Dictionary<string, List<TaskData>> _taskByNumber;
        private readonly Random _rnd;
        private readonly int[] LINKEDTASKS = { 19, 20, 21 };


        public OptionGenerator(string rootPath)
        {
            _taskByNumber = new Dictionary<string, List<TaskData>>();
            _rnd = new Random();

            LoadTasks(rootPath);
        }

        public (TestingOption, ResponseOption) GetOption()
        {
            string optionId = Guid.NewGuid().ToString();
            var option = new TestingOption(optionId);
            var response = new ResponseOption(optionId);

            var (optionTasks, optionAnswers) = GenerateTaskList();

            foreach (var task in optionTasks)
                option.AddTask(task);

            foreach (var answer in optionAnswers)
                response.AddAnswer(answer);

            return (option, response);
        }

        private (List<TaskData>, List<Answer>) GenerateTaskList()
        {
            var tasks = new List<TaskData>();
            var answers = new List<Answer>();


            for (int i = 1; i <= _taskByNumber.Count; i++)
            {
                string taskNumber = i.ToString();

                List<TaskData> availableTasks = _taskByNumber[taskNumber];

                int index = 0;

                if (availableTasks.Count > 0)
                {
                    if (!LINKEDTASKS.Contains(i)) 
                        index = _rnd.Next(availableTasks.Count);

                        var selectedTask = availableTasks[index];
                    selectedTask.TaskNumber = taskNumber;

                    var selectedAnswers = Encoding.UTF8.GetString(selectedTask.Answer[0]);

                    var answer = new Answer(taskNumber, selectedAnswers);

                    tasks.Add(selectedTask);
                    answers.Add(answer);
                }
            }

            return (tasks, answers);
        }

        public void LoadTasks(string rootPath)
        {
            var numberDirs = Directory.GetDirectories(rootPath);

            foreach (var dir in numberDirs)
            {
                string dirName = Path.GetFileName(dir);

                var taskList = new List<TaskData>();
                string taskNumber = "";

                if (!int.TryParse(dirName, out int num))
                    continue;

                taskNumber = num.ToString();
                var subDirs = Directory.GetDirectories(dir);

                foreach (var taskDir in subDirs)
                {
                    TaskData taskData = CreateTaskData(taskDir);
                    taskList.Add(taskData);
                }

                _taskByNumber[taskNumber] = taskList;
            }
        }

        private TaskData CreateTaskData(string taskDir)
        {
            var taskData = new TaskData();

            var pngFile = Directory.GetFiles(taskDir, "*.png").FirstOrDefault();

            if (pngFile != null)
                taskData.Image = File.ReadAllBytes(pngFile);

            taskData.Files = new List<FileData>();
            taskData.Answer = new List<byte[]>();
            taskData.TaskWeight = 1;

            var allFiles = Directory.GetFiles(taskDir);

            foreach (var file in allFiles)
            {
                if (file != pngFile && Path.GetExtension(file) != ".png")
                {
                    string fileName = Path.GetFileName(file);

                    if (Regex.Match(fileName, @"[aA]nswer.txt").Success)
                        taskData.Answer.Add(File.ReadAllBytes(file));
                    else
                    {
                        string name = Path.GetFileName(file);
                        byte[] data = File.ReadAllBytes(file);

                        taskData.Files.Add(new FileData()
                        {
                            FileName = name,
                            Data = data
                        });
                    }
                }
            }

            return taskData;
        }
    }
}