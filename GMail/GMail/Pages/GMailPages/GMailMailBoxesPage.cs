using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace GMailTest.Pages.GMailPages
{
    class GMailMailBoxesPage : Page
    {
        public readonly string TITLE = "Gmail";

        [FindsBy(How = How.XPath, Using = "//div[text()='COMPOSE']")]
        private IWebElement composeNewMessageButton { get; set; }

        [FindsBy(How = How.Name, Using = "to")]
        private IWebElement newMessageToField { get; set; }

        [FindsBy(How = How.Name, Using = "subjectbox")]
        private IWebElement newMessageSubjectField { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@aria-label='Message Body']")]
        private IWebElement newMessageTextField { get; set; }

        [FindsBy(How = How.XPath, Using = "//img[@aria-label='Save & Close']")]
        private IWebElement newMessageSaveAndCloseButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'Drafts')]")]
        private IWebElement draftsBoxButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(text(),'Send')]")]
        private IWebElement sendButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'Sent Mail')]")]
        private IWebElement sentMailBoxButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@href,'accounts.google.com/SignOutOptions?')]")]
        private IWebElement googleAccount { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'Sign out')]")]
        private IWebElement signOutButton { get; set; }
        public GMailMailBoxesPage(IWebDriver driver) : base(driver) { }

        public bool LoggedIn()
        {
            return this.composeNewMessageButton.Displayed;
        }

        public void composeMessage(string email, string subject, string text)
        {
            newMessageToField.SendKeys(email);
            newMessageSubjectField.SendKeys(subject);
            newMessageTextField.Click();
            newMessageTextField.SendKeys(text);
        }

        public void selectDraftsMailbox()
        {
            draftsBoxButton.Click();
        }

        public void selectSentMailbox()
        {
            sentMailBoxButton.Click();
        }

        public bool messageWithSubjectPresents(string subject)
        {
            return isElementPresentByLocator(By.XPath("//span[contains(text(),'" + subject + "')]")) || isElementPresentByLocator(By.XPath("//b[contains(text(),'" + subject + "')]"));
        }

        public void openMessageWithSubject(string subject)
        {
            driver.FindElement(By.XPath("//span[contains(text(),'" + subject + "')]")).Click();
        }

        public string getDraftMessageEmail()
        {
            return newMessageToField.GetAttribute("value");
        }

        public string getDraftMessageSubject()
        {
            return newMessageSubjectField.GetAttribute("value");
        }

        public string getDraftMessageText()
        {
            return newMessageTextField.Text;
        }

        public void closeNewMessageWindow()
        {
            newMessageSaveAndCloseButton.Click();
        }

        public void clickSendButton()
        {
            sendButton.Click();
        }

        public void clickCompose()
        {
            composeNewMessageButton.Click();
        }

        public void logOut()
        {
            googleAccount.Click();
            signOutButton.Click();
        }

    }
}
