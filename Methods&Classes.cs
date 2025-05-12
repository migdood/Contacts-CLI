using Spectre.Console;

namespace Contacts_CLI;
partial class Program
{
  public class ReadAllContactsDB
  {
    public int? id;
    public string? name;
    public string? phone_number;
    public string? email;
    public string? note;
  }

  public static void DisplayTable(int OFFSET, List<ReadAllContactsDB> ListQuery)
  {
    try
    {
      bool SeparateRows = FetchRowSeparator();
      var table = new Table().Centered();

      table.Title("Contacts List", Style.Plain);
      table.AddColumns("ID", "Name", "Phone Number", "Email", "Note").Width(110).ShowRowSeparators = SeparateRows;
      if (ListQuery.Count <= 0)
        return;
      foreach (var item in ListQuery)
      {
        table.AddRow(
          new Markup($"[yellow]{item.id}[/]").RightJustified(),
          new Markup($"[yellow]{item.name}[/]"),
          new Markup($"[yellow]{item.phone_number}[/]").RightJustified(),
          new Markup($"[yellow]{item.email}[/]"),
          new Markup($"[yellow]{item.note}[/]"));
      }
      AnsiConsole.Write(table);
      AnsiConsole.WriteLine(FetchPageCount(OFFSET)!);
    }
    catch { throw; }
  }
}