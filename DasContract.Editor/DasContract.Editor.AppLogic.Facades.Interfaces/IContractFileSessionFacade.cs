using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.DataPersistence.Entities;
using DasContract.Editor.Interfaces;

namespace DasContract.Editor.AppLogic.Facades.Interfaces
{
    public interface IContractFileSessionFacade : ICRUDInterfaceAsync<ContractFileSession, string>
    {

    }
}
