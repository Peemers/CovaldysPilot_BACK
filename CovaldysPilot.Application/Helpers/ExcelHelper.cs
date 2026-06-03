using ClosedXML.Excel;
using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Helpers;

public static class ExcelHelper
{
  public static byte[] GenerateMembersExcel(IEnumerable<User> users)
  {
    using XLWorkbook workbook = new XLWorkbook();
    IXLWorksheet worksheet = workbook.Worksheets.Add("Membres");

    // En-têtes
    string[] headers = ["Pseudo", "Prénom", "Nom", "Email", "Téléphone", "Statut", "Date naissance", "Membre depuis"];
    for (int i = 0; i < headers.Length; i++)
      worksheet.Cell(1, i + 1).Value = headers[i]; //ligne 1 colonnes 1 à 8 (excel commence à 1 pas 0)

    // Style en-têtes
    IXLRange headerRange = worksheet.Range(1, 1, 1, headers.Length);
    headerRange.Style.Font.Bold = true;
    headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#4CAF50");
    headerRange.Style.Font.FontColor = XLColor.White;

    // Données
    int row = 2;
    foreach (User user in users)
    {
      worksheet.Cell(row, 1).Value = user.Pseudo;
      worksheet.Cell(row, 2).Value = user.FirstName;
      worksheet.Cell(row, 3).Value = user.LastName ?? "";
      worksheet.Cell(row, 4).Value = user.Email;
      worksheet.Cell(row, 5).Value = user.PhoneNumber ?? "";
      worksheet.Cell(row, 6).Value = user.IsMembershipUpToDate ? "Effectif" : "Normal";
      worksheet.Cell(row, 7).Value = user.Birthday.ToString("dd/MM/yyyy");
      worksheet.Cell(row, 8).Value = user.CreatedAt.ToString("dd/MM/yyyy");
      row++;
    }

    worksheet.Columns().AdjustToContents();

    using MemoryStream stream = new MemoryStream();
    workbook.SaveAs(stream);
    return stream.ToArray();
  }
}