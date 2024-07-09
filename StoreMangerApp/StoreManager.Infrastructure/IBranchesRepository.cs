using StoreManager.Domain;

namespace StoreManager.Infrastructure
{
  public interface IBranchesRepository
  {
    Task<IEnumerable<Branch>> List();
    Task<Branch> FindById(int id);
    Task<int> Add(Branch branch);
    Task<bool> AddFile(Stream fileStream);
    Task Edit(Branch updatedBranch);
    Task Delete(Branch branch);
  }
}
