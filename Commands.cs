using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Contacts_CLI;
partial class Program
{
  public class AddSettings : CommandSettings
  {
  }
  public class AddContactSettings : AddSettings
  {
    [CommandArgument(0, "<name>")]
    public required string Name { get; set; }

    [CommandArgument(1, "<number>")]
    public long PhoneNumber { get; set; }

    [CommandOption("-e|--email <email>")]
    [DefaultValue("")]
    public required string Email { get; set; }

    [CommandOption("-n|--note <note>", IsHidden = false)]
    [DefaultValue("")]
    public required string Note { get; set; }
  }
  public class AddSettingsCommand : Command<AddContactSettings>
  {
    public override int Execute(CommandContext context, AddContactSettings st)
    {
      AddContactsQuery(st.Name, st.PhoneNumber, st.Email, st.Note);
      AnsiConsole.MarkupLine("[bold green]Success![/]");
      return 0;
    }
  }
  public class ListContactsSettings : CommandSettings { }

  public class ListContactsCommand : Command<ListContactsSettings>
  {
    public override int Execute(CommandContext context, ListContactsSettings settings)
    {
      DisplayTable();
      return 0;
    }
  }
}