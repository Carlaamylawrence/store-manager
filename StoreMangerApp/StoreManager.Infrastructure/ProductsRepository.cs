using StoreManager.Domain;
using Dapper;
using System.Data;


namespace StoreManager.Infrastructure
{
  public class ProductsRepository : IProductsRepository
  {
    private readonly IDbConnection _connection;
    public ProductsRepository(IDbConnection connection)
    {
      _connection = connection;
    }

    public async Task<IEnumerable<Product>> List()
    {
      return await _connection.QueryAsync<Product>("product_list", commandType: CommandType.StoredProcedure);
    }

    public async Task<Product> FindById(int id)
    {
      var products = await _connection.QueryAsync<Product>("product_findbyid", new { id },
     commandType: CommandType.StoredProcedure);
      return products.FirstOrDefault();
    }

    public async Task<int> Add(Product product)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@id", product.Id);
      parameters.Add("@name", product.Name);
      parameters.Add("@weightedItem", product.WeightedItem);
      parameters.Add("@suggestedSellingPrice", product.SuggestedSellingPrice);

      var affectedRows = await _connection.ExecuteAsync("product_add", parameters, commandType: CommandType.StoredProcedure);

      return affectedRows;
    }

    public async Task Edit(Product updatedProduct)
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

    public async Task Delete(Product product)
    {
      await _connection.ExecuteAsync("product_delete", new { id = product.Id },
        commandType: CommandType.StoredProcedure);
    }

  }
}
