using Dapper;
using StoreManager.Domain;
using System.Data;

namespace StoreManager.Infrastructure
{
  public class BranchProductRepository : IBranchProductRepository
  {
    private readonly IDbConnection _connection;

    public BranchProductRepository(IDbConnection connection)
    {
      _connection = connection;
    }

    public async Task<IEnumerable<BranchProduct>> List()
    {
      return await _connection.QueryAsync<BranchProduct>("branch_product_list", commandType: CommandType.StoredProcedure);
    }

    public async Task<int> Add(BranchProduct branchProduct)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@BranchName", branchProduct.BranchName);
      parameters.Add("@ProductName", branchProduct.ProductName);

      var affectedRows = await _connection.ExecuteAsync("branch_product_assign", parameters, commandType: CommandType.StoredProcedure);

      return affectedRows;
    }
  }
}
