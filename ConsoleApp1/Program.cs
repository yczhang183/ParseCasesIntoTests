using System.Text.RegularExpressions;

const string toBeParsed = "Case 1 - Given a parking lot, and a car, When park the car, Then return a parking ticket.\r\n• Case 2 - Given a parking lot with a parked car, and a parking ticket, When fetch the car, \r\nThen return the parked car. \r\n• Case 3 - Given a parking lot with two parked cars, and two parking tickets, When fetch the \r\ncar twice, Then return the right car with each ticket.\r\n• Case 4 - Given a parking lot, and a wrong parking ticket, When fetch the car, Then return \r\nnothing.\r\n• Case 5 - Given a parking lot, and a used parking ticket, When fetch the car, Then return \r\nnothing.\r\n• Case 6 - Given a parking lot without any position, and a car, When park the car, Then\r\nreturn nothing";

Console.WriteLine(CaseParser.Parse(toBeParsed));

public static class CaseParser
{
    //Can only parse case seperated by "." and contain "given", "when", "then" only once each in single case.
    public static string Parse(string toBeParsed)
    {
        toBeParsed = CleanString(toBeParsed);

        string[] lines = toBeParsed.Split(".");

        return string.Join('\n', lines.Select(line => BuildTestFromCase(line)));
    }

    private static string CleanString(string str)
    {
        str = Regex.Replace(str, @"[^0-9a-zA-Z .]+", " "); //replace special characters into space
        str = Regex.Replace(str.Trim(), @"\s+", " ").ToLower(); //keep only one space and all to lower
        return str;
    }

    private static string BuildTestFromCase(string str)
    {
        int givenClauseStart = str.IndexOf("given") + "given".Length;
        int whenClauseStart = str.IndexOf("when") + "when".Length;
        int thenClauseStart = str.IndexOf("then") + "then".Length;
        string givenClause = str.Substring(givenClauseStart, str.IndexOf("when") - givenClauseStart).Trim().Replace(" ", "_");
        string whenClause = str.Substring(whenClauseStart, str.IndexOf("then") - whenClauseStart).Trim().Replace(" ", "_");
        string thenClause = str.Substring(thenClauseStart).Trim().Replace(" ", "_");

        return $"[Fact]\npublic void Should_{thenClause}_when_{whenClause}_given_{givenClause}\n{{\n}}\n";
    }
}