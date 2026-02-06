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

        private async void SendButton_Click(object sender, RoutedEventArgs e, string nazwa)
        {
            var id = await Connect(nazwa);
            MessageBox.Show($"Inserted record with ID: {id}");
        }

        public MainWindow()
        {
   
            var nazwa = "Ala ma kota";

            InitializeComponent();

            SendButton.Click += (s, e) => SendButton_Click(s,e,nazwa);

        }

    }
}