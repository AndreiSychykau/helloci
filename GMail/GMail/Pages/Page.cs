using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Threading;

namespace GMailTest.Pages
{
    public abstract class Page
    {

        protected IWebDriver driver;

        public Page(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public string getPageTitle()
        {
            return this.driver.Title;
        }

        public bool isElementPresentByLocator(By locator)
        {
            return driver.FindElements(locator).Count() > 0;
        }

        public void pageRefresh()
        {
            this.driver.Navigate().Refresh();
            Thread.Sleep(1000);
            try
            {
                this.driver.SwitchTo().Alert().Accept(); 
            }
            catch (NoAlertPresentException) { }
        }

    }

}
