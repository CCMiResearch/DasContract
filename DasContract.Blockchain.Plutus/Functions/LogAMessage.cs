namespace DasContract.Blockchain.Plutus.Functions
{
    public class LogAMessage : Function
    {
        public LogAMessage ()
        {
            Libraries.Add("Wallet");
        }

        public string Message { get; set; }
       
    }
}
