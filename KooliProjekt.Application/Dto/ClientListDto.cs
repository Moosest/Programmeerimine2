namespace KooliProjekt.Application.Dto
{
    public class ClientItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal Discount { get; set; }
        public bool IsDone { get; set; }
    }
}