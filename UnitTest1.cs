using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using static OpenQA.Selenium.BiDi.Modules.BrowsingContext.Locator;
using System.Reflection.Emit;

namespace Desire_Chukwudi_8905482_Assignment4
{
    [TestFixture]
    public class InsuranceQuoteTests
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://localhost/prog8171_A04/");
            // Wait until the form link is visible
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementIsVisible(By.XPath("(//a[contains(text(),'Get a New')])[1]")));
            driver.FindElement(By.XPath("(//a[contains(text(),'Get a New')])[1]")).Click();
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();   // Ends the session and closes the browser
                driver.Dispose(); // Explicitly disposes the driver resources
            }
        }

        
        // Test: Age < 16
        [Test]
        public void InsuranceQuote1_InvalidAge_LessThan16()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "Desire.Chukwudi@mail.com", 15, 0, 0);
            ValidateErrorMessage("age-error", "Please enter a value greater than or equal to 16.");
        }

        // Test: Years of Experience not provided
        [Test]
        public void InsuranceQuote2_MissingExperience()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "Desire.Chukwudi@mail.com", 25, null, 0);
            ValidateErrorMessage("experience-error", "Years of experience is required");
        }

        // Test: Number of Accidents not provided
        [Test]
        public void InsuranceQuote3_MissingAccidents()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "Desire.Chukwudi@mail.com", 25, 5, null);
            ValidateErrorMessage("accidents-error", "Number of accidents is required");
        }

        // Test: Invalid Phone Number
        [Test]
        public void InsuranceQuote4_InvalidPhoneNumber()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123123123", "Desire.Chukwudi@mail.com", 25, 5, 0);
            ValidateErrorMessage("phone-error", "Phone Number must follow the patterns 111-111-1111 or (111)111-1111");
        }

        // Test: Too Many Accidents
        [Test]
        public void InsuranceQuote5_TooManyAccidents()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "Desire.Chukwudi@mail.com", 25, 5, 3);
            ValidateFinalQuote("No Insurance for you!!  Too many accidents - go take a course!");
        }

        // Test: Age = 16
        [Test]
        public void InsuranceQuote6_AgeEquals16()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "Desire.Chukwudi@mail.com", 16, 0, 0);
            ValidateFinalQuote("$6000");
        }

        // Test: Base Rate $6000 for Accidents < 3 and Age > 16
        [Test]
        public void InsuranceQuote7_BaseRate_6000()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "Desire.Chukwudi@mail.com", 25, 0, 2);
            ValidateFinalQuote("$6000");
        }

        // Test: Base Rate $4500 for Accidents < 3 and Age > 16
        [Test]
        public void InsuranceQuote8_BaseRate_4500()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "Desire.Chukwudi@mail.com", 27, 5, 1);
            ValidateFinalQuote("$4500");
        }

        // Test: Reduce Rate by 27% for Age > 30 and Experience > 2
        [Test]
        public void InsuranceQuote9_RateReduction_27Percent()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "Desire.Chukwudi@mail.com", 35, 10, 1);
            ValidateFinalQuote("$2190"); // Reduced 27% from $4500
        }

        // Test: Invalid Phone Format (e.g., 1234567890)
        [Test]
        public void InsuranceQuote10_InvalidPhoneFormat()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "1234567890", "Desire.Chukwudi@mail.com", 25, 5, 0);
            ValidateErrorMessage("phone-error", "Phone Number must follow the patterns 111-111-1111 or (111)111-1111");
        }

        // Test: Valid Phone Format (111-111-1111)
        [Test]
        public void InsuranceQuote11_ValidPhoneFormat()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "111-111-1111", "Desire.Chukwudi@mail.com", 25, 5, 0);
            ValidateFinalQuote("$4500");
        }




        //This ensures the valid phone number format (111)111-1111 is correctly accepted.
        [Test]
        public void InsuranceQuote12_ValidPhoneNumberWithParentheses()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "(123)111-1111", "Desire.Chukwudi@mail.com", 25, 5, 0);
            ValidateFinalQuote("$4500");  // This should return a valid quote.
        }

        //Test: Invalid Age (Negative Value)
        [Test]
        public void InsuranceQuote13_InvalidAge_NegativeValue()
        {
            FillMandatoryFields("Desire", "Chukwudi", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "Desire.Chukwudi@mail.com", -5, 5, 0);
            ValidateErrorMessage("age-error", "Please enter a value greater than or equal to 16.");
        }







        private void FillMandatoryFields(string firstName, string lastName, string address, string city, string province, string postalCode, string phone, string email, int? age, int? experience, int? accidents)
        {
            driver.FindElement(By.Id("firstName")).SendKeys(firstName);
            driver.FindElement(By.Id("lastName")).SendKeys(lastName);
            driver.FindElement(By.Id("address")).SendKeys(address);
            driver.FindElement(By.Id("city")).SendKeys(city);
            driver.FindElement(By.Id("postalCode")).SendKeys(postalCode);
            driver.FindElement(By.Id("phone")).SendKeys(phone);
            driver.FindElement(By.Id("email")).SendKeys(email);

            if (age.HasValue)
                driver.FindElement(By.Id("age")).SendKeys(age.ToString());
            if (experience.HasValue)
                driver.FindElement(By.Id("experience")).SendKeys(experience.ToString());
            if (accidents.HasValue)
                driver.FindElement(By.Id("accidents")).SendKeys(accidents.ToString());

            driver.FindElement(By.Id("btnSubmit")).Click();
        }

        private void ValidateErrorMessage(string elementId, string expectedMessage)
        {
            var actualMessage = driver.FindElement(By.Id(elementId)).Text;
            Assert.AreEqual(expectedMessage, actualMessage, $"Expected error message '{expectedMessage}', but got '{actualMessage}'");
        }

        private void ValidateFinalQuote(string expectedQuote)
        {
            var actualQuote = driver.FindElement(By.Id("finalQuote")).GetAttribute("value");
            Assert.AreEqual(expectedQuote, actualQuote, $"Expected quote '{expectedQuote}', but got '{actualQuote}'");
        }
    }
}

