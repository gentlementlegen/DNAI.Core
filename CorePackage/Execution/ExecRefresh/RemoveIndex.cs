namespace CorePackage.Execution
{
    public class RemoveIndex : ARemove
    {
        public RemoveIndex()
        {
            AddInput("index", new Entity.Variable(Entity.Type.Scalar.Integer));
        }

        protected override bool RemoveElement()
        {
            var array = inputs["array"].Value.definition.Value;
            var idx = inputs["index"].Value.definition.Value;
            try
            {
                array.RemoveAt(idx);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}