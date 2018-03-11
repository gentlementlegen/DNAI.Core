using CorePackage.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class GetAttributes : AccessRefreshInstruction
    {
        private Entity.Type.ObjectType _stored;

        public GetAttributes(Entity.Type.ObjectType typetosplit) :
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { "this", new Entity.Variable(typetosplit) }
                },
                new Dictionary<string, Entity.Variable>())
        {
            this._stored = typetosplit;
            foreach (KeyValuePair<string, Definition> attr in typetosplit.GetAttributes())
            {
                AddOutput(attr.Key, new Entity.Variable((Entity.DataType)attr.Value));
            }
        }

        public override void Execute()
        {
            Dictionary<string, dynamic> value = GetInput("this").Value.definition.Value;

            foreach (KeyValuePair<string, Definition> attr in _stored.GetAttributes())
            {
                outputs[attr.Key].Value.definition.Value = value[attr.Key];
            }
        }
    }
}
