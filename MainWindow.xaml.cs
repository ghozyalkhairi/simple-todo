using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TodoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DatabaseHelper dbHelper = new DatabaseHelper("Data Source=TodoItems.db;Version=3;");
            dbHelper.CreateDatabase();

        }
    }
    public partial class MainWindow : Window
    {
        private ObservableCollection<TodoItem> _todoItems;
        private string _connectionString = "Data Source=TodoItems.db;Version=3;";
        public MainWindow()
        {
            InitializeComponent();
            _todoItems = new ObservableCollection<TodoItem>();
            ToDoListBox.ItemsSource = _todoItems;
            LoadTodoItems();
        }
        private void LoadTodoItems()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM ToDoItems";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _todoItems.Add(new TodoItem
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            IsCompleted = reader.GetInt32(2) == 1,
                        });
                    }
                }
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string description = NewItemTextBox.Text;
            if (!string.IsNullOrEmpty(description))
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO TodoItems (Description, IsCompleted) VALUES (@description, 0)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@description", description);
                        command.ExecuteNonQuery();
                    }
                }
                _todoItems.Add(new TodoItem { Description = description, IsCompleted = false });
                NewItemTextBox.Clear();
            }
        }
    }
    public class TodoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}