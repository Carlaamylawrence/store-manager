using Serilog;
using StoreManager.Domain;
using System.Data;
using Dapper;

namespace StoreManager.Infrastructure
{
  public class BranchProductRepository : IBranchProductRepository
  {
    private readonly IDbConnection _connection;

    public BranchProductRepository(IDbConnection connection)
    {
      _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task<IEnumerable<BranchProduct>> List()
    {
      try
      {
        return await _connection.QueryAsync<BranchProduct>("branch_product_list", commandType: CommandType.StoredProcedure);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error fetching branch products");
        throw;
      }
    }

    public async Task<int> Add(BranchProduct branchProduct)
    {
      try
      {
        var parameters = new DynamicParameters();
        parameters.Add("@BranchName", branchProduct.BranchName);
        parameters.Add("@ProductName", branchProduct.ProductName);

        return await _connection.ExecuteAsync("branch_product_assign", parameters, commandType: CommandType.StoredProcedure);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error adding branch product");
        throw;
      }
    }

    public async Task<BranchProduct> FindById(int id)
    {
      try
      {
        var branchProducts = await _connection.QueryAsync<BranchProduct>("branch_product_findbyid", new { id },
            commandType: CommandType.StoredProcedure);
        return branchProducts.FirstOrDefault();
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error finding branch product by ID: {Id}", id);
        throw;
      }
    }

    public async Task Delete(BranchProduct branchProduct)
    {
      try
      {
        await _connection.ExecuteAsync("branch_product_delete", new { id = branchProduct.Id },
            commandType: CommandType.StoredProcedure);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error deleting branch product");
        throw;
      }
    }
  }
}
