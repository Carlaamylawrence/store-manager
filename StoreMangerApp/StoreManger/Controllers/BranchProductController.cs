using Microsoft.AspNetCore.Mvc;
using StoreManager.ApplicationService;
using StoreManager.Domain;
using System.Net;

namespace StoreManger.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class BranchProductController : ControllerBase
  {
    private readonly BranchProductApplicationService _branchProductApplicationService;
    public BranchProductController(BranchProductApplicationService branchProductApplicationService)
    {
      _branchProductApplicationService = branchProductApplicationService;
      ;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var result = await _branchProductApplicationService.List();
      return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BranchProduct branchProduct)
    {
      var result = await _branchProductApplicationService.Add(branchProduct);
      if (!result.isValid)
      {
        return BadRequest(result);
      }
      return new ObjectResult("") { StatusCode = (int)HttpStatusCode.Created };
    }
  }
}
