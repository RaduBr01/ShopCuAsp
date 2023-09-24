using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static Shop.Pages.Admin.IndexModel;

namespace Shop.Pages.Admin
{
    public class DetailsModel : PageModel
    {
        public MessageInfo messageInfo = new MessageInfo();
        public void OnGet()
        {
            string requestId = Request.Query["id"];

            try
            {
                string connectionString = "Data Source =.\\sqlexpress; Initial Catalog = Shop; Integrated Security = True";
                using (SqlConnection connection=new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM messages where id=@id";
                    SqlCommand cmd =new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", requestId);
                    SqlDataReader reader=cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        messageInfo.Id = reader.GetInt32(0);
                        messageInfo.FirstName = reader.GetString(1);
                        messageInfo.LastName = reader.GetString(2);
                        messageInfo.Email = reader.GetString(3);
                        messageInfo.Phone = reader.GetString(4);
                        messageInfo.Subject = reader.GetString(5);
                        messageInfo.Message = reader.GetString(6);
                        messageInfo.CreatedAt = reader.GetDateTime(7).ToString("MM/dd//yyyy");

                    }
                    else
                    {
                        Response.Redirect("/Admin/Index");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.Redirect("/Admin/Index");
            }
        }
    }
}
