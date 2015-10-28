using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace GMailTest.Pages.GMailPages
{
    class GMailSignInPage : Page
    {
        public readonly string TITLE = "Gmail";

        [FindsBy(How = How.Id, Using = "Email")]
        private IWebElement inputEmailField { get; set; }

        [FindsBy(How = How.Id, Using = "Passwd")]
        private IWebElement inputPasswordField { get; set; }

        [FindsBy(How = How.Id, Using = "next")]
        private IWebElement nextButton { get; set; }

        [FindsBy(How = How.Id, Using = "signIn")]
        private IWebElement signInButton { get; set; }

        public GMailSignInPage(IWebDriver driver) : base(driver) { }

        public GMailMailBoxesPage logIn(string username, string password)
        {
            inputEmailField.SendKeys(username);
            nextButton.Click();
            inputPasswordField.SendKeys(password);
            signInButton.Click();
            return new GMailMailBoxesPage(this.driver);
        }

    }
}
