using DAL.Models;

namespace DAL.Api;

public interface IInvoiceDal
{
    List<Invoice> SearchInvoices(int? propertyId, string? supplierName, DateTime? fromDate, DateTime? toDate, int userId);
    string? GetInvoiceFilePath(int invoiceId, int userId);
}
