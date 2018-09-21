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
        private Entity.Type.ListType _listType;

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
                _listType = new Entity.Type.ListType(_containerType);
            }
        }

        /// <summary>
        /// The Add node has two inputs : the collection and the element to add.
        /// </summary>
        public Append(DataType type = null) : base()
        {
            AddInput("array", new Variable(new Entity.Type.ListType(Entity.Type.Scalar.Integer)), true);
            AddInput("element", new Variable(Entity.Type.Scalar.Integer));
            AddOutput("count", new Variable(Entity.Type.Scalar.Integer));
            if (type != null)
                ContainerType = type;
        }

        /// <summary>
        /// Will append an element to the given list
        /// </summary>
        public override void Execute()
        {
            dynamic val = GetInputValue("array");
            System.Type valType = val.GetType();

            Input input = GetInput("element");
            dynamic item = input.Definition.Type.GetDeepCopyOf(input.Value, valType.GenericTypeArguments[0]);

            val?.Add(item);
            SetOutputValue("count", val?.Count);
        }
    }
}