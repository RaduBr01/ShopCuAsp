using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
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
            if (Phone == null) Phone = "";

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
