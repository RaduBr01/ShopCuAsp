using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Pages.Admin.Books;
using System.Data.SqlClient;


namespace Shop.Pages
{
    public class BookDetailsModel : PageModel
    {
        public BookInfo bookInfo = new BookInfo();
        public void OnGet(int ?id)
        {
          
            if (id == null)
            {
                Response.Redirect("Error");
                return; 
            }
            string requestId = Request.Query["id"];
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "SELECT * from BOOKS WHERE id=@id ";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", requestId);
                cmd.Parameters.AddWithValue("@title", requestId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    bookInfo.Id = reader.GetInt32(0);
                    bookInfo.Title = reader.GetString(1);
                    bookInfo.Author = reader.GetString(2);
                    bookInfo.NumPages = reader.GetInt32(3);
                    bookInfo.Price = reader.GetDecimal(4);
                    bookInfo.Category = reader.GetString(5);
                    bookInfo.Description = reader.GetString(6);
                    bookInfo.ImageFileName = reader.GetString(7);
                }
                else
                {
                    Response.Redirect("/");
                    return;
                }
            }







            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.Redirect("/");
            }

        }
    }
    
}
