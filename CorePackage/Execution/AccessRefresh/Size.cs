using CorePackage.Entity;
using System.Collections.Generic;

namespace CorePackage.Execution
{
    /// <summary>
    /// Retrieves the size of a collection.
    /// </summary>
    public class Size : AccessRefreshInstruction
    {
        /// <summary>
        /// Type contained in the list
        /// </summary>
        private DataType _containerType = Entity.Type.Scalar.Integer;

        /// <summary>
        /// The type of the container.
        /// </summary>
        public DataType ContainerType
        {
            get { return _containerType; }
            set
            {
                GetInput("array").Definition.Type = new Entity.Type.ListType(value);
                _containerType = value;
            }
        }

        /// <summary>
        /// Basic default constructor that add a list 'array' input and an integer 'count' output
        /// </summary>
        public Size(DataType type = null) : base()
        {
            AddInput("array", new Variable(new Entity.Type.ListType(Entity.Type.Scalar.Integer)));
            AddOutput("count", new Variable(Entity.Type.Scalar.Integer));
            if (type != null)
                ContainerType = type;
        }

        /// <summary>
        /// Will update the count output in function of the given list size
        /// </summary>
        public override void Execute()
        {
            SetOutputValue("count", GetInput("array").Value.Count);
        }
    }
}