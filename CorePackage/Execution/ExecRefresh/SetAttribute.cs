using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class SetAttribute : ExecutionRefreshInstruction
    {
        Entity.Variable toset;
        Dictionary<string, Global.Definition> attributes;

        public SetAttribute(Entity.Variable obj) :
            base(new Dictionary<string, Entity.Variable> { },
                new Dictionary<string, Entity.Variable> { })
        {
            toset = obj;
            Entity.Type.ObjectType type = obj.Type as Entity.Type.ObjectType;

            if (type == null)
                throw new InvalidOperationException("Given variable have to be of object type");

            attributes = type.GetAttributes();

            foreach (KeyValuePair<string, Global.Definition> curr in attributes)
            {
                AddInput(curr.Key, new Entity.Variable((Entity.DataType)curr.Value));
            }
        }

        public override void Execute()
        {
            foreach (KeyValuePair<string, Global.Definition> curr in attributes)
            {
                toset.Value[curr.Key] = GetInputValue(curr.Key);
                //outputs[curr.Key].Value.definition.Value = GetInputValue(curr.Key);
            }
        }
    }
}
