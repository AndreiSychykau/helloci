using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Threading;
using OpenQA.Selenium.Interactions;

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

        public void PageRefresh()
        {
            this.driver.Navigate().Refresh();
            try
            {
                this.driver.SwitchTo().Alert().Accept(); 
            }
            catch (NoAlertPresentException) { }
        }

        public void WaitForElementPresentAndEnabled(By locator, int secondsToWait = 30)
        {
            new WebDriverWait(this.driver, new TimeSpan(0, 0, secondsToWait))
               .Until(d => d.FindElement(locator).Enabled
                   && d.FindElement(locator).Displayed
               );
        }
   
    }

}
