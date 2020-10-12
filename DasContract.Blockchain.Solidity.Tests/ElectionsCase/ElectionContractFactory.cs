using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Data.Tokens;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DasContract.Abstraction.Processes;

namespace DasContract.Blockchain.Solidity.Tests.ElectionsCase
{
    public static class ElectionContractFactory
    {

        public static Contract CreateContract()
        {
            return new Contract
            {
                Id = "Contract",
                DataTypes = ElectionDataModelFactory.CreateDataModel(),
                Processes = new List<Process>
                {
                    ElectionsProcessFactory.CreateElectionsProcess(),
                    CountryElectionsProcessFactory.CreateCountryElectionsProcess()
                }
            };
        }

        

    }
}
