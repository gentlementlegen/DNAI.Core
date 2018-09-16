using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class SetAttribute : ExecutionRefreshInstruction
    {
        List<String> attributes = new List<String>();
        Entity.Type.ObjectType stored;

        public SetAttribute(Entity.Type.ObjectType type) : base()
        {
            stored = type;
            AddInput("this", new Entity.Variable(type), true);
            foreach (KeyValuePair<string, Global.IDefinition> curr in type.GetAttributes())
            {
                Entity.Variable definition = new Entity.Variable((Entity.DataType)curr.Value);
                AddInput(curr.Key, definition); //each time the node is executed, input are refreshed
                AddOutput(curr.Key, definition, true); //if the definition of output is the same as the input they will be refreshed too
                attributes.Add(curr.Key);
            }
        }

        public override void Execute()
        {
            Entity.Variable objReference = GetInput("this").Definition;

            foreach (String curr in attributes)
            {
                Input inp = GetInput(curr);

                if (inp.IsValueSet)
                    stored.SetAttributeValue(objReference.Value, curr, inp.Value);
            }
        }
    }
}
