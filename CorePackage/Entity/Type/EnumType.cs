using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Entity.Type
{
    /// <summary>
    /// This class represents an enumeration type used
    /// </summary>
    public class EnumType : DataType
    {
        /// <summary>
        /// Contains enumeration values which are variable declarations
        /// Those variables can be of any type
        /// </summary>
        private Dictionary<string, Global.Declaration<Variable>> values = new Dictionary<string, Global.Declaration<Variable>>();

        /// <summary>
        /// Contains the type of the variables stored in the enumeration
        /// </summary>
        private DataType stored;

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
        public void AddValue(string name, Entity.Variable definition)
        {
            //check given definition validity
            this.values[name] = new Global.Declaration<Variable> { name = name, definition = definition };
        }

        /// <see cref="DataType.Instantiate"/>
        public override dynamic Instantiate()
        {
            return values.Values.First().definition.Value;
        }

        /// <see cref="Global.Definition.IsValid"/>
        public override bool IsValid()
        {
            //incohérence des types stockés par rapport à celui défini
            foreach (Global.Declaration<Variable> curr in values.Values)
            {
                if (curr.definition.Type != this.stored)
                    return false;
            }
            return true;
        }

        /// <see cref="DataType.IsValueOfType(dynamic)"/>
        public override bool IsValueOfType(dynamic value)
        {
            //return value.GetType() != typeof(string) && values.Keys.Contains((string)value);
            if (value.GetType() != values.First().Value.definition.Value.GetType())
                return false;
            foreach (Global.Declaration<Entity.Variable> curr in values.Values)
            {
                if (curr.definition.Value == value)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the variable that corresponds to the given name
        /// </summary>
        /// <param name="name">Name of the enum value to return</param>
        /// <returns>Variable corresponding to the enum value</returns>
        public Variable GetValue(string name)
        {
            return values[name].definition;
        }
    }
}
