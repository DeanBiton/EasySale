using System;
using System.Data;
using System.Text.Json;

public class Order
{
    public int orderID { get; set; }
    public int customerID { get; set; }
    public float price { get; set; }
}