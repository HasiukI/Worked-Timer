using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Worked_Timer.Model;
using Worked_Timer.ViewModel;

namespace Worked_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"height: {this.Height}, width: {this.Width}");
        }
    }
}