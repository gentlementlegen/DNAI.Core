using CorePackage.Entity;

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
        public RemoveIndex(DataType type = null) : base(type)
        {
            AddInput("index", new Entity.Variable(Entity.Type.Scalar.Integer));
        }

        ///<see cref="ARemove.RemoveElement"/>
        protected override bool RemoveElement()
        {
            try
            {
                GetInputValue("array").RemoveAt(GetInputValue("index"));
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}