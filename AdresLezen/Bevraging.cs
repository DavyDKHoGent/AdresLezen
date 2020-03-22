using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AdresLezen
{
    public class Bevraging
    {
        public Bevraging(string connectionstring)
        {
            this._connectionString = connectionstring;
        }
        private string _connectionString;
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }
        public Adres GetAdres(int Id) 
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT a.Id, a.straatnaamid, huisnummer, a.appartementnummer, a.busnummer, a.huisnummer, a.huisnummerlabel, s.straatnaam, s.niscode, " +
                "g.gemeentenaam FROM adres a JOIN straatnaam s ON a.straatnaamid = s.Id JOIN gemeente g ON s.niscode = g.niscode WHERE a.Id=@Id";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
                command.Parameters["@Id"].Value = Id;
                connection.Open();
                try
                {
                    IDataReader reader = command.ExecuteReader(); //of SqlDataReader
                    reader.Read();
                    Gemeente gmt = new Gemeente((int)reader["niscode"], (string)reader["gemeentenaam"]);
                    Straatnaam strn = new Straatnaam((int)reader["straatnaamid"], (string)reader["straatnaam"], gmt);

                    string appartementnummer = (object)reader["appartementnummer"] == DBNull.Value ? "null" : (string)reader["appartementnummer"];
                    string busnummer = (object)reader["busnummer"] == DBNull.Value ? "null" : (string)reader["busnummer"];
                    string huisnummerLabel = (object)reader["huisnummerlabel"] == DBNull.Value ? "null" : (string)reader["huisnummerlabel"];

                    AdresLocatie adrl = new AdresLocatie(0, 0);
                    Adres adres = new Adres((int)reader["Id"], strn, appartementnummer, busnummer, (string)reader["huisnummer"], huisnummerLabel, adrl);

                    reader.Close();
                    return adres;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
        } // tostring() van string nog aanpassen?
        public SortedSet<Straatnaam> GetStratenPerGemeente(string gemeentenaam)
        {
            SqlConnection connection = GetConnection();
            string queryS = "SELECT* FROM gemeente WHERE gemeentenaam=@gemeentenaam";
            string querySC = "SELECT* FROM straatnaam WHERE niscode=@niscode";
            SortedSet<Straatnaam> straten = new SortedSet<Straatnaam>();

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = queryS;
                command.Parameters.Add(new SqlParameter("@gemeentenaam", SqlDbType.NVarChar));
                command.Parameters["@gemeentenaam"].Value = gemeentenaam;
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader(); //of SqlDataReader
                    reader.Read();
                    int niscode = (int)reader["niscode"];
                    reader.Close();

                    command.CommandText = querySC;
                    command.Parameters.Add(new SqlParameter("@niscode", SqlDbType.Int));
                    command.Parameters["@niscode"].Value = niscode;
                    reader = command.ExecuteReader();
                    Gemeente gmt = new Gemeente(niscode, gemeentenaam);
                    while (reader.Read())
                    {
                        Straatnaam strn = new Straatnaam((int)reader["id"], (string)reader["straatnaam"], gmt);
                        straten.Add(strn);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
                return straten;
            }
        } // OK!
        public List<Adres> GetAdressenPerStraatnaamId(int straatnaamid)
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT* FROM adres WHERE straatnaamid=@straatnaamid";
            List<Adres> adressen = new List<Adres>();

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.Add(new SqlParameter("@straatnaamid", SqlDbType.Int));
                command.Parameters["@straatnaamid"].Value = straatnaamid;
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Adres adres = GetAdres((int)reader["id"]);
                        adressen.Add(adres);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
                return adressen;
            }
        } // OK!
    }
}
