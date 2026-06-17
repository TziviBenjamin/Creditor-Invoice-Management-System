namespace DAL.Models;

public class Invoice
{
    public int Id { get; set; }
    public int? DDBeleg_RecordID { get; set; }
    public int LiegNR { get; set; }                        // PropertyID (FK)
    public string PropertyName { get; set; } = string.Empty; // joined from Properties
    public int? Beleg_ID { get; set; }
    public int? Beleg_Datum { get; set; }
    public DateTime? Beleg_DatumFormat { get; set; }
    public int? KrediNR { get; set; }
    public string? Kreditor { get; set; }                  // שם ספק
    public decimal? Betrag { get; set; }
    public decimal? RG_Betrag { get; set; }
    public string? B_User { get; set; }
    public DateTime? B_Datum { get; set; }
    public DateTime? B_Valuta { get; set; }
    public int? Bezahlt { get; set; }
    public string? FilePath { get; set; }                  // נתיב PDF
    public int? KatNR { get; set; }
    public string? Kat { get; set; }
}
