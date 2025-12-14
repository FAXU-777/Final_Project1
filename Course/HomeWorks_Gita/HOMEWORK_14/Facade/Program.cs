namespace HOMEWORK_14;

class Program
{
    static void Main(string[] args)
    {
        ReportContent content = new ReportContent();
        
        IReportGenerator htmlGenerator = new HtmlReportGenerator();
        IReportGenerator pdfGenerator = new PdfGenerator();

        // HTML Output
        string htmlOutput = htmlGenerator.Generate(content);
        Console.WriteLine("HTML Report:\n" + htmlOutput);

        // PDF Output
        string pdfPath = pdfGenerator.Generate(content);
        Console.WriteLine("\nPDF Report saved at: " + pdfPath);
        
    }
}