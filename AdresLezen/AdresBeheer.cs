using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace AdresLezen
{
    public class AdresBeheer
    {
        private Dictionary<int, Gemeente> _gemeentes;
        private Dictionary<int, Straatnaam> _straatnamen;
        public AdresBeheer(string connectionString)
        {
            this._connectionString = connectionString;
            _gemeentes = new Dictionary<int, Gemeente>();
            _straatnamen = new Dictionary<int, Straatnaam>();
        }
        public void LeesGML()
        {
            List<string> collectie = new List<string>();
            string line;
            using (StreamReader r = new StreamReader(@"C:\Users\davy\Documents\data\CRAB_Adressenlijst_GML\GML\CrabAdr.gml"))
            {
                // Adres in 1 collectie steken en dan verder sturen voor verwerking.
                int i = 0;
                while ((line = r.ReadLine()) != null)
                {
                    if (line.Contains("<agiv:CrabAdr>"))
                        i++;
                    if (i == 1)
                        collectie.Add(line);
                    if (line.Contains("</agiv:CrabAdr>"))
                    {
                        i--;
                        if (collectie.Count != 21)
                            throw new ArgumentException("De lengte van de List is niet correct");
                        MaakAdres(collectie);
                        collectie.Clear();
                    }
                }
            }
        }
        //public void TestGML()
        //{
        //    List<string> collectie = new List<string>();
        //    string line;
        //    using (StreamReader r = new StreamReader(@"C:\Users\davy\Documents\data\CRAB_Adressenlijst_GML\GML\CrabAdr.gml"))
        //    {
        //        // Adres in 1 collectie steken en dan verder sturen voor verwerking.
        //        int i = 0;
        //        int testi = 0;
        //        while ((line = r.ReadLine()) != null)
        //        {
        //            if (line.Contains("<agiv:CrabAdr>"))
        //                i++;
        //            if (i == 1)
        //                collectie.Add(line);
        //            if (line.Contains("</agiv:CrabAdr>"))
        //            {
        //                i--;
        //                if (collectie.Count == 21)
        //                    Console.WriteLine();
        //                Console.WriteLine(collectie[5]);
        //                collectie.Clear();
        //                testi++;
        //            }
        //            if (testi == 3000)
        //                Console.ReadLine();
        //        }
        //    }
        //}
        public void MaakAdres(List<string> collectie) 
        {
            // gemeente aanmaken en in dictionary steken indien nodig.
            int NISCode = int.Parse(Isoleer(collectie[8]));
            if (!_gemeentes.ContainsKey(NISCode))
            {
                Gemeente gmt = new Gemeente(NISCode, Isoleer(collectie[9]));
                VoegGemeenteToe(gmt);
                _gemeentes.Add(NISCode, gmt);
            }

            // straatnaam aanmaken en in dictionary steken indien nodig.
            int straatnaamId = int.Parse(Isoleer(collectie[2]));
            if (!_straatnamen.ContainsKey(straatnaamId))
            {
                Straatnaam strn = new Straatnaam(straatnaamId, Isoleer(collectie[3]), _gemeentes[NISCode]);
                VoegStraatnaamToe(strn);
                _straatnamen.Add(straatnaamId, strn);
            }

            // adreslocatie en adres aanmaken. aangezien deze altijd uniek zijn worden ze niet in een dictionary gestopt.
            double x = double.Parse(Isoleer(collectie[15]));
            double y = double.Parse(Isoleer(collectie[16]));
            AdresLocatie adrl = new AdresLocatie(x, y);

            int adresId = int.Parse(Isoleer(collectie[1]));
            Adres adres = new Adres(adresId, _straatnamen[straatnaamId], Isoleer(collectie[5]), Isoleer(collectie[6]), Isoleer(collectie[4]), Isoleer(collectie[7]), adrl);
            VoegAdresToe(adres);
        }
        private string Isoleer(string value)
        {
            // het middenste dat nodig is isoleren en controle ofdat het niet bestaat.
            string resultaat;
            if (!value.Contains("/>"))
            {
                int begin = value.IndexOf(">") + 1;
                int eind = value.LastIndexOf("<");
                resultaat = value.Substring(begin, (eind - begin));
            }
            else
                resultaat = null;
            return resultaat;
        }
        private string _connectionString;
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }
        public void VoegGemeenteToe(Gemeente gemeente)
        {
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO gemeente (niscode, gemeentenaam) VALUES(@niscode, @gemeentenaam)";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@niscode", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@gemeentenaam", SqlDbType.NVarChar));
                    command.CommandText = query;
                    command.Parameters["@niscode"].Value = gemeente.NISCode;
                    command.Parameters["@gemeentenaam"].Value = gemeente.Gemeentenaam;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void VoegStraatnaamToe(Straatnaam straatnaam)
        {
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO straatnaam (id, straatnaam, niscode) VALUES(@id, @straatnaam, @niscode)";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@straatnaam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@niscode", SqlDbType.Int));
                    command.CommandText = query;
                    command.Parameters["@id"].Value = straatnaam.ID;
                    command.Parameters["@straatnaam"].Value = straatnaam.Naam == null ? DBNull.Value : (object)straatnaam.Naam;
                    command.Parameters["@niscode"].Value = straatnaam.Gemeente.NISCode;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void VoegAdresToe(Adres adres)  
        {
            SqlConnection connection = GetConnection();
            string queryS = "INSERT INTO adreslocatie (x, y) output INSERTED.ID VALUES(@x, @y)";
            string querySC = "INSERT INTO adres (id, straatnaamid, huisnummer, appartementnummer, busnummer, huisnummerlabel, adreslocatieid) " +
                "VALUES(@id, @straatnaamid, @huisnummer, @appartementnummer, @busnummer, @huisnummerlabel, @adreslocatieid)";

            using (SqlCommand command1 = connection.CreateCommand())
            using (SqlCommand command2 = connection.CreateCommand())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                command1.Transaction = transaction;
                command2.Transaction = transaction;
                try
                {
                    // adreslocatie toevoegen
                    command1.Parameters.Add(new SqlParameter("@x", SqlDbType.Float));
                    command1.Parameters.Add(new SqlParameter("@y", SqlDbType.Float));
                    command1.CommandText = queryS;
                    command1.Parameters["@x"].Value = adres.Locatie.X;
                    command1.Parameters["@y"].Value = adres.Locatie.Y;
                    // nieuw Id opvragen
                    int newId = (int)command1.ExecuteScalar();
                    //adres toevoegen
                    command2.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    command2.Parameters.Add(new SqlParameter("@straatnaamid", SqlDbType.Int));
                    command2.Parameters.Add(new SqlParameter("@huisnummer", SqlDbType.NVarChar));
                    command2.Parameters.Add(new SqlParameter("@appartementnummer", SqlDbType.NVarChar));
                    command2.Parameters.Add(new SqlParameter("@busnummer", SqlDbType.NVarChar));
                    command2.Parameters.Add(new SqlParameter("@huisnummerlabel", SqlDbType.NVarChar));
                    command2.Parameters.Add(new SqlParameter("@adreslocatieid", SqlDbType.Int));

                    command2.CommandText = querySC;
                    command2.Parameters["@id"].Value = adres.ID;
                    command2.Parameters["@straatnaamid"].Value = adres.Straatnaam.ID;
                    command2.Parameters["@huisnummer"].Value = adres.Huisnummer;
                    command2.Parameters["@appartementnummer"].Value = adres.Appartementnummer == null ? DBNull.Value : (object)adres.Appartementnummer;
                    command2.Parameters["@busnummer"].Value = adres.Busnummer == null ? DBNull.Value : (object)adres.Busnummer;
                    command2.Parameters["@huisnummerlabel"].Value = adres.HuisnummerLabel == null ? DBNull.Value : (object)adres.HuisnummerLabel;
                    command2.Parameters["@adreslocatieid"].Value = newId;

                    command2.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
