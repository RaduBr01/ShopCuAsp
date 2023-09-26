using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Shop.Pages.Admin.Books
{
    public class CreationModel : PageModel
    {
        IWebHostEnvironment webHostEnvironment { get; set; }
        public CreationModel(IWebHostEnvironment env) {
            webHostEnvironment = env;
        }
        [BindProperty]
        [Required(ErrorMessage="This section is required.")]
        [MaxLength(100, ErrorMessage ="Too long")]
        public string Title { get; set; } = " ";
        [BindProperty]
        [Required(ErrorMessage = "This section is required.")]
        [MaxLength(100, ErrorMessage = "Too long")]
        public string Authors { get; set; } = " ";
        [BindProperty]
        [Required(ErrorMessage = "This section is required.")]
        [Range(0,10000,ErrorMessage="Too many pages")]
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
        public string ?Description { get; set; } = " ";
        [BindProperty]
        [Required(ErrorMessage = "This section is required.")]
        public IFormFile ImageFile { get; set; }

        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }



        public void OnPost() 
        { 
            if(!ModelState.IsValid)
            {
                errorMessage = "Data valiidation failed!";
                return;
            }

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            fileName += Path.GetExtension(ImageFile.FileName);
            string imageFolder = webHostEnvironment.WebRootPath + "/imagesBooks/";
            string imageFullPath=Path.Combine(imageFolder, fileName);
            var stream=System.IO.File.Create(imageFullPath);
            {
                ImageFile.CopyTo(stream);
            }

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "INSERT INTO BOOKS " +
                    "(title,authors,num_pages,price,category,description,image_filename) VALUES " +
                     "(@title,@authors,@num_pages,@price,@category,@description,@image_filename)";
                SqlCommand cmd = new SqlCommand(sql, connection);
                {
                    cmd.Parameters.AddWithValue("@title", Title);
                    cmd.Parameters.AddWithValue("@authors", Authors);
                    cmd.Parameters.AddWithValue("@num_pages", NumPages);
                    cmd.Parameters.AddWithValue("@price", Price);
                    cmd.Parameters.AddWithValue("@category", Category);
                    cmd.Parameters.AddWithValue("@description", Description);
                    cmd.Parameters.AddWithValue("@image_filename", fileName);
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }



            if (Description == null) Description = "";
            successMessage = "Data saved";
            Response.Redirect("/Admin/Books/Index");

        }
    }
}
