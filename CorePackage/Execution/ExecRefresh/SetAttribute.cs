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
        Dictionary<string, Global.IDefinition> attributes;

        public SetAttribute(Entity.Variable obj) : base()
        {
            toset = obj;
            Entity.Type.ObjectType type = obj.Type as Entity.Type.ObjectType;

            if (type == null)
                throw new InvalidOperationException("Given variable have to be of object type");

            attributes = type.GetAttributes();

            foreach (KeyValuePair<string, Global.IDefinition> curr in attributes)
            {
                Entity.Variable definition = new Entity.Variable((Entity.DataType)curr.Value);
                AddInput(curr.Key, definition); //each time the node is executed, input are refreshed
                AddOutput(curr.Key, definition); //if the definition of output is the same as the input they will be refreshed too
            }
        }

        public override void Execute()
        {
            foreach (KeyValuePair<string, Global.IDefinition> curr in attributes)
            {
                toset.Value[curr.Key] = GetInputValue(curr.Key);
            }
        }
    }
}
