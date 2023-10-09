using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Pages.Admin.Books;
using System.Data.SqlClient;

namespace Shop.Pages
{
    [BindProperties(SupportsGet = true)]
    public class BooksModel : PageModel
    {
        public string? Search { get; set; }
        public string PriceRange { get; set; } = "any";
        public string Category { get; set; } = "any";

        public int page = 1;
        public int totalPages = 0;
        private readonly int pageSize = 4;

        public List<BookInfo> listBooks=new List<BookInfo>();
        public void OnGet()
        {
            page = 1;
            string requestPage = Request.Query["page"]; 
            if(requestPage != null)
            {
                try
                {
                    page = int.Parse(requestPage);
                }
                catch(Exception ex)
                {
                    page = 1;
                }
            }
            try
            {


                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string sqlCount = "SELECT COUNT(*) FROM BOOKS ";
                sqlCount += " WHERE (title LIKE @search OR authors LIKE @search) ";

                if (PriceRange.Equals("0_100"))
                {
                    sqlCount += " AND price <= 100 ";
                    Console.WriteLine("Da");
                }

                if (PriceRange.Equals("100_200"))
                {
                    sqlCount += " AND price <= 200 AND PRICE >= 100 ";
                }


                if (PriceRange.Equals("above200"))
                {
                    sqlCount += "AND price > 200 ";
                }


                if (!Category.Equals("any"))
                {
                    sqlCount += " AND category=@category";
                    Console.WriteLine("Da");
                }

                SqlCommand cmd2 = new SqlCommand(sqlCount, connection);
                cmd2.Parameters.AddWithValue("@search", "%" + Search + "%");
                cmd2.Parameters.AddWithValue("@category", Category);
                decimal count=(int)cmd2.ExecuteScalar();
                totalPages=(int)Math.Ceiling(count/pageSize);


                string sql = "SELECT * FROM BOOKS ";
                sql += " WHERE (title LIKE @search OR authors LIKE @search) ";
                Console.WriteLine(PriceRange);
                Console.WriteLine(Category);
                if (PriceRange.Equals("0_100"))
                {
                    sql += " AND price <= 100 ";
                    Console.WriteLine("Da");
                }

                if (PriceRange.Equals("100_200"))
                {
                    sql += " AND price <= 200 AND PRICE >= 100 ";
                }


                if (PriceRange.Equals("above200"))
                {
                    sql += "AND price > 200 ";
                }


                if (!Category.Equals("any")) 
                {
                    sql += " AND category=@category";
                    Console.WriteLine("Da");
                }

                sql += " ORDER BY id ASC ";
                sql += "OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY ";



                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@search", "%" + Search + "%"); 
                cmd.Parameters.AddWithValue("@category",Category ); 
                cmd.Parameters.AddWithValue("@skip",(page-1)*pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                
             
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
}
    