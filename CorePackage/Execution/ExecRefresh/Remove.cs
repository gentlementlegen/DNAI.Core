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
                AddInput("array", new Variable(new Entity.Type.ListType(value)));
                AddInput("element", new Variable(value));
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