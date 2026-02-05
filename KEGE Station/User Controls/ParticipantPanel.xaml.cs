using KEGE_Station.Windows;
using Participant_Result;
using Result_Analyzer;
using System.Windows;
using System.Windows.Controls;

namespace KEGE_Station.User_Controls
{
    /// <summary>
    /// Interaction logic for ParticipantPanel.xaml
    /// </summary>
    public partial class ParticipantPanel : UserControl
    {
        private Result _result;
        private Analyzer _analyzer;
        private List<AnswerStatistic> _statistics;

        public string ParticipantName
            => _result.Name;
        public string ParticipantSecondName
            => _result.SecondName;

        public string ParticipantMiddleName
            => _result.MiddleName;

        public List<Answer> ParticipantAnswers
            => _result.Answers;

        public ParticipantPanel(Result result)
        {
            InitializeComponent();

            _analyzer = Analyzer.Instance();
            _result = result;
            SetProps();
        }

        private void SetProps()
        {
            _Name.Text = ParticipantName;
            _SecondName.Text = ParticipantSecondName;
            _MiddleName.Text = ParticipantMiddleName;

            var (score, statistic) = _analyzer.GetScore(_result);

            _statistics = statistic;
            _Score.Value = score;
            _Score.ScoreText = score.ToString();
        }

        private void _Panel_btn_Click(object sender, RoutedEventArgs e)
        {
            CloseAllStatisticWindows();

            var sw = new StatisticWindow(_statistics);

            sw.Owner = Window.GetWindow(this);

            sw._TitlePerson.Text = $"{ParticipantSecondName} {ParticipantName} {ParticipantMiddleName}";
            sw._TitleID.Text = _result.OptionID;

            sw.Show();
        }

        private void CloseAllStatisticWindows()
        {
            var windowsToClose = Application.Current.Windows.OfType<StatisticWindow>().ToList();

            foreach (var window in windowsToClose)
                window.Close();
        }
    }
}
