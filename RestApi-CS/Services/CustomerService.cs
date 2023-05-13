using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Nodes;
using RestAPI.Models;
using Newtonsoft.Json;
using System.Formats.Asn1;
using System.Reflection;

namespace RestAPI.Services;

public class CustomerService
{
    SqlConnection con;
    private readonly HttpClient client;
    string serverName = "DESKTOP-5R5EJ4F";
    string databaseName = "Final";
    string tableName = "Customer";

    public CustomerService()
    {
        client = new HttpClient();
        con = new SqlConnection(string.Format("server={0}; database={1}; Integrated Security=True;", serverName, databaseName));
    }

    public DataTable Get()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM " + tableName, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public DataTable GetCustomersAndOrders()
    {
        SqlDataAdapter da = new SqlDataAdapter(
            "SELECT * FROM " + tableName + " c join Orders o on c.CustomerID = o.CustomerID", con
            );
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public int Post(JsonElement value)
    {
        Customer cus = JsonConvert.DeserializeObject<Customer>(value.ToString());
        string command = string.Format("Insert into {0}({1}) VALUES({2})", 
            tableName, Customer.getFieldNames(), cus.getFieldsString());
        SqlCommand cmd = new SqlCommand(command, con);
        con.Open();
        int i = cmd.ExecuteNonQuery();
        con.Close();
        return i;
    }

    public void Put(int id, JsonElement value)
    {
        Customer cus = JsonConvert.DeserializeObject<Customer>(value.ToString());
        SqlCommand cmd = new SqlCommand("UPDATE "+tableName+" SET Name = '" + cus.name + "', Birthday = '" + cus.birthday + "' WHERE CustomerID = '" + id + "' ", con);
        con.Open();
        int querySuccess = cmd.ExecuteNonQuery();
        con.Close();
        if(querySuccess == 0)
            throw new Exception("ID doesn't exist");
    }

    public void Delete(int id)
    {
        SqlCommand cmd = new SqlCommand("DELETE FROM "+ tableName + " WHERE CustomerID = '" + id + "' ", con);
        con.Open();
        int querySuccess = cmd.ExecuteNonQuery();
        con.Close();
        if (querySuccess == 0)
            throw new Exception("ID doesn't exist");
    }
}