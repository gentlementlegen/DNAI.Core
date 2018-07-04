using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Resolver
{
    public class V1_0_0 : ACommandResolver
    {
        public static string Code
        {
            get { return "v1.0.0"; }
        }

        public override bool Resolve(string command, Stream input, Stream output)
        {
            try
            {
                return Resolve(Code, command, input, output);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("No such command named \"" + command + "\" registered in any version");
            }
        }

        /*
         * For new versions, add the following code
         * 
         * public override bool Resolve(string command, Stream input, Stream output)
         * {
         *      try
         *      {
         *          return Resolve(Code, command, input, output);
         *      }
         *      catch (KeyNotFoundException)
         *      {
         *          return super.Resolve(command, input, output);
         *      }
         *  }
         * 
         */
    }
}
