using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        HttpClient client = new HttpClient();
        List<Order> orders;

        public Window1()
        {
            client.BaseAddress = new Uri("http://localhost:5274/order/");
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
            orders = JsonConvert.DeserializeObject<List<Order>>(json);
            datagrid.ItemsSource = orders;
        }
        
        private Order createOrder()
        {
            Order order = new Order();
            order.orderID = int.Parse(id_txt.Text);
            order.customerID = int.Parse(customerID_txt.Text);
            order.price = float.Parse(price_txt.Text);
            return order;
        }
        
        private async void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            msg_txt.Text = "";
            await client.PostAsJsonAsync("", createOrder());
            LoadData();
        }
        
        private async void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            msg_txt.Text = "";
            int id;
            if (validateID() && int.TryParse(id_txt.Text, out id))
            {
                string url = string.Format("{0}", id);
                await client.PutAsJsonAsync(url, createOrder());
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
                foreach (Order order in orders)
                {
                    if (order.orderID == id)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    valid = false;
                    msg_txt.Text = "ID does not exists";
                }
            }
            return valid;
        }
    }
}
