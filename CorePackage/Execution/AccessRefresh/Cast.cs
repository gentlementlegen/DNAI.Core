using CorePackage.Entity.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Cast : AccessRefreshInstruction
    {
        public static string Reference = "reference";
        public static string Value = "value";
        public static string Succeed = "succeed";

        private Entity.DataType ResultType { get; set; }

        public Cast(Entity.DataType resultType)
        {
            ResultType = resultType;
            AddInput(Reference, new Entity.Variable(AnyType.Instance), true);
            AddOutput(Value, new Entity.Variable(resultType), true);
            AddOutput(Succeed, new Entity.Variable(Scalar.Boolean));
        }

        public override void Execute()
        {
            dynamic value = GetInputValue(Reference);
            System.Type type = value.GetType();

            if (type.IsValueType)
            {
                try
                {
                    dynamic castedValue = Convert.ChangeType(value, ResultType.Instantiate().GetType());

                    SetOutputValue(Value, castedValue);
                    SetOutputValue(Succeed, true);
                }
                catch (InvalidCastException)
                {
                    SetOutputValue(Succeed, false);
                }
            }
            else if (ResultType.IsValueOfType(value))
            {
                SetOutputValue(Value, value);
                SetOutputValue(Succeed, true);
            }
            else
            {
                SetOutputValue(Succeed, false);
            }
        }
    }
}
