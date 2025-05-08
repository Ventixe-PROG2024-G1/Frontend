using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.SignUp
{
    public class ProfileInformationViewModel
    {
        [Display(Name = "First Name", Prompt = "First name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name", Prompt = "Last name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = null!;

        [Display(Name = "Street Address", Prompt = "Street Address")]
        [DataType(DataType.Text)]
        public string StreetAddress { get; set; } = null!;

        [Display(Name = "Zip Code", Prompt = "Zip code")]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; } = null!;

        [Display(Name = "City", Prompt = "City")]
        [DataType(DataType.Text)]
        public string City { get; set; } = null!;

        [Display(Name = "Profile picture", Prompt = "Profile picture")]
        public string? ProfilePictureUrl { get; set; }

        [Display(Name = "Upload picture")]
        public IFormFile? ProfilePictureFile { get; set; }
    }
}