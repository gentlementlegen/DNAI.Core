using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private DataType stored;

        /// <summary>
        /// Constructor that asks for the type stored
        /// </summary>
        /// <param name="stored"></param>
        public ListType(DataType stored)
        {
            this.stored = stored;
        }
        
        /// <see cref="DataType.Instanciate"/>
        public override dynamic Instanciate()
        {
            throw new NotImplementedException();
        }

        /// <see cref="Global.Definition.IsValid"/>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        /// <see cref="DataType.IsValueOfType(dynamic)"/>
        public override bool IsValueOfType(dynamic value)
        {
            throw new NotImplementedException();
        }
    }
}
