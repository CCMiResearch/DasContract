namespace DasContract.Abstraction.DataModel.Property.Primitive
{
    public enum PrimitiveContractPropertyType
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
        DataCollection, //Array of byte arrays (multiple files)
    }
}
