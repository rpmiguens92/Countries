using Countries.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace Countries.Services
{
    public class DataService
    {
        private const string CountryDB = "Countries_BD.dbo";

        public DataService()
        {

            try
            {
                if (!File.Exists(CountryDB))
                {
                    SQLiteConnection.CreateFile(CountryDB);
                }
                InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao iniciar a BD");
            }
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection($"Data Source={CountryDB}"))
            { //se não existe vai criar a BD
                try
                {
                    connection.Open();
                    var commandText = "create table if not exists Countries (Name text, Capital text, Region text, Subregion text, Gini text, Population int, Flag text )";
                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao criar base de dados");
                }
            }
        }

        public void SaveData(List<Country> countries)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={CountryDB}"))
                {
                    connection.Open();
                    var sql = "insert into Countries (Name, Capital, Region, Subregion, Gini, Population, Flag) values (@Name, @Capital, @Region, @Subregion, @Gini, @Population, @Flag)";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        foreach (var country in countries)
                        {

                            command.Parameters.AddWithValue("@Name", country.Nome.commonName);
                            command.Parameters.AddWithValue("@Capital", country.Capital != null ? JsonConvert.SerializeObject(country.Capital) : null);
                          
                            command.Parameters.AddWithValue("@Region", country.Region);
                            command.Parameters.AddWithValue("@Subregion", country.Subregion);
                           
                            command.Parameters.AddWithValue("@Gini", country.Gini != null && country.Gini.Count > 0 ? JsonConvert.SerializeObject(country.Gini) : "0");
  
                            command.Parameters.AddWithValue("@Population", country.Population);                     
                            command.Parameters.AddWithValue("@Flag", country.Flags != null ? JsonConvert.SerializeObject(country.Flags) : null);
                            command.ExecuteNonQuery();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar informação na Base de Dados.");
            }
        }

        public List<Country> GetData()
        {
            var countries = new List<Country>();
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={CountryDB}"))
                {
                    connection.Open();
                    string sql = "select * from Countries";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    var country = new Country
                                    {
                                        Nome = new Name { commonName = reader["Name"].ToString() },
                                        Capital = !string.IsNullOrEmpty(reader["Capital"].ToString()) ? JsonConvert.DeserializeObject<List<string>>(reader["Capital"].ToString()) : null,
                                        Region = reader["Region"].ToString(),
                                        Subregion = reader["Subregion"].ToString(),
                                        //Currency = reader["Currency"].ToString(),
                                        Gini = !string.IsNullOrEmpty(reader["Gini"].ToString()) && reader["Gini"].ToString() != "0" ? JsonConvert.DeserializeObject<Dictionary<string, float>>(reader["Gini"].ToString()) : null,
                                        Population = int.Parse(reader["Population"].ToString()),
                                        Flags = !string.IsNullOrEmpty(reader["Flag"].ToString()) ? JsonConvert.DeserializeObject<Flag>(reader["Flag"].ToString()) : null
                                        //Maps = new Map { GoogleMaps = reader["Maps"].ToString() }

                                    };
                                    countries.Add(country);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Erro ao desserializar dados do país: {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao obter info da base de dados:", ex.Message);
            }
            return countries;
        }
    }
    
}