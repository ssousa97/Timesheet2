using System;
using System.Configuration;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TimesheetCore;
using TimesheetData;

namespace TimesheetUI {
  
    public partial class MainWindow : Window {

        public MainWindow() {
                 
            string responsavel = ConfigurationLayer.GetConfig("Responsavel");

            if (string.IsNullOrEmpty(responsavel) || responsavel == "Dev") {

                new Login().ShowDialog();

            }

            InitializeComponent();

            SetupTimer();

            ComboBoxProjetos.ItemsSource = Timesheet.Projetos;

        }


        private void PlayPauseTarefa(object sender, MouseButtonEventArgs e) {

            try {

                var tarefa = (TarefaModel)TimesheetDataGrid.SelectedItem;

                if (tarefa.Status == "Em Execução") {

                    Timesheet.PausarTarefa(tarefa);

                }
                else {

                    Timesheet.IniciarTarefa(tarefa);

                }
            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);

            }

        }

        private void FinalizarTarefa(object sender, RoutedEventArgs e) {

            try {

                Timesheet.FinalizarTarefa(TimesheetDataGrid.SelectedItem as TarefaModel);

            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }
        }

        private void AdiarTarefa(object sender, RoutedEventArgs e) {

            try {

                Timesheet.AdiarTarefa(TimesheetDataGrid.SelectedItem as TarefaModel);

            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }
        }

        private void ResetarTarefa(object sender, RoutedEventArgs e) {

            try {

                Timesheet.ResetarTarefa(TimesheetDataGrid.SelectedItem as TarefaModel);

            }
            catch(Exception ex) {

                MessageBox.Show(ex.Message);

            }

        }

        private async void ExportarTarefas(object sender, RoutedEventArgs e) {

            try {

                await DataLayer.ExportarBancoDeDados(Timesheet.Tarefas);
            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e) {

            try {

                Timesheet.SalvarTarefas();

            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);

            }
        }

        private void SetupTimer() {

            var saveTimer = new Timer(5*60000) {
                Enabled = true,
                AutoReset = true
            };
            saveTimer.Elapsed += (s, e) => {
                try {

                    Timesheet.SalvamentoAutomático();

                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }

            };
        }
    }
}
