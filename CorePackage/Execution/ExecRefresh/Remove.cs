using CorePackage.Entity;

namespace CorePackage.Execution
{
    /// <summary>
    /// Removes an element from a collection.
    /// </summary>
    public class Remove : ARemove
    {
        /// <summary>
        /// Represents the list contained type
        /// </summary>
        new public DataType ContainerType
        {
            get { return _containerType; }
            set
            {
                GetInput("array").Value.definition.Type = new Entity.Type.ListType(value);
                GetInput("element").Value.definition.Type = value;
                _containerType = value;
            }
        }

        /// <summary>
        /// Basic default constructor which will add an integer 'element' input
        /// </summary>
        public Remove()
        {
            AddInput("element", new Entity.Variable(Entity.Type.Scalar.Integer));
        }

        /// <see cref="ARemove.RemoveElement"/>
        protected override bool RemoveElement()
        {
            var array = inputs["array"].Value.definition.Value;
            var elem = inputs["element"].Value.definition.Value;
            return array.Remove(elem);
        }
    }
}