using CorePackage.Entity;

namespace CorePackage.Execution
{
    /// <summary>
    /// Instruction to insert an element at a spécific index
    /// </summary>
    public class Insert : Append
    {
        /// <summary>
        /// Default constructor which will add an integer 'index' input
        /// </summary>
        public Insert()
        {
            AddInput("index", new Variable(Entity.Type.Scalar.Integer));
        }

        /// <summary>
        /// Will append the given element at the given index into the given list
        /// </summary>
        public override void Execute()
        {
            var val = inputs["array"].Value.definition.Value;
            val?.Insert(inputs["index"].Value.definition.Value, inputs["element"].Value.definition.Value);
            outputs["count"].Value.definition.Value = val?.Count;
        }
    }
}