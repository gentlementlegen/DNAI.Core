using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Entity
{
    /// <summary>
    /// Represents a variable definition
    /// </summary>
    public class Variable : Global.Definition
    {
        /// <summary>
        /// A variable must have an associated type
        /// </summary>
        private DataType type = null;

        /// <summary>
        /// Represents a variable value
        /// </summary>
        private dynamic value = null;

        /// <summary>
        /// Basic default construction which is needed by factory
        /// </summary>
        public Variable()
        {

        }

        /// <summary>
        /// Constructor that asks for type and value
        /// </summary>
        /// <param name="type">Type of the variable</param>
        /// <param name="value">Value of the variable</param>
        public Variable(DataType type, dynamic value = null)
        {
            Type = type;
            if (value != null) Value = value;
        }

        /// <summary>
        /// Make type attribute public in read only
        /// </summary>
        public DataType Type
        {
            get { return type; }
            set {
                type = value;
                if (Type != null) Value = Type.Instantiate();
            }
        }

        /// <summary>
        /// Make value attribute public in read write
        /// Write will check if the given value is valid for the variable type
        /// </summary>
        public dynamic Value
        {
            get { return value; }
            set
            {
                if (type == null)
                {
                    throw new InvalidOperationException("Type haven't been set to variable yet");
                }

                if (type.IsValueOfType(value))
                {
                    this.value = value;
                }
                else
                {
                    throw new ArgumentException("Trying to set a value of a type inconsistant with " + this.type.GetType() + ": " + value.ToString());
                }
            }
        }

        /// <see cref="Global.IDefinition.IsValid"/>
        public override bool IsValid()
        {
            return this.value != null && this.type != null && this.type.IsValueOfType(this.value);
        }
    }
}
