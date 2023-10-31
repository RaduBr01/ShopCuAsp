using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Shop.Pages.Admin
{
    public class DetailsOrderItemsModel : PageModel
    {
        public List<MessageInfo> Messages = new List<MessageInfo>();

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM order_items";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MessageInfo messageInfo = new MessageInfo();
                                messageInfo.orderId = reader.GetInt32(1); // Adjust as per your database schema
                                messageInfo.bookId = reader.GetInt32(2);  // Adjust as per your database schema
                                messageInfo.quantity = reader.GetInt32(3); // Adjust as per your database schema
                                messageInfo.unitPrice = reader.GetDecimal(4); // Adjust as per your database schema
                                Messages.Add(messageInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public class MessageInfo
        {
            public int orderId;
            public int bookId;
            public int quantity;
            public decimal unitPrice;
        }
    }
}