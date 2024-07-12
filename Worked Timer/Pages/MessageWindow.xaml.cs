using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Worked_Timer.Pages
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    /// [STAThread]
    public partial class MessageWindow : Window
    {
        
        public MessageWindow()
        {
            InitializeComponent();
        }

        private void Border_Close(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
