﻿using StoreManager.Domain;
using StoreManager.Infrastructure;

namespace StoreManager.ApplicationService
{
  public class BranchProductApplicationService
  {

    private readonly IBranchProductRepository _branchProductRepository;

    public BranchProductApplicationService(IBranchProductRepository branchProductRepository)
    {
      _branchProductRepository = branchProductRepository;
    }

    public async Task<IEnumerable<BranchProduct>> List()
    {
      return await _branchProductRepository.List();
    }

    public async Task<(int id, bool isValid, List<string> messages)> Add(BranchProduct branchProduct)
    {
      var valid = branchProduct.BranchProductIsValid();
      if (valid.isValid)
      {
        
        var id = await _branchProductRepository.Add(branchProduct);
        return (id, true, valid.messages);
      }
      return (0, false, valid.messages);
    }
  }
}
