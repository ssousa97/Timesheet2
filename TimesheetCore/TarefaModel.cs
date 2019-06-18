using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TimesheetCore
{
    [Serializable]
    public class TarefaModel : IDataErrorInfo, INotifyPropertyChanged {

        private string _Nome;
        private string _Status = "Em Andamento";
        private TimeSpan _TempoEstimado;
        private TimeSpan _TempoSemanal;
        private TimeSpan _TempoTotal;
        private DateTime? _Prazo;
        private DateTime? _Conclusao;
        private string _Responsavel;
        private string _Projeto;
        private string _Descricao;
        private Tuple<DateTime, DateTime> _Periodo;

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Nome {
            get => _Nome;
            set {
                if (value != _Nome) {
                    _Nome = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Status {
            get => _Status;
            set {
               
                if (value != _Status) {

                    if (_Status == "Concluída") {
                        throw new Exception("Essa tarefa ja foi finalizada!");
                    }
                    
                    _Status = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public TimeSpan TempoEstimado {
            get => _TempoEstimado;
            set {
                if (value != _TempoEstimado) {
                    _TempoEstimado = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public TimeSpan TempoSemanal {
            get => _TempoSemanal;
            set {
                if (value != _TempoSemanal) {
                    _TempoSemanal = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public TimeSpan TempoTotal {
            get => _TempoTotal;
            set {
                if (value != _TempoTotal) {
                    _TempoTotal = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime? Prazo {
            get => _Prazo;
            set {
                if (value != _Prazo) {
                    _Prazo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime? Conclusao {
            get => _Conclusao;
            set {
                if (value != _Conclusao) {
                    _Conclusao = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Responsavel {
            get => _Responsavel;
            set {
                if (value != _Responsavel) {
                    _Responsavel = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Projeto {
            get => _Projeto;
            set {
                if (value != _Projeto) {
                    _Projeto = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Descricao {
            get => _Descricao;
            set {
                if (value != _Descricao) {
                    _Descricao = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Tuple<DateTime,DateTime> Periodo {
            get => _Periodo; 
            set {
                if (value != _Periodo) {
                    _Periodo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string PeriodoString() {

            var segunda = _Periodo.Item1;
            var sexta = _Periodo.Item2;

            return $"{segunda.ToString("dd/MM/yyyy").Replace('-','/')} - {sexta.ToString("dd/MM/yyyy").Replace('-','/')}";
        }

              
        public string this[string columnName] {
            get {
                if (columnName == "Nome") {
                    if (string.IsNullOrEmpty(Nome)) {
                        return "O nome da tarefa não pode ser vazio!";
                    }
                }
                if(columnName == "TempoEstimado") {
                    if (TempoEstimado == new TimeSpan(0,0,0)) {
                        return "Insira um Tempo estimado!";
                    }
                }
                if(columnName == "Prazo") {
                    if(Prazo == null) {
                        return "Insira um prazo!";
                    }
                }
                return string.Empty;
            }
        }

        public string Error {
            get {
                return string.Empty;
            }
        }

        public override string ToString() {
            return 
                $"('{_Nome}'," +
                $"'{_Status}'," +
                $"'{_TempoEstimado.ToString("hh\\:mm\\:ss")}'," +
                $"'{_TempoSemanal.ToString("hh\\:mm\\:ss")}'," +
                $"'{_TempoTotal.ToString("hh\\:mm\\:ss")}'," +
                $"'{_Prazo.Value.ToString("dd/MM/yyyy").Replace('-','/')}'," +
                $"'{_Conclusao}'," +
                $"'{_Responsavel}'," +
                $"'{_Projeto}'," +
                $"'{_Descricao}'," +
                $"'{PeriodoString()}')";
        }
    }
}
