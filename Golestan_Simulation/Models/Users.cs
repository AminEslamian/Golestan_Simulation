namespace Golestan_Simulation.Models
{
    public class Users
    {
        public int Id { get; set; }
        public DateTime Created_At { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Hashed_Password { get; set; }
    }
}
