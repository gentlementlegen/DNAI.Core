using CorePackage.Error;
using CorePackage.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Entity.Type
{
    /// <summary>
    /// Represents a Object definition type
    /// </summary>
    public class ObjectType : DataType, IDeclarator
    {
        /// <summary>
        /// The context of an object in which you can declare method, static attributes, nested types and other contexts
        /// </summary>
        private Context context = new Context();

        /// <summary>
        /// Represents the objects attributes through a declarator of DataType
        /// </summary>
        private Declarator attributes = new Declarator(new List<System.Type> { typeof(DataType) });

        private Dictionary<Operator.Name, Function> overloadedOperators = new Dictionary<Operator.Name, Function>();

        /// <summary>
        /// Basic default constructor which is necessary for factory
        /// </summary>
        public ObjectType()
        {

        }
                
        /// <summary>
        /// Allow user to add an attribute with a name, a type and a visibility
        /// </summary>
        /// <param name="name">Name of the attribute to add</param>
        /// <param name="attrType">Type of the attribute to add</param>
        /// <param name="visibility">Visibility of the attribute (INTERNAL, EXTERNAL)</param>
        public void AddAttribute(string name, DataType attrType, Global.AccessMode visibility)
        {
            attributes.Declare(attrType, name, visibility);
        }

        /// <summary>
        /// Allow user to remove an internal attribute with a specific name
        /// </summary>
        /// <param name="name">Name of the attribute to remove</param>
        public void RemoveAttribute(string name)
        {
            attributes.Pop(name);
        }

        /// <summary>
        /// Allow user to change an attribute visibility
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="visibility">Visibility to set</param>
        public void ChangeAttributeVisibility(string name, Global.AccessMode visibility)
        {
            attributes.ChangeVisibility(name, visibility);
        }

        /// <summary>
        /// Allow user to rename an attribute
        /// </summary>
        /// <param name="lastName">Current name of the attribute</param>
        /// <param name="newName">New name to set to the attribute</param>
        public void RenameAttribute(string lastName, string newName)
        {
            attributes.Rename(lastName, newName);
        }

        public Variable SetFunctionAsMember(string name, Global.AccessMode visibility)
        {
            Function func = (Function)Find(name, visibility);

            if (func == null)
                throw new NotFoundException("No such function named \"" + name + "\" with visibility " + visibility.ToString());
            
            Variable toret = (Variable)func.Declare(new Variable(this), "this", AccessMode.EXTERNAL);
            func.SetVariableAs("this", Function.VariableRole.PARAMETER);
            return toret;
        }

        /// <see cref="DataType.Instantiate"/>
        public override dynamic Instantiate()
        {
            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

            foreach(KeyValuePair<string, IDefinition> attrtype in attributes.GetEntities(AccessMode.EXTERNAL))
            {
                data[attrtype.Key] = ((DataType)attrtype.Value).Instantiate();
            }
            foreach (KeyValuePair<string, IDefinition> attrtype in attributes.GetEntities(AccessMode.INTERNAL))
            {
                data[attrtype.Key] = ((DataType)attrtype.Value).Instantiate();
            }
            return data;
        }

        /// <see cref="DataType.IsValueOfType(dynamic)"/>
        public override bool IsValueOfType(dynamic value)
        {
            foreach (KeyValuePair<string, IDefinition> attrtype in attributes.GetEntities(AccessMode.EXTERNAL))
            {
                if (!value.ContainsKey(attrtype.Key)
                    || !((DataType)attrtype.Value).IsValueOfType(value[attrtype.Key]))
                    return false;
            }
            
            foreach (KeyValuePair<string, IDefinition> attrtype in attributes.GetEntities(AccessMode.INTERNAL))
            {
                if (!value.ContainsKey(attrtype.Key)
                    || !((DataType)attrtype.Value).IsValueOfType(value[attrtype.Key]))
                    return false;
            }
            return true;
        }

        /// <see cref="Global.IDefinition.IsValid"/>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        public IDefinition Declare(IDefinition entity, string name, AccessMode visibility)
        {
            return context.Declare(entity, name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        public IDefinition Pop(string name)
        {
            return context.Pop(name);
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        public IDefinition Find(string name, AccessMode visibility)
        {
            return context.Find(name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        public void Rename(string lastName, string newName)
        {
            context.Rename(lastName, newName);
        }

        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        public AccessMode GetVisibilityOf(string name)
        {
            return context.GetVisibilityOf(name);
        }

        ///<see cref="IDeclarator{definitionType}.ChangeVisibility(string, AccessMode)"/>
        public void ChangeVisibility(string name, AccessMode newVisibility)
        {
            context.ChangeVisibility(name, newVisibility);
        }

        ///<see cref="IDeclarator{definitionType}.Clear"/>
        public List<IDefinition> Clear()
        {
            return context.Clear();
        }

        ///<see cref="IDeclarator{definitionType}.GetEntities(AccessMode)"/>
        public Dictionary<string, IDefinition> GetEntities(AccessMode visibility)
        {
            return context.GetEntities(visibility);
        }

        public Dictionary<string, IDefinition> GetAttributes()
        {
            return attributes.GetEntities();
        }

        public DataType GetAttribute(String name)
        {
            Dictionary<string, DataType> attrs = attributes.GetEntities();

            if (!attrs.ContainsKey(name))
                throw new NotFoundException("No such attribute in class: " + name);
            return attrs[name];
        }

        public void OverloadOperator(Operator.Name toOverload, string externalFuncName)
        {
            Function overload = (Function)Find(externalFuncName, AccessMode.EXTERNAL);

            if (overload == null)
                throw new NotFoundException("No such function \"" + externalFuncName + "\" in object");

            if (overload.GetParameter("this") == null)
                throw new InvalidOperationException("Overload function as to be member function (with \"this\" parameter)");

            Operator.Type opType = Operator.GetTypeOf(toOverload);

            if (opType == Operator.Type.UNARY
                && overload.GetParameter("this").Type != this)
                throw new InvalidOperatorSignature("Unary operator must have 1 parameter named`\"Operand\" of type " + this.ToString());

            if (opType == Operator.Type.BINARY
                && (overload.GetParameter("this").Type != this
                    || overload.GetParameter(Operator.Right) == null))
                throw new InvalidOperatorSignature("Binary operator must have 2 parameters named \"LeftOperand\" (of type " + this.ToString() + ") and \"RightOperand\"");

            if (overload.GetReturn(Operator.Result) == null)
                throw new InvalidOperatorSignature("Operator must have \"result\" return value");

            overloadedOperators[toOverload] = overload;
        }

        private Dictionary<string, dynamic> CallOperator(Operator.Name tocall, Dictionary<string, dynamic> parameters)
        {
            if (!overloadedOperators.ContainsKey(tocall))
                throw new OperatorNotOverloaded();
            return overloadedOperators[tocall].Call(parameters);
        }

        private dynamic CallOperator(Operator.Name tocall, dynamic lOp, dynamic rOp)
        {
            return CallOperator(tocall, new Dictionary<string, dynamic>
            {
                { "this", lOp },
                { Operator.Right, rOp }
            })[Operator.Result];
        }

        public override dynamic OperatorAdd(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.ADD, lOp, rOp);
        }

        public override dynamic OperatorSub(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.SUB, lOp, rOp);
        }

        public override dynamic OperatorMul(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.MUL, lOp, rOp);
        }

        public override dynamic OperatorDiv(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.DIV, lOp, rOp);
        }

        public override dynamic OperatorMod(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.MOD, lOp, rOp);
        }

        public override bool OperatorGt(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.GT, lOp, rOp);
        }

        public override bool OperatorGtEq(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.GT_EQ, lOp, rOp);
        }

        public override bool OperatorLt(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.LT, lOp, rOp);
        }

        public override bool OperatorLtEq(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.LT_EQ, lOp, rOp);
        }

        public override bool OperatorEqual(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.EQUAL, lOp, rOp);
        }

        public override dynamic OperatorBAnd(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.B_AND, lOp, rOp);
        }

        public override dynamic OperatorBOr(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.B_OR, lOp, rOp);
        }

        public override dynamic OperatorRightShift(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.R_SHIFT, lOp, rOp);
        }

        public override dynamic OperatorLeftShift(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.L_SHIFT, lOp, rOp);
        }

        public override dynamic OperatorXor(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.XOR, lOp, rOp);
        }

        public override dynamic OperatorBNot(dynamic op)
        {
            return CallOperator(Operator.Name.B_NOT, new Dictionary<string, dynamic>
            {
                { "Operand", op }
            })["result"];
        }

        public override dynamic OperatorAccess(dynamic lOp, dynamic rOp)
        {
            return CallOperator(Operator.Name.ACCESS, lOp, rOp);
        }

        ///<see cref="IDeclarator.Contains(string)"/>
        public bool Contains(string name)
        {
            return context.Contains(name);
        }
    }
}
