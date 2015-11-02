using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace GMailTest.Pages.GMailPages
{
    class GmailHomePage : Page
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
        private IWebElement draftsMail { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(text(),'Send')]")]
        private IWebElement sendButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'Sent Mail')]")]
        private IWebElement sentMail { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'Inbox')]")]
        private IWebElement inboxMail { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@href,'accounts.google.com/SignOutOptions?')]")]
        private IWebElement googleAccount { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'Sign out')]")]
        private IWebElement signOutButton { get; set; }
        public GmailHomePage(IWebDriver driver) : base(driver) { }

        public void LogOut()
        {
            Console.WriteLine("Logging out...");
            googleAccount.Click();
            signOutButton.Click();
            Console.WriteLine("PASSED!");
        }

        public void ComposeNewMessage(string email, string subject, string text, bool send)
        {

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            string _temp = draftsMail.Text;

            Console.WriteLine((send) ? "Compose new message and send..." : "Compose new message and save in drafts folder...");
            Console.WriteLine("... Click 'COMPOSE' button...");
            composeNewMessageButton.Click();
            Console.WriteLine("... PASSED!");
            WaitForElementPresentAndEnabled(By.XPath("//div[@role='dialog']"));
            Console.WriteLine("... Fill data for message...");
            newMessageToField.SendKeys(email);
            newMessageSubjectField.SendKeys(subject);
            newMessageTextField.Click();
            newMessageTextField.SendKeys(text);
            Console.WriteLine("... PASSED!");
            Console.WriteLine((send) ? "... Click 'Send' button..." : "... Click 'Save & Close'...");

            if (send) { sendButton.Click(); } else { newMessageSaveAndCloseButton.Click(); }

            if (!send)
            {
                wait.Until(ExpectedConditions.InvisibilityOfElementWithText(By.XPath("//a[contains(text(),'Drafts')]"),_temp));   
            }
            
            Console.WriteLine("PASSED!");
            ResetEnvironment();
        }

        public bool CheckMessageWithSubjectPresents(string folder, string subject)
        {

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            Console.WriteLine("Verifying presence of the message with the subject \"{0}\" in {1} folder...", subject, folder);

            switch (folder.ToLower())
            {
                case "drafts":
                        draftsMail.Click();
                        wait.Until(ExpectedConditions.UrlContains("#drafts"));
                        break;

                case "sentmail":
                        sentMail.Click();
                        wait.Until(ExpectedConditions.UrlContains("#sent"));
                        break;

                default :
                        Console.WriteLine("Not implemented yet...");
                        break;
            }

            if (folder.ToLower()=="drafts")
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@role='main']//span[contains(text(),'" + subject + "')]")));
            }
            else
            {
                Console.WriteLine("looking for b");
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@role='main']//b[contains(text(),'" + subject + "')]")));
            }

            ResetEnvironment();
            Console.WriteLine("PASSED!");
            return true;
        }

        public void CheckDraftsMessageContentAndSend(string email, string subject, string text)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            string _temp = draftsMail.Text;

            Console.WriteLine("Verifying content of drafts message and send it...");

            Console.WriteLine("... Go to Drafts folder...");
            draftsMail.Click();
            wait.Until(ExpectedConditions.UrlContains("#drafts"));
            Console.WriteLine("... PASSED!");
            
            Console.WriteLine("... Search for message with subject \"{0}\" and open it...", subject);
            driver.FindElement(By.XPath("//span[contains(text(),'" + subject + "')]")).Click();
            Console.WriteLine("... PASSED!");

            Console.WriteLine("... Verifying message email...", subject);
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Name("to"), email));
            Console.WriteLine("... PASSED!");

            Console.WriteLine("... Verifying message subject...", subject);
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Name("subjectbox"), subject));
            Console.WriteLine("... PASSED!");

            Console.WriteLine("... Verifying message text...", subject);
            wait.Until(ExpectedConditions.TextToBePresentInElement(newMessageTextField, text));
            Console.WriteLine("... PASSED!");

            Console.WriteLine("... Click 'Send' button...", subject);
            sendButton.Click();
            Console.WriteLine("... PASSED!");

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Your message has been sent.')]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementWithText(By.XPath("//a[contains(text(),'Drafts')]"), _temp));

            ResetEnvironment();
            Console.WriteLine("PASSED!");
        }

        public bool CheckMessageWithSubjectAbsents(string folder, string subject)
        {

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            Console.WriteLine("Verifying absence of the message with the subject \"{0}\" in {1} folder...", subject, folder);

            switch (folder.ToLower())
            {
                case "drafts":
                    draftsMail.Click();
                    wait.Until(ExpectedConditions.UrlContains("#drafts"));
                    break;

                case "sentmail":
                    sentMail.Click();
                    wait.Until(ExpectedConditions.UrlContains("#sent"));
                    break;

                default:
                    Console.WriteLine("Not implemented yet...");
                    break;
            }

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@role='main']//span[contains(text(),'" + subject + "')]")));
            Console.WriteLine("PASSED!");
            ResetEnvironment();
            return true;
        }     
  
        public void ResetEnvironment()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            inboxMail.Click();
            wait.Until(ExpectedConditions.UrlContains("#inbox"));
        }

    }
}
