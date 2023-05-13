using System;
using System.Data;
using System.Text.Json;

public class Customer
{
    public int CustomerID { get; set; }
    public string Name { get; set; }
    public string Birthday { get; set; }
    public string City { get; set; }

    public Customer()
    {
        Name = string.Empty;
    }
}