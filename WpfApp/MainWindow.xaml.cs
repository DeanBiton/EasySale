﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        List<Customer> customers;

        public MainWindow()
        {
            client.BaseAddress = new Uri("http://localhost:5274/customer/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            var response = await client.GetStringAsync("");
            Console.WriteLine(response);
            var json = JsonConvert.DeserializeObject<string>(response);
            datagrid.Columns.Clear();
            customers = JsonConvert.DeserializeObject<List<Customer>>(json);
            datagrid.ItemsSource = customers;
        }

        private Customer createCustomer()
        {
            Customer customer = new Customer();
            customer.Name = name_txt.Text;
            customer.Birthday = birthday_txt.Text;
            customer.City = city_txt.Text;
            return customer;
        }

        private async void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            msg_txt.Text = "";
            await client.PostAsJsonAsync("", createCustomer());
            LoadData();
        }

        private async void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            msg_txt.Text = "";
            int id;
            if (validateID() && int.TryParse(id_txt.Text, out id))
            {
                string url = string.Format("{0}", id);
                await client.PutAsJsonAsync(url, createCustomer());
                LoadData();
            }

        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            msg_txt.Text = "";
            int id;
            if (validateID() && int.TryParse(id_txt.Text, out id))
            {
                string url = string.Format("{0}", id);
                await client.DeleteAsync(url);
                LoadData();
            }
        }

        // validations
        private bool validateID()
        {
            bool valid = true;
            int id;
            if (!int.TryParse(id_txt.Text, out id))
            {
                valid = false;
                msg_txt.Text = "ID is not an integer";
            }
            else
            {
                bool found = false;
                foreach (Customer customer in customers)
                {
                    if (customer.CustomerID == id)
                    { 
                        found = true; 
                        break; 
                    }
                }

                if(!found)
                {
                    valid = false;
                    msg_txt.Text = "ID does not exists";
                }
            }
            return valid;
        }

        private void OrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            Window1 win = new Window1();
            win.Show();
        }
    }
}
