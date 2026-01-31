namespace Server.Dtos
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Type { get; set; } = "user"; // "user" или "admin"
    }
}