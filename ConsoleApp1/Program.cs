using NUnit;
using NUnit.Engine;
using System.Xml;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var testEventListener = new TestEventListener();

            var ids = new[] { "1", "2", "3" };

            foreach (var id in ids)
            {
                Task.Run(() =>
                {
                    Console.WriteLine("Queued: {0}", id);

                    var filterBuilder = new TestFilterBuilder();

                    filterBuilder.AddTest("TestProject1.Tests.Test1");

                    var testFilter = filterBuilder.GetFilter();

                    var testPackage = new TestPackage("TestProject1.dll");

                    testPackage.AddSetting(FrameworkPackageSettings.TestParametersDictionary, new Dictionary<string, string>
                    {
                        { "IDFromParameter", id }
                    });

                    var engine = TestEngineActivator.CreateInstance();

                    var runner = engine.GetRunner(testPackage);

                    runner.Run(testEventListener, testFilter);
                });
            }

            Console.ReadLine();
        }
    }

    internal class TestEventListener : ITestEventListener
    {
        public void OnTestEvent(string report)
        {
            var doc = new XmlDocument();
            doc.LoadXml(report);

            var outputNode = doc.SelectSingleNode("//test-case/output");

            var cmd = doc.FirstChild?.Name;

            if (outputNode != null && cmd == "test-run")
            {
                var outputContent = outputNode.InnerText.Trim();

                Console.WriteLine(outputContent);
            }
        }
    }
}