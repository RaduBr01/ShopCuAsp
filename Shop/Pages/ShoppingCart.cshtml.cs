using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Pages;
using Shop.Pages.Admin.Books;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Transactions;

namespace Shop.Pages
{
    [BindProperties]
    public class ShoppingCartModel : PageModel
    {
        [Required(ErrorMessage = "The address is required")]
        public string Address { get; set; } = "";


        [Required(ErrorMessage = "The payment method is required")]
        public string PaymentMethod { get; set; } = "";
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
            var bookDictionary= getBookDictionary();
            string? action = Request.Query["action"];
            string? id = Request.Query["id"];
            if(action != null && id!=null && bookDictionary.ContainsKey(id))
            {



               if(action.Equals("add"))
                {
                    bookDictionary[id] += 1;
                }
               else if (action.Equals("sub")) 
                {
                    if (bookDictionary[id]>1)
                        bookDictionary[id] -= 1;
                }
               else if (action.Equals("delete"))
                {
                    bookDictionary.Remove(id);
                }
                string newCookie = "";
                foreach(var keyValuePair in bookDictionary)
                {
                    for(int i=0;i<keyValuePair.Value;i++)   
                    {
                        newCookie += "-" + keyValuePair.Key;
                    }
                }

                if(newCookie.Length> 0) 
                {
                    newCookie = newCookie.Substring(1);
                
                }

                var cookieOptions=new CookieOptions();
                cookieOptions.Expires=DateTime.Now.AddDays(365);
                cookieOptions.Path = "/";
                Response.Cookies.Append("shopping_cart",newCookie,cookieOptions);
                Response.Redirect(Request.Path.ToString());
            
            }
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
        public string ErrorMessage="";
        public string SuccessMessage="";
        public void OnPost()
        {
            if(!ModelState.IsValid)
            {
                
            }
            var bookDictionary=getBookDictionary();
            if(bookDictionary.Count < 1) 
            {
                ErrorMessage = "Cart is empty";
                return;
            }
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "INSERT INTO orders (client_id,order_date, "+
                    "delivery_adress, payment_method, payment_status,order_status) "+
                    "VALUES('0', CURRENT_TIMESTAMP, " +
                    "@delivery_adress, @payment_method, 'pending', 'created')";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@delivery_adress", Address);
                cmd.Parameters.AddWithValue("payment_method", PaymentMethod);
          

            }
            catch(Exception ex) 
            {
                ErrorMessage = ex.Message; return;

            }
            Response.Cookies.Delete("shopping_cart");
            SuccessMessage = "Order Created";
            
        }

    }

    public class OrderItem
    {
        public BookInfo bookInfo = new();
        public int numCopies = 0;
        public decimal totalPrice = 0;
    }
}


