namespace CorePackage.Execution
{
    /// <summary>
    /// Instruction which will remove an element from a list at a specific index
    /// </summary>
    public class RemoveIndex : ARemove
    {
        /// <summary>
        /// Basic default constructor which will add an integer 'index' input
        /// </summary>
        public RemoveIndex()
        {
            AddInput("index", new Entity.Variable(Entity.Type.Scalar.Integer));
        }

        ///<see cref="ARemove.RemoveElement"/>
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