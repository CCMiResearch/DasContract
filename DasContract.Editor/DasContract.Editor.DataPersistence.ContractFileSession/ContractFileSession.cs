using System;
using System.ComponentModel.DataAnnotations;
using DasContract.Editor.Entities;
using DasContract.Editor.Entities.Serialization.XML;

namespace DasContract.Editor.DataPersistence.Entities
{
    public class ContractFileSession
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Contract in a serialized form
        /// </summary>
        public string SerializedContract { get; set; }

        /// <summary>
        /// When the session expires
        /// </summary>
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(10);

        /// <summary>
        /// Resets the expiration date
        /// </summary>
        public void ResetExpirationDate()
        {
            ExpirationDate = DateTime.Now.AddDays(10);
        }

        /// <summary>
        /// Indicates if this session is expired
        /// </summary>
        /// <returns>True if the session is expired, else false</returns>
        public bool IsExpired()
        {
            if (DateTime.Now > ExpirationDate)
                return true;
            return false;
        }

        public static ContractFileSession FromContract(EditorContract contract)
        {
            return new ContractFileSession()
            {
                SerializedContract = EditorContractXML.To(contract)
            };
        }
    }
}
