using EnvDTE;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using System;
using System.Collections.Generic;

namespace Core.Plugin.Unity.Generator
{
    public partial class GeneratedCodeTemplate
    {
        public List<string> Entries = new List<string>();
    }

    internal class TemplateReader
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ITextTemplating t4;
        private readonly ITextTemplatingSessionHost sessionHost;

        private readonly GeneratedCodeTemplate _template = new GeneratedCodeTemplate();

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
            _template.Entries.Add("var1");
            _template.Entries.Add("var2");
        }

        internal string GenerateTemplateContent()
        {
            return _template.TransformText();
        }
    }
}