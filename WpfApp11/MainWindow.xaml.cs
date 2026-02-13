using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace WpfApp11
{

    public class User
    {
        public User(string name, string surname) {
            this.Name = name;
            this.Surname = surname;
        }

        public string Name { get; set; }
        public string Surname { get; set; }

        public override string ToString() { 
            return this.Name + " " + this.Surname;
        }
    }

    public partial class MainWindow : Window
    {

        public static async Task<int> Connect(string nazwa)
        {
            using var connection = new NpgsqlConnection(
                "Host=ep-proud-cell-ai4qbzz9-pooler.c-4.us-east-1.aws.neon.tech;" +
                "Database=neondb;" +
                "Username=neondb_owner;" +
                "Password=npg_9SgQl6MOnBom;" +
                "Ssl Mode=Require;" +
                "Channel Binding=Require;"
                );
            await connection.OpenAsync();

            var sql = "INSERT INTO test(text) values (@nazwa) returning id;";
            
            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("nazwa", nazwa);

            var id = await command.ExecuteScalarAsync();

            return (int) id!;
        }

        public static List<User> GetData()
        {
            var data = new List<User>();
            data.Add(new User("Karol", "May"));
            data.Add(new User("Joanna", "Wilk"));
            data.Add(new User("Helena", "Modrzejewska"));

            return data;
        }


        private async void SendButton_Click(object sender, RoutedEventArgs e, string nazwa)
        {
            var id = await Connect(nazwa);
            MessageBox.Show($"Inserted record with ID: {id}");
        }

        public List<User> nameList;
        
        public MainWindow()
        {
            
            InitializeComponent();

            this.nameList = GetData();

            ImionaListBox.ItemsSource = this.nameList;

            SaveButton.Click += (s, e) => {
                var data = NameTextBox.Text.ToString().Split(' ', 2);

                if (data.Length < 2)
                {
                    MessageBox.Show("Podaj Imię i Nazwisko");
                    return;
                }
                if (data[0]=="Alfred")
                {
                    MessageBox.Show("Alfredom dziękujemy!");
                    return;
                }
                this.nameList.Add(new User(data[0], data[1])); 
                ImionaListBox.ItemsSource = this.nameList;
                ImionaListBox.Items.Refresh();
            };

            SendButton.Click += (s, e) => SendButton_Click(s,e,NameTextBox.Text);
            DeleteButton.Click += (s, e) => DeleteButton_Click(s, e);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = ImionaListBox.SelectedIndex;
            if (selected == -1)
            {
                MessageBox.Show("Nic nie zaznaczono!");
            }
            else
            {
                this.nameList.RemoveAt(selected);
                ImionaListBox.Items.Refresh();
            }

        }
    }
}