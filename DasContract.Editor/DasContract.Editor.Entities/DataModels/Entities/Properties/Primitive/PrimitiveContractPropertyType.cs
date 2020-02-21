namespace DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive
{
    public enum PrimitiveContractPropertyType
    {
        Number,
        NumberCollection,
        UnsignedNumber,
        UnsignedCollection,
        Bool,
        BoolCollection,
        Text,
        TextCollection,
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
