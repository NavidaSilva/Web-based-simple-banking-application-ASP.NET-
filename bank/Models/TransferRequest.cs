namespace bank.Models
{
    public class TransferRequest
    {
        public int FromAccNumber { get; set; }
        public int ToAccNumber { get; set; }
        public double Amount { get; set; }
    }
}


