using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;

namespace Shop.Pages.Admin
{
    public class DetailsOrderItemsModel : PageModel
    {
        public List<MessageInfo> Messages = new List<MessageInfo>();

        public void OnGet()
        {
            try
            {
                string id = Request.Query["id"];
   
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM order_items WHERE order_id=@OrderId ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", id); // Use a parameter to avoid SQL injection

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("am ajuns aici");
                                MessageInfo messageInfo = new MessageInfo();
                                messageInfo.order_item_id = reader.GetInt32(0);
                                messageInfo.orderId = reader.GetInt32(1); // Assuming the order_id is in the first column
                                messageInfo.bookId = reader.GetInt32(2);  // Assuming the book_id is in the second column
                                messageInfo.quantity = reader.GetInt32(3); // Assuming the quantity is in the third column
                                messageInfo.unitPrice = reader.GetDecimal(4); // Assuming the unit_price is in the fourth column
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
            public int order_item_id;
            public int orderId;
            public int bookId;
            public int quantity;
            public decimal unitPrice;
        }
    }
}