using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList
{
    class DatabaseHelper
    {
        private string _connectionString;
        public DatabaseHelper(string databasePath)
        {
            _connectionString = databasePath;
        }
        public void CreateDatabase()
        {
            SQLiteConnection.CreateFile("TodoItems.db");

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string createTableQuery = @"CREATE TABLE IF NOT EXISTS ToDoItems (Id INTEGER PRIMARY KEY AUTOINCREMENT,Description TEXT NOT NULL,IsCompleted INTEGER NOT NULL);";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
