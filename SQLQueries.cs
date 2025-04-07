using System.Data.Common;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace Contacts_CLI;
partial class Program
{
  static readonly string databasePath = Path.Combine(AppContext.BaseDirectory, "contacts.db");
  static readonly SqliteConnection con = new($"Data Source={databasePath}");
  public static void EnsureDB()
  {
    using var cmd = new SqliteCommand(@"
      CREATE TABLE IF NOT EXISTS 
      contacts(
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      name TEXT, 
      phone_number INT,
      email TEXT,
      note TEXT);", con);
    con.Open();
    if (cmd.ExecuteNonQuery() > 0)
      AnsiConsole.MarkupLine("[bold yellow]Database not found.\nCreated new one.\n\n[/]");
    con.Close();
  }
  public static List<ReadAllContactsDB> ReadContactsQuery()
  {
    try
    {
      List<ReadAllContactsDB> DBList = [];
      var cmd = new SqliteCommand(@"SELECT * FROM contacts;", con);
      con.Open();
      SqliteDataReader reader = cmd.ExecuteReader();
      if (!reader.HasRows)
      {
        AnsiConsole.MarkupLine("[bold red]No Rows Found[/]");
        return DBList;
      }
      while (reader.Read())
      {
        ReadAllContactsDB contact = new()
        {
          id = int.Parse(reader[0].ToString()!),
          name = reader[1].ToString(),
          phone_number = int.Parse(reader[2].ToString()!),
          email = reader[3].ToString(),
          note = reader[4].ToString()
        };
        DBList.Add(contact);
      }
      return DBList;
    }
    catch
    {
      throw;
    }
  }
}