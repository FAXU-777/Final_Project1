namespace HOMEWORK_14;

public class PdfGenerator : IReportGenerator
{
    public string Generate(ReportContent content)
    {
        string text = "Video provides a powerful way to help you prove your point.\n " +
                      "\"When you click Online Video, you can paste in the embed code for the"+
                      "\"video you want to add";
        return text;
    }
}