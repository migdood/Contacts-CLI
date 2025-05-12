using System.ComponentModel.DataAnnotations;
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
  
  // Limit
  public static int FetchLimit()
  {
    int limit;

    using var cmd = new SqliteCommand("SELECT option1 FROM settings WHERE name = 'Pagination';", con);
    con.Open();
    var reader = cmd.ExecuteReader();
    reader.Read();
    if (!reader.HasRows)
    {
      AnsiConsole.MarkupLine("[bold red]Database error, no limit selected.[/]");
      return -1;
    }
    limit = int.Parse(reader[0].ToString()!);
    con.Close();
    return limit;
  }
  // Order Direction
  public static string? FetchOrderDirection(string? AscDesc = "ASC")
  {
    using var cmd = new SqliteCommand("SELECT option1 FROM settings WHERE name = 'AscDesc';", con);
    con.Open();
    var reader = cmd.ExecuteReader();
    reader.Read();
    if (!reader.HasRows)
    {
      AnsiConsole.MarkupLine("[bold red]Database error, no order of ascending / descending selected.[/]");
      return "";
    }
    AscDesc = reader[0].ToString();
    con.Close();
    return AscDesc;
  }
  // Sort Order
  public static string? FetchSortOrder()
  {
    string? order;
    using var cmd = new SqliteCommand("SELECT option1 FROM settings WHERE name = 'Sort Order';", con);
    con.Open();
    var reader = cmd.ExecuteReader();
    reader.Read();
    if (!reader.HasRows)
    {
      AnsiConsole.MarkupLine("[bold red]Please select a sorting style.[/]");
      return "";
    }
    order = reader[0].ToString();
    con.Close();
    return order;
  }
  // Page Count
  public static string? FetchPageCount(int OFFSET)
  {
    int PageCount;
    using var cmd = new SqliteCommand("SELECT COUNT(*) FROM contacts;", con);

    con.Open();
    var reader = cmd.ExecuteReader();
    reader.Read();
    if (!reader.HasRows)
    {
      AnsiConsole.MarkupLine("[bold red]Failed to get a count of the pages from DB.[/]");
      return null;
    }
    PageCount = int.Parse(reader[0].ToString()!) /FetchLimit();
    con.Close();

    return $"Page: {OFFSET}/{PageCount}";

  }
  public static List<ReadAllContactsDB> ReadContactsQuery(int OFFSET)
  {
    List<ReadAllContactsDB> DBList = [];
    try
    {
      string SortOrder = FetchSortOrder()!;
      string OrderDirection = FetchOrderDirection()!;
      int Limit = FetchLimit();

      using var cmd = new SqliteCommand(@$"SELECT * FROM contacts ORDER BY {SortOrder} {OrderDirection} LIMIT {Limit} OFFSET {OFFSET * Limit};", con);

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
          phone_number = reader[2].ToString(),
          email = reader[3].ToString(),
          note = reader[4].ToString()
        };
        DBList.Add(contact);
      }
      con.Close();
      return DBList;
    }
    catch { throw; }
  }
  
  public static void AddContactsQuery(string name, string phone_number, string email, string note)
  {
    var cmd = new SqliteCommand(@"
                                      INSERT INTO contacts(
                                      name,
                                      phone_number,
                                      email,
                                      note)
                                      VALUES(
                                      @name,
                                      @phone_number,
                                      @email,
                                      @note)", con);
    cmd.Parameters.AddWithValue("@name", name);
    cmd.Parameters.AddWithValue("@phone_number", phone_number);
    cmd.Parameters.AddWithValue("@email", email);
    cmd.Parameters.AddWithValue("@note", note);
    con.Open();
    cmd.ExecuteNonQuery();
    con.Close();
  }
  public static void DeleteContactQuery(int ID)
  {
    using var cmd = new SqliteCommand(@"DELETE FROM contacts 
                                        WHERE id=@id", con);
    cmd.Parameters.AddWithValue("@id", ID);
    con.Open();
    cmd.ExecuteNonQuery();
    con.Close();
  }
  public static void UpdateContactQuery(int ID, string? Name, string? PhoneNumber, string? Email, string? Note)
  {
    try
    {
      using var cmd = new SqliteCommand("UPDATE contacts SET ", con);

      bool first = true;

      if (!string.IsNullOrWhiteSpace(Name))
      {
        if (!first) cmd.CommandText += ", ";
        cmd.CommandText += "name = @name";
        cmd.Parameters.AddWithValue("@name", Name);
        first = false;
      }

      if (PhoneNumber != null)
      {
        if (!first) cmd.CommandText += ", ";
        cmd.CommandText += "phone_number = @phone_number";
        cmd.Parameters.AddWithValue("@phone_number", PhoneNumber);
        first = false;
      }

      if (!string.IsNullOrWhiteSpace(Email))
      {
        if (!first) cmd.CommandText += ", ";
        cmd.CommandText += "email = @email";
        cmd.Parameters.AddWithValue("@email", Email);
        first = false;
      }

      if (!string.IsNullOrWhiteSpace(Note))
      {
        if (!first) cmd.CommandText += ", ";
        cmd.CommandText += "note = @note";
        cmd.Parameters.AddWithValue("@note", Note);
        first = false;
      }

      cmd.CommandText += " WHERE id = @id;";
      cmd.Parameters.AddWithValue("@id", ID);
      con.Open();
      cmd.ExecuteNonQuery();
      con.Close();
    }
    catch { throw; }
  }
  public static void UpdateSortOrder(string Choice)
  {
    try
    {
      using var cmd = new SqliteCommand("UPDATE settings SET option1 = @option WHERE name = 'Sort Order';", con);
      cmd.Parameters.AddWithValue("@option", Choice);
      con.Open();
      cmd.ExecuteNonQuery();
      con.Close();
    }
    catch { throw; }
  }
  public static void UpdateAscendingOrder(string Choice)
  {
    try
    {
      using var cmd = new SqliteCommand("UPDATE settings SET option1 = @option WHERE name = 'AscDesc';", con);
      cmd.Parameters.AddWithValue("@option", Choice);
      con.Open();
      cmd.ExecuteNonQuery();
      con.Close();
    }
    catch { throw; }
  }
}