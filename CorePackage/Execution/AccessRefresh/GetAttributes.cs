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

        public GetAttributes(Entity.Type.ObjectType typetosplit) : base()
        {
            AddInput("this", new Entity.Variable(typetosplit));
            this._stored = typetosplit;
            foreach (KeyValuePair<string, IDefinition> attr in typetosplit.GetAttributes())
            {
                AddOutput(attr.Key, new Entity.Variable((Entity.DataType)attr.Value));
            }
        }

        public override void Execute()
        {
            Dictionary<string, dynamic> value = GetInputValue("this");

            foreach (KeyValuePair<string, IDefinition> attr in _stored.GetAttributes())
            {
                SetOutputValue(attr.Key, value[attr.Key]);
            }
        }
    }
}
