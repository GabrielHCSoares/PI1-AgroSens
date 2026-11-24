namespace Agrosens.Models
{
    public class UserProfile
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string FirstName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return string.Empty;
                var parts = Name.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return parts.Length > 0 ? parts[0] : string.Empty;
            }
        }
    }
}
