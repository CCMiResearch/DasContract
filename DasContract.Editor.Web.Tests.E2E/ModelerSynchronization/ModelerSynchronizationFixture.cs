using Microsoft.Extensions.Configuration;
using Microsoft.Playwright.NUnit;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using Task = System.Threading.Tasks.Task;
using NUnit.Framework;

namespace DasContract.Editor.Web.Tests.E2E.ModelerSynchronization
{
    public abstract class ModelerSynchronizationFixture : PageTest
    {
        protected readonly IConfiguration _config;
        protected readonly string _baseUrl;

        protected ModelerCommandManager _commandManager;
        protected const string DEFAULT_PROCESS_ID = "Process_1";

        public ModelerSynchronizationFixture()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            _baseUrl = _config.GetSection("BaseUrl").Value;
        }
        [SetUp]
        public async Task Init()
        {
            _commandManager = new ModelerCommandManager();
            await Page.GotoAsync(_baseUrl);
            await Page.Locator("#create-link").ClickAsync();
            await _commandManager.SetupVariables(Page);
        }

        protected async Task CompareCreatedContracts()
        {
            var downloaded = await DownloadAndReadDasContract();
            var expected = _commandManager.GetContractAsXElement();

            var downloadedProc = downloaded.Descendants("Processes").First();
            var expectedProc = expected.Descendants("Processes").First();

            var equals = XNode.DeepEquals(downloadedProc, expectedProc);
            Assert.That(equals, "The downloaded process model does not match the expected. Expected:\n{0}\n Actual:\n {1}",
                new string[] { expectedProc.ToString(), downloadedProc.ToString() });
        }

        protected async Task<XElement> DownloadAndReadDasContract()
        {
            var waitForDownloadTask = Page.WaitForDownloadAsync();
            await Page.ClickAsync("#toolbar-button-download-contract");
            var download = await waitForDownloadTask;
            var path = await download.PathAsync();
            var xml = await File.ReadAllTextAsync(path);
            return XElement.Parse(xml);
        }
    }
}
