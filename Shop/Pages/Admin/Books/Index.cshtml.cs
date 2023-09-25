using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Shop.Pages.Admin.Books
{
    public class IndexModel : PageModel
    {
        public List<BookInfo> listBooks= new();
        public string search = "";
        public int page = 1;
        private readonly int pageSize = 2;
        public  int totalPages;


        public string column = "id";
        public string order = "desc";



        public void OnGet()
        {
            string requestPage = Request.Query["page"];
            if(requestPage != null)
            {
                try
                {
                    page=int.Parse(requestPage);

                }
                catch (Exception ex)
                {
                    page = 1;
                    Console.WriteLine(ex.ToString());
                }
            }

            string[] ColumnsForSort = { "id", "title", "authors", "num_pages", "price", "category", "desription", "image_filename", "created_at" };
            column = Request.Query["column"];
            if(column == null || !ColumnsForSort.Contains(column))
            {
                column = "id";
            }
            order = Request.Query["order"];
            if (order == null || !order.Equals("asc")) 
                
                order = "desc";
            


            try
            {

                search = Request.Query["search"];
                if (search == null) search = "";

                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                SqlConnection connection= new SqlConnection(connectionString);
                connection.Open();
                string sql = "SELECT * FROM BOOKS ";
                string sqlCount = "SELECT COUNT(*) FROM BOOKS ";
                if (search.Length > 0)
                {
                    sqlCount += " WHERE title LIKE @search OR authors LIKE @search OR id LIKE @search ";

                }

                if (search.Length > 0)
                {
                    sql += "WHERE title LIKE @search OR authors LIKE @search OR id LIKE @search ";

                }
                SqlCommand cmdCount = new SqlCommand(sqlCount, connection);
                cmdCount.Parameters.AddWithValue("@search", "%" + search + "%");
                decimal Count = (int)cmdCount.ExecuteScalar();
                totalPages = (int)Math.Ceiling(Count / pageSize);

                sql += "ORDER BY " + column + " " + order;
                sql += " OFFSET @skip ROWS FETCH NEXT @pagesize ROWS ONLY ";

                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                cmd.Parameters.AddWithValue("skip", (page - 1) * pageSize);
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
