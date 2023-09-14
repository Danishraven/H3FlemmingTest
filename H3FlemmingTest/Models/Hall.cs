namespace H3FlemmingTest.Models
{
    public class Hall
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public List<Seat> Seats { get; set; } = new();
    }
}
