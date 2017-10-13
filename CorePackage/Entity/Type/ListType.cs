using System;
using System.Collections.Generic;

namespace CorePackage.Entity.Type
{
    /// <summary>
    /// Represent a list type definition
    /// </summary>
    public class ListType : DataType
    {
        /// <summary>
        /// Represents the type which will be stored into list
        /// </summary>
        private readonly DataType stored;

        /// <summary>
        /// Constructor that asks for the type stored
        /// </summary>
        /// <param name="stored"></param>
        public ListType(DataType stored)
        {
            this.stored = stored;
        }

        /// <summary>
        /// <see cref="DataType.Instantiate"/>
        /// </summary>
        public override dynamic Instantiate()
        {
            return Activator.CreateInstance(typeof(List<>).MakeGenericType(stored.Instantiate().GetType()));
        }

        /// <see cref="Global.Definition.IsValid"/>
        public override bool IsValid()
        {
            return stored != null;
        }

        /// <see cref="DataType.IsValueOfType(dynamic)"/>
        public override bool IsValueOfType(dynamic value)
        {
            return stored.IsValueOfType(value);
        }
    }
}