using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TimesheetCore;

namespace TimesheetUI {
    /// <summary>
    /// Interaction logic for PopUpLogin.xaml
    /// </summary>
    public partial class Login : Window {

        public Login() {
            InitializeComponent();
        }

        private void ValidarResponsavel() {

            ConfigurationLayer.SetConfig("Responsavel", NomeTextBox.Text);

            Close();
        }

        private void Entrar(object sender, RoutedEventArgs e) {

            if (string.IsNullOrEmpty(NomeTextBox.Text)) {
                MessageBox.Show("Por favor insira seu nome!");
                return;
            }

            ValidarResponsavel();

        }

        private void Entrar(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                if (string.IsNullOrEmpty(NomeTextBox.Text)) {
                    MessageBox.Show("Por favor insira seu nome!");
                    return;
                }
                ValidarResponsavel();
            }
        }
    }
}
