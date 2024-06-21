using StoreManager.Domain;

namespace StoreManager.Infrastructure
{
  public interface IBranchProductRepository
  {
    Task<IEnumerable<BranchProduct>> List();
    Task<int> Add(BranchProduct branchProduct);
  }
}
