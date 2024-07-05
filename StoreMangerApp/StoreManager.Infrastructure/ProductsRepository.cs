using Serilog;
using StoreManager.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

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
        using (var reader = new StreamReader(fileStream))
        {
          string fileContent = await reader.ReadToEndAsync();
          var products = ParseFileContent(fileContent);
          var dataTable = ConvertToDataTable(products);

          var parameters = new DynamicParameters();
          parameters.Add("@products", dataTable.AsTableValuedParameter("ProductType"));

          await _connection.ExecuteAsync("product_bulk_insert", parameters, commandType: CommandType.StoredProcedure);
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
      var lines = fileContent.Split('\n');
      foreach (var line in lines)
      {
        var values = line.Split(',');
        if (values.Length == 4)
        {
          products.Add(new Product
          {
            Id = int.Parse(values[0]),
            Name = values[1],
            WeightedItem = bool.Parse(values[2]),
            SuggestedSellingPrice = decimal.Parse(values[3])
          });
        }
      }
      return products;
    }

    private DataTable ConvertToDataTable(List<Product> products)
    {
      var table = new DataTable();
      table.Columns.Add("Id", typeof(int));
      table.Columns.Add("Name", typeof(string));
      table.Columns.Add("WeightedItem", typeof(bool));
      table.Columns.Add("SuggestedSellingPrice", typeof(decimal));

      foreach (var product in products)
      {
        table.Rows.Add(product.Id, product.Name, product.WeightedItem, product.SuggestedSellingPrice);
      }

      return table;
    }

  }
}