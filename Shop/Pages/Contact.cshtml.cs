using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace Shop.Pages
{
    public class ContactModel : PageModel
    {
        public void OnGet()
        {

        }
        [BindProperty]
        [Required(ErrorMessage="The First Name is required")]
        [Display(Name="First Name*")]
        public string FirstName { get; set; } = "";
        [BindProperty]
        [Required(ErrorMessage = "The Last Name is required")]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; } = "";
        [BindProperty]
        [Required(ErrorMessage = "The mail is required")]
        [Display(Name = "Email*")]
        public string Email { get; set; } = "";
        [BindProperty]
        public string? Phone { get; set; } = "";
        [BindProperty]
        [Display(Name = "Subject*")]

        public string Subject { get; set; } = "";
        [BindProperty]
        [Required(ErrorMessage = "The Message is required")]
        [MinLength(10,ErrorMessage="N-ai bagat destula  vrajeala")]
        [MaxLength(1024,ErrorMessage="Prea multa vrajeala")]
        [Display(Name = "Message*")]

        public string Message { get; set; } = "";

        public string SuccesMessage { get; set; } = "";
        public string ErrorMessage { get;set ; } = "";

        public List<SelectListItem> SubjectList { get; } = new List<SelectListItem>
        {
            new SelectListItem{Value="Order Status", Text="Order Status"},
            new SelectListItem{Value="Refund  Request", Text="Refund Request"},
            new SelectListItem{Value="Order Status", Text="Job Application"},
            new SelectListItem{Value="Other", Text="Other"},

        };

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill all required fields";
                return;
            }
            Phone ??= ""; //compound assingment if phone== null atunci ia valoarea ""

            try
            {
                string ConnectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shop;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO messages " +
                                 "(firstname,lastname,email,phone,subject,message) VALUES " +
                                 "(@firstname,@lastname,@email,@phone,@subject,@message);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@firstname", SqlDbType.NVarChar, 50)).Value = FirstName;
                        command.Parameters.Add(new SqlParameter("@lastname", SqlDbType.NVarChar, 50)).Value = LastName;
                        command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 100)).Value = Email;
                        command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar, 20)).Value = Phone;
                        command.Parameters.Add(new SqlParameter("@subject", SqlDbType.NVarChar, 100)).Value = Subject;
                        command.Parameters.Add(new SqlParameter("@message", SqlDbType.NVarChar, -1)).Value = Message; // Use -1 for max length

                        command.ExecuteNonQuery();
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            string username = FirstName + " " + LastName;
            string EmailSubject = "Message notification";
            string EmailMessage = "Dear " + username + ",\n" +
                "Message received. Thank you for contacting us.\n" +
                "We will try to reach you soon";
            
            
            EmailSender.SendEmail(Email, username, EmailSubject, EmailMessage).Wait();
 

            SuccesMessage = "Message received";


            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
            Subject = "";
            Message = " ";


            ModelState.Clear();




        }
    }
}
