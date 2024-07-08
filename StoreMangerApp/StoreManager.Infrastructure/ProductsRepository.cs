using Serilog;
using StoreManager.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Text;

namespace StoreManager.Infrastructure
{
  public class ProductsRepository : IProductsRepository
  {
    private readonly IDbConnection _connection;

    public ProductsRepository(IDbConnection connection)
    {
      _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task<IEnumerable<Product>> List()
    {
      try
      {
        return await _connection.QueryAsync<Product>("product_list", commandType: CommandType.StoredProcedure);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error fetching products");
        throw;
      }
    }

    public async Task<Product> FindById(int id)
    {
      try
      {
        var products = await _connection.QueryAsync<Product>("product_findbyid", new { id },
            commandType: CommandType.StoredProcedure);
        return products.FirstOrDefault();
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error finding product by ID: {Id}", id);
        throw;
      }
    }

    public async Task<int> Add(Product product)
    {
      try
      {
        var parameters = new DynamicParameters();
        parameters.Add("@id", product.Id);
        parameters.Add("@name", product.Name);
        parameters.Add("@weightedItem", product.WeightedItem);
        parameters.Add("@suggestedSellingPrice", product.SuggestedSellingPrice);

        return await _connection.ExecuteAsync("product_add", parameters, commandType: CommandType.StoredProcedure);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error adding product");
        throw;
      }
    }

    public async Task Edit(Product updatedProduct)
    {
      try
      {
        var parameters = new
        {
          id = updatedProduct.Id,
          name = updatedProduct.Name,
          weightedItem = updatedProduct.WeightedItem,
          suggestedSellingPrice = updatedProduct.SuggestedSellingPrice
        };

        await _connection.ExecuteAsync("product_edit", parameters, commandType: CommandType.StoredProcedure);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error editing product with ID: {Id}", updatedProduct.Id);
        throw;
      }
    }

    public async Task Delete(Product product)
    {
      try
      {
        await _connection.ExecuteAsync("product_delete", new { id = product.Id },
            commandType: CommandType.StoredProcedure);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error deleting product with ID: {Id}", product.Id);
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

          var products = ParseFileContent(fileContent);
          Log.Information("{ProductCount} products parsed from file", products.Count);

          var dataTable = ConvertToDataTable(products);
          var parameters = new DynamicParameters();
          parameters.Add("@Products", dataTable.AsTableValuedParameter("ProductType"));

          await _connection.ExecuteAsync("product_file_insert", parameters, commandType: CommandType.StoredProcedure);
          Log.Information("Inserted {ProductCount} products into the database", products.Count);

          return true;
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error processing uploaded file");
        throw;
      }
    }

    private List<Product> ParseFileContent(string fileContent)
    {
      var products = new List<Product>();
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
          var product = new Product
          {
            Id = int.Parse(values[0]),
            Name = values[1],
            WeightedItem = string.IsNullOrEmpty(values[2]) ? (bool?)null : bool.Parse(values[2]),
            SuggestedSellingPrice = string.IsNullOrEmpty(values[3]) || values[3].ToUpper() == "NULL" ? (decimal?)null : decimal.Parse(values[3])
          };

          products.Add(product);
          Log.Information("Parsed product: {Product}", product);
        }
        catch (FormatException ex)
        {
          Log.Error(ex, "Error parsing line: {LineContent}", line);
        }
      }

      return products;
    }

    private DataTable ConvertToDataTable(List<Product> products)
    {
      var dataTable = new DataTable();

      dataTable.Columns.Add("Id", typeof(int));
      dataTable.Columns.Add("Name", typeof(string));
      dataTable.Columns.Add("WeightedItem", typeof(bool));
      dataTable.Columns.Add("SuggestedSellingPrice", typeof(decimal));

      foreach (var product in products)
      {
        var row = dataTable.NewRow();

        row["Id"] = product.Id;
        row["Name"] = product.Name;
        row["WeightedItem"] = product.WeightedItem.HasValue ? (object)product.WeightedItem.Value : DBNull.Value;
        row["SuggestedSellingPrice"] = product.SuggestedSellingPrice.HasValue ? (object)product.SuggestedSellingPrice.Value : DBNull.Value;

        dataTable.Rows.Add(row);
      }

      return dataTable;
    }
  }
}