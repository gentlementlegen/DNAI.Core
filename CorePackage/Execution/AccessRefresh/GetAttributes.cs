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
        private List<String> attrs = new List<string>();
        private Entity.Type.ObjectType _stored;

        public GetAttributes(Entity.Type.ObjectType typetosplit) : base()
        {
            AddInput("this", new Entity.Variable(typetosplit), true);
            this._stored = typetosplit;
            attrs = _stored.GetAttributes().Keys.ToList();
            foreach (KeyValuePair<string, IDefinition> attr in typetosplit.GetAttributes())
            {
                AddOutput(attr.Key, new Entity.Variable((Entity.DataType)attr.Value));
            }
        }

        public override void Execute()
        {
            dynamic value = GetInputValue("this");

            foreach (string curr in attrs)
            {
                SetOutputValue(curr, _stored.GetAttributeValue(value, curr));
            }
        }
    }
}
