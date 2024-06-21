using Microsoft.AspNetCore.Mvc;
using StoreManager.ApplicationService;
using StoreManager.Domain;
using System.Net;

namespace StoreManger.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    private readonly ProductsApplicationService _productsApplicationService;
    public ProductsController(ProductsApplicationService productsApplicationService)
    {
      _productsApplicationService = productsApplicationService;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var result = await _productsApplicationService.List();
      return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var result = await _productsApplicationService.Find(id);
      return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Product product)
    {
      var result = await _productsApplicationService.Add(product);
      if (!result.isValid)
      {
        return BadRequest(result);
      }
      return new ObjectResult("") { StatusCode = (int)HttpStatusCode.Created };
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] Product product)
    {
      var result = await _productsApplicationService.Edit(product);
      if (!result.isValid)
      {
        return BadRequest(result.messages);
      }
      return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _productsApplicationService.Delete(id);
      if (!result)
        return BadRequest("Unable to delete Product");
      return Ok();
    }
  }
}
