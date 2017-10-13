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
        private DataType type;

        /// <summary>
        /// Represents a variable value
        /// </summary>
        private dynamic value;

        /// <summary>
        /// Constructor that asks for type and value
        /// </summary>
        /// <param name="type">Type of the variable</param>
        /// <param name="value">Value of the variable</param>
        public Variable(DataType type, dynamic value = null)
        {
            this.type = type;
            if (value == null)
                this.Value = type.Instantiate();
            else
                this.Value = value;
        }

        /// <summary>
        /// Make type attribute public in read only
        /// </summary>
        public DataType Type
        {
            get { return type; }
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

        /// <see cref="Global.Definition.IsValid"/>
        public override bool IsValid()
        {
            return this.value != null && this.type != null && this.type.IsValueOfType(this.value);
        }
    }
}
