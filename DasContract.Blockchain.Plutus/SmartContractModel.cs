using DasContract.Blockchain.Plutus.Properties;
using DasContract.Blockchain.Plutus.Functions;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Blockchain.Plutus
{
    public class SmartContractModel
    {
        public string ProcessName { get; set; }

        public Contract Contract { get; set; }

        public IList<string> Libraries { get; set; } = new List<string>();

        public IList<Function> Functions { get; set; } = new List<Function>();

        public IList<Function> WalletFunctions { get; set; } = new List<Function>();

        public IList<Function> Endpoints { get; set; } = new List<Function>();

        public IList<SCStateMachine> StateMachines { get; set; } = new List<SCStateMachine>();

        public IList<SCFact> Facts { get; set; } = new List<SCFact>();

        public IList<SCTransactionKind> TransactionKinds { get; set; } = new List<SCTransactionKind>();

        public void CreateStructureFromContract ( Contract contract )
        {
            ProcessName = contract.Name.Replace(" ", string.Empty);
            AddLibraries();

            AddFactsWithAttributes(contract);
            AddTransactionKindsWithFacts(contract);

            AddProcessIdFunction();
            AddFactFunctions();
            AddStateInputFunctions();
            AddTransactionStateMachineFunctions();

            AddParametersFunctions();
            AddStateMachineErrors();
            AddEndpointFunctions();
        }

        public void AddLibraries ()
        {
            AddLibraries(new string[]
            {
                "Playground.Contract",
                "Language.Plutus.Contract",
                "Language.PlutusTx.Eq as E",
                "Ledger",
                "Ledger.Value                     (Value, geq)",
                "Ledger.Ada                     as ADA",
                "Control.Applicative              (pure, (<|>))",
                "Control.Monad                    (void)",
                "Control.Monad.Error.Lens",
                "Prelude",
                "Language.PlutusTx.List(uniqueElement, find)",
                "Ledger.Validation(findContinuingOutputs)",
                "Data.Text(Text)",
                "Wallet.Emulator",
                "Data.ByteString.Lazy.UTF8 as BLU",
                "Wallet.Emulator.Wallet",
                "Ledger.Typed.Scripts",
                "Language.Plutus.Contract",
                "Language.PlutusTx",
                "Language.PlutusTx.Numeric(zero)",
                "Ledger.AddressMap",
                "Language.Plutus.Contract.StateMachine",
                "Control.Monad.Freer",
                "Language.PlutusTx.AssocMap",
                "qualified Data.Map as Map",
                "Data.Map(Map)",
                "qualified Data.Set as Set",
                "Data.Maybe",
                "Ledger.Interval(always)",
                "Control.Lens(makeClassyPrisms)"
            });
        }

        public void AddFactsWithAttributes ( Contract contract )
        {
            foreach (DataModel dm in contract.DataModels)
                foreach (EntityType et in dm.EntityTypes)
                {
                    var attributes = new List<SCAttribute>();
                    foreach (AttributeType at in dm.AttributeTypes)
                        if ( et.Id == at.EntityType )
                        {
                            var attr = new SCAttribute()
                            {
                                Name = at.Name.Replace(" ", string.Empty),
                                Type = at.ValueType.ToString()
                            };
                            attributes.Add(attr);
                        }
                    Facts.Add(new SCFact() { Name = et.Name.Replace(" ", string.Empty), Attributes = attributes });
                }
        }

        public void AddTransactionKindsWithFacts ( Contract contract )
        {
            foreach (TransactionKind tk in contract.TransactionKinds)
            {
                TransactionKinds.Add(new SCTransactionKind()
                {
                    Name = tk.Name.Replace(" ", string.Empty),
                    Fact = Facts.FirstOrDefault(s => tk.Name.Replace(" ", string.Empty).Contains(s.Name))
                });
            }
        }

        public void AddProcessIdFunction ()
        {
            var dataType = new DataType()
            {
                Name = ProcessName,
                MakeLift = true,
                MakeIsData = false,
                TemplateSourceCode = Resources.DataType
            };
            dataType.AddMember($"id{dataType.Name}", "ByteString");
            dataType.AddDerivingType("Prelude.Eq");
            dataType.AddDerivingType("Prelude.Show");
            dataType.AddDerivingType("Generic");
            dataType.AddDerivingType("IotsType");
            dataType.AddDerivingType("ToSchema");

            AddFunction(dataType);
        }

        public void AddFactFunctions ()
        {
            foreach (SCFact fact in Facts)
            {
                var dataType = new DataType()
                {
                    Name = $"Fact{fact.Name}",
                    MakeLift = true,
                    MakeIsData = true,
                    TemplateSourceCode = Resources.DataType
                };
                foreach (SCAttribute attr in fact.Attributes)
                    dataType.AddMember($"attr{attr.Name}", $"{attr.Type}");
                dataType.AddDerivingType("E.Eq");
                dataType.AddDerivingType("Prelude.Eq");
                dataType.AddDerivingType("Prelude.Show");
                dataType.AddDerivingType("Prelude.Ord");
                dataType.AddDerivingType("Generic");
                dataType.AddDerivingType("IotsType");
                dataType.AddDerivingType("ToSchema");

                AddFunction(dataType);
            }
        }

        public void AddParametersFunctions ()
        {
            foreach (SCTransactionKind tk in TransactionKinds )
                foreach (string ca in tk.CActs)
                {
                    var dataType = new DataType()
                    {
                        Name = $"Parameters{tk.Name}{ca}",
                        MakeLift = false,
                        MakeIsData = false,
                        TemplateSourceCode = Resources.DataType
                    };
                    dataType.AddMember($"param{tk.Name}{ca}Id{ProcessName}", "ByteString");
                    if (Equals(ca, "Initial"))
                    {
                        dataType.AddMember($"param{tk.Name}{ca}InWallet", "PubKey");
                        dataType.AddMember($"param{tk.Name}{ca}ExWallet", "PubKey");
                    }
                    if (tk.Fact != null)
                        foreach (SCAttribute attr in tk.Fact.Attributes)
                        {
                            dataType.AddMember($"param{tk.Name}{ca}{attr.Name}", $"{attr.Type}");
                        }
                    dataType.AddDerivingType("Prelude.Eq");
                    dataType.AddDerivingType("Prelude.Show");
                    dataType.AddDerivingType("Generic");
                    dataType.AddDerivingType("FromJSON");
                    dataType.AddDerivingType("ToJSON");
                    dataType.AddDerivingType("IotsType");
                    dataType.AddDerivingType("ToSchema");

                    AddFunction(dataType);
                }  
        }

        public void AddStateInputFunctions ()
        {
            AddFunction(new Function()
            {
                Name = "Role",
                TemplateSourceCode = Resources.SimpleTransactionRole
            });
            foreach (SCFact fct in Facts)
            {
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"TransactionKind{fct.Name}State",
                    TemplateSourceCode = Resources.TransactionState,
                    Fact = fct.Name
                });
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"TransactionKind{fct.Name}Action",
                    TemplateSourceCode = Resources.TransactionInput,
                    Fact = fct.Name
                });
            }
        }

        public void AddTransactionStateMachineFunctions ()
        {
            foreach (SCTransactionKind tk in TransactionKinds)
            {
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"transition{tk.Name}",
                    TransactionKindName = tk.Name,
                    Fact = tk.Fact.Name,
                    ProcessName = ProcessName,
                    TemplateSourceCode = Resources.TransactionTransition
                });
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"check{tk.Name}",
                    TransactionKindName = tk.Name,
                    Fact = tk.Fact.Name,
                    ProcessName = ProcessName,
                    TemplateSourceCode = Resources.TransactionCheck
                });
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"isFinal{tk.Name}",
                    TransactionKindName = tk.Name,
                    Fact = tk.Fact.Name,
                    ProcessName = ProcessName,
                    TemplateSourceCode = Resources.TransactionFinal
                });
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"machine{tk.Name}",
                    TransactionKindName = tk.Name,
                    Fact = tk.Fact.Name,
                    ProcessName = ProcessName,
                    TemplateSourceCode = Resources.TransactionStateMachine
                });
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"validator{tk.Name}",
                    TransactionKindName = tk.Name,
                    Fact = tk.Fact.Name,
                    ProcessName = ProcessName,
                    TemplateSourceCode = Resources.TransactionValidator
                });
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"script{tk.Name}",
                    TransactionKindName = tk.Name,
                    Fact = tk.Fact.Name,
                    ProcessName = ProcessName,
                    TemplateSourceCode = Resources.TransactionScriptInstance
                });
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"instance{tk.Name}",
                    TransactionKindName = tk.Name,
                    Fact = tk.Fact.Name,
                    ProcessName = ProcessName,
                    TemplateSourceCode = Resources.TransactionStateMachineInstance
                });
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"allocate{tk.Name}",
                    TransactionKindName = tk.Name,
                    Fact = tk.Fact.Name,
                    ProcessName = ProcessName,
                    TemplateSourceCode = Resources.TransactionAllocation
                });
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"client{tk.Name}",
                    TransactionKindName = tk.Name,
                    Fact = tk.Fact.Name,
                    ProcessName = ProcessName,
                    TemplateSourceCode = Resources.TransactionStateMachineClient
                });
            }
        }

        public void AddEndpointFunctions ()
        {
            foreach (SCTransactionKind tk in TransactionKinds)
                foreach (string ca in tk.CActs)
                {
                    var endpoint = new TransactionStateMachine()
                    {
                        Name = $"do{tk.Name}{ca}",
                        TransactionKindName = tk.Name,
                        CoordinationAct = ca,
                        ProcessName = ProcessName,
                        TemplateSourceCode = Resources.EndpointFunction,
                        Parameters= $"Parameters{tk.Name}{ca}"
                    };
                    AddWalletFunction(endpoint);
                    Endpoints.Add(endpoint);
                }   
        }

        public void AddStateMachineErrors ()
        {
            foreach (SCFact fct in Facts)
            {
                AddFunction(new TransactionStateMachine()
                {
                    Name = $"ErrorTransactionKind{fct.Name}",
                    ProcessName = ProcessName,
                    Fact = fct.Name,
                    TemplateSourceCode = Resources.TransactionStateMachineError
                });
            }
        }

        public void AddLibraries ( IList<string> libraries )
        {
            ((List<string>)Libraries).AddRange(libraries.Where(l => !LibraryAlreadyIncluded(l)));
        }

        private bool LibraryAlreadyIncluded ( string name )
        {
            if (Libraries.Any(l => Equals(name, l)))
            {
                return true;
            }
            return false;
        }

        public void AddWalletFunction ( Function newFunction )
        {
            if (FunctionNameAlreadyExists(newFunction.Name))
            {
                return;
            }
            AddLibraries(newFunction.Libraries);
            newFunction.RenderTemplate();
            WalletFunctions.Add(newFunction);
        }

        public void AddFunction(Function newFunction)
        {
            if (FunctionNameAlreadyExists(newFunction.Name))
            {
                return;
            }
            AddLibraries(newFunction.Libraries);
            newFunction.RenderTemplate();
            Functions.Add(newFunction);
        }

        private bool FunctionNameAlreadyExists ( string name )
        {
            if (Functions.Any(f => Equals(name, f.Name)) || WalletFunctions.Any(wf => Equals(name, wf.Name)))
            {
                return true;
            }
            return false;
        }
    }
}
