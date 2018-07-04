using CorePackage.Entity;
using System;
using System.Collections.Generic;

namespace CorePackage.Execution
{
    public class Foreach : Loop
    {
        private int _index;
        /// <summary>
        /// Current index in the collection.
        /// </summary>
        public int Index
        {
            get { return _index; }
            private set
            {
                _index = value;
                SetOutputValue("index", value);
            }
        }

        private dynamic _element;
        /// <summary>
        /// Current element in the collection.
        /// </summary>
        public dynamic Element
        {
            get { return _element; }
            private set
            {
                _element = value;
                SetOutputValue("element", value);
            }
        }

        private DataType _containerType = Entity.Type.Scalar.Integer;
        /// <summary>
        /// The type of the container to iterate in.
        /// </summary>
        public DataType ContainerType
        {
            get { return _containerType; }
            set
            {
                ((Entity.Type.ListType)GetInput("array").Definition.Type).Stored = value;
                GetOutput("element").Definition.Type = value;
                _containerType = value;
            }
        }

        /// <summary>
        /// True if node finished its job and should reset on next call.
        /// </summary>
        private bool _shouldReset;

        /// <summary>
        /// Default constructor that initialises input "array" as array and set 2 outpoints capacity
        /// </summary>
        public Foreach(DataType stored = null) : base()
        {
            AddInput("array", new Variable(new Entity.Type.ListType(Entity.Type.Scalar.Integer)));
            AddOutput("index", new Variable(Entity.Type.Scalar.Integer));
            AddOutput("element", new Variable());
            if (stored != null)
                ContainerType = stored;
        }

        ///<see cref="ExecutionRefreshInstruction.GetNextInstructions"/>
        public override ExecutionRefreshInstruction[] GetNextInstructions()
        {
            var currList = GetInputValue("array");
            if (_shouldReset)
            {
                Index = 0;
                Element = ContainerType.Instantiate();
            }
            if (currList?.Count > 0 && Index < currList.Count) //if foreach condition is true
            {
                _shouldReset = false;
                nextToExecute[0] = this;
                nextToExecute[1] = GetDoInstruction();
                Element = currList[Index];
                Index++;
            }
            else //if foreach condition is false
            {
                //you only have to execute the code "out loop"
                nextToExecute[0] = GetDoneInstruction();
                nextToExecute[1] = null;
                _shouldReset = true;
            }
            return this.nextToExecute;
        }
    }
}