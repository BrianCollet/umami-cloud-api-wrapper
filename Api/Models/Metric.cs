namespace Api.Models;

public class Metric
{
    public int Value { get; set; }
    public int Prev { get; set; }
    public string Diff { get; set; }
    public string ValueWithDiff { get; set; }

    public Metric(int value = 0, int prev = 0)
    {
        Value = value;
        Prev = prev;

        double change = 0.0;

        if (prev != 0 && value > 0)
        {
            change = (((double)value - (double)prev) / (double)prev) * 100;
        }

        Diff = change > 0.0 ? ($"+{(int)change}%") : ($"{(int)change}%");

        ValueWithDiff = $"{value}({Diff})";
    }
}