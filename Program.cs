using Spectre.Console;
using Microsoft.Data.Sqlite;
using Spectre.Console.Cli;

namespace Contacts_CLI;
partial class Program
{
  // Game plan
  // Create Delete Query + Command
  // Delete from a multi-select prompt to the user which will have names
  public static int Main(string[] args)
  {
    try
    {
      EnsureDB();

      var app = new CommandApp();

      app.Configure(config =>
      {
        config.AddCommand<ListContactsCommand>("list");
        config.AddExample("list", "[Contacts]");
        config.AddCommand<AddSettingsCommand>("add");
        config.AddCommand<DeleteContactCommand>("delete");
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