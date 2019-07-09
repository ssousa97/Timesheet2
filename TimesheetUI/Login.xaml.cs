using System;
using System.Windows;
using System.Windows.Input;
using TimesheetCore;
using TimesheetData;

namespace TimesheetUI {
    /// <summary>
    /// Interaction logic for PopUpLogin.xaml
    /// </summary>
    public partial class Login : Window {

        public Login() {

            InitializeComponent();
            BuscarAtualizações();
        }

        private void ValidarResponsavel() {

            ConfigurationLayer.SetConfig("Responsavel", NomeTextBox.Text);

            Close();
        }

        private void BuscarAtualizações() {

            try {

                string versaoAtual = "_2.0";
                string proxVersao = "_2.1";
                string proxAtualização = Title.Replace(versaoAtual, proxVersao);

                if (UpdateLayer.TemNovaAtualização(proxAtualização)) {
                    UpdateLayer.BaixarAtualização(proxAtualização);
                    MessageBox.Show(
                        $"Atualização baixada com sucesso para sua pasta de Downloads!\n" +
                        $"C:\\Users\\{Environment.UserName}\\Downloads\\{proxAtualização}\n" +
                        $"Descompacte e substitua na pasta antiga da Timesheet!\n"
                    );
                }
            }
            catch(Exception e) {
                MessageBox.Show($"Houve um erro ao buscar a nova atualização!\n{e.Message}\n");
            }
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
