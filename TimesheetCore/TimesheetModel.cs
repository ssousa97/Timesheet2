using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace TimesheetCore {

    [Serializable]
    public class TimesheetModel : INotifyPropertyChanged {


        public string Responsavel { get; set; } 
        public List<string> Projetos { get; set; }
        public ObservableCollection<TarefaModel> Tarefas { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private TimeSpan _TempoSemanalTotal;
        public TimeSpan TempoSemanalTotal {
            get => _TempoSemanalTotal;
            set {
                if (value != _TempoSemanalTotal) {
                    _TempoSemanalTotal = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool TarefaEmExecucao;

        private readonly Stopwatch Cronometro = new Stopwatch();

        public TimesheetModel() {

            Responsavel = ConfigurationLayer.GetConfig("Responsavel");

            Tarefas = CarregarTarefas();

            Projetos = CarregarProjetos();

            TempoSemanalTotal = new TimeSpan((from t in Tarefas select t.TempoSemanal.Ticks).Sum());

            Tarefas.CollectionChanged += Tarefas_CollectionChanged;

        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Tarefas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {

            switch (e.Action) {

                case NotifyCollectionChangedAction.Add:
                    {
                        var tarefa = e.NewItems[0] as TarefaModel;
                        tarefa.Periodo = GerarPeriodo();
                        tarefa.Responsavel = Responsavel;
                    }
                break;

            }

        }

        private List<string> CarregarProjetos() {

            return ConfigurationLayer.GetConfig("Projetos").Split(',').ToList();

        }

        private ObservableCollection<TarefaModel> CarregarTarefas() {

            var dir = @"C:\Timesheet";
            var file = dir + @"\data.bin";

            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            if (File.Exists(file)) {
                using (var stream = File.OpenRead(file)) {
                    return (ObservableCollection<TarefaModel>)new BinaryFormatter().Deserialize(stream);
                }
            }
            else {
                return new ObservableCollection<TarefaModel>();
            }

        }

        private Tuple<DateTime, DateTime> GerarPeriodo() {

            var hoje = DateTime.Today;
            var segunda = hoje.AddDays(1 - (int)hoje.DayOfWeek);
            var sexta = segunda.AddDays(4);

            return new Tuple<DateTime, DateTime>(segunda, sexta);

        }

        public void SalvarTarefas() {

            if (TarefaEmExecucao) {
                var tarefa = (from t in Tarefas where t.Status == "Em Execução" select t).FirstOrDefault();
                tarefa.Status = "Em Andamento";
            }
            
            using (var stream = File.Create(@"C:\Timesheet\data.bin")) {
                new BinaryFormatter().Serialize(stream, Tarefas);
            }

        }

        public void SalvamentoAutomático() {

            if (TarefaEmExecucao) {

                var tarefa = (from t in Tarefas where t.Status == "Em Execução" select t).FirstOrDefault();
                SalvarTarefas();
                tarefa.Status = "Em Execução";

            }
            else {

                SalvarTarefas();
            }
        }

        public void IniciarTarefa(TarefaModel tarefa) {

            if (TarefaEmExecucao) {

                throw new Exception("Já existe uma tarefa em execução!");

            }

            if (tarefa.Status == "Concluída") {

                throw new Exception("Não é possível iniciar uma tarefa já finalizada!");

            }

            if(tarefa.TempoEstimado == null || tarefa.Prazo == null){

                throw new Exception("Insira o prazo e o tempo estimado da tarefa!");

            }

            Cronometro.Start();
            tarefa.Status = "Em Execução";
            TarefaEmExecucao = true;

        }

        public void PausarTarefa(TarefaModel tarefa) {

            Cronometro.Stop();

            TarefaEmExecucao = false;

            tarefa.Status = "Em Andamento";

            tarefa.TempoSemanal += Cronometro.Elapsed;

            tarefa.TempoTotal += Cronometro.Elapsed;

            TempoSemanalTotal = new TimeSpan((from t in Tarefas select t.TempoSemanal.Ticks).Sum());

            Cronometro.Reset();

        }

        public void FinalizarTarefa(TarefaModel tarefa){

            tarefa.Status = "Concluída";
            tarefa.Conclusao = DateTime.Today;

        }

        public void AdiarTarefa(TarefaModel tarefa) {
            tarefa.Status = "Pendente";
        }

        public void ResetarTarefa(TarefaModel tarefa) {

            tarefa.Status = "Em Andamento";
            tarefa.TempoSemanal = new TimeSpan();

        }
     
    }
}
