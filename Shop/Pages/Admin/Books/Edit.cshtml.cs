using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Shop.Pages.Admin.Books
{
    public class EditModel : PageModel
    {
        IWebHostEnvironment webHostEnvironment { get; set; }
        public EditModel(IWebHostEnvironment env)
        {
            webHostEnvironment = env;
        }


        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "This section is required.")]
        [MaxLength(100, ErrorMessage = "Too long")]
        public string Title { get; set; } = " ";
        [BindProperty]
        [Required(ErrorMessage = "This section is required.")]
        [MaxLength(100, ErrorMessage = "Too long")]
        public string Authors { get; set; } = " ";
        [BindProperty]
        [Required(ErrorMessage = "This section is required.")]
        [Range(0, 10000, ErrorMessage = "Too many pages")]
        public int NumPages { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "This section is required.")]
        [Range(0, 1000, ErrorMessage = "Too expensive")]
        public decimal Price { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "This section is required.")]
        [MaxLength(100, ErrorMessage = "Too long")]
        public string Category { get; set; } = " ";
        [BindProperty]
        [MaxLength(1000, ErrorMessage = "Too long")]
        public string? Description { get; set; } = " ";
        [BindProperty]

       
        public string imageFileName { get; set; } = "";
        [BindProperty]
        public IFormFile? ImageFile { get; set; }


        public string errorMessage = "";
        public string successMessage = "";
  
        public void OnGet()
        {
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
                if(reader.Read())
                {
                 Id = reader.GetInt32(0);
                 Title = reader.GetString(1);
                 Authors = reader.GetString(2);
                 NumPages = reader.GetInt32(3);
                 Price = reader.GetDecimal(4);
                 Category = reader.GetString(5);
                 Description = reader.GetString(6);
                 imageFileName = reader.GetString(7);
                }
                else
                {
                    Response.Redirect("Admin/Books/Index");
                }
            }







            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.Redirect("/Admin/Books/Index");
            }

        }
        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Data validation failed";
                return;
            }
            if (Description == null) Description = "";

            string newFileName = imageFileName;
            if(ImageFile != null)
            {
                newFileName=DateTime.Now.ToString("yyyyMMdd");
                newFileName += Path.GetExtension(ImageFile.FileName);
                string imageFolder = webHostEnvironment.WebRootPath + "/imagesBooks";
                string imageFullPath=Path.Combine(imageFolder, newFileName);
                var stream=System.IO.File.Create(imageFullPath);
                ImageFile.CopyTo(stream);
                string oldImagePath = Path.Combine(imageFolder, imageFileName);
                System.IO.File.Delete(oldImagePath);

            }

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "UPDATE BOOKS SET title=@title, authors=@authors, num_pages=@num_pages,  " +
                    "price=@price, category=@category, description=@description, image_filename=@image_filename WHERE id=@id";
                SqlCommand cmd = new SqlCommand(sql, connection);
                {
                    cmd.Parameters.AddWithValue("@title", Title);
                    cmd.Parameters.AddWithValue("@authors", Authors);
                    cmd.Parameters.AddWithValue("@num_pages", NumPages);
                    cmd.Parameters.AddWithValue("@price", Price);
                    cmd.Parameters.AddWithValue("@category", Category);
                    cmd.Parameters.AddWithValue("@description", Description);
                    cmd.Parameters.AddWithValue("@image_filename", imageFileName);
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;


            }   


            successMessage = "Data saved";
            Response.Redirect("/Admin/Books");

        }
 


    }
}
