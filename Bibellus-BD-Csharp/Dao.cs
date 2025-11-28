using System;
using MySql.Data.MySqlClient;

namespace Bibellus_BD_Csharp
{
    public class DaO
    {
        public static MySqlConnection Connection { get; set; }
        public DaO()
        {
            Connection = new MySqlConnection();
        }

        public DaO(string connectionstring)
        {
            Connection = new MySqlConnection(connectionstring);
            Connection.Open();
        }

        public string[] MYSQLSelect(string tabela, string condicao = null, string colunas = null)
        {
            MySqlDataReader reader = new MySqlCommand($"SELECT {(colunas == null ? "*" : $"{colunas}")} FROM {tabela}{(condicao == null ? "" : $"WHERE {condicao}")};", Connection).ExecuteReader();
            string[] retorno = new string[1];

            for (int contador=0; reader.Read(); contador++) {
                for (int coluna = 0; coluna < reader.FieldCount; coluna++)
                {
                    retorno[contador] = reader[coluna].ToString()+",";
                }
                Array.Resize(ref retorno, retorno.Length+1);
            }

            return retorno;
        }

        public int MYSQLInsert(string tabela, string values, string valuestochange)
        {
            return new MySqlCommand($"INSERT INTO {tabela}{(valuestochange == null ? "" : $"({valuestochange})")} VALUES({values})", Connection).ExecuteNonQuery();
        }

        public int MYSQLDelete(string tabela, string condicao)
        {
            return new MySqlCommand($"DELETE FROM {tabela} WHERE {condicao}", Connection).ExecuteNonQuery();
        }

        public int MYSQLUpdate(string tabela, string value, string condicao)
        {
            return new MySqlCommand($"UPDATE {tabela} SET {value} WHERE {condicao}", Connection).ExecuteNonQuery();
        }
    }
}
