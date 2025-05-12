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
        //TODO: Add Search, Pagination, Row separator option into settings table
        config.AddCommand<ListContactsCommand>("list");
        config.AddCommand<AddSettingsCommand>("add");
        config.AddExample("add", "Marcus","187657484");
        config.AddCommand<DeleteContactCommand>("delete");
        config.AddExample("delete","42");
        config.AddCommand<UpdateContactCommand>("update");
        config.AddCommand<SortingCommand>("sort");
        config.AddCommand<RandomContacts>("ran");
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

}