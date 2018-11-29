using CorePackage.Entity;
using CorePackage.Entity.Type;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCore.TestEntities
{
    [TestClass]
    public class TestMatrix
    {
        [TestMethod]
        public void Basics()
        {
            var data = Matrix.Instance.CreateFromJSON(@"{ ""Values"": [2, 2, 2, 2, 2, 2], ""RowCount"": 2, ""ColumnCount"": 3 }");

            Assert.IsTrue(Matrix.Instance.IsValueOfType(data));
            
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Assert.IsTrue(data[row, col] == 2);
                }
            }
        }
    }
}
