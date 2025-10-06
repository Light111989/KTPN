using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Domain;
using API.Handlers.Sales.Companies;

namespace API.Controllers;

[Authorize(Roles = "Sales")]
public class CompaniesController : BaseController
{
    // GET: api/companies
    [HttpPost("listing")]
    public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
    {
        var companies = await Mediator.Send(new Listing.ListCompaniesQuery());
        return Ok(companies);
    }

    // GET: api/companies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> GetCompany(Guid id)
    {
        var company = await Mediator.Send(new GetById.GetCompanyByIdQuery { Id = id });

        if (company == null)
        {
            return NotFound();
        }

        return Ok(company);
    }

    // POST: api/companies
    [HttpPost]
    public async Task<ActionResult<Company>> CreateCompany(Create.CreateCompanyCommand command)
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

    // PUT: api/companies/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCompany(Guid id, Update.UpdateCompanyCommand command)
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

    // DELETE: api/companies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        var deleted = await Mediator.Send(new Delete.DeleteCompanyCommand { Id = id });
        
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
} 