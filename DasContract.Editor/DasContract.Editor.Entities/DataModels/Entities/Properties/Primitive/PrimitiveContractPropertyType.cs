using System.ComponentModel.DataAnnotations;

namespace DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive
{
    public enum PrimitiveContractPropertyType
    {
        [Display(Name = "Number")]
        Number,

        [Display(Name = "Collection of numbers")]
        NumberCollection,

        [Display(Name = "Positive number")]
        UnsignedNumber,

        [Display(Name = "Collection of positive numbers")]
        UnsignedCollection,

        [Display(Name = "Boolean")]
        Bool,

        [Display(Name = "Collection of booleans")]
        BoolCollection,

        [Display(Name = "Text")]
        Text,

        [Display(Name = "Collection of texts")]
        TextCollection,

        [Display(Name = "Date and time")]
        DateTime,

        [Display(Name = "Collection of dates and times")]
        DateTimeCollection,

        [Display(Name = "Address")]
        Address,

        [Display(Name = "Collection of addresses")]
        AddressCollection,

        [Display(Name = "Billable address")]
        AddressPayable,

        [Display(Name = "Collection of billable adresses")]
        AddressPayableCollection,

        [Display(Name = "Binary array (file, ...)")]
        Data, //Byte array (for example for files)

        [Display(Name = "Collection of binary arrays (files, ...)")]
        DataCollection, //Array of byte arrays (multiple files)
    }
}
