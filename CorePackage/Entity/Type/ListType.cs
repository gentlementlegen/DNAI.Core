using System;
using System.Collections.Generic;

namespace CorePackage.Entity.Type
{
    /// <summary>
    /// Represent a list type definition
    /// </summary>
    public class ListType : DataType
    {
        private DataType stored = null;

        /// <summary>
        /// Represents the type which will be stored into list
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

        private System.Type _listType;

        public ListType()
        {

        }

        /// <summary>
        /// Constructor that asks for the type stored
        /// </summary>
        /// <param name="stored"></param>
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