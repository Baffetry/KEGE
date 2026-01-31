using Participant_Result;
using Result_Analyzer;
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

            _analyzer = new Analyzer();
            _result = result;
            SetProps();
        }

        private void SetProps()
        {
            _Name.Text = ParticipantName;
            _SecondName.Text = ParticipantSecondName;
            _MiddleName.Text = ParticipantMiddleName;

            int score = _analyzer.GetScore(_result);

            _Score.Value = score;
            _Score.ScoreText = score.ToString();
        }
    }
}
