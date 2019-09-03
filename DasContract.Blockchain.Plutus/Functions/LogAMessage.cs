namespace DasContract.Blockchain.Plutus.Functions
{
    public class LogAMessage : Function
    {
        public LogAMessage ( string name, string message )
        {
            Name = name;
            Head = "MonadWallet m => m()";
            Body = "logMsg " + '"' + message + '"' + '\n';
        }
    }
}
