using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Contacts_CLI;
partial class Program
{
  #region  Add
  public class AddContactSettings : CommandSettings
  {
    [CommandArgument(0, "<name>")]
    public required string Name { get; set; }

    [CommandArgument(1, "<number>")]
    public required string PhoneNumber { get; set; }

    [CommandOption("-e|--email")]
    [DefaultValue("-")]
    public required string Email { get; set; }

    [CommandOption("-n|--note")]
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
  public class ListContactsSettings : CommandSettings
  {
    [CommandOption("-o|--offset")]
    [DefaultValue(0)]
    public int OFFSET { get; set; }
  }
  public class ListContactsCommand : Command<ListContactsSettings>
  {
    public override int Execute(CommandContext context, ListContactsSettings settings)
    {
      try
      {
        DisplayTable(settings.OFFSET);
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

    [CommandOption("-n|--name")]
    public string? Name { get; set; }

    [CommandOption("-p|--phone <phone-number>")]
    public string? PhoneNumber { get; set; }

    [CommandOption("-e|--email")]
    public string? Email { get; set; }

    [CommandOption("-s|--note")]
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
  public class SortingSettings : CommandSettings
  {
    [CommandOption("-o|--order")]
    [DefaultValue("-")]
    public string? AscDesc { get; set; }

    [CommandOption("-r|--row-separator")]
    [DefaultValue(false)]
    public bool SeparateRows { get; set; }
  }
  public class SortingCommand : Command<SortingSettings>
  {
    public override int Execute(CommandContext context, SortingSettings settings)
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

        UpdateSortOrder(OrderDict[SelectedOrder]);
        AnsiConsole.MarkupLineInterpolated($"[yellow]{SelectedOrder}[/] sorting style will be used from now on.");

        Dictionary<string, string> OrderDict2 = new(){
          {"Ascending","ASC"},
          {"Descending","DESC"},
        };
        var order = AnsiConsole.Prompt(new SelectionPrompt<string>()
                  .Title("Select the order you want.")
                  .PageSize(10)
                  .AddChoices(["Ascending", "Descending"]));
        UpdateAscendingOrder(OrderDict2[order]);
        AnsiConsole.MarkupLineInterpolated($"[yellow]{order}[/] order will be used from now on.");

        return 0;
      }
      catch { throw; }
    }
  }
  #endregion

  #region Random Names
  public class RandomContacts : Command
  {
    public override int Execute(CommandContext context)
    {
      try
      {
        int smallestNum = int.MaxValue;
        List<List<string>> arr =
        [
        GenerateRandomNames(),
        GeneratePhoneNumbers(),
        GenerateEmails(),
        GenerateDescriptions()
        ];
        foreach (var item in arr)
        {
          if (item.Count < smallestNum)
            smallestNum = item.Count;
        }
        int total = new();

        for (int i = 0; i < 50; i++)
        {
          total += smallestNum;
          for (int j = 0; j < smallestNum; j++)
          {
            AddContactsQuery(
              arr[0][j],
              arr[1][j],
              arr[2][j],
              arr[3][j]
              );
          }
        }
        AnsiConsole.MarkupLineInterpolated($"Added [green]{total}[/] contacts.");
        return 0;
      }
      catch { throw; }
    }
  }

