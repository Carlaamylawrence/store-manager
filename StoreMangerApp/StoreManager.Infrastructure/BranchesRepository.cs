using Dapper;
using StoreManager.Domain;
using System.Data;

namespace StoreManager.Infrastructure
{
  public class BranchesRepository : IBranchesRepository
  {
    private readonly IDbConnection _connection;
    public BranchesRepository(IDbConnection connection)
    {
      _connection = connection;
    }

    public async Task<IEnumerable<Branch>> List()
    {
      return await _connection.QueryAsync<Branch>("branch_list", commandType: CommandType.StoredProcedure);
    }

    public async Task<Branch> FindById(int id)
    {
      var branches = await _connection.QueryAsync<Branch>("branch_findbyid", new { id },
     commandType: CommandType.StoredProcedure);
      return branches.FirstOrDefault();
    }


    public async Task<int> Add(Branch branch)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@id", branch.Id);
      parameters.Add("@name", branch.Name);
      parameters.Add("@telephoneNumber", branch.TelephoneNumber);
      parameters.Add("@openDate", branch.OpenDate);

      var affectedRows = await _connection.ExecuteAsync("branch_add", parameters, commandType: CommandType.StoredProcedure);

      return affectedRows;
    }

    public async Task Edit(Branch updatedBranch)
    {
      var parameters = new
      {
        id = updatedBranch.Id,
        name = updatedBranch.Name,
        telephoneNumber = updatedBranch.TelephoneNumber,
        openDate = updatedBranch.OpenDate

      };
      await _connection.ExecuteAsync("branch_edit", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task Delete(Branch branch)
    {
      await _connection.ExecuteAsync("branch_delete", new { id = branch.Id },
        commandType: CommandType.StoredProcedure);
    }
  }
}
