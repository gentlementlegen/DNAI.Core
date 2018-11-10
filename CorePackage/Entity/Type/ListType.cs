using CorePackage.Error;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CorePackage.Entity.Type
{
    /// <summary>
    /// Represent a list type definition
    /// </summary>
    public class ListType : DataType
    {
        /// <summary>
        /// Attribute that represents the internal list stored type
        /// </summary>
        private DataType stored = null;

        /// <summary>
        /// Basic getter and Advanced setter for stored type
        /// </summary>
        public DataType Stored
        {
            get { return stored; }
            set
            {
                if (value != null)
                {
                    _listGenericType = value.Instantiate().GetType();
                    _listType = typeof(List<>).MakeGenericType(_listGenericType);
                    stored = value;
                }
            }
        }

        public System.Type RealStoredType
        {
            get { return _listGenericType; }
        }

        /// <summary>
        /// Represents the real type of the list
        /// </summary>
        private System.Type _listType;
        private System.Type _listGenericType;

        /// <summary>
        /// Basic default constructor which is necessary for factory
        /// </summary>
        public ListType()
        {

        }

        /// <summary>
        /// Constructor that asks for the type stored
        /// </summary>
        /// <param name="stored">List stored type</param>
        public ListType(DataType stored)
        {
            this.Stored = stored;
        }

        /// <summary>
        /// <see cref="DataType.Instantiate"/>
        /// </summary>
        public override dynamic Instantiate()
        {
            return Activator.CreateInstance(_listType);
        }

        /// <see cref="Global.IDefinition.IsValid"/>
        public override bool IsValid()
        {
            return Stored != null;
        }

        /// <summary>
        /// <see cref="DataType.IsValueOfType(dynamic)"/>
        /// </summary>
        public override bool IsValueOfType(dynamic value)
        {
            //If the value is enumerable
            if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(value.GetType()))
                return false;

            //If its empty, its ok
            if (value.Count == 0)
                return true;

            //Else check the first value
            dynamic fval = value[0];

            try
            {
                //If its scalar, get the real value
                if ((stored as ScalarType) != null)
                    fval = fval.Value;
            } catch (Exception) { }

            //and finaly check that is of stored type
            return  stored.IsValueOfType(fval);
        }

        public override dynamic OperatorAdd(dynamic lOp, dynamic rOp)
        {
            dynamic ret = Instantiate();

            ret.AddRange(System.Linq.Enumerable.Concat(lOp, rOp));
            return ret;
        }

        public override dynamic OperatorSub(dynamic lOp, dynamic rOp)
        {
            dynamic ret = Instantiate();

            ret.AddRange(System.Linq.Enumerable.Except(lOp, rOp));
            return ret;
        }

        public override dynamic OperatorMul(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override dynamic OperatorDiv(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override dynamic OperatorMod(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override bool OperatorGt(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override bool OperatorGtEq(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override bool OperatorLt(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override bool OperatorLtEq(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override bool OperatorEqual(dynamic lOp, dynamic rOp)
        {
            if (lOp.Count != rOp.Count)
                return false;

            for (int i = 0; i < lOp.Count; i++)
            {
                if (!stored.OperatorEqual(lOp[i], rOp[i]))
                    return false;
            }

            return true;
        }

        public override dynamic OperatorBAnd(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override dynamic OperatorBOr(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override dynamic OperatorRightShift(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override dynamic OperatorLeftShift(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override dynamic OperatorXor(dynamic lOp, dynamic rOp)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override dynamic OperatorBNot(dynamic op)
        {
            throw new OperatorNotPermitted("This operator is not permitted on list type");
        }

        public override dynamic OperatorAccess(dynamic lOp, dynamic rOp)
        {
            return lOp[(int)rOp];
        }

        /// <see cref="DataType.GetDeepCopyOf(dynamic)"/>
        public override dynamic GetDeepCopyOf(dynamic value, System.Type type = null)
        {
            if (type == null)
                type = value.GetType();

            dynamic toret = Activator.CreateInstance(type);

            foreach (var curr in value)
            {
                toret.Add(Convert.ChangeType(stored.GetDeepCopyOf(curr), curr.GetType()));
            }
            return toret;
        }

        public override dynamic CreateFromJSON(string value)
        {
            var arr = (JArray)JsonConvert.DeserializeObject(value);
            var data = new List<dynamic>();

            foreach (var var in arr)
            {
                data.Add(Stored.CreateFromJSON(JsonConvert.SerializeObject(var)));
            }
            return data;
        }
    }
}