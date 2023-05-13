using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Nodes;
using RestAPI.Models;
using Newtonsoft.Json;
using System.Formats.Asn1;
using System.Reflection;

namespace RestAPI.Services;

public class OrderService
{
    SqlConnection con;
    private readonly HttpClient client;
    string serverName = "DESKTOP-5R5EJ4F";
    string databaseName = "Final";
    string tableName = "Orders";
    public OrderService()
    {
        client = new HttpClient();
        con = new SqlConnection(string.Format("server={0}; database={1}; Integrated Security=True;", serverName, databaseName));
    }

    async public Task<DataTable> Get()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM " + tableName, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public int Post(JsonElement value)
    {
        Order order = JsonConvert.DeserializeObject<Order>(value.ToString());
        string command = string.Format("Insert into {0}({1}) VALUES({2})", 
            tableName, Order.getFieldNames(), order.getFieldsString());
        SqlCommand cmd = new SqlCommand(command, con);
        con.Open();
        int i = cmd.ExecuteNonQuery();
        con.Close();
        return i;
    }

    public void Put(int id, JsonElement value)
    {
        Order order = JsonConvert.DeserializeObject<Order>(value.ToString());
        SqlCommand cmd = new SqlCommand(
            "UPDATE "+tableName+ " SET CustomerID = '" + order.customerID +
            "', Price = '" + order.price + "' WHERE OrderID = '" + id + "' "
            ,con);
        con.Open();
        int querySuccess = cmd.ExecuteNonQuery();
        con.Close();
        if(querySuccess == 0)
            throw new Exception("ID doesn't exist");
    }

    public void Delete(int id)
    {
        SqlCommand cmd = new SqlCommand("DELETE FROM "+ tableName + " WHERE OrderID = '" + id + "' ", con);
        con.Open();
        int querySuccess = cmd.ExecuteNonQuery();
        con.Close();
        if (querySuccess == 0)
            throw new Exception("ID doesn't exist");
    }
}