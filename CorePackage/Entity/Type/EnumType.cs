using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Entity.Type
{
    /// <summary>
    /// This class represents an enumeration type
    /// </summary>
    public class EnumType : DataType
    {
        /// <summary>
        /// Contains enumeration values which are variable declarations
        /// Those variables can be of any type
        /// </summary>
        private Dictionary<string, Variable> values = new Dictionary<string, Variable>();

        /// <summary>
        /// Contains the type of the variables stored in the enumeration
        /// </summary>
        private DataType stored = Scalar.Integer;

        /// <summary>
        /// Basic Getter and Setter for internal stored type
        /// </summary>
        public DataType Stored
        {
            get { return stored; }
            set { stored = value; }
        }

        /// <summary>
        /// Getter for Internal enum values
        /// </summary>
        public Dictionary<string, Variable> Values
        {
            get { return values; }
        }

        /// <summary>
        /// Basic default constructor to default instanciate the type
        /// </summary>
        /// <remarks>
        /// Important for the factory because it must found a constructor with no arguments (default arguments value doesn't count)
        /// </remarks>
        public EnumType()
        {

        }

        /// <summary>
        /// Constructor that forces to given the enumeration values' type
        /// </summary>
        /// <param name="stored">Type of the stored enumeration values</param>
        public EnumType(DataType stored)
        {
            this.stored = stored;
        }

        /// <summary>
        /// Allow to add a value to the enumeration
        /// </summary>
        /// <param name="name">Represents the name of the value</param>
        /// <param name="definition">Represents the variable definition of the value</param>
        public void SetValue(string name, Entity.Variable definition)
        {
            //check given definition validity
            this.values[name] = definition;
        }

        /// <see cref="DataType.Instantiate"/>
        public override dynamic Instantiate()
        {
            return values.Values.First().Value;
        }

        /// <see cref="Global.Definition.IsValid"/>
        public override bool IsValid()
        {
            //incohérence des types stockés par rapport à celui défini
            foreach (Variable curr in values.Values)
            {
                if (curr.Value.Type != this.stored)
                    return false;
            }
            return true;
        }

        /// <see cref="DataType.IsValueOfType(dynamic)"/>
        public override bool IsValueOfType(dynamic value)
        {
            System.Type valType = value.GetType();

            //return value.GetType() != typeof(string) && values.Keys.Contains((string)value);
            if (valType.IsEnum && values.ContainsKey(valType.GetEnumName(value)))
                return true;

            if (valType != values.Values.First().Value.GetType())
                return false;

            foreach (Variable curr in values.Values)
            {
                if (curr.Value == value)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the variable that corresponds to the given name
        /// </summary>
        /// <remarks>Throws an Error.NotFoundException if given name is not in enumeration</remarks>
        /// <param name="name">Name of the enum value to return</param>
        /// <returns>Variable corresponding to the enum value</returns>
        public Variable GetValue(string name)
        {
            if (!values.ContainsKey(name))
                throw new Error.NotFoundException("No such value named \"" + name + "\" in enumeration");
            return values[name];
        }

        /// <summary>
        /// Allow user to remove an internal enumeration value
        /// </summary>
        /// <remarks>Throws an Error.NotFoundException if given name is not in enumeration</remarks>
        /// <param name="name">Name of the value to remove</param>
        public void RemoveValue(string name)
        {
            if (!values.ContainsKey(name))
                throw new Error.NotFoundException("No such value named \"" + name + "\" in enumeration");

            values.Remove(name);
        }
    }
}
