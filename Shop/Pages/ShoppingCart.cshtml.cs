using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Pages.Admin.Books;
using System.Data.SqlClient;

namespace Shop.Pages
{
    public class ShoppingCartModel : PageModel
    {
        public List <OrderItem> items= new List<OrderItem> ();

        private Dictionary<String, int> getBookDictionary()
        {
            var bookDictionary = new Dictionary<String, int>();
            string cookieValue = Request.Cookies["shopping_cart"] ?? "";

            if (cookieValue.Length > 0)
            {
                string[] bookIdArray = cookieValue.Split('-');
                Console.WriteLine(bookIdArray);
                for (int i = 0; i < bookIdArray.Length; i++) // Fixed the termination condition here
                {
                    string bookId = bookIdArray[i];
                    if (bookDictionary.ContainsKey(bookId))
                    {
                        bookDictionary[bookId]++;
                    }
                    else
                    {
                        bookDictionary.Add(bookId, 1);
                    }
                }
            }
            return bookDictionary;
        }




        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "SELECT * FROM BOOKS WHERE id=@id ";
                foreach(var keyValuePair in getBookDictionary())
                {
                    string bookId=keyValuePair.Key;
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id",bookId);
                    SqlDataReader reader=cmd.ExecuteReader();
                       while(reader.Read()) 
                        {
                            OrderItem item = new OrderItem();
  
                            item.bookInfo.Id = reader.GetInt32(0);
                            item.bookInfo.Title = reader.GetString(1);
                            item.bookInfo.Author = reader.GetString(2);
                            item.bookInfo.NumPages = reader.GetInt32(3);
                            item.bookInfo.Price = reader.GetDecimal(4);
                            item.bookInfo.Category = reader.GetString(5);
                            item.bookInfo.Description = reader.GetString(6);
                            item.bookInfo.ImageFileName = reader.GetString(7);
                            item.bookInfo.CreatedAt = reader.GetDateTime(8).ToString("MM/dd/yyyy");
                            item.numCopies=keyValuePair.Value;
                            item.totalPrice=item.numCopies *item.bookInfo.Price;
                            items.Add(item);

                        }
                    reader.Close();
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class OrderItem
    {
        public BookInfo bookInfo = new();
        public int numCopies = 0;
        public decimal totalPrice = 0;
    }
}