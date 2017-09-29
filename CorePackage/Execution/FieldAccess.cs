using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class FieldAccess : AccessRefreshInstruction
    {
        public FieldAccess(Entity.Variable toget) :
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { "reference", toget }
                },
                new Dictionary<string, Entity.Variable>())
        {
            if (toget.Type.GetType() != typeof(Entity.Type.ObjectType))
                throw new InvalidOperationException("Given variable is not an object");
            
            //add output for each field;
        }

        public override void Execute()
        {
            //refresh outputs value
            throw new NotImplementedException();
        }
    }
}
