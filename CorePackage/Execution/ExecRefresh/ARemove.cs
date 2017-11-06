using CorePackage.Entity;
using System.Collections.Generic;

namespace CorePackage.Execution
{
    /// <summary>
    /// Abstract class for element removal operations.
    /// </summary>
    public abstract class ARemove : ExecutionRefreshInstruction
    {
        protected DataType _containerType = Entity.Type.Scalar.Integer;

        /// <summary>
        /// The type of the container.
        /// </summary>
        public DataType ContainerType
        {
            get { return _containerType; }
            set
            {
                GetInput("array").Value.definition.Type = new Entity.Type.ListType(value);
                _containerType = value;
            }
        }

        protected ARemove() : base(
            new Dictionary<string, Variable>
            {
                {
                    "array",
                    new Variable(new Entity.Type.ListType(Entity.Type.Scalar.Integer))
                }
            },
            new Dictionary<string, Variable>
            {
                {
                    "removed",
                    new Variable(Entity.Type.Scalar.Boolean)
                }
            })
        {
        }

        public override void Execute()
        {
            outputs["removed"].Value.definition.Value = RemoveElement();
        }

        protected abstract bool RemoveElement();
    }
}