using BL.Api;
using DAL.Api;
using DAL.Models;

namespace BL.Services;

public class InvoiceBl : IInvoiceBl
{
    private readonly IInvoiceDal _invoiceDal;

    public InvoiceBl(IInvoiceDal invoiceDal) => _invoiceDal = invoiceDal;

    public List<Invoice> SearchInvoices(int? propertyId, string? supplierName, DateTime? fromDate, DateTime? toDate, int userId) =>
        _invoiceDal.SearchInvoices(propertyId, supplierName, fromDate, toDate, userId);

    public string? GetInvoiceFilePath(int invoiceId, int userId) =>
        _invoiceDal.GetInvoiceFilePath(invoiceId, userId);
}
