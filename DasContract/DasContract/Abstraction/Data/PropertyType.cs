namespace DasContract.Abstraction.Data
{
    public enum PropertyType
    {
        Int, 
        IntCollection,
        Uint, 
        UintCollection,
        Bool, 
        BoolCollection,
        String, 
        StringCollection,
        DateTime, 
        DateTimeCollection,
        Address, 
        AddressCollection,
        AddressPayable,
        AddressPayableCollection,
        Data, //Byte array (for example for files)
        DataCollection,
        Entity,
        EntityCollection
    }
}
