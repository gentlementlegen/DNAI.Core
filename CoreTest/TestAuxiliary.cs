using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTest
{
    public class TestAuxiliary
    {
        /// <summary>
        /// Method that handle operators' test
        /// </summary>
        /// <typeparam name="T">Expected results type</typeparam>
        /// <param name="to_test">List of operators to test</param>
        /// <param name="init">Initialisation function for operator parameters</param>
        /// <param name="expected">List of expected values after each initialisation for each operators</param>
        public static void HandleOperations<T>(List<CorePackage.Execution.Operator> to_test, List<Func<CorePackage.Execution.Instruction, bool>> init, List<List<T>> expected)
        {
            if (init.Count != expected.Count)
                throw new Exception("Expected result number have to be equal to <to_test.Count * init.Count>");

            for (int j = 0; j < init.Count; j++)
            {
                if (expected[j].Count != to_test.Count)
                    throw new Exception("Missing expected results in list at index " + j.ToString());
                for (int i = 0; i < to_test.Count; i++)
                {
                    init[j].DynamicInvoke(to_test[i]);
                    if (to_test[i].GetOutputValue("result") != expected[j][i])
                        throw new Exception("Invalid result for " + to_test[i].GetType().ToString() + ": Expected " + expected[j][i].ToString() + " got " + ((T)to_test[i].GetOutputValue("result")).ToString());
                }
            }
        }

    }
}
