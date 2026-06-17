using BL.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceBl _invoiceBl;
    private readonly IPropertyBl _propertyBl;

    public InvoiceController(IInvoiceBl invoiceBl, IPropertyBl propertyBl)
    {
        _invoiceBl = invoiceBl;
        _propertyBl = propertyBl;
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] int? propertyId, [FromQuery] string? supplierName,
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        var results = _invoiceBl.SearchInvoices(propertyId, supplierName, fromDate, toDate, userId);
        return Ok(results);
    }

    [HttpGet("my-properties")]
    public IActionResult GetMyProperties()
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        return Ok(_propertyBl.GetPropertiesByUser(userId));
    }

    [HttpGet("{id}/pdf")]
    public IActionResult GetPdf(int id)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        var filePath = _invoiceBl.GetInvoiceFilePath(id, userId);
        if (filePath == null || !System.IO.File.Exists(filePath))
            return NotFound();
        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/pdf");
    }
}
