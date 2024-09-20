using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Input;
using Microsoft.Office.Interop.Word;
using ShopsGoodsLibrary.Models;
using System.IO;
using System.Runtime.InteropServices;

namespace ShopsGoodsProject.Windows.Administrator;

public partial class Admin : System.Windows.Window
{
    public Admin()
    {
        InitializeComponent();
        MainFrame.Navigate(new Pages.Shops());
        ShopsGoodsLibrary.Functional.Manager.MainFrame = MainFrame;
    }

    private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }

    private void ShopButton_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Shops());
    }

    private void BrandShop_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Brands());
    }

    private void TypeShop_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Types());
    }

    private void GoodsShop_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Goods());
    }

    private void GoodsReports_Click(object sender, RoutedEventArgs e)
    {
        int totalSoldGoods = ShopsGoodsContext.GetContext()
                                              .OrderGoods
                                              .Select(og => og.GoodId)
                                              .Distinct()
                                              .Count();

        var goodsSold = ShopsGoodsContext.GetContext()
                                         .OrderGoods
                                         .Include(og => og.Good)
                                         .ThenInclude(g => g.Brand)
                                         .GroupBy(og => new
                                         {
                                             og.Good.Brand.BrandName,
                                             og.Good.NameGood
                                         })
                                         .Select(g => new
                                         {
                                             g.Key.BrandName,
                                             g.Key.NameGood,
                                             SoldCount = g.Count()
                                         })
                                         .ToList();

        int totalSoldBrands = ShopsGoodsContext.GetContext()
                                               .OrderGoods
                                               .Include(og => og.Good)
                                               .Select(og => og.Good.BrandId)
                                               .Distinct()
                                               .Count();

        var brandsSold = ShopsGoodsContext.GetContext()
                                          .OrderGoods
                                          .Include(og => og.Good)
                                          .ThenInclude(g => g.Brand)
                                          .GroupBy(og => og.Good.Brand.BrandName)
                                          .Select(g => new
                                          {
                                              BrandName = g.Key,
                                              SoldCount = g.Count()
                                          })
                                          .ToList();

        int totalSoldTypes = ShopsGoodsContext.GetContext()
                                              .OrderGoods
                                              .Include(og => og.Good)
                                              .Select(og => og.Good.TypeId)
                                              .Distinct()
                                              .Count();

        var typesSold = ShopsGoodsContext.GetContext()
                                         .OrderGoods
                                         .Include(og => og.Good)
                                         .ThenInclude(g => g.Type)
                                         .GroupBy(og => og.Good.Type.TypeName)
                                         .Select(t => new
                                         {
                                             TypeName = t.Key,
                                             SoldCount = t.Count()
                                         })
                                         .ToList();
        if (MessageBox.Show("Вы уверены, что хотите сформировать отчет по магазину?",
                            "Внимание!",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            GenerateWordReport(
                totalSoldGoods,
                goodsSold,
                totalSoldBrands,
                brandsSold,
                totalSoldTypes,
                typesSold);
        }
    }

    private void GenerateWordReport(
        int totalSoldGoods,
        IEnumerable<dynamic> goodsSold,
        int totalSoldBrands,
        IEnumerable<dynamic> brandsSold,
        int totalSoldTypes,
        IEnumerable<dynamic> typesSold)
    {
        var wordApp = new Microsoft.Office.Interop.Word.Application();
        var doc = wordApp.Documents.Add();

        var mainParagraph = doc.Content.Paragraphs.Add();
        var rand = new Random();
        var storeNumber = rand.Next(100, 1000);
        var reportText = $"Отчет магазина №{storeNumber}";
        mainParagraph.Range.Text = reportText;
        mainParagraph.Range.Font.Name = "Comic Sans MS";
        mainParagraph.Range.Font.Size = 14;
        mainParagraph.Range.Font.Bold = 1;
        mainParagraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
        mainParagraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;
        mainParagraph.Format.LeftIndent = 0;
        mainParagraph.Format.RightIndent = 0;
        mainParagraph.Format.SpaceBefore = 0;
        mainParagraph.Format.SpaceAfter = 0;
        mainParagraph.Range.InsertParagraphAfter();

        var generalInfoParagraph = doc.Content.Paragraphs.Add();
        generalInfoParagraph.Range.Font.Name = "Comic Sans MS";
        generalInfoParagraph.Range.Font.Size = 14;
        generalInfoParagraph.Range.Text = "Всего товаров продано: ";
        generalInfoParagraph.Range.Font.Bold = 1;
        var rangeAfterBold = generalInfoParagraph.Range.Duplicate;
        rangeAfterBold.SetRange(generalInfoParagraph.Range.End, generalInfoParagraph.Range.End);
        rangeAfterBold.InsertAfter($"\t{totalSoldGoods} шт."); 
        rangeAfterBold.Font.Bold = 0; 
        generalInfoParagraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
        generalInfoParagraph.Format.LeftIndent = wordApp.CentimetersToPoints(1.25f);
        generalInfoParagraph.Format.RightIndent = 0;
        generalInfoParagraph.Format.SpaceBefore = 0;
        generalInfoParagraph.Format.SpaceAfter = 0;
        generalInfoParagraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;
        generalInfoParagraph.Range.InsertParagraphAfter();

        var goodsParagraph = doc.Content.Paragraphs.Add();
        goodsParagraph.Range.Font.Name = "Comic Sans MS";
        goodsParagraph.Range.Font.Size = 14;
        foreach (var good in goodsSold)
        {
            var range = goodsParagraph.Range;
            range.Text = "Товар: ";
            range.Font.Bold = 1;
            range.Collapse(WdCollapseDirection.wdCollapseEnd);
            range.InsertAfter($"«{good.BrandName} {good.NameGood}» - ");
            range.Font.Bold = 0; 
            var boldRange = range.Duplicate;
            boldRange.Collapse(WdCollapseDirection.wdCollapseEnd); 
            boldRange.Font.Bold = 1;
            boldRange.Collapse(WdCollapseDirection.wdCollapseEnd);
            boldRange.InsertAfter($"в количестве {good.SoldCount} шт.");
            boldRange.Font.Bold = 0;
            goodsParagraph.Range.InsertParagraphAfter();
        }
        goodsParagraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
        goodsParagraph.Format.LeftIndent = wordApp.CentimetersToPoints(1.25f);
        goodsParagraph.Format.RightIndent = 0;
        goodsParagraph.Format.SpaceBefore = 0;
        goodsParagraph.Format.SpaceAfter = 0;
        goodsParagraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;

        var brandsGeneralParagraph = doc.Content.Paragraphs.Add();
        brandsGeneralParagraph.Range.Font.Name = "Comic Sans MS";
        brandsGeneralParagraph.Range.Font.Size = 14;
        brandsGeneralParagraph.Range.Text = "Всего брендов продано: ";
        brandsGeneralParagraph.Range.Font.Bold = 1;
        var rangeAfterBoldBrand = brandsGeneralParagraph.Range.Duplicate;
        rangeAfterBoldBrand.SetRange(brandsGeneralParagraph.Range.End, brandsGeneralParagraph.Range.End);
        rangeAfterBoldBrand.InsertAfter($"\t{totalSoldBrands} шт.");
        rangeAfterBoldBrand.Font.Bold = 0;
        brandsGeneralParagraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
        brandsGeneralParagraph.Format.LeftIndent = wordApp.CentimetersToPoints(1.25f);
        brandsGeneralParagraph.Format.RightIndent = 0;
        brandsGeneralParagraph.Format.SpaceBefore = 0;
        brandsGeneralParagraph.Format.SpaceAfter = 0;
        brandsGeneralParagraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;
        brandsGeneralParagraph.Range.InsertParagraphAfter();

        var brandsParagraph = doc.Content.Paragraphs.Add();
        brandsParagraph.Range.Font.Name = "Comic Sans MS";
        brandsParagraph.Range.Font.Size = 14;
        foreach (var brands in brandsSold)
        {
            var range = brandsParagraph.Range;
            range.Text = "Бренд: ";
            range.Font.Bold = 1;
            range.Collapse(WdCollapseDirection.wdCollapseEnd);
            range.InsertAfter($"«{brands.BrandName}» - ");
            range.Font.Bold = 0;
            var boldRange = range.Duplicate;
            boldRange.Collapse(WdCollapseDirection.wdCollapseEnd);
            boldRange.Font.Bold = 1;
            boldRange.Collapse(WdCollapseDirection.wdCollapseEnd);
            boldRange.InsertAfter($"в количестве {brands.SoldCount} шт.");
            boldRange.Font.Bold = 0;
            brandsParagraph.Range.InsertParagraphAfter();
        }
        brandsParagraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
        brandsParagraph.Format.LeftIndent = wordApp.CentimetersToPoints(1.25f);
        brandsParagraph.Format.RightIndent = 0;
        brandsParagraph.Format.SpaceBefore = 0;
        brandsParagraph.Format.SpaceAfter = 0;
        brandsParagraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;

        var typeGeneralParagraph = doc.Content.Paragraphs.Add();
        typeGeneralParagraph.Range.Font.Name = "Comic Sans MS";
        typeGeneralParagraph.Range.Font.Size = 14;
        typeGeneralParagraph.Range.Text = "Всего типов продано: ";
        typeGeneralParagraph.Range.Font.Bold = 1;
        var rangeAfterBoldType = typeGeneralParagraph.Range.Duplicate;
        rangeAfterBoldType.SetRange(typeGeneralParagraph.Range.End, typeGeneralParagraph.Range.End);
        rangeAfterBoldType.InsertAfter($"\t{totalSoldTypes} шт.");
        rangeAfterBoldType.Font.Bold = 0;
        typeGeneralParagraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
        typeGeneralParagraph.Format.LeftIndent = wordApp.CentimetersToPoints(1.25f);
        typeGeneralParagraph.Format.RightIndent = 0;
        typeGeneralParagraph.Format.SpaceBefore = 0;
        typeGeneralParagraph.Format.SpaceAfter = 0;
        typeGeneralParagraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;
        typeGeneralParagraph.Range.InsertParagraphAfter();

        var typeParagraph = doc.Content.Paragraphs.Add();
        typeParagraph.Range.Font.Name = "Comic Sans MS";
        typeParagraph.Range.Font.Size = 14;
        foreach (var type in typesSold)
        {
            var range = typeParagraph.Range;
            range.Text = "Тип: ";
            range.Font.Bold = 1;
            range.Collapse(WdCollapseDirection.wdCollapseEnd);
            range.InsertAfter($"«{type.TypeName}» - ");
            range.Font.Bold = 0;
            var boldRange = range.Duplicate;
            boldRange.Collapse(WdCollapseDirection.wdCollapseEnd);
            boldRange.Font.Bold = 1;
            boldRange.Collapse(WdCollapseDirection.wdCollapseEnd);
            boldRange.InsertAfter($"в количестве {type.SoldCount} шт.");
            boldRange.Font.Bold = 0;
            typeParagraph.Range.InsertParagraphAfter();
        }
        typeParagraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
        typeParagraph.Format.LeftIndent = wordApp.CentimetersToPoints(1.25f);
        typeParagraph.Format.RightIndent = 0;
        typeParagraph.Format.SpaceBefore = 0;
        typeParagraph.Format.SpaceAfter = 0;
        typeParagraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;

        for (int i = 0; i < 5; i++)
            doc.Content.Paragraphs.Add().Range.InsertParagraphAfter();

        var dateParagraph = doc.Content.Paragraphs.Add();
        dateParagraph.Range.Text = $"Дата: {DateTime.Now:dd.MM.yyyy}";
        dateParagraph.Range.Font.Name = "Comic Sans MS";
        dateParagraph.Range.Font.Size = 14;
        dateParagraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
        dateParagraph.Format.LeftIndent = 0;
        dateParagraph.Format.RightIndent = 0;
        dateParagraph.Format.SpaceBefore = 0;
        dateParagraph.Format.SpaceAfter = 0;
        dateParagraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;
        dateParagraph.Range.InsertParagraphAfter();

        var signatureParagraph = doc.Content.Paragraphs.Add();
        signatureParagraph.Range.Text = "Подпись: ShumilkinAO (Шумилкин А.О.)";
        signatureParagraph.Range.Font.Name = "Comic Sans MS";
        signatureParagraph.Range.Font.Size = 14;
        signatureParagraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
        signatureParagraph.Format.LeftIndent = 0;
        signatureParagraph.Format.RightIndent = 0;
        signatureParagraph.Format.SpaceBefore = 0;
        signatureParagraph.Format.SpaceAfter = 0;
        signatureParagraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;
        var signatureTextRange = signatureParagraph.Range.Duplicate;
        signatureTextRange.SetRange(
            signatureParagraph.Range.Start + signatureParagraph.Range.Text.IndexOf("ShumilkinAO"),
            signatureParagraph.Range.Start + signatureParagraph.Range.Text.IndexOf("ShumilkinAO") + "ShumilkinAO".Length
        );
        signatureTextRange.Font.Name = "Monotype Corsiva";
        signatureTextRange.Font.Size = 25;
        signatureTextRange.Font.Bold = 1;
        signatureTextRange.Font.Italic = 0;
        signatureParagraph.Range.InsertParagraphAfter();


        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "SalesReport.docx");

        doc.SaveAs2(filePath);

        wordApp.Visible = true;
        doc = wordApp.Documents.Open(filePath);
        Marshal.ReleaseComObject(doc);
        Marshal.ReleaseComObject(wordApp);
    }

    private void UserShop_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Users());
    }

    private void ExitAccount_Click(object sender, RoutedEventArgs e)
    {
        new Auth.Login().Show();
        Close();
    }
}