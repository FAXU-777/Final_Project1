namespace HOMEWORK_14;

public class HtmlReportGenerator : IReportGenerator
{
    public string Generate(ReportContent content)
    {
        return content.ToString();
        
    }
}