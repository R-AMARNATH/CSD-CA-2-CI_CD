using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace BP_E2E
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class BpPageTests : PageTest
    {
        [Test, Category("E2E")]
        public async Task HomePage_Loads()
        {
            await Page.GotoAsync(TestSettings.BaseUrl);

            var heading = Page.Locator("h4:text('BP Category Calculator')");
            await Expect(heading).ToBeVisibleAsync();
        }

        [Test, Category("E2E")]
        public async Task IdealBloodPressure_ShowsIdealCategory()
        {
            await Page.GotoAsync(TestSettings.BaseUrl);

            await Page.FillAsync("#BP_Systolic", "110");
            await Page.FillAsync("#BP_Diastolic", "70");

            await Page.ClickAsync("input[type='submit']");

            // Checking that the page contains "Ideal Blood Pressure"
            var body = Page.Locator("body");
            await Expect(body).ToContainTextAsync("Ideal Blood Pressure");
        }

        [Test, Category("E2E")]
        public async Task HighBloodPressure_ShowsHighCategory()
        {
            await Page.GotoAsync(TestSettings.BaseUrl);

            await Page.FillAsync("#BP_Systolic", "160");
            await Page.FillAsync("#BP_Diastolic", "95");

            await Page.ClickAsync("input[type='submit']");

            var body = Page.Locator("body");
            await Expect(body).ToContainTextAsync("High Blood Pressure");
        }

        [Test, Category("E2E")]
        public async Task InvalidValues_ShowValidationError()
        {
            await Page.GotoAsync(TestSettings.BaseUrl);

            await Page.FillAsync("#BP_Systolic", "80");
            await Page.FillAsync("#BP_Diastolic", "90");

            await Page.ClickAsync("input[type='submit']");

            var summary = Page.Locator("div.validation-summary-errors");
            await Expect(summary).ToBeVisibleAsync();
            await Expect(summary).ToContainTextAsync(
                "Systolic must be greater than",
                new() { IgnoreCase = true }
            );
        }
    }
}
