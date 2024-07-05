using StoreManager.Domain;

namespace StoreManager.Infrastructure
{
  public interface IProductsRepository
  {
    Task<IEnumerable<Product>> List();
    Task<Product> FindById(int id);
    Task<int> Add(Product product);
    Task<bool> AddFile(Stream fileStream);
    Task Edit(Product updatedProduct);
    Task Delete(Product product);

  }
}
