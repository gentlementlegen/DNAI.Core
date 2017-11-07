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
        /// Attribute that represents the internal list stored type
        /// </summary>
        private DataType stored = null;

        /// <summary>
        /// Basic getter and Advanced setter for stored type
        /// </summary>
        public DataType Stored
        {
            get { return stored; }
            set
            {
                if (value != null)
                {
                    _listType = typeof(List<>).MakeGenericType(value.Instantiate().GetType());
                    stored = value;
                }
            }
        }

        /// <summary>
        /// Represents the real type of the list
        /// </summary>
        private System.Type _listType;

        /// <summary>
        /// Basic default constructor which is necessary for factory
        /// </summary>
        public ListType()
        {

        }

        /// <summary>
        /// Constructor that asks for the type stored
        /// </summary>
        /// <param name="stored">List stored type</param>
        public ListType(DataType stored)
        {
            this.Stored = stored;
        }

        /// <summary>
        /// <see cref="DataType.Instantiate"/>
        /// </summary>
        public override dynamic Instantiate()
        {
            return Activator.CreateInstance(_listType);
        }

        /// <see cref="Global.Definition.IsValid"/>
        public override bool IsValid()
        {
            return Stored != null;
        }

        /// <summary>
        /// <see cref="DataType.IsValueOfType(dynamic)"/>
        /// </summary>
        public override bool IsValueOfType(dynamic value)
        {
            return _listType == value.GetType();
        }
    }
}