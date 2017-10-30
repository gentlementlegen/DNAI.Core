using CorePackage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Global
{
    public interface IContext : IDeclarator<IContext>, IDeclarator<Variable>, IDeclarator<DataType>, IDeclarator<Function>
    {
        void SetParent(IContext parent);
    }
}
