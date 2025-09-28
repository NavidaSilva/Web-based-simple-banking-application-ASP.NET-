namespace bank.Models
{
    public class Transact
    {

        public int transId { get; set; }
        public int accNumber { get; set; }
        public string? type { get; set; }
        public double amount { get; set; }
        public string? transDate { get; set; }
    }
}

