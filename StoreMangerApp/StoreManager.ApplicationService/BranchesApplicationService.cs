using StoreManager.Domain;
using StoreManager.Infrastructure;

namespace StoreManager.ApplicationService
{
  public class BranchesApplicationService
  {
    private readonly IBranchesRepository _branchesRepository;

    public BranchesApplicationService(IBranchesRepository branchesRepository)
    {
      _branchesRepository = branchesRepository;
    }

    public async Task<IEnumerable<Branch>> List()
    {
      return await _branchesRepository.List();
    }

    public async Task<Branch> Find(int id)
    {

      return await _branchesRepository.FindById(id);
    }


    public async Task<(int id, bool isValid, List<string> messages)> Add(Branch branch)
    {
      var valid = branch.BranchIsValid();
      if (valid.isValid)
      {
        var id = await _branchesRepository.Add(branch);
        return (id, true, valid.messages);
      }
      return (0, false, valid.messages);
    }

    public async Task<(bool isValid, List<string> messages)> Edit(Branch updatedbranch)
    {
      var valid = updatedbranch.BranchIsValid();

      if (valid.isValid)
      {
        await _branchesRepository.Edit(updatedbranch);
      }
      return valid;
    }

    public async Task<bool> Delete(int id)
    {
      var branch = await Find(id);

      if (branch != null)
      {
        await _branchesRepository.Delete(branch);
        return true;
      }
      return false;
    }
  }
}
