using CorePackage.Entity;
using System.Collections.Generic;

namespace CorePackage.Execution
{
    /// <summary>
    /// Abstract class for element removal operations.
    /// </summary>
    public abstract class ARemove : ExecutionRefreshInstruction
    {
        /// <summary>
        /// Represents the type which is contained in the list
        /// </summary>
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

        /// <summary>
        /// Default constructor which will add a list 'array' input and a boolean 'removed' output
        /// </summary>
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

        /// <summary>
        /// Will call the abstract method RemoveElement and set the output to true or false
        /// </summary>
        public override void Execute()
        {
            outputs["removed"].Value.definition.Value = RemoveElement();
        }

        /// <summary>
        /// Will remove an element and tell if the element was quite removed
        /// </summary>
        /// <returns>True if element was removed, false either</returns>
        protected abstract bool RemoveElement();
    }
}