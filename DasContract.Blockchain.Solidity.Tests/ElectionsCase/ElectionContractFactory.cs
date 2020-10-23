using DasContract.Abstraction;
using System.Collections.Generic;
using DasContract.Abstraction.Processes;
using System.Linq;

namespace DasContract.Blockchain.Solidity.Tests.ElectionsCase
{
    public static class ElectionContractFactory
    {

        public static Contract CreateContract()
        {
            return new Contract
            {
                Id = "Contract",
                DataTypes = ElectionDataModelFactory.CreateDataModel().ToDictionary(d => d.Id),
                Processes = new List<Process>
                {
                    ElectionsProcessFactory.CreateElectionsProcess(),
                    CountryElectionsProcessFactory.CreateCountryElectionsProcess()
                }
            };
        }

        

    }
}
