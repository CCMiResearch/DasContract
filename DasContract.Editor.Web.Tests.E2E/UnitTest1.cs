using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

namespace DasContract.Editor.Web.Tests.E2E
{
    public class Tests : PageTest
    {
        private IPage Page { get; set; }
        public Tests()
        {
            
        }

        [Test]
        public async Task ShouldAdd()
        {
            Page = await Context.NewPageAsync(); 
            await Page.GotoAsync("https://localhost:44348");
            await Page.Locator("#create-link").ClickAsync();
            await Page.EvaluateAsync(@"() =>{ const modeler = window.modeler// (1) Get the modules
  window.elementFactory = modeler.get('elementFactory'),
        window.elementRegistry = modeler.get('elementRegistry'),
        window.modeling = modeler.get('modeling');

  // (2) Get the existing process and the start event
  window.process = elementRegistry.get('Process_1'),
        window.startEvent = elementRegistry.get('StartEvent_1');

  // (3) Create a new diagram shape
  const task = window.elementFactory.createShape({ type: 'bpmn:Task' });

  // (4) Add the new task to the diagram
  window.modeling.createShape(task, { x: 400, y: 100 }, process); }");

            await Task.Delay(1000);

            await Page.EvaluateAsync(@"() =>{ 

  // (3) Create a new diagram shape
  const task2 = window.elementFactory.createShape({ type: 'bpmn:Task' });

  // (4) Add the new task to the diagram
  window.modeling.createShape(task2, { x: 600, y: 100 }, process); }");
            await Task.Delay(1000);
        }

        [Test]
        public async Task ShouldMultiply()
        {

            int result = await Page.EvaluateAsync<int>("() => 7 * 3");
            Assert.AreEqual(21, result);
        }
    }
}