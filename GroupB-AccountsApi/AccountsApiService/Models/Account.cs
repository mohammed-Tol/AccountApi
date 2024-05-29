namespace AccountsApiService.Models
{
    public class Account
    {
        public long accountID { get; set; }
        public decimal balance { get; set; }
        public bool hasCheque  { get; set; }
        public int wd_quota   { get; set; }
        public int dp_quota   { get; set; }
        public bool isActive  { get; set; }
        public int customerID  { get; set; }
        public int type_id   { get; set; }
        public string branchID  { get; set; }
    }
}
