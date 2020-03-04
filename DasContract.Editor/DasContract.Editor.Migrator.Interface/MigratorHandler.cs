using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Migrator
{
    public delegate void MigratorHandler(IMigrator caller, IMigratorArgs args);
}
