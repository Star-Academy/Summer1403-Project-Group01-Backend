using Microsoft.AspNetCore.Mvc;

namespace Web.Helper;

public static class Errors
{
    public static Object New(string title, string message)
    {
        return new {title = new List<string>{message}};
    }
}