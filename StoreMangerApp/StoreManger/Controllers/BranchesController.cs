using Microsoft.AspNetCore.Mvc;
using StoreManager.ApplicationService;
using StoreManager.Domain;
using System.Net;

namespace StoreManger.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class BranchesController : ControllerBase
  {
    private readonly BranchesApplicationService _branchesApplicationService;

    public BranchesController(BranchesApplicationService branchesApplicationService)
    {
      _branchesApplicationService = branchesApplicationService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var result = await _branchesApplicationService.List();
      return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var result = await _branchesApplicationService.Find(id);
      return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Branch branch)
    {
      var result = await _branchesApplicationService.Add(branch);
      if (!result.isValid)
      {
        return BadRequest(result);
      }
      return new ObjectResult("") { StatusCode = (int)HttpStatusCode.Created };
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] Branch branch)
    {
      var result = await _branchesApplicationService.Edit(branch);
      if (!result.isValid)
      {
        return BadRequest(result.messages);
      }
      return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _branchesApplicationService.Delete(id);
      if (!result)
        return BadRequest("Unable to delete branch");
      return Ok();
    }
  }
}
