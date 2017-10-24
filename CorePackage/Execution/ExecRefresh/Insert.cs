using CorePackage.Entity;

namespace CorePackage.Execution
{
    public class Insert : Add
    {
        public Insert()
        {
            AddInput("index", new Variable(Entity.Type.Scalar.Integer));
        }

        public override void Execute()
        {
            var val = inputs["array"].Value.definition.Value;
            val?.Insert(inputs["index"].Value.definition.Value, inputs["element"].Value.definition.Value);
            outputs["count"].Value.definition.Value = val?.Count;
        }
    }
}