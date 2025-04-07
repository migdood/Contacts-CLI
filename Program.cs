using Spectre.Console;
using Microsoft.Data.Sqlite;

namespace Contacts_CLI;
partial class Program
{
  public static void Main(string[] args)
  {
    try
    {
      EnsureDB();

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
    catch (SqliteException SqliteException)
    {
      AnsiConsole.MarkupLineInterpolated($"[bold red]Error: {SqliteException.StackTrace}[/]");
      return;
    }
    catch (Exception ex)
    {
      AnsiConsole.MarkupLineInterpolated($"[bold red]Error: {ex.StackTrace}[/]");
      return;
    }
  }
  public static void Run()
  {
    bool running = true;

    while (running)
    {
      Console.Clear();

      AnsiConsole.WriteLine("Your Contacts:");
      AnsiConsole.WriteLine("--------------------");
      AnsiConsole.WriteLine($"");

      // Prompt for a command
      string command = AnsiConsole.Prompt(
          new TextPrompt<string>("Enter command (or 'q|quit' to quit):")
              .PromptStyle("green")
      );

      // Process the command
      if (command.ToLower() == "exit")
      {
        running = false;
      }
      else
      {
        AnsiConsole.WriteLine("You're Gay");
        // Add your command processing logic here
        AnsiConsole.WriteLine($"You entered command: {command}");
        AnsiConsole.WriteLine("Press any key to continue...");
        Console.ReadKey(true); // Pause before clearing the screen again
      }
    }

    AnsiConsole.WriteLine("Exiting application.");
  }
}

