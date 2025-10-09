using Microsoft.AspNetCore.Mvc;
using API.Domain;
using API.Handlers.LinhVucListing;

namespace API.Controllers;

public class LinhVucsController : BaseController
{
    // GET: api/linhvuc
    [HttpPost("listing")]
    public async Task<ActionResult<IEnumerable<LinhVuc>>> GetLinhVucs()
    {
        var linhvucs = await Mediator.Send(new Listing.ListLinhVucsQuery());
        return Ok(linhvucs);
    }

}

