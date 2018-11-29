using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Entity.Type
{
    public class DictType : DataType
    {
        public static DictType Instance { get; } = new DictType();

        private DictType()
        {

        }

        public override dynamic GetDeepCopyOf(dynamic value, System.Type type = null)
        {
            if (type == null)
            {
                type = typeof(Dictionary<string, dynamic>);
            }

            dynamic toret = Activator.CreateInstance(type);

            foreach (var pair in value)
            {
                toret[pair.Key] = pair.Value;
            }
            return toret;
        }

        public override dynamic Instantiate()
        {
            return new Dictionary<string, dynamic>();
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        public override bool IsValueOfType(dynamic value)
        {
            return value.GetType() == typeof(Dictionary<string, dynamic>);
        }

        public override dynamic OperatorAccess(dynamic lOp, dynamic rOp)
        {
            return lOp[rOp];
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
            throw new NotImplementedException();
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

        public override dynamic CreateFromJSON(string value)
        {
            var data = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(value);
            var toret = Instantiate();

            foreach (var val in data)
            {
                toret[val.Key] = val.Value;
            }
            return toret;
        }
    }
}
