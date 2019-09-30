using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarWaleExample
{
    public class CarTests : BaseTest
    {
       
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
        private By lnkShowPriceInCity = By.CssSelector("[data-action='showpriceinmycity']");
        private By txtSelectCity = By.CssSelector("[placeholder='Select City']");
        private By lnkCityOptions = By.CssSelector("li.ui-menu-item a");
        private By lnkAreas = By.CssSelector("[cityname]");
        private By txtArea = By.CssSelector("[placeholder='Select Area']");
        private By btnCheckNow = By.Id("ctaClick");
        private By lblCity = By.CssSelector(".selectcustom-input span");
        private By lblLocation = By.Id("global-place");
        private By lnkEMI = By.CssSelector("[data-action='EMICalculatorLink']");
        private By tagTd = By.TagName("td");
        private By sliderDownPayment = By.CssSelector(".downpayment-unit button.rheostat-handle");
        #endregion 

      


        [Test]
        [TestCase ("Maruti Suzuki")]
        [TestCase("Mahindra")]
        public void Test(string brandName)
        {
            List<Vehicle> carTypes = new List<Vehicle>();
            List<Vehicle> listVariants = new List<Vehicle>();
            Dictionary<String, List<Vehicle>> carVariants = new Dictionary<String, List<Vehicle>>();
          
            Driver.Navigate().GoToUrl("https://www.carwale.com/");

            // Click on Brand
            SelectBrand(brandName);

            // Get all car types by brand
            var listCars = FindMultipleElements(listCarModels).ToList();
 

            for(int counter=0; counter < listCars.Count; counter++)
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
                    //variants.ForEach(x => listVariants.Add(new Vehicle(x.FindElement(lblVariantName).Text, x.FindElement(lblVariantPrice).Text)));
                    foreach(var variant in variants)
                    {
                        var columns = variant.FindElements(tagTd).ToList();
                        columns.ForEach(x => Console.WriteLine(x.Text));

                        // To save values for future use
                        var variantname = columns[0].Text; // save car name
                        var variantPrice = columns[1].Text; // save price
                    }
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


        /// <summary>
        /// One time tear down
        /// </summary>
       // [OneTimeTearDown]
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
        /// Select Brand
        /// </summary>
        /// <param name="brand"></param>
        public void SelectBrand(string brand)
        {
            var brands = FindMultipleElements(lnkBrands).ToList();
            brands.Find(x => x.Text.Equals(brand)).Click();
        }


        [Test]
        [TestCase("Mahindra", "Scorpio")]
        public void LocationTest(string brand, string car)
        {
            Driver.Navigate().GoToUrl("https://www.carwale.com/");

            // Click on Brand
            SelectBrand(brand);

            // Get all car types by brand
            var listCarsShowPrice = FindMultipleElements(lnkShowPriceInCity).ToList();

            // Click on show price in my city
            Click(listCarsShowPrice.Find(x => x.GetAttribute("title").Contains(car)));
           

            // Enter City
            Find(txtSelectCity).SendKeys("pu");

            // Select city from dropdown
            var cities = FindMultipleElements(lnkCityOptions).ToList();
            Click(cities.Find(x => x.Text.Contains("Pune")));

            // Select area
            Find(txtArea).SendKeys("baner");
            Thread.Sleep(10000);
            var areas = FindMultipleElements(lnkAreas).ToList();
            foreach (var area in areas)
            {
                var city = area.GetAttribute("cityname");
            }
           
            var element = areas.Find(x => x.GetAttribute("cityname").Contains("baner"));
            element.Click();

            // Click on check now
            ClickElement(btnCheckNow);

            // Check city is selected
            Assert.That(Find(lblCity).Text.Contains("Pune"), "City is not displayed");

            // Check global place updated
            Assert.That(Find(lblLocation).GetAttribute("title").Contains("Pune"), "City is not displayed");
            Assert.That(Find(lblLocation).GetAttribute("title").Contains("Baner"), "City is not displayed");

            // Click on Emi calculator
            Find(lnkEMI).Click();

            // Move slider
            var move = new Actions(Driver);
            move.ClickAndHold(Find(sliderDownPayment)).MoveByOffset(((int)Find(sliderDownPayment).Size.Width / 4), 0)
            .Release().Perform();



        }
    }

}
