using CorePackage.Error;
using System;
using System.Collections.Generic;
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
                    _listType = typeof(List<>).MakeGenericType(value.Instantiate().GetType());
                    stored = value;
                }
            }
        }

        /// <summary>
        /// Represents the real type of the list
        /// </summary>
        private System.Type _listType;

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

        /// <see cref="Global.Definition.IsValid"/>
        public override bool IsValid()
        {
            return Stored != null;
        }

        /// <summary>
        /// <see cref="DataType.IsValueOfType(dynamic)"/>
        /// </summary>
        public override bool IsValueOfType(dynamic value)
        {
            return _listType == value.GetType();
        }

        public override dynamic OperatorAdd(dynamic lOp, dynamic rOp)
        {
            return lOp.Union(rOp).ToList();
        }

        public override dynamic OperatorSub(dynamic lOp, dynamic rOp)
        {
            return lOp.Except(rOp).ToList();
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
            return lOp.Equals(rOp);
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
            return lOp[rOp];
        }
    }
}