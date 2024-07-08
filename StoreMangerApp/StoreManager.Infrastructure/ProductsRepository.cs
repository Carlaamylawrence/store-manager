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

          foreach (var product in products)
          {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", product.Id);
            parameters.Add("@Name", product.Name);
            parameters.Add("@WeightedItem", product.WeightedItem);
            parameters.Add("@SuggestedSellingPrice", product.SuggestedSellingPrice);

            await _connection.ExecuteAsync("product_bulk_insert", parameters, commandType: CommandType.StoredProcedure);
            Log.Information("Inserted product: {Product}", product);
          }

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

      for (int i = 0; i < lines.Length; i++)
      {
        var line = lines[i].Trim();
        if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines

        var values = line.Split(',');
        if (values.Length != 4)
        {
          Log.Warning("Line {LineNumber} does not contain exactly 4 values: {LineContent}", i + 1, line);
          continue;
        }

        try
        {
          var product = new Product
          {
            Id = int.Parse(values[0]),
            Name = values[1],
            WeightedItem = bool.Parse(values[2]),
            SuggestedSellingPrice = decimal.Parse(values[3])
          };
          products.Add(product);
          Log.Information("Parsed product: {Product}", product);
        }
        catch (FormatException ex)
        {
          Log.Error(ex, "Error parsing line {LineNumber}: {LineContent}", i + 1, line);
        }
      }

      return products;
    }

  }
}