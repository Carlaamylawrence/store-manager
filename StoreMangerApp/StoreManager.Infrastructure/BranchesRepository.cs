using Serilog;
using StoreManager.Domain;
using System.Data;
using Dapper;
using System.Text;

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

    public async Task<bool> AddFile(Stream fileStream)
    {
      try
      {
        using (var reader = new StreamReader(fileStream, Encoding.UTF8))
        {
          string fileContent = await reader.ReadToEndAsync();
          Log.Information("File content read successfully");

          var branches = ParseFileContent(fileContent);
          Log.Information("{BranchCount} branches parsed from file", branches.Count);

          var dataTable = ConvertToDataTable(branches);
          var parameters = new DynamicParameters();
          parameters.Add("@Branches", dataTable.AsTableValuedParameter("BranchType"));

          await _connection.ExecuteAsync("branch_file_insert", parameters, commandType: CommandType.StoredProcedure);
          Log.Information("Inserted {BranchCount} branches into the database", branches.Count);

          return true;
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error processing uploaded file");
        throw;
      }
    }

    private List<Branch> ParseFileContent(string fileContent)
    {
      var branches = new List<Branch>();
      var lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

      foreach (var line in lines)
      {
        var values = line.Split(',');

        if (values.Length != 4)
        {
          Log.Warning("Line does not contain exactly 4 values: {LineContent}", line);
          continue;
        }

        try
        {
          var branch = new Branch
          {
            Id = int.Parse(values[0]),
            Name = values[1],
            TelephoneNumber = string.IsNullOrEmpty(values[2]) || values[2].ToUpper() == "NULL" ? null : values[2],
            OpenDate = string.IsNullOrEmpty(values[3]) || values[3].ToUpper() == "NULL" ? (DateTime?)null : DateTime.TryParse(values[3], out var openDate) ? (DateTime?)openDate : null
          };

          branches.Add(branch);
          Log.Information("Parsed branch: {Branch}", branch);
        }
        catch (FormatException ex)
        {
          Log.Error(ex, "Error parsing line: {LineContent}", line);
        }
      }

      return branches;
    }

    private DataTable ConvertToDataTable(List<Branch> branches)
    {
      var dataTable = new DataTable();

      dataTable.Columns.Add("Id", typeof(int));
      dataTable.Columns.Add("Name", typeof(string));
      dataTable.Columns.Add("TelephoneNumber", typeof(string));
      dataTable.Columns.Add("OpenDate", typeof(DateTime));

      foreach (var branch in branches)
      {
        var row = dataTable.NewRow();

        row["Id"] = branch.Id;
        row["Name"] = branch.Name;
        row["TelephoneNumber"] = string.IsNullOrEmpty(branch.TelephoneNumber) ? (object)DBNull.Value : branch.TelephoneNumber;
        row["OpenDate"] = branch.OpenDate.HasValue ? (object)branch.OpenDate.Value : DBNull.Value;

        dataTable.Rows.Add(row);
      }

      return dataTable;
    }




  }
}
