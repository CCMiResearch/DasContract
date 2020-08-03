using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DasContract.Editor.Migrator.Migrations.UpDownMigration
{
    public class UpDownMigration<TProperty>: Migration
    {
        protected Action ActionUp { get; set; }

        protected Action ActionDown { get; set; }

        public TProperty LastKnownValue { get; set; }

        protected bool IsUp { get; set; } = true;

        public UpDownMigration(Expression propertyExpression,
            Action up,
            Action down)
        {
            PropertyExpression = propertyExpression;
            ActionUp = up;
            ActionDown = down;
        }

        public override void Down()
        {
            if (!IsUp)
                return;

            IsUp = false;
            ActionDown();
        }

        public override void Up()
        {
            if (IsUp)
                return;

            IsUp = true;
            ActionUp();
        }
    }
}
