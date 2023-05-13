using Newtonsoft.Json;

namespace RestAPI.Models;

public class Order
{
    public int orderID { get; set; }
    [JsonRequired]
    public int customerID { get; set; }
    [JsonRequired]
    public float price { get; set; }

    static public string getFieldNames()
    {
        return "orderID, customerID, price";
    }

    public string getFieldsString()
    {
        return string.Format("'{0}', '{1}', '{2}'", orderID, customerID, price);
    }
}