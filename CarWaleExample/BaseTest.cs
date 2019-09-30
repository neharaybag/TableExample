using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace CarWaleExample
{
    public class BaseTest
    {

        protected IWebDriver Driver;
       

        /// <summary>
        /// Setup
        /// </summary>
        [SetUp]
        public void Setup()
        {
            Driver = InitialLizDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        }


        /// <summary>
        /// Initialize Driver
        /// </summary>
        /// <returns></returns>
        public IWebDriver InitialLizDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            return new ChromeDriver(options);

        }


        /// <summary>
        /// Tear Down
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Driver.Quit();
        }


        /// <summary>
        /// Find elements 
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public IWebElement Find(By locator)
        {
            return Driver.FindElement(locator);
        }


        /// <summary>
        /// Find multiple elements
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> FindMultipleElements(By locator)
        {
            return Driver.FindElements(locator);
        }


        /// <summary>
        /// Click on element
        /// </summary>
        /// <param name="locator"></param>
        protected void ClickElement(By locator)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(ElementToBeClickable(locator));
            Find(locator).Click();
        }


        protected void WaitForElementToDisplay(IWebElement webElement)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(ElementToBeClickable(webElement));
        }

        protected void WaitForElementToDisplay(By locator)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(ElementToBeClickable(locator));
        }


        protected void Click(IWebElement webElement)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(ElementToBeClickable(webElement));          
            webElement.Click();
        }

    }
}

   