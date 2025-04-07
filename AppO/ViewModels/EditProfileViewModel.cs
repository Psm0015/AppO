namespace AppO.ViewModels
{
    public class EditProfileViewModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Biography { get; set; }
        public IFormFile? UserImage { get; set; }
        public IFormFile? BannerImage { get; set; }
    }
}
