using DAL.Models;

namespace BL.Api;

public interface IInvoiceBl
{
    List<Invoice> SearchInvoices(int? propertyId, string? supplierName, DateTime? fromDate, DateTime? toDate, int userId);
    string? GetInvoiceFilePath(int invoiceId, int userId);
}
