using CorePackage.Entity;
using System;
using System.Collections.Generic;

namespace CorePackage.Execution
{
    public class Foreach : Statement
    {
        /// <summary>
        /// Represents the array of instructions to execute
        /// </summary>
        private readonly ExecutionRefreshInstruction[] nextToExecute = new ExecutionRefreshInstruction[2];

        /// <summary>
        /// Represents the outPoints indexes of the instruction
        /// </summary>
        private enum ForeachIndexes
        {
            OUTLOOP = 0,
            INLOOP = 1,
        }
        
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
        public Foreach(DataType stored = null) : base(2)
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
                nextToExecute[1] = ExecutionPins[(int)ForeachIndexes.INLOOP];
                Element = currList[Index];
                Index++;
            }
            else //if foreach condition is false
            {
                //you only have to execute the code "out loop"
                nextToExecute[0] = ExecutionPins[(int)ForeachIndexes.OUTLOOP];
                nextToExecute[1] = null;
                _shouldReset = true;
            }
            return this.nextToExecute;
        }

        /// <summary>
        /// Link the instruction to the "IN LOOP" index
        /// </summary>
        /// <param name="next">Instruction to execute if while condition is true</param>
        public void Do(ExecutionRefreshInstruction next)
        {
            this.LinkTo((int)ForeachIndexes.INLOOP, next);
        }

        /// <summary>
        /// Link the instruction to the "OUT LOOP" index
        /// </summary>
        /// <param name="next">Instruction to execute if while condition is false</param>
        public void Done(ExecutionRefreshInstruction next)
        {
            this.LinkTo((int)ForeachIndexes.OUTLOOP, next);
        }
    }
}