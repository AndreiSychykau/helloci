using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace GMailTest.Pages.GMailPages
{
    class GmailStartPage : Page
    {
        public readonly string TITLE = "Gmail";

        [FindsBy(How = How.Id, Using = "Email")]
        private IWebElement inputUsernameField { get; set; }

        [FindsBy(How = How.Id, Using = "Passwd")]
        private IWebElement inputPasswordField { get; set; }

        [FindsBy(How = How.Id, Using = "next")]
        private IWebElement nextButton { get; set; }

        [FindsBy(How = How.Id, Using = "signIn")]
        private IWebElement signInButton { get; set; }

        public GmailStartPage(IWebDriver driver) : base(driver) { }

        public GmailHomePage LogIn(string username, string password)
        {
            Console.WriteLine("Login GMail...");
            Console.WriteLine("... Go to http://gmail.com...");
            this.driver.Navigate().GoToUrl("http://gmail.com");
            Console.WriteLine("... PASSED!");
            Console.WriteLine("... Input username: {0}...",username);
            inputUsernameField.SendKeys(username);
            nextButton.Click();
            Console.WriteLine("... PASSED!");
            Console.WriteLine("... Input password: {0}...", password);
            inputPasswordField.SendKeys(password);
            signInButton.Click();
            Console.WriteLine("... PASSED!");
            GmailHomePage page = new GmailHomePage(this.driver);
            Console.WriteLine("... Check that user {0} logged in...",username);
            WaitForElementPresentAndEnabled(By.XPath("//a[@title='Google Account: " + username + "@gmail.com']"));
            Console.WriteLine("PASSED!");
            return new GmailHomePage(this.driver);
        }
    }
}
