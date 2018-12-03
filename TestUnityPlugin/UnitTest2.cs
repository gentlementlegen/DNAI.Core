using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Core.Plugin.Unity.API;
using Core.Plugin.Unity.Editor.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUnityPlugin
{
    [TestClass]
    public class UnitTest2
    {
        //[TestMethod]
        public void TestConditions()
        {
            List<ACondition> cdts = new List<ACondition>();
            ConditionInput<int> i = new ConditionInput<int>
            {
                Value = 42
            };

            var intCdt = new ConditionEvaluator
            {
                Condition = ConditionEvaluator.CONDITION.EQUAL,
                Input = 42
            };
            intCdt.SetRefOutput(i);
            //cdts.Add(intCdt);

            Assert.IsTrue(ACondition.EvaluateSet(cdts), "1 Conditions are not satisfied");

            i.Value = 0;
            Assert.IsFalse(ACondition.EvaluateSet(cdts), "2 Conditions are not satisfied");
        }

        //[TestMethod]
        public void TestConditionCallback()
        {
            List<ACondition> cdts = new List<ACondition>();
            ConditionInput<int> i = 128;

            var intCdt = new ConditionEvaluator
            {
                Condition = ConditionEvaluator.CONDITION.MORE,
                Input = 127
            };
            intCdt.SetRefOutput(i);
            //intCdt.Callback = () => intCdt.Input++;

            //cdts.Add(intCdt);

            Assert.IsTrue(ACondition.EvaluateSet(cdts), "1 Conditions are not satisfied");
            //intCdt.Callback();
            Assert.IsFalse(ACondition.EvaluateSet(cdts), "2 Conditions are not satisfied");
        }

        private bool downloading = true;

        [TestMethod]
        public void TestDownloadMl()
        {

            var http = new HttpClientDownloadWithProgress("https://github.com/Nicolas-Constanty/Dnai.ML.PluginDependencies/releases/download/v1.0/Dnai.ML.PluginDependencies.zip",
                @"C:\Users\Mentlegen\Desktop\Dnai.ML.PluginDependencies.zip");
            http.ProgressChanged += Http_ProgressChanged;
            Task.Run(async () =>
            {
                await http.StartDownload();
            }).Wait();

            //WebClient wc = new WebClient();
            //wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
            //wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
            //wc.DownloadFileAsync(new Uri("https://github.com/Nicolas-Constanty/Dnai.ML.PluginDependencies/releases/download/v1.0/Dnai.ML.PluginDependencies.zip"), @"C:\Users\Mentlegen\Desktop\Dnai.ML.PluginDependencies.zip");
            //while (downloading) ;
        }

        private void Http_ProgressChanged(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage)
        {
            
        }

        private void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine("done !");
            downloading = false;
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //Console.WriteLine(e.ProgressPercentage);
        }
    }
}
