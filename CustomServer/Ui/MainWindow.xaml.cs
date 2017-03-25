using System.Windows;

namespace CustomServer.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_OnExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
                
        }
    }
}
