using StoreManager.Domain;
using StoreManager.Infrastructure;

namespace StoreManager.ApplicationService
{
  public class ProductsApplicationService
  {
    private readonly IProductsRepository _productsRepository;

    public ProductsApplicationService(IProductsRepository productsRepository)
    {
      _productsRepository = productsRepository;
    }

    public async Task<IEnumerable<Product>> List()
    {
      return await _productsRepository.List();
    }

    public async Task<Product> Find(int id)
    {

      return await _productsRepository.FindById(id);
    }

    public async Task<(int id, bool isValid, List<string> messages)> Add(Product product)
    {
      var valid = product.ProductIsValid();
      if (valid.isValid)
      {
        var id = await _productsRepository.Add(product);
        return (id, true, valid.messages);
      }
      return (0, false, valid.messages);
    }

    public async Task<(bool isValid, List<string> messages)> Edit(Product updatedProduct)
    {
      var valid = updatedProduct.ProductIsValid();

      if (valid.isValid)
      {
        await _productsRepository.Edit(updatedProduct);
      }
      return valid;
    }

    public async Task<bool> Delete(int id)
    {
      var product = await Find(id);

      if (product != null)
      {
        await _productsRepository.Delete(product);
        return true;
      }
      return false;
    }

  }
}
