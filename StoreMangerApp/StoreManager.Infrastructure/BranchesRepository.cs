using Serilog;
using StoreManager.Domain;
using System.Data;
using Dapper;

namespace StoreManager.Infrastructure
{
  public class BranchesRepository : IBranchesRepository
  {
    private readonly IDbConnection _connection;

    public BranchesRepository(IDbConnection connection)
    {
      _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task<IEnumerable<Branch>> List()
    {
      try
      {
        return await _connection.QueryAsync<Branch>("branch_list", commandType: CommandType.StoredProcedure);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error fetching branches");
        throw;
      }
    }

    public async Task<Branch> FindById(int id)
    {
      try
      {
        var branches = await _connection.QueryAsync<Branch>("branch_findbyid", new { id },
            commandType: CommandType.StoredProcedure);
        return branches.FirstOrDefault();
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error finding branch by ID: {Id}", id);
        throw;
      }
    }

    public async Task<int> Add(Branch branch)
    {
      try
      {
        var parameters = new DynamicParameters();
        parameters.Add("@id", branch.Id);
        parameters.Add("@name", branch.Name);
        parameters.Add("@telephoneNumber", branch.TelephoneNumber);
        parameters.Add("@openDate", branch.OpenDate);

        return await _connection.ExecuteAsync("branch_add", parameters, commandType: CommandType.StoredProcedure);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error adding branch");
        throw;
      }
    }

    public async Task Edit(Branch updatedBranch)
    {
      try
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
      catch (Exception ex)
      {
        Log.Error(ex, "Error editing branch with ID: {Id}", updatedBranch.Id);
        throw;
      }
    }

    public async Task Delete(Branch branch)
    {
      try
      {
        await _connection.ExecuteAsync("branch_delete", new { id = branch.Id },
            commandType: CommandType.StoredProcedure);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error deleting branch with ID: {Id}", branch.Id);
        throw;
      }
    }
  }
}
