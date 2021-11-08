namespace RepairMarketPlace.Infrastructure.Services
{
    public class EmailServerSettings
    {
        public string SMTPServer { get; set; }
        public int SMTPPortNumber { get; set; }
        public string SMTPLogin { get; set; }
        public string SMTPPassword { get; set; }
    }
}
