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
                GetInput("element").Value.definition.Type = value;
                GetInput("array").Value.definition.Type = new Entity.Type.ListType(value);
                _containerType = value;
            }
        }

        /// <summary>
        /// The Add node has two inputs : the collection and the element to add.
        /// </summary>
        public Append() : base(
            new Dictionary<string, Variable>
            {
                {
                    "array",
                    new Variable(new Entity.Type.ListType(Entity.Type.Scalar.Integer))
                },
                {
                    "element",
                    new Variable(Entity.Type.Scalar.Integer)
                }
            },
            new Dictionary<string, Variable>
            {
                {
                    "count",
                    new Variable(Entity.Type.Scalar.Integer)
                }
            })
        {

        }

        public override void Execute()
        {
            var val = inputs["array"].Value.definition.Value;
            val?.Add(inputs["element"].Value.definition.Value);
            outputs["count"].Value.definition.Value = val?.Count;
        }
    }
}