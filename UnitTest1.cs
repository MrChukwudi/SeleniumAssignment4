using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

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

        [Test]
        public void InsuranceQuote01_ValidData_NoAccidents()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doe@mail.com", 25, 3, 0);
            AssertValidation("Insurance quote generated successfully");
        }

        [Test]
        public void InsuranceQuote02_ValidData_TwoAccidents()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doe@mail.com", 25, 3, 2);
            AssertValidation("Insurance quote generated successfully");
        }

        [Test]
        public void InsuranceQuote03_ValidData_FourAccidents()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doe@mail.com", 35, 10, 4);
            AssertValidation("Insurance denied due to too many accidents");
        }

        [Test]
        public void InsuranceQuote04_InvalidPhoneNumber()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123123123", "john.doe@mail.com", 27, 3, 0);
            AssertValidation("Invalid phone number format");
        }

        [Test]
        public void InsuranceQuote05_InvalidEmail()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doemail.com", 28, 3, 0);
            AssertValidation("Invalid email address format");
        }

        [Test]
        public void InsuranceQuote06_InvalidPostalCode()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "123456", "123-123-1234", "john.doe@mail.com", 35, 17, 1);
            AssertValidation("Invalid postal code format");
        }

        [Test]
        public void InsuranceQuote07_MissingAge()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doe@mail.com", null, 5, 0);
            AssertValidation("Age is a required field");
        }

        [Test]
        public void InsuranceQuote08_MissingAccidents()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doe@mail.com", 37, 8, null);
            AssertValidation("Number of accidents is a required field");
        }

        [Test]
        public void InsuranceQuote09_MissingExperience()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doe@mail.com", 45, null, 0);
            AssertValidation("Driving experience is a required field");
        }

        [Test]
        public void InsuranceQuote10_InvalidAge()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doe@mail.com", 15, 0, 0);
            AssertValidation("Age must be greater than or equal to 16");
        }

        [Test]
        public void InsuranceQuote11_InvalidAgeExperienceDifference()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doe@mail.com", 30, 20, 0);
            AssertValidation("Experience must not exceed Age - 16");
        }

        [Test]
        public void InsuranceQuote12_InvalidExperience()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doe@mail.com", 25, -1, 0);
            AssertValidation("Experience must be a positive integer");
        }

        [Test]
        public void InsuranceQuote13_InvalidPhoneFormat()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "1234567890", "john.doe@mail.com", 30, 10, 0);
            AssertValidation("Invalid phone number format");
        }

        [Test]
        public void InsuranceQuote14_AgeBoundary()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doe@mail.com", 16, 0, 0);
            AssertValidation("Insurance quote generated successfully");
        }

        [Test]
        public void InsuranceQuote15_MaxExperienceBoundary()
        {
            FillMandatoryFields("John", "Doe", "123 Test St", "Waterloo", "ON", "N2L 3G1", "123-123-1234", "john.doe@mail.com", 50, 34, 0);
            AssertValidation("Insurance quote generated successfully");
        }

        private void FillMandatoryFields(string firstName, string lastName, string address, string city, string province, string postalCode, string phone, string email, int? age, int? experience, int? accidents)
        {
            if (firstName != null) SetField(By.Id("firstName"), firstName);
            if (lastName != null) SetField(By.Id("lastName"), lastName);
            SetField(By.Id("address"), address);
            SetField(By.Id("city"), city);
            SetField(By.Id("postalCode"), postalCode);
            SetField(By.Id("phone"), phone);
            SetField(By.Id("email"), email);
            if (age.HasValue) SetField(By.Id("age"), age.ToString());
            if (experience.HasValue) SetField(By.Id("experience"), experience.ToString());
            if (accidents.HasValue) SetField(By.Id("accidents"), accidents.ToString());

            driver.FindElement(By.Id("SubmitButton")).Click();
        }

        private void SetField(By by, string value)
        {
            var element = driver.FindElement(by);
            element.Clear();
            element.SendKeys(value);
        }

        private void AssertValidation(string expectedMessage)
        {
            var finalQuote = driver.FindElement(By.Id("finalQuote")).Text;
            Assert.AreEqual(expectedMessage, finalQuote);
        }
    }
}

