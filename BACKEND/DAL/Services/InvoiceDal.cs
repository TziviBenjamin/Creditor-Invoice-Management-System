using DAL.Api;
using DAL.Models;
using System.Data.SqlClient;

namespace DAL.Services;

public class InvoiceDal : IInvoiceDal
{
    private readonly string _connectionString;

    public InvoiceDal(string connectionString) => _connectionString = connectionString;

    public List<Invoice> SearchInvoices(int? propertyId, string? supplierName, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var invoices = new List<Invoice>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var sql = """
            SELECT i.Id, i.DDBeleg_RecordID, i.LiegNR, p.PropertyName,
                   i.Beleg_ID, i.Beleg_Datum, i.Beleg_DatumFormat,
                   i.KrediNR, i.Kreditor, i.Betrag, i.RG_Betrag,
                   i.B_User, i.B_Datum, i.B_Valuta, i.Bezahlt,
                   i.FilePath, i.KatNR, i.Kat
            FROM CreditorInvoices i
            JOIN Properties p ON i.LiegNR = p.PropertyID
            JOIN UserProperties up ON up.PropertyID = i.LiegNR
            WHERE up.UserID = @UserId
              AND (@PropertyId IS NULL OR i.LiegNR = @PropertyId)
              AND (@SupplierName IS NULL OR i.Kreditor LIKE '%' + @SupplierName + '%')
              AND (@FromDate IS NULL OR i.Beleg_DatumFormat >= @FromDate)
              AND (@ToDate IS NULL OR i.Beleg_DatumFormat <= @ToDate)
            ORDER BY i.Beleg_DatumFormat DESC
            """;

        var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@PropertyId", (object?)propertyId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@SupplierName", string.IsNullOrEmpty(supplierName) ? DBNull.Value : supplierName);
        cmd.Parameters.AddWithValue("@FromDate", (object?)fromDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ToDate", (object?)toDate ?? DBNull.Value);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            invoices.Add(new Invoice
            {
                Id = (int)reader["Id"],
                DDBeleg_RecordID = reader["DDBeleg_RecordID"] as int?,
                LiegNR = (int)reader["LiegNR"],
                PropertyName = (string)reader["PropertyName"],
                Beleg_ID = reader["Beleg_ID"] as int?,
                Beleg_Datum = reader["Beleg_Datum"] as int?,
                Beleg_DatumFormat = reader["Beleg_DatumFormat"] as DateTime?,
                KrediNR = reader["KrediNR"] as int?,
                Kreditor = reader["Kreditor"] as string,
                Betrag = reader["Betrag"] as decimal?,
                RG_Betrag = reader["RG_Betrag"] as decimal?,
                B_User = reader["B_User"] as string,
                B_Datum = reader["B_Datum"] as DateTime?,
                B_Valuta = reader["B_Valuta"] as DateTime?,
                Bezahlt = reader["Bezahlt"] as int?,
                FilePath = reader["FilePath"] as string,
                KatNR = reader["KatNR"] as int?,
                Kat = reader["Kat"] as string
            });

        return invoices;
    }

    public string? GetInvoiceFilePath(int invoiceId, int userId)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var cmd = new SqlCommand(
            "SELECT i.FilePath FROM CreditorInvoices i " +
            "JOIN UserProperties up ON up.PropertyID = i.LiegNR " +
            "WHERE i.Id = @InvoiceId AND up.UserID = @UserId",
            connection);
        cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
        cmd.Parameters.AddWithValue("@UserId", userId);
        var result = cmd.ExecuteScalar();
        return result as string;
    }
}
