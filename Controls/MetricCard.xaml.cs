using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TypingTutor.Controls
{
    public partial class MetricCard : UserControl
    {
        public MetricCard()
        {
            InitializeComponent();
        }

        public string CardTitle
        {
            get => TxtTitle.Text;
            set => TxtTitle.Text = value;
        }

        public string Value
        {
            get => TxtValue.Text;
            set => TxtValue.Text = value;
        }

        public string Unit
        {
            get => TxtUnit.Text;
            set => TxtUnit.Text = value;
        }

        public string Subtitle
        {
            get => TxtSubtitle.Text;
            set => TxtSubtitle.Text = value;
        }

        public Brush ValueColor
        {
            get => TxtValue.Foreground;
            set => TxtValue.Foreground = value;
        }

        public Brush UnitColor
        {
            get => TxtUnit.Foreground;
            set => TxtUnit.Foreground = value;
        }
    }
}
