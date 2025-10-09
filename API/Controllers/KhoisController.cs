using Microsoft.AspNetCore.Mvc;
using API.Domain;
using API.Handlers.KhoiListing;

namespace API.Controllers;

public class KhoisController : BaseController
{
    // GET: api/khoi
    [HttpPost("listing")]
    public async Task<ActionResult<IEnumerable<Khoi>>> GetKhois()
    {
        var khois = await Mediator.Send(new Listing.ListKhoisQuery());
        return Ok(khois);
    }

}

