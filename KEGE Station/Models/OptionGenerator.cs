using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Task_Data;
using Testing_Option;

namespace Option_Generator
{
    public class OptionGenerator : IGenerator
    {
        private readonly Dictionary<string, List<TaskData>> _taskByNumber;
        private readonly Random _rnd;


        public OptionGenerator(string rootPath) 
        {
            _taskByNumber = new Dictionary<string, List<TaskData>>();
            _rnd = new Random();
            
            LoadTasks(rootPath); // Не забудьте вызвать загрузку!
        }

        public TestingOption GetOption()
        {
            string optionId = Guid.NewGuid().ToString(); // Исправил формат даты
            var option = new TestingOption(optionId);

            List<TaskData> optionTasks = GenerateTaskList();

            foreach (var task in optionTasks)
                option.AddTask(task);

            return option;
        }

        private List<TaskData> GenerateTaskList()
        {
            var tasks = new List<TaskData>();

            for (int i = 1; i <= 27; i++)
            {
                string taskNumber = i.ToString();

                List<TaskData> availableTasks = null;

                switch (taskNumber)
                {
                    case "19": case "20": case "21":
                        availableTasks = _taskByNumber["19-21"];
                        break;
                    default:
                        availableTasks = _taskByNumber[taskNumber];
                        break;
                }

                if (availableTasks.Count > 0)
                {
                    int index = _rnd.Next(availableTasks.Count);
                    TaskData selectedTask = availableTasks[index];

                    selectedTask.TaskNumber = taskNumber;
                    tasks.Add(selectedTask);
                }
            }

            return tasks;
        }

        public void LoadTasks(string rootPath)
        {
            var numberDirs = Directory.GetDirectories(rootPath);
            var ls = Directory.GetDirectories(rootPath);

            foreach (var dir in numberDirs)
            {
                string dirName = Path.GetFileName(dir);

                var taskList = new List<TaskData>();
                string taskNumber = "";

                // Обработка папок c "-" в имени
                if (dirName.Contains('-'))
                {
                    taskNumber = "19-21";
                    taskList = ProcessSharedTasksFolder(dir);
                }
                else
                {

                    // Обычная папка с номером задания
                    if (!int.TryParse(dirName, out int num))
                        continue; // Пропускаем, если не число

                    taskNumber = num.ToString();
                    var subDirs = Directory.GetDirectories(dir);

                    foreach (var taskDir in subDirs)
                    {
                        TaskData taskData = CreateTaskData(taskDir);
                        taskList.Add(taskData);
                    }
                }

                _taskByNumber[taskNumber] = taskList;
            }
        }

        private List<TaskData> ProcessSharedTasksFolder(string sharedFolderPath)
        {
            List<TaskData> tasks = new List<TaskData>();

            string[] rangeParts = Path.GetFileName(sharedFolderPath).Split('-');
            if (rangeParts.Length != 2 || 
                !int.TryParse(rangeParts[0], out int start) || 
                !int.TryParse(rangeParts[1], out int end))
            {
                throw new ArgumentException("Неверный формат");
            }

            var variantFolders = Directory.GetDirectories(sharedFolderPath);
            if (variantFolders.Length == 0)
                throw new ArgumentException("Папка с вариантами не найдена");

            foreach (string variantsRoot in variantFolders)
            {
                var allVariantsDirs = Directory.GetDirectories(variantsRoot);

                TaskData task19 = CreateTaskData(allVariantsDirs[0]);
                TaskData task20 = CreateTaskData(allVariantsDirs[1]);
                TaskData task21 = CreateTaskData(allVariantsDirs[2]);
                
                TaskData task = new TaskData();


                task.Image = task19.Image;

                task.Answers = new List<byte[]>();
                task.Files = new List<FileData>();

                task.Answers.Add(task19.Answers[0]);
                task.Answers.Add(task20.Answers[0]);
                task.Answers.Add(task21.Answers[0]);

                tasks.Add(task);
            }

            return tasks;
        }

        private TaskData CreateTaskData(string taskDir)
        {
            var taskData = new TaskData();

            var pngFile = Directory.GetFiles(taskDir, "*.png").FirstOrDefault();

            if (pngFile != null)
                taskData.Image = File.ReadAllBytes(pngFile);

            taskData.Files = new List<FileData>();
            taskData.Answers = new List<byte[]>();
            taskData.TaskWeight = 1;

            var allFiles = Directory.GetFiles(taskDir);

            foreach (var file in allFiles)
            {
                if (file != pngFile && Path.GetExtension(file) != ".png")
                {
                    string fileName = Path.GetFileName(file);

                    if (Regex.Match(fileName, @"[aA]nswer.txt").Success)
                        taskData.Answers.Add(File.ReadAllBytes(file));
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