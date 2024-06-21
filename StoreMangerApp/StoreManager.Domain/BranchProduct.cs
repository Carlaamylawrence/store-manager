using System.Xml.Linq;

namespace StoreManager.Domain
{
  public class BranchProduct
  {
    public string BranchName { get; set; }
    public string ProductName { get; set; }

    public (bool isValid, List<string> messages) BranchProductIsValid()
    {
      var messages = new List<string>();
      bool isValid = true;

      if (string.IsNullOrWhiteSpace(BranchName) || BranchName.Length <= 1 || BranchName.Length >= 100)
      {
        isValid = false;
        messages.Add("BranchName should have a length between 1 and 100 characters");
      }
      if (string.IsNullOrWhiteSpace(ProductName) || ProductName.Length <= 1 || ProductName.Length >= 100)
      {
        isValid = false;
        messages.Add("ProductName should have a length between 1 and 100 characters");
      }
      return (isValid, messages);
    }
  }
}
