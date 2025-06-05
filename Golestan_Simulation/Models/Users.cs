namespace Golestan_Simulation.Models
{
    public class Users
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
    }
}
