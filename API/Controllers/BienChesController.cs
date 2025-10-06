using System;
using API.Domain;
using API.Handlers.BienCheListing;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

    public class BienChesController: BaseController
    {
        
    // GET: api/
   [HttpPost("listing")]
public async Task<ActionResult> GetBienChes([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
{
    var result = await Mediator.Send(new Listing.ListBienChesQuery { Page = page, PageSize = pageSize });
    return Ok(result);
}


    // GET: api/
    [HttpGet("{id}")]
    public async Task<ActionResult<BienChe>> GetBienChe(Guid id)
    {
        var bienche = await Mediator.Send(new GetById.GetBienCheByIdQuery { Id = id });

        if (bienche == null)
        {
            return NotFound();
        }

        return Ok(bienche);
    }

    // POST: api/
    [HttpPost]
    public async Task<ActionResult<BienChe>> CreateBienChe(Create.CreateBienCheCommand command)
    {
        try
        {
            return await Mediator.Send(command);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBienChe(Guid id, Update.UpdateBienCheCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await Mediator.Send(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBienChe(Guid id)
    {
        var deleted = await Mediator.Send(new Delete.DeleteBiencheCommand { Id = id });
        
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
    }

