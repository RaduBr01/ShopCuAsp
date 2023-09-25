using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Shop.Pages.Admin.Books
{
    public class IndexModel : PageModel
    {
        public List<BookInfo> listBooks= new();
        public string search = "";



        public void OnGet()
        {
            

         
            try
            {
                search = Request.Query["search"];
                if (search == null) search = "";

                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                SqlConnection connection= new SqlConnection(connectionString);
                connection.Open();
                string sql = "SELECT * FROM BOOKS ";
                Console.WriteLine(search.Length);

               if(search.Length> 0)
                {
                    sql += "WHERE title LIKE @search OR authors LIKE @search OR id LIKE @search ";
                    Console.WriteLine("s-a ajuns");
                }
                sql += "ORDER BY id ASC";
                SqlCommand cmd = new SqlCommand(sql, connection);   
                cmd.Parameters.AddWithValue("@search", "%"+ search + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())   
                {
                    BookInfo bookInfo = new BookInfo();
                    bookInfo.Id = reader.GetInt32(0);
                    bookInfo.Title = reader.GetString(1);
                    bookInfo.Author = reader.GetString(2);
                    bookInfo.NumPages = reader.GetInt32(3);
                    bookInfo.Price = reader.GetDecimal(4);
                    bookInfo.Category = reader.GetString(5);
                    bookInfo.Description = reader.GetString(6);
                    bookInfo.ImageFileName = reader.GetString(7);
                    bookInfo.CreatedAt = reader.GetDateTime(8).ToString("MM/dd/yyyy");
                    listBooks.Add(bookInfo);
                   
                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
    public class BookInfo
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";

        public string Author { get; set; } = "";
        public int NumPages { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
        public string ImageFileName { get; set; } = "";
        public string CreatedAt { get; set; } = "";
    }


        

}
