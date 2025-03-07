namespace CountryAPI.Entities;
public class Entities
{

  public class CountryDetails : Country
  {
    public int Population { get; set; }
    public string Capital { get; set; }
  }

  public class Country
  {
    public Name Name { get; set; }
    public int Population { get; set; }
    public string Region { get; set; }
    public List<string> Capital { get; set; }
    public Flags Flags { get; set; }
  }

  public class Name
  {
    public string Common { get; set; }
    public string Official { get; set; }

  }

  public class Flags
  {
    public string Png { get; set; }
    public string svg { get; set; }
  }

}