  public static List<string> GenerateRandomNames()
  {
    List<string> firstNames =
    [
        // Fantasy Names
        "Aerion", "Lyra", "Zephyra", "Kaelen", "Elowen", "Thaldir", "Seraphina", "Orion", "Nyxandra", "Valerius",
            "Elora", "Gideon", "Isolde", "Caspian", "Anya", "Ragnar", "Freya", "Torvin", "Lilith", "Xander",
            "Alistair", "Rowan", "Saoirse", "Finnian", "Maeve", "Declan", "Niamh", "Cian", "Aisling", "Lorcan",
            "Siobhan", "Padraig", "Grainne", "Faelan", "Riona", "Tadhg", "Clodagh", "Ronan", "Briallen", "Calliope",
            "Lysander", "Aramis", "Evadne", "Oberon", "Titania", "Puck", "Ariel", "Caliban", "Miranda", "Prospero",
            "Gandalf", "Aragorn", "Legolas", "Gimli", "Frodo", "Samwise", "Merry", "Pippin", "Bilbo", "Elrond",
            "Galadriel", "Arwen", "Eowyn", "Theoden", "Faramir", "Boromir", "Radagast", "Smaug", "Balrog",
            "Draco", "Hermione", "Ronan", "Severus", "Minerva", "Albus", "Sirius", "Remus", "Luna", "Neville",
            "Geralt", "Yennefer", "Triss", "Ciri", "Dandelion", "Vesemir", "Eredin", "Letho", "Emhyr", "Foltest",
            "Daenerys", "Jon Snow", "Tyrion", "Arya", "Sansa", "Jaime", "Cersei", "Khal Drogo", "Petyr", "Varys",
            // Cultural Names
            "Aaliyah", "Jaxon", "Mei", "Kenji", "Priya", "Arjun", "Fatima", "Omar", "Sofia", "Mateo",
            "Isabella", "Ethan", "Chloe", "Liam", "Ava", "Noah", "Olivia", "William", "Emma", "James",
            "Sophia", "Benjamin", "Mia", "Jacob", "Amelia", "Michael", "Charlotte", "Alexander", "Harper", "Elijah",
            "Abigail", "Daniel", "Emily", "David", "Madison", "Joseph", "Scarlett", "Samuel", "Victoria", "Anthony",
            "Elizabeth", "Joshua", "Grace", "Andrew", "Lily", "Christopher", "Riley", "Ryan", "Zoey", "Nicholas",
            "Natalie", "Tyler", "Hannah", "Brandon", "Sarah", "Austin", "Samantha", "Zachary", "Anna", "Dylan",
            "Mackenzie", "Caleb", "Kaylee", "Nathan", "Taylor", "Connor", "Brianna", "Justin", "Allison", "Hunter",
            "Sydney", "Logan", "Lauren", "Jordan", "Michelle", "Kyle", "Jessica", "Kevin", "Ashley", "Eric",
            "Nicole", "Adam", "Rachel", "Timothy", "Katherine", "Brian", "Olivia", "Scott", "Megan", "Jason",
            "Kimberly", "Jeffrey", "Stephanie", "Travis", "Chelsea", "Patrick", "Brittany", "Jesse", "Amanda",
            "Jose", "Maria", "Luis", "Sofia", "Carlos", "Valentina", "Javier", "Camila", "Andres", "Isabela",
            "Diego", "Ximena", "Sebastian", "Daniela", "Santiago", "Gabriela", "Juan", "Lucia", "Felipe", "Mariana",
            "Raul", "Valeria"
    ];

    List<string> lastNames = new List<string>
        {
            "Stormblade", "Nightwhisper", "Shadowwalker", "Swiftarrow", "Silvermoon", "Ironfist", "Stoneheart", "Firebrand", "Frostmourne", "Skywatcher",
            "Emberglow", "Thornwood", "Bloodstone", "Dawnbreaker", "Duskhaven", "Winterfell", "Summerisle", "Ravenclaw", "Goldenshield", "Whisperwind",
            "Blackwood", "Brightflame", "Silentstep", "Oakenshield", "Stormrider", "Hawkswood", "Lighthaven", "Shadowmere", "Ironforge", "Silverleaf",
            "Nightshade", "Frostfang", "Emberstrike", "Thornbush", "Bloodfist", "Dawnhaven", "Duskwood", "Winterthorn", "Summerfield", "Ravenwood",
            "Goldensun", "Whisperstream", "Blackthorn", "Brightstar", "Silenthill", "Oakheart", "Stormpeak", "Hawksridge", "Lightbringer", "Shadowstrike",
            "Ironhammer", "Silverbrook", "Nightsong", "Frostwhisper", "Emberfall", "Thornewood", "Bloodmoon", "Dawnlight", "Duskfire", "Wintershade",
            "Summerbreeze", "Ravenscroft", "Goldenleaf", "Whisperwillow", "Blackwater", "Brightstone", "Silentnight", "Oakridge", "Stormwatch", "Hawkshadow",
            "Lightfoot", "Shadowcaster", "Ironclad", "Silverglen", "Nightwing", "Frostbloom", "Emberwind", "Thornfield", "Bloodriver", "Dawnstar",
            "Duskstone", "Winterbourne", "Summergrove", "Ravenhill", "Goldenshadow", "Whisperglen", "Blackfire", "Brightmoon", "Silentgrove", "Oakwood",
            "Stormchaser", "Hawksglen", "Lightwhisper", "Shadowdancer", "Ironwill", "Silvermist", "Nightwalker", "Frostflower", "Embersky", "Thornhollow"
        };

    List<string> fullNames = [];
    Random rand = new();

    // Ensure we don't go out of bounds if the lists aren't the same length.
    int maxNames = Math.Min(100, Math.Min(firstNames.Count, lastNames.Count));

    for (int i = 0; i < maxNames; i++)
    {
      string firstName = firstNames[rand.Next(firstNames.Count)];
      string lastName = lastNames[rand.Next(lastNames.Count)];
      fullNames.Add($"{firstName} {lastName}");
    }
    return fullNames;
  }
  public static List<string> GeneratePhoneNumbers()
  {
    List<string> phoneNumbers =
        [
            "+1-555-123-4567", "+1-555-234-5678", "+1-555-345-6789", "+1-555-456-7890", "+1-555-567-8901",
            "+1-555-678-9012", "+1-555-789-0123", "+1-555-890-1234", "+1-555-901-2345", "+1-555-012-3456",
            "+1-555-123-5678", "+1-555-234-6789", "+1-555-345-7890", "+1-555-456-8901", "+1-555-567-9012",
            "+1-555-678-0123", "+1-555-789-1234", "+1-555-890-2345", "+1-555-901-3456", "+1-555-012-4567",
            "+1-555-135-7911", "+1-555-246-8022", "+1-555-357-9133", "+1-555-468-0244", "+1-555-579-1355",
            "+1-555-680-2466", "+1-555-791-3577", "+1-555-802-4688", "+1-555-913-5799", "+1-555-024-6800",
            "+1-555-147-2581", "+1-555-258-3692", "+1-555-369-4703", "+1-555-470-5814", "+1-555-581-6925",
            "+1-555-692-7036", "+1-555-703-8147", "+1-555-814-9258", "+1-555-925-0369", "+1-555-036-1470",
            "+1-555-111-2222", "+1-555-222-3333", "+1-555-333-4444", "+1-555-444-5555", "+1-555-555-6666",
            "+1-555-666-7777", "+1-555-777-8888", "+1-555-888-9999", "+1-555-999-0000", "+1-555-000-1111",
            "+44-1234-567890", "+44-2345-678901", "+44-3456-789012", "+44-4567-890123", "+44-5678-901234",
            "+44-6789-012345", "+44-7890-123456", "+44-8901-234567", "+44-9012-345678", "+44-0123-456789",
            "+33-1-23-45-67-89", "+33-2-34-56-78-90", "+33-3-45-67-89-01", "+33-4-56-78-90-12", "+33-5-67-89-01-23",
            "+33-6-78-90-12-34", "+33-7-89-01-23-45", "+33-8-90-12-34-56", "+33-9-01-23-45-67", "+33-0-12-34-56-78",
            "+81-3-1234-5678", "+81-3-2345-6789", "+81-3-3456-7890", "+81-3-4567-8901", "+81-3-5678-9012",
            "+81-3-6789-0123", "+81-3-7890-1234", "+81-3-8901-2345", "+81-3-9012-3456", "+81-3-0123-4567",
            "+86-10-12345678", "+86-20-23456789", "+86-21-34567890", "+86-22-45678901", "+86-23-56789012",
            "+86-24-67890123", "+86-25-78901234", "+86-26-89012345", "+86-27-90123456", "+86-28-01234567",
            "+91-9876543210", "+91-9765432109", "+91-9654321098", "+91-9543210987", "+91-9432109876",
            "+91-9321098765", "+91-9210987654", "+91-9109876543", "+91-9009876542", "+91-8909876541",
            "+52-55-1234-5678", "+52-55-2345-6789", "+52-55-3456-7890", "+52-55-4567-8901", "+52-55-5678-9012",
            "+52-55-6789-0123", "+52-55-7890-1234", "+52-55-8901-2345", "+52-55-9012-3456", "+52-55-0123-4567"
        ];
    return phoneNumbers;
  }
  public static List<string> GenerateEmails()
  {
    List<string> emails =
        [
            "aerion@example.com", "lyra@example.com", "zephyra@example.com", "kaelen@example.com", "elowen@example.com",
            "thaldir@example.com", "seraphina@example.com", "orion@example.com", "nyxandra@example.com", "valerius@example.com",
            "elora@example.com", "gideon@example.com", "isolde@example.com", "caspian@example.com", "anya@example.com",
            "ragnar@example.com", "freya@example.com", "torvin@example.com", "lilith@example.com", "xander@example.com",
            "alistair@example.com", "rowan@example.com", "saoirse@example.com", "finnian@example.com", "maeve@example.com",
            "declan@example.com", "niamh@example.com", "cian@example.com", "aisling@example.com", "lorcan@example.com",
            "siobhan@example.com", "padraig@example.com", "grainne@example.com", "faelan@example.com", "riona@example.com",
            "tadhg@example.com", "clodagh@example.com", "ronan@example.com", "briallen@example.com", "calliope@example.com",
            "lysander@example.com", "aramis@example.com", "evadne@example.com", "oberon@example.com", "titania@example.com",
            "puck@example.com", "ariel@example.com", "caliban@example.com", "miranda@example.com", "prospero@example.com",
            "gandalf@middleearth.com", "aragorn@middleearth.com", "legolas@middleearth.com", "gimli@middleearth.com", "frodo@middleearth.com",
            "samwise@middleearth.com", "merry@middleearth.com", "pippin@middleearth.com", "bilbo@middleearth.com", "elrond@middleearth.com",
            "galadriel@middleearth.com", "arwen@middleearth.com", "eowyn@middleearth.com", "theoden@middleearth.com", "faramir@middleearth.com",
            "boromir@middleearth.com", "radagast@middleearth.com", "smaug@middleearth.com", "balrog@middleearth.com", "draco@hogwarts.com",
            "hermione@hogwarts.com", "ronan@hogwarts.com", "severus@hogwarts.com", "minerva@hogwarts.com", "albus@hogwarts.com",
            "sirius@hogwarts.com", "remus@hogwarts.com", "luna@hogwarts.com", "neville@hogwarts.com", "geralt@witcher.com",
            "yennefer@witcher.com", "triss@witcher.com", "ciri@witcher.com", "dandelion@witcher.com", "vesemir@witcher.com",
            "eredin@witcher.com", "letho@witcher.com", "emhyr@witcher.com", "foltest@witcher.com", "daenerys@westeros.com",
            "jonsnow@westeros.com", "tyrion@westeros.com", "arya@westeros.com", "sansa@westeros.com", "jaime@westeros.com",
            "cersei@westeros.com", "khaldrogo@westeros.com", "petyr@westeros.com", "varys@westeros.com",  "aaliyah@example.com", "jaxon@example.com", "mei@example.com", "kenji@example.com", "priya@example.com", "arjun@example.com", "fatima@example.com", "omar@example.com", "sofia@example.com", "mateo@example.com",
            "isabella@example.com", "ethan@example.com", "chloe@example.com", "liam@example.com", "ava@example.com", "noah@example.com", "olivia@example.com", "william@example.com", "emma@example.com", "james@example.com",
            "sophia@example.com", "benjamin@example.com", "mia@example.com", "jacob@example.com", "amelia@example.com", "michael@example.com", "charlotte@example.com", "alexander@example.com", "harper@example.com", "elijah@example.com",
            "abigail@example.com", "daniel@example.com", "emily@example.com", "david@example.com", "madison@example.com", "joseph@example.com", "scarlett@example.com", "samuel@example.com", "victoria@example.com", "anthony@example.com",
            "elizabeth@example.com", "joshua@example.com", "grace@example.com", "andrew@example.com", "lily@example.com", "christopher@example.com", "riley@example.com", "ryan@example.com", "zoey@example.com", "nicholas@example.com",
            "natalie@example.com", "tyler@example.com", "hannah@example.com", "brandon@example.com", "sarah@example.com", "austin@example.com", "samantha@example.com", "zachary@example.com", "anna@example.com", "dylan@example.com",
            "mackenzie@example.com", "caleb@example.com", "kaylee@example.com", "nathan@example.com", "taylor@example.com", "connor@example.com", "brianna@example.com", "justin@example.com", "allison@example.com", "hunter@example.com",
            "sydney@example.com", "logan@example.com", "lauren@example.com", "jordan@example.com", "michelle@example.com", "kyle@example.com", "jessica@example.com", "kevin@example.com", "ashley@example.com", "eric@example.com",
            "nicole@example.com", "adam@example.com", "rachel@example.com", "timothy@example.com", "katherine@example.com", "brian@example.com", "olivia@example.com", "scott@example.com", "megan@example.com", "jason@example.com",
            "kimberly@example.com", "jeffrey@example.com", "stephanie@example.com", "travis@example.com", "chelsea@example.com", "patrick@example.com", "brittany@example.com", "jesse@example.com", "amanda@example.com", "jose@example.com",
            "maria@example.com", "luis@example.com", "sofia@example.com", "carlos@example.com", "valentina@example.com", "javier@example.com", "camila@example.com", "andres@example.com", "isabela@example.com", "diego@example.com",
            "ximena@example.com", "sebastian@example.com", "daniela@example.com", "santiago@example.com", "gabriela@example.com", "juan@example.com", "lucia@example.com", "felipe@example.com", "mariana@example.com", "raul@example.com", "valeria@example.com"
        ];
    return emails;
  }
  public static List<string> GenerateDescriptions()
  {
    List<string> descriptions = new List<string>
        {
            "A valiant warrior with a strong sense of justice.",
            "A skilled sorceress with a mysterious past.",
            "A swift and agile rogue, always one step ahead.",
            "A wise and powerful mage, master of the arcane arts.",
            "A graceful and nature-loving druid.",
            "A brave and honorable knight.",
            "A celestial being with a divine purpose.",
            "A skilled hunter and tracker.",
            "A dark and enigmatic sorceress.",
            "A noble and charismatic leader.",
            "A kind and gentle healer.",
            "A fierce and determined warrior.",
            "A beautiful and tragic queen.",
            "A brave and adventurous prince.",
            "A skilled and cunning thief.",
            "A powerful and fearsome viking warrior.",
            "A beautiful and powerful goddess.",
            "A strong and loyal dwarf warrior.",
            "A dark and seductive sorceress.",
            "A charming and roguish adventurer.",
            "A noble and honorable knight, defender of the realm.",
            "A skilled archer with a connection to the forest.",
            "A wise and enchanting Celtic sorceress.",
            "A brave and charismatic Celtic warrior.",
            "A powerful and alluring Celtic queen.",
             "A fierce and loyal Celtic warrior.",
            "A beautiful and enchanting Celtic princess.",
            "A skilled and daring Celtic hero.",
            "A mysterious and ethereal Celtic spirit.",
            "A wise and powerful Celtic druid.",
            "A strong-willed and independent Celtic warrior.",
            "A devout and courageous Celtic priest.",
            "A passionate and determined Celtic leader.",
            "A mysterious and elusive Celtic wanderer.",
            "A beautiful and spirited Celtic huntress.",
            "A skilled and resourceful Celtic craftsman.",
            "A wise and compassionate Celtic healer.",
            "A brave and noble Celtic king.",
            "A free-spirited and adventurous Celtic bard.",
            "The muse of epic poetry, inspiring great tales.",
            "A charismatic and eloquent leader with a silver tongue.",
            "A skilled swordsman and charming rogue, one of the Three Musketeers.",
            "A tragic queen from Greek mythology, known for her fierce pride.",
            "The powerful king of the fairies in Shakespeare's A Midsummer Night's Dream.",
            "The beautiful and proud queen of the fairies in Shakespeare's A Midsummer Night's Dream.",
            "A mischievous and playful fairy, also known as Robin Goodfellow.",
            "A delicate and airy spirit in Shakespeare's The Tempest.",
            "A wild and savage creature in Shakespeare's The Tempest.",
            "A beautiful and innocent young woman in Shakespeare's The Tempest.",
            "A powerful magician and the exiled Duke of Milan in Shakespeare's The Tempest.",
            "A wise and powerful wizard, guide to the Fellowship.",
            "The rightful heir to the throne of Gondor, a skilled warrior.",
            "A skilled elven archer and member of the Fellowship.",
            "A brave and loyal dwarf warrior and member of the Fellowship.",
            "The hobbit who bears the One Ring on a perilous quest.",
            "Frodo's loyal companion and gardener, steadfast and true.",
        };
    return descriptions;
  }

  #endregion

  #region Search
  public class SearchSettings : CommandSettings
  {
    [CommandArgument(0, "<SearchTerm>")]
    public required string SearchTerm { get; set; }
  }

  public class SearchCommand : Command<SearchSettings>
  {
    public override int Execute(CommandContext context, SearchSettings settings)
    {
      try
      {
        AnsiConsole.MarkupLine("[bold blue]WIP[/]");
        return 0;
      }
      catch { throw; }
    }
  }
  #endregion
}
