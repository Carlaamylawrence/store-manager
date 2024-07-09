using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StoreManager.Domain
{
  public class Branch
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string? TelephoneNumber { get; set; }
    public DateTime? OpenDate { get; set; }

    public (bool isValid, List<string> messages) BranchIsValid()
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
