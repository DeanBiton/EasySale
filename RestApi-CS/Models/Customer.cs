using Newtonsoft.Json;

namespace RestAPI.Models;

public class Customer
{
    [JsonRequired]
    public string name { get; set; }
    [JsonRequired]
    public string birthday { get; set; }
    [JsonRequired]
    public string city { get; set; }

    static public string getFieldNames()
    {
        return "Name, Birthday, City";
    }

    public string getFieldsString()
    {
        return string.Format("'{0}', '{1}', '{2}'", name, birthday, city);
    }
}