namespace Api.Models;

public class WebsiteStats
{
    public Metric? PageViews { get; set; }
    public Metric? Visitors { get; set; }
    public Metric? Visits { get; set; }
    public Metric? Bounces { get; set; }
    public Metric? TotalTime { get; set; }
}