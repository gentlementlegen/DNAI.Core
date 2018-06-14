using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Global
{
    public abstract class Definition : IDefinition
    {
        public string Name { get; set; }

        public String FullName
        {
            get
            {
                return (Parent != null ? ((Definition)Parent).FullName : "") + "/" + Name;
            }
        }

        public IDeclarator Parent { get; set; }

        public abstract bool IsValid();
    }
}
