using CorePackage.Entity;
using System.Collections.Generic;

namespace CorePackage.Execution
{
    /// <summary>
    /// Retrieves the size of a collection.
    /// </summary>
    public class Size : ExecutionRefreshInstruction
    {
        private DataType _containerType = Entity.Type.Scalar.Integer;

        /// <summary>
        /// The type of the container.
        /// </summary>
        public DataType ContainerType
        {
            get { return _containerType; }
            set
            {
                AddInput("array", new Variable(new Entity.Type.ListType(value)));
                _containerType = value;
            }
        }

        public Size() : base(new Dictionary<string, Variable> {
            { "array", new Variable(new Entity.Type.ListType(Entity.Type.Scalar.Integer)) } })
        {
            AddOutput("count", new Variable(Entity.Type.Scalar.Integer));
        }

        public override void Execute()
        {
            outputs["count"].Value.definition.Value = inputs["array"].Value.definition.Value.Count;
        }
    }
}