using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace GMailTest.Pages.GMailPages
{
    class GMailAboutPage : Page
    {

        private static string URL="https://www.gmail.com/intl/en/mail/help/about.html";
        public readonly string TITLE = "Gmail - Free Storage and Email from Google";

        [FindsBy(How = How.Id, Using = "gmail-sign-in")]
        private IWebElement signInButton { get; set; }

        public GMailAboutPage(IWebDriver driver) : base(driver) {}

        public GMailAboutPage openPage()
        {
            this.driver.Navigate().GoToUrl(URL);
            return this;
        }

        public GMailSignInPage signIn()
        {
            signInButton.Click();
            return new GMailSignInPage(this.driver);
        }

    }
}
