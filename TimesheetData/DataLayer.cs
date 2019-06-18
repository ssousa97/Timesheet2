using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimesheetCore;

namespace TimesheetData {

    public class DataLayer {

        public static string ConnectionString { get; set; } = "SERVER=192.185.211.53;DATABASE=gt2co816_TimesheetDB;USERNAME=gt2co816_tmsht;PASSWORD=Servidor@gt2";

        public static async Task ExportarBancoDeDados(IEnumerable<TarefaModel> Tarefas) {

            await Task.Run(() => {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString)) {

                    conn.OpenAsync();

                    string query = CriarQuery(Tarefas);

                    using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                        cmd.ExecuteNonQuery();
                    }
                }
            });
          
        }

        private static string CriarQuery(IEnumerable<TarefaModel> Tarefas) {

            var query = new StringBuilder();

            query.Append("INSERT INTO Tarefas (Tarefa, Status, TempoEstimado, TempoSemanal, TempoTotal, Prazo, Concluido, Responsavel, Projeto, Descricao, Periodo ) VALUES ");

            foreach (TarefaModel tarefa in Tarefas) {
                query.Append(tarefa.ToString());
                query.Append(",");
            }

            query.Remove(query.Length - 1, 1);

            return query.ToString();
        }

    }


}
