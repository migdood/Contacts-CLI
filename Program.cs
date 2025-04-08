using Spectre.Console;
using Microsoft.Data.Sqlite;
using Spectre.Console.Cli;

namespace Contacts_CLI;
partial class Program
{
  public static int Main(string[] args)
  {
    try
    {
      EnsureDB();


      var app = new CommandApp();
      app.Configure(config =>
      {
        config.AddCommand<AddSettingsCommand>("add");
        config.AddCommand<ListContactsCommand>("list");
        config.AddExample("list", "[Contacts]");
      });
      return app.Run(args);
    }
    catch (CommandAppException CommandAppException)
    {
      AnsiConsole.MarkupLineInterpolated($"[bold red]Error: {CommandAppException.Message}[/]");
      AnsiConsole.MarkupLineInterpolated($"[bold yellow]Stack Trace: {CommandAppException.StackTrace}[/]");
      return 0;
    }
    catch (SqliteException SqliteException)
    {
      AnsiConsole.MarkupLineInterpolated($"[bold red]Error: {SqliteException.Message}[/]");
      AnsiConsole.MarkupLineInterpolated($"[bold yellow]Stack Trace: {SqliteException.StackTrace}[/]");
      return 0;
    }
    catch (Exception ex)
    {
      AnsiConsole.MarkupLineInterpolated($"[bold red]Error: {ex.Message}[/]");
      AnsiConsole.MarkupLineInterpolated($"[bold yellow]Stack Trace: {ex.StackTrace}[/]");
      return 0;
    }
  }
  public static void DisplayTable()
  {
    var table = new Table().Centered();

    table.Title("Contacts List", Style.Plain);
    table.AddColumns("ID", "Name", "Phone Number", "Email", "Note");

    foreach (var item in ReadContactsQuery())
    {
      table.AddRow(
        new Markup($"[yellow]{item.id}[/]").RightJustified(),
        new Markup($"[yellow]{item.name}[/]"),
        new Markup($"[yellow]{item.phone_number}[/]").RightJustified(),
        new Markup($"[yellow]{item.email}[/]"),
        new Markup($"[yellow]{item.note}[/]"));
    }

    AnsiConsole.Write(table);
  }
}