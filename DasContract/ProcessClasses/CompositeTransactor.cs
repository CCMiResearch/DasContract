namespace DasContract.ProcessClasses
{
    public class CompositeTransactor : Transactor
    {
        public ElementaryTransactor[] Children;
    }
}