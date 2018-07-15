using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VenturaFTP
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

        private void OnButtonClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            if (button.Name == "SendButton")
            {
                
                ClientBoy();//IPTextBox.Text);

            }
            else if (button.Name == "ListenButton")
            {
                ServerBoy();
            }
            else if (button.Name == "exitbutton")
            {
                this.Close();
                Environment.Exit(0);
            }
        }
        private void ClientBoy()
        {
            TextBoxStreamWriter t = new TextBoxStreamWriter(this.Dispatcher, ClientOutputField);
            Console.WriteLine("sendin");
            //set stdout to go to textbox
            VenturaFTPClassName v = new VenturaFTPClassName();
            v.SendStringToGuy();
            t.Close();

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }
        private void ServerBoy()
        {
            new Thread(new ThreadStart(() =>
            {
                TextBoxStreamWriter t = new TextBoxStreamWriter(this.Dispatcher, ServerOutputField);
                VenturaFTPClassName v = new VenturaFTPClassName();
                v.GetStringFromGuy();
                t.Close();
            })).Start();
            
        }
    }
}
