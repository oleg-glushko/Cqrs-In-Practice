namespace UI.Api;

public static class DbStaticData
{
    public static string[] Courses { get; } = [
        "",
        "Calculus",
        "Chemistry",
        "Composition",
        "Literature",
        "Trigonometry",
        "Microeconomics",
        "Macroeconomics"
    ];
    public static string[] NumberOfCourses { get; } = ["", "0", "1", "2"];
    public static string[] Grades { get; } = ["", "A", "B", "C", "D", "F"];
}
