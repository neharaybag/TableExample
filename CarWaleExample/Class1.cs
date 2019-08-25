using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWaleExample
{
    public class CarTests
    {
        private IWebDriver driver;
        Dictionary<string, Dictionary<String, List<Vehicle>>> carBrands = new Dictionary<string, Dictionary<String, List<Vehicle>>>();

        #region  WebElements

        private By lnkBrands = By.ClassName("brand-type-title");
        private By lnkCarTypes = By.ClassName("text-unbold");
        private By listCarModels = By.CssSelector("#divModels li.list-seperator");
        private By lblCarName = By.CssSelector(".grid-7.omega .text-unbold");
        private By lblCarPrice = By.CssSelector(".omega.grid-7 div.margin-top15.font20");
        private By lblBreadCrumb = By.CssSelector("[itemprop='itemlistelement']");
        private By variantTable = By.CssSelector("tbody .variant");
        private By lblVariantName = By.CssSelector(".variant__name-cell .variant__name a");
        private By lblVariantPrice = By.CssSelector(".variant__price-cell span.variant__price");

        #endregion 

        [SetUp]
        public void Setup()
        {
            driver = InitialLizDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

        }


        [Test]
        [TestCase ("Maruti Suzuki")]
        [TestCase("Mahindra")]
        public void Test(string brandName)
        {
            List<Vehicle> carTypes = new List<Vehicle>();
            List<Vehicle> listVariants = new List<Vehicle>();
            Dictionary<String, List<Vehicle>> carVariants = new Dictionary<String, List<Vehicle>>();
          
            driver.Navigate().GoToUrl("https://www.carwale.com/");

            // Click on Brand
            SelectBrand(brandName);

            // Get all car types by brand
            var listCars = FindMultipleElements(listCarModels).ToList();
 

            for(int counter=0;counter< listCars.Count; counter++)
            {
                listVariants = new List<Vehicle>();
                carTypes = new List<Vehicle>();
                var car = listCars[counter];
                var name = car.FindElement(lblCarName).Text;
                var price = car.FindElement(lblCarPrice).Text;
                car.FindElement(lblCarName).Click();
                var variants = FindMultipleElements(variantTable).ToList();

                // Get list of car vairiants
                if(variants!=null)
                {
                    variants.ForEach(x => listVariants.Add(new Vehicle(x.FindElement(lblVariantName).Text, x.FindElement(lblVariantPrice).Text)));
                }
                
                // Store car name and its variants
                carTypes.Add(new Vehicle(name, price, listVariants));

                //Add key and value to dictionary
                carVariants.Add(name, carTypes);

                //Naviaget back to list of cars by brand
                Find(lblBreadCrumb).Click();

                // Get elements again after navigating back 
                listCars = FindMultipleElements(listCarModels).ToList();
            }
           

            //Add car brand and list of car types in dictionary
            carBrands.Add(brandName, carVariants);
        }


        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }


        /// <summary>
        /// One time tear down
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt")))
            {
                foreach (var carBrand in carBrands)
                {
                    // Print brand name
                    outputFile.WriteLine($"{carBrand.Key} =>");
                    foreach (var carVariant in carBrand.Value)
                    {
                        // Print car in brand
                        outputFile.WriteLine($"  {carVariant.Key} =>");
                        var cars = carVariant.Value;

                        // Print lis of car variants for each car
                        foreach (var car in cars)
                        {
                            var carVariants = car.Variants;
                            foreach (var variants in carVariants)
                            {
                                outputFile.WriteLine($"    {variants.CarName} {variants.Price}");
                            }
                        }

                        outputFile.WriteLine();
                    }
                }
            }           
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
        /// Find elements 
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public IWebElement Find(By locator)
        {
            return driver.FindElement(locator);
        }


        /// <summary>
        /// Find multiple elements
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> FindMultipleElements(By locator)
        {
            return driver.FindElements(locator);
        }


        /// <summary>
        /// Select Brand
        /// </summary>
        /// <param name="brand"></param>
        public void SelectBrand(string brand)
        {
            var brands = FindMultipleElements(lnkBrands).ToList();
            brands.Find(x => x.Text.Equals(brand)).Click();
        }
    }

}
