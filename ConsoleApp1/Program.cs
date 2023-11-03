using System.Text.RegularExpressions;

const string toBeParsed = "Case 1 - Given a standard parking boy, who manage two parking lots, both with available position, \r\nand a car, When park the car, Then the car will be parked to the first parking lot\r\n• Case 2 - Given a standard parking boy, who manage two parking lots, first is full and second with \r\navailable position, and a car, When park the car, Then the car will be parked to the second parking \r\nlot\r\n• Case 3 - Given a standard parking boy, who manage two parking lots, both with a parked car, and \r\ntwo parking ticket, When fetch the car twice, Then return the right car with each ticket \r\n• Case 4 - Given a standard parking boy, who manage two parking lots, and an unrecognized ticket, \r\nWhen fetch the car, Then return nothing with error message \"Unrecognized parking ticket.”\r\n• Case 5 - Given a standard parking boy, who manage two parking lots, and a used ticket, When\r\nfetch the car, Then return nothing with error message \"Unrecognized parking ticket.\"\r\n• Case 6 - Given a standard parking boy, who manage two parking lots, both without any position, \r\nand a car, When park the car, Then return nothing with error message \"No available position.\"";

Console.WriteLine(CaseParser.Parse(toBeParsed));

public static class CaseParser
{
    //Can only parse case seperated by "•" and contain "given", "when", "then" only once each in single case.
    public static string Parse(string toBeParsed)
    {
        toBeParsed = CleanString(toBeParsed);

        string[] lines = toBeParsed.Split("•");

        return string.Join('\n', lines.Select(line => BuildTestFromCase(line)));
    }

    private static string CleanString(string str)
    {
        str = Regex.Replace(str, @"[^0-9a-zA-Z •]+", " "); //replace special characters into space
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

        return $"[Fact]\npublic void Should_{thenClause}_when_{whenClause}_given_{givenClause}()\n{{\n}}\n";
    }
}
