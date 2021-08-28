using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace HomeWorkLesson4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       // string message = String.Empty;
       // string key = String.Empty;
       // List<string> buffer = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
        }
        Thread[] threads = new Thread[2];
        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(pathTxtBox.Text))
            {
                return;
            }
            if (!(bool)decryptRadioButton.IsChecked && !(bool) encryptRadioButton.IsChecked)
            {
                return;
            }
            
            if ((bool)decryptRadioButton.IsChecked)
            {
                ParameterizedThreadStart ps = new ParameterizedThreadStart(DecryptFile);
                threads[0] = new Thread(ps);
                threads[0].Start();
                
                //ThreadPool.QueueUserWorkItem(DecryptFile, null);
                return;
            }
            ParameterizedThreadStart ps2 = new ParameterizedThreadStart(EncryptFile);
            threads[1] = new Thread(ps2);
            threads[1].Start();
            //ThreadPool.QueueUserWorkItem(EncryptFile,null);
        }

        private void DecryptFile(object obj)
        {
            try
            {
                int key = 0;
                string[] text = null;
                string path = null;
                //Dispatcher.Invoke(() => {  });
                Dispatcher.Invoke(
                    () => {
                        key = Int32.Parse(keyPswdBox.Password);
                        path = pathTxtBox.Text;
                        text = File.ReadAllLines(path);
                        mainProgressBar.Maximum = text.Length;
                    }
                    );
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        sw.WriteLine(Decrypt(text[i], key));
                        ThreadPool.QueueUserWorkItem(ProgressBarAddValue, null);
                    }
                    

                }
                MessageBox.Show("Done");
                Dispatcher.Invoke(() => mainProgressBar.Value = 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ProgressBarAddValue(object obj) {
            Dispatcher.Invoke(() => { ++mainProgressBar.Value; });
            
        }
        public void EncryptFile(object obj)
        {
            try
            {
                int key = 0;
                string[] text = null;
                string path = null;
                Dispatcher.Invoke(() =>
                {
                    key = Int32.Parse(keyPswdBox.Password);
                    text = File.ReadAllLines(pathTxtBox.Text);
                    path = pathTxtBox.Text;
                    mainProgressBar.Maximum = text.Length;
                });
                
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        sw.WriteLine(Encrypt(text[i], key));
                        ThreadPool.QueueUserWorkItem(ProgressBarAddValue, null);
                    }
                }
                //Dispatcher.BeginInvoke(new Action(
                //    () =>
                //    {
                       

                //    }
                //    ));

                
                MessageBox.Show("Done");
                Dispatcher.Invoke(() => mainProgressBar.Value = 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public string Encrypt(string message, int key)
        {
            string result = String.Empty;
            for (int i = 0; i < message.Length; i++)
            {
                result += (char)(message[i] ^ key);
            }
            
            return result;
        }
        public string Decrypt(string message, int key)
        {
            return Encrypt(message, key);
        }

        private void fileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                pathTxtBox.Text = ofd.FileName;
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (threads[0].IsAlive)
            {
                threads[0].Abort();
            }
            else {
                threads[1].Abort();
            }
        }
    }
}
