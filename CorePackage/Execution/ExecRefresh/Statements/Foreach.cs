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
                outputs["index"].Value.definition.Value = value;
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
                outputs["element"].Value.definition.Value = value;
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
                ((Entity.Type.ListType)GetInput("array").Value.definition.Type).Stored = value;
                GetOutput("element").Value.definition.Type = value;
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
        public Foreach(DataType stored = null) :
            base(
                new Dictionary<string, Variable>
                {
                    { "array", new Variable(new Entity.Type.ListType(Entity.Type.Scalar.Integer)) }
                },
                new Dictionary<string, Variable>
                {
                    { "index", new Variable(Entity.Type.Scalar.Integer) },
                    { "element", new Variable() }
                }, 2)
        {
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
                nextToExecute[1] = OutPoints[(int)ForeachIndexes.INLOOP];
                Element = currList[Index];
                Index++;
            }
            else //if foreach condition is false
            {
                //you only have to execute the code "out loop"
                nextToExecute[0] = OutPoints[(int)ForeachIndexes.OUTLOOP];
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