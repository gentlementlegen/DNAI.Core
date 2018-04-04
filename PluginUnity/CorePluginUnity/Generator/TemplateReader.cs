//using EnvDTE;
//using Microsoft.VisualStudio.TextTemplating;
//using Microsoft.VisualStudio.TextTemplating.VSHost;
//using System;
using Core.Plugin.Unity.Extensions;
using CoreCommand;
using CoreControl;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Plugin.Unity.Generator
{
    public partial class GeneratedCodeTemplate
    {
        public List<string> Inputs = new List<string>();
        public List<string> Outputs = new List<string>();
        public List<string> DataTypes = new List<string>();
        public string FilePath = "";
        public uint FunctionId;
        public string FunctionArguments = "";
        public string Namespace = "Behaviour";
        public string ClassName = "DNAIBehaviour";
    }

    /// <summary>
    /// Class in charge of the reading of the templated file.
    /// </summary>
    internal class TemplateReader
    {
        private readonly Dictionary<uint, string> enumNames = new Dictionary<uint, string>();

        //private readonly IServiceProvider serviceProvider;
        //private readonly ITextTemplating t4;
        //private readonly ITextTemplatingSessionHost sessionHost;

        internal TemplateReader()
        {
            //// Get an instance of the currently running Visual Studio IDE.
            //EnvDTE.DTE dte2 = (EnvDTE.DTE)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.15.0");

            //// Get a service provider – how you do this depends on the context:
            //serviceProvider = (IServiceProvider)dte2;

            //// Get the text template service:
            //t4 = serviceProvider.GetService(typeof(STextTemplating)) as ITextTemplating;

            //sessionHost = t4 as ITextTemplatingSessionHost;

            //// Create a Session in which to pass parameters:
            //sessionHost.Session = sessionHost.CreateSession();
            //sessionHost.Session["parameter1"] = "Hello";
            //sessionHost.Session["parameter2"] = DateTime.Now;

            //// Pass another value in CallContext:
            //System.Runtime.Remoting.Messaging.CallContext.LogicalSetData("parameter3", 42);

            //// Process a text template:
            //string result = t4.ProcessTemplate("",
            //   // This is the test template:
            //   "<#@parameter type=\"System.String\" name=\"parameter1\"#>"
            // + "<#@parameter type=\"System.DateTime\" name=\"parameter2\"#>"
            // + "<#@parameter type=\"System.Int32\" name=\"parameter3\"#>"
            // + "Test: <#=parameter1#>    <#=parameter2#>    <#=parameter3#>");

            //// This test code yields a result similar to the following line:
            ////     Test: Hello    07/06/2010 12:37:45    42
            //_template.Inputs.Add("input1");
            //_template.Inputs.Add("input2");
            //_template.Outputs.Add("output1");
            //_template.Outputs.Add("output2");
        }

        /// <summary>
        /// Generates the template source code file that will be understood by Unity.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="variables"></param>
        /// <param name="functions"></param>
        /// <param name="dataTypes"></param>
        /// <returns></returns>
        internal string GenerateTemplateContent(CoreCommand.BinaryManager manager = null, List<CoreControl.EntityFactory.Entity> variables = null,
            List<CoreControl.EntityFactory.Entity> functions = null, List<CoreControl.EntityFactory.Entity> dataTypes = null)
        {
            var template = new GeneratedCodeTemplate();
            var customObjects = new Queue<CustomObject>();
            enumNames.Clear();

            if (manager != null)
            {
                template.FilePath = Path.GetFileName(manager.FilePath);
                //Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                //template.Namespace = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rgx.Replace(Path.GetFileNameWithoutExtension(template.FilePath), ""));
                template.Namespace = Path.GetFileNameWithoutExtension(template.FilePath).RemoveIllegalCharacters();
            }

            if (functions?.Count > 0)
                template.ClassName = functions[0].Name;

            if (dataTypes != null)
            {
                foreach (var item in dataTypes)
                {
                    var type = manager.Controller.GetEntityType(item.Id);
                    if (type == CoreControl.EntityFactory.ENTITY.ENUM_TYPE)
                    {
                        var ret = "";
                        ret += $"enum {item.Name} {{";
                        foreach (var v in manager.Controller.GetEnumerationValues(item.Id))
                            ret += $"{v} = {manager.Controller.GetEnumerationValue(item.Id, v)},";
                        ret += "}";
                        template.DataTypes.Add(ret);
                        enumNames.Add(item.Id, item.Name);
                    }
                    else if (type == CoreControl.EntityFactory.ENTITY.OBJECT_TYPE)
                    {
                        //template.DataTypes.Add(CreateObject(manager, item));
                        customObjects.Enqueue(new CustomObject(item, manager.Controller));
                    }
                }
                foreach (var obj in customObjects)
                    template.DataTypes.Add(CreateObject(obj));
            }

            if (variables != null)
            {
                template.Inputs.Clear();
                foreach (var item in variables)
                    template.Inputs.Add(item.ToSerialString(manager.Controller));
            }

            if (functions?.Count > 0)
            {
                template.Outputs.Clear();
                template.FunctionId = functions[0].Id;
                foreach (var item in functions)
                {
                    //template.FunctionId = item.Id;
                    var pars = manager.Controller.GetFunctionParameters(item.Id);

                    // Gets the variables with the function container id
                    foreach (var v in pars)
                    {
                        var typeId = manager.Controller.GetVariableType(v.Id);
                        var realType = manager.Controller.GetEntityType(typeId);
                        if (realType == CoreControl.EntityFactory.ENTITY.ENUM_TYPE)
                        {
                            var ret = $"{enumNames[typeId]} {v.Name}";
                            template.Inputs.Add(ret);
                        }
                        else
                        {
                            template.Inputs.Add(v.ToSerialString(manager.Controller));
                        }
                    }

                    for (int i = 0; i < pars.Count; i++)
                        template.FunctionArguments += $"{{\"{pars[i].Name}\", ({manager.Controller.GetVariableValue(pars[i].Id).GetType().ToString()}) {pars[i].Name}}},";
                    foreach (var ret in manager.Controller.GetFunctionReturns(item.Id))
                        template.Outputs.Add(ret.ToSerialString(manager.Controller));
                }
            }
            return template.TransformText();
        }

        private void GetFunctionBody(BinaryManager manager, EntityFactory.Entity item,
                                        out List<string> inputs, out List<string> outputs, out string arguments)
        {
            var pars = manager.Controller.GetFunctionParameters(item.Id);
            inputs = new List<string>();
            outputs = new List<string>();
            arguments = "";

            // Gets the variables with the function container id
            foreach (var v in pars)
            {
                var typeId = manager.Controller.GetVariableType(v.Id);
                var realType = manager.Controller.GetEntityType(typeId);
                if (realType == CoreControl.EntityFactory.ENTITY.ENUM_TYPE)
                {
                    var ret = $"{enumNames[typeId]} {v.Name}";
                    inputs.Add(ret);
                }
                else
                {
                    inputs.Add(v.ToSerialString(manager.Controller));
                }
            }

            for (int i = 0; i < pars.Count; i++)
                arguments += $"{{\"{pars[i].Name}\", ({manager.Controller.GetVariableValue(pars[i].Id).GetType().ToString()}) {pars[i].Name}}},";
            foreach (var ret in manager.Controller.GetFunctionReturns(item.Id))
                outputs.Add(ret.ToSerialString(manager.Controller));
        }

        /// <summary>
        /// Retrieves the string representing the given object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string CreateObject(CustomObject obj)
        {
            var ret = "class " + obj.ObjectName + "{";

            foreach (var attrib in obj.Fields)
            {
                ret += obj.GetFieldType(attrib.Value) + " " + attrib.Key + ";";
            }
            return ret + "}";
        }

        /// <summary>
        /// Generates a custom object from the given entity.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private string CreateObject(BinaryManager manager, EntityFactory.Entity item)
        {
            var ret = "class " + item.Name + "{";
            var funcList = manager.Controller.GetEntitiesOfType(EntityFactory.ENTITY.FUNCTION, item.Id);
            var funcTemplate = new GenerateFunctionTemplate();

            var custom = new CustomObject(item, manager.Controller);

            foreach (var attrib in custom.Fields)
            {
                ret += custom.GetFieldType(attrib.Value) + " " + attrib.Key + ";";
            }

            //foreach (var func in funcList)
            //{
            //    var inputs = new List<string>();
            //    var outputs = new List<string>();
            //    funcTemplate.FunctionId = func.Id;
            //    GetFunctionBody(manager, func, out funcTemplate.Inputs, out funcTemplate.Outputs, out funcTemplate.FunctionArguments);
            //    ret += funcTemplate.TransformText();
            //}
            return ret + "}";
        }
    }
}