using CorePackage.Entity;
using System.Collections.Generic;

namespace CorePackage.Execution
{
    /// <summary>
    /// Appends element to a collection.
    /// </summary>
    public class Append : ExecutionRefreshInstruction
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
                GetInput("element").Definition.Type = value;
                GetInput("array").Definition.Type = new Entity.Type.ListType(value);
                _containerType = value;
            }
        }

        /// <summary>
        /// The Add node has two inputs : the collection and the element to add.
        /// </summary>
        public Append() : base()
        {
            AddInput("array", new Variable(new Entity.Type.ListType(Entity.Type.Scalar.Integer)));
            AddInput("element", new Variable(Entity.Type.Scalar.Integer));
            AddOutput("count", new Variable(Entity.Type.Scalar.Integer));
        }

        /// <summary>
        /// Will append an element to the given list
        /// </summary>
        public override void Execute()
        {
            var val = GetInputValue("array");

            val?.Add(GetInputValue("element"));
            SetOutputValue("count", val?.Count);
        }
    }
}