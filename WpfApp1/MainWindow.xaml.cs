using System.Diagnostics;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tb_Input.Text = "";
            rtb_Output.Document.Blocks.Clear();
            tb_Input.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string input = tb_Input.Text;
            RunCommand(input);
        }

        private void RunCommand(string command)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",          
                    Arguments = "/C " + command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process();
                process.StartInfo = psi;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                int exitCode = process.ExitCode;

                rtb_Output.Document.Blocks.Clear();

                AppendColoredText(output, Colors.WhiteSmoke); // stdout
                AppendColoredText(error, Colors.Red);         // stderr
                AppendColoredText($"\nExit Code: {exitCode}", Colors.SkyBlue); // status

            }
            catch (Exception ex)
            {
                AppendColoredText("Exception: " + ex.Message, Colors.OrangeRed);
            }
        }

        private void AppendColoredText(string text, Color color)
        {
            TextRange tr = new TextRange(rtb_Output.Document.ContentEnd, rtb_Output.Document.ContentEnd);
            tr.Text = text + Environment.NewLine;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
        }

        private void tb_Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RunCommand(tb_Input.Text); 
                e.Handled = true; 
            }
        }
    }

}
