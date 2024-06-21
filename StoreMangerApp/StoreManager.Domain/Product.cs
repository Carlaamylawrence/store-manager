namespace StoreManager.Domain
{
  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool WeightedItem { get; set; }
    public decimal SuggestedSellingPrice { get; set; }

    public Product()
    {

    }

    public (bool isValid, List<string> messages) ProductIsValid()
    {
      var messages = new List<string>();
      bool isValid = true;

      if (string.IsNullOrWhiteSpace(Name) || Name.Length <= 1 || Name.Length >= 100)
      {
        isValid = false;
        messages.Add("Name should have a length between 1 and 100 characters");
      }
      return (isValid, messages);
    }

  }
}
