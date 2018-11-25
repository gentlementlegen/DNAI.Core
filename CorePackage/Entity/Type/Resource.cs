using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Entity.Type
{
    public class Resource : DataType
    {
        public static Resource Instance { get; } = new Resource();

        public string Directory { get; set; } = ".";

        private Resource()
        {

        }

        public override dynamic CreateFromJSON(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value);
        }

        public override dynamic GetDeepCopyOf(dynamic value, System.Type type = null)
        {
            return string.Copy(value);
        }

        public override dynamic Instantiate()
        {
            return "";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        public override bool IsValueOfType(dynamic value)
        {
            return value.GetType().IsAssignableFrom(typeof(string));
        }

        public override dynamic OperatorAccess(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorAdd(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorBAnd(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorBNot(dynamic op)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorBOr(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorDiv(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override bool OperatorEqual(dynamic lOp, dynamic rOp)
        {
            return lOp == rOp;
        }

        public override bool OperatorGt(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override bool OperatorGtEq(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorLeftShift(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override bool OperatorLt(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override bool OperatorLtEq(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorMod(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorMul(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorRightShift(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorSub(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorXor(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }
    }
}
