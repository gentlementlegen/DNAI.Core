using Core.Plugin.Unity.Generator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestUnityPlugin
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CompileTest()
        {
            var c = new Compiler();

            var res = c.Compile();

            var main = res.CompiledAssembly.GetType("First.Program").GetMethod("Main");
            main.Invoke(null, null);
        }

        [TestMethod]
        public void TemplateFileTest()
        {
            var t = new TemplateReader();

            var content = t.GenerateTemplateContent();
        }

        [TestMethod]
        public void GenerationAndCompile()
        {
            var compiler = new Compiler();
            var template = new TemplateReader();

            string code = template.GenerateTemplateContent();
            var res = compiler.Compile(code);

            var type = res.CompiledAssembly.GetType("DulyBehaviour");
            Assert.IsNotNull(type);
        }

        [TestMethod]
        public void GenerationFromController()
        {
            var compiler = new Compiler();
            var template = new TemplateReader();

            string code = template.GenerateTemplateContent();
            var res = compiler.Compile(code);

            var type = res.CompiledAssembly.GetType("DulyBehaviour");
            Assert.IsNotNull(type);
        }
    }
}