﻿using CorePackage.Entity;

namespace CorePackage.Execution
{
    /// <summary>
    /// Instruction to insert an element at a spécific index
    /// </summary>
    public class Insert : Append
    {
        /// <summary>
        /// Default constructor which will add an integer 'index' input
        /// </summary>
        public Insert(DataType type = null) : base(type)
        {
            AddInput("index", new Variable(Entity.Type.Scalar.Integer));
        }

        /// <summary>
        /// Will append the given element at the given index into the given list
        /// </summary>
        public override void Execute()
        {
            var val = GetInputValue("array");

            val?.Insert((int)GetInputValue("index"), System.Convert.ChangeType(GetInputValue("element"), val?.GetType().GetGenericArguments()[0]));
            SetOutputValue("count", val?.Count);
        }
    }
}