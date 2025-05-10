using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Contacts_CLI;
partial class Program
{
  #region  Add
  public class AddSettings : CommandSettings { }
  public class AddContactSettings : AddSettings
  {
    [CommandArgument(0, "<name>")]
    public required string Name { get; set; }

    [CommandArgument(1, "<number>")]
    public required string PhoneNumber { get; set; }

    [CommandOption("-e|--email <email>")]
    [DefaultValue("-")]
    public required string Email { get; set; }

    [CommandOption("-n|--note <note>")]
    [DefaultValue("-")]

    public required string Note { get; set; }
  }
  public class AddSettingsCommand : Command<AddContactSettings>
  {
    public override int Execute(CommandContext context, AddContactSettings st)
    {
      try
      {
        AddContactsQuery(st.Name, st.PhoneNumber, st.Email, st.Note);
        AnsiConsole.MarkupLine("[bold green]Success![/]");
        return 0;
      }
      catch { throw; }
    }
  }
  #endregion

  #region List
  public class ListContactsCommand : Command
  {
    public override int Execute(CommandContext context)
    {
      try
      {
        DisplayTable();
        return 0;
      }
      catch { throw; }
    }
  }
  #endregion

  #region Delete
  public class DeleteContactSettings : CommandSettings
  {
    [CommandArgument(0, "<ID>")]
    public int ContactID { get; set; }
  }
  public class DeleteContactCommand : Command<DeleteContactSettings>
  {
    public override int Execute(CommandContext context, DeleteContactSettings settings)
    {
      try
      {
        DeleteContactQuery(settings.ContactID);
        AnsiConsole.MarkupLine("[bold green]Success![/]");
        return 0;
      }
      catch { throw; }
    }
  }
  #endregion

  #region Update
  public class UpdateSettings : CommandSettings
  {
    [CommandArgument(0, "<ID>")]
    public int ID { get; set; }

    [CommandOption("-n|--name <name>")]
    public string? Name { get; set; }

    [CommandOption("-p|--phone <phone-number>")]
    public string? PhoneNumber { get; set; }

    [CommandOption("-e|--email <email>")]
    public string? Email { get; set; }

    [CommandOption("-s|--note <note>")]
    public string? Note { get; set; }
  }
  public class UpdateContactCommand : Command<UpdateSettings>
  {
    public override int Execute(CommandContext context, UpdateSettings contact)
    {
      try
      {
        UpdateContactQuery(contact.ID, contact.Name, contact.PhoneNumber, contact.Email, contact.Note);
        AnsiConsole.MarkupLine("[bold green]Success![/]");
        return 0;
      }
      catch { throw; }
    }
  }
  #endregion

  #region Sorting
  public class SortingCommand : Command
  {
    public override int Execute(CommandContext context)
    {
      try
      {
        Dictionary<string, string> OrderDict = new(){
          {"Latest","ID"},
          {"Name","Name"},
          {"Number","Phone_Number"},
          {"Email","Email"}
        };

        var SelectedOrder = AnsiConsole.Prompt(new SelectionPrompt<string>()
          .Title("Select the sorting style you want.")
          .PageSize(10)
          .AddChoices(["Latest", "Name", "Number", "Email"]));

        SortOrder(OrderDict[SelectedOrder]);
        AnsiConsole.MarkupLineInterpolated($"[yellow]{SelectedOrder}[/] sorting style will be used from now on.");

        return 0;
      }
      catch { throw; }
    }
  }
  #endregion
}
