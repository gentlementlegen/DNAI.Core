using CorePackage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Global
{
    /// <summary>
    /// Represents the way that a context works
    /// </summary>
    public interface IContext : IDeclarator
    {
        /// <summary>
        /// Allow to set internal parent context
        /// </summary>
        /// <param name="parent">parent context to set</param>
        void SetParent(IContext parent);
    }
}
