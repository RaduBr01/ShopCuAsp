using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Pages.Admin.Books;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Shop.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        public List<OrderInfo> orders = new List<OrderInfo>();

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "SELECT * FROM orders  ORDER by id DESC ";
                SqlCommand cmd = new SqlCommand(sql, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    OrderInfo orderInfo = new OrderInfo();
                    orderInfo.Id = reader.GetInt32(0);
                    orderInfo.orderDate = reader.GetDateTime(1).ToString("MM/dd/yyyy");
                    orderInfo.deliveryAddress = reader.GetString(2);
                    orderInfo.paymentMethod = reader.GetString(3);
                    orderInfo.paymentStatus = reader.GetString(4);
                    orderInfo.orderStatus = reader.GetString(5);
                    orderInfo.items = OrderInfo.getOrderItems(orderInfo.Id);
                    orders.Add(orderInfo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public class OrderItemInfo
        {
            public int Id;
            public int orderId;
            public int bookId;
            public int quantity;
            public decimal unit_price;

            public BookInfo bookInfo = new BookInfo();
        }

        public class OrderInfo
        {
            public int Id;
            public int clientId;
            public string orderDate;
            public decimal shppingFee;
            public string deliveryAddress;
            public string paymentMethod;
            public string paymentStatus;
            public string orderStatus;

            public List<OrderItemInfo> items = new List<OrderItemInfo>();

            public static List<OrderItemInfo> getOrderItems(int orderId)
            {
                List<OrderItemInfo> items = new List<OrderItemInfo>();

                try
                {
                    string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    string sql = "SELECT order_items.*, books.* FROM order_items, books " +
                                 "WHERE order_items.order_id=@order_id AND order_items.book_id = books.id ";
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@order_id", orderId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        OrderItemInfo item = new OrderItemInfo();

                        item.Id = reader.GetInt32(0);
                        item.orderId = reader.GetInt32(1);
                        item.bookId = reader.GetInt32(2);
                        item.quantity = reader.GetInt32(3);
                        item.unit_price = reader.GetDecimal(4);

                        item.bookInfo.Id = reader.GetInt32(5);
                        item.bookInfo.Title = reader.GetString(6);
                        item.bookInfo.Author = reader.GetString(7);
                        item.bookInfo.NumPages = reader.GetInt32(8);
                        item.bookInfo.Price = reader.GetDecimal(9);
                        item.bookInfo.Category = reader.GetString(10);
                        item.bookInfo.Description = reader.GetString(11);
                        item.bookInfo.ImageFileName = reader.GetString(12);
                        item.bookInfo.CreatedAt = reader.GetDateTime(13).ToString("MM/dd/yyyy");

                        items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return items;
            }
        }
    }
}