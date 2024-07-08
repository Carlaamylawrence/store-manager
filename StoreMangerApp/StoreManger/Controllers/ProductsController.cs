using Microsoft.AspNetCore.Mvc;
using Serilog;
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

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
      if (file == null || file.Length == 0)
      {
        return BadRequest("No file uploaded");
      }

      try
      {
        using (var stream = file.OpenReadStream())
        {
          var success = await _productsApplicationService.AddFile(stream);
          if (success)
          {
            return Created("File uploaded successfully", new { message = "File uploaded successfully" });
          }
          else
          {
            return StatusCode(500, "An error occurred while processing the file");
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error uploading file");
        return StatusCode(500, "Internal server error");
      }
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
