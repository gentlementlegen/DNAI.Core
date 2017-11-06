using CorePackage.Entity;

namespace CorePackage.Execution
{
    /// <summary>
    /// Removes an element from a collection.
    /// </summary>
    public class Remove : ARemove
    {
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

        public Remove()
        {
            AddInput("element", new Entity.Variable(Entity.Type.Scalar.Integer));
        }

        protected override bool RemoveElement()
        {
            var array = inputs["array"].Value.definition.Value;
            var elem = inputs["element"].Value.definition.Value;
            return array.Remove(elem);
        }
    }
}