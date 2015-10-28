using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;
using GMailTest.Pages.GMailPages;
using System.Threading;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace GMailTest
{
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(ChromeDriver))]
    public class GMailTest<TWebdriver> where TWebdriver:IWebDriver , new()
    {
        private static DateTime dt = DateTime.Now;
        private static string USERNAME = "johnsmithtestqwer";
        private static string PASSWORD = "1234rewq1234";
        private static string MESEMAIL = "johnsmithtestqwer@gmail.com";
        private static string MESSUBJECT = "Test message - " + dt.ToString("MM/dd/yyyy HH:mm:ss");
        private static string MESTEXT = "Test message. Sent on " + dt.ToString("MM/dd/yyyy HH:mm:ss");
        private IWebDriver driver;

        [SetUp]
        public void BeforeTest()
        {
            //Choose English language for browser
            //FirefoxProfile profile = new FirefoxProfile();
            //profile.SetPreference("intl.accept_languages", "en-us");
            driver= new TWebdriver();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(100));
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
        }

        [Test]
        public void GmailTest()
        {
            Console.WriteLine("Start browser and go to https://www.gmail.com/intl/en/mail/help/about.html...");
            GMailAboutPage gmailaboutpage = new GMailAboutPage(driver);
            gmailaboutpage.openPage();
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Checking page title...");
            Assert.AreEqual(gmailaboutpage.TITLE, gmailaboutpage.getPageTitle());
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Click 'Sign in' and go to GMail sign in/log in page...");
            GMailSignInPage gmailsigninpage = gmailaboutpage.signIn();
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Checking page title...");
            Assert.AreEqual(gmailsigninpage.TITLE, gmailsigninpage.getPageTitle());
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Enter data for log in and logging in...");
            GMailMailBoxesPage gmailmailboxespage = gmailsigninpage.logIn(USERNAME,PASSWORD);
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Checking page title...");
            Assert.AreEqual(gmailmailboxespage.TITLE, gmailmailboxespage.getPageTitle());
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Check that we logged in...");
            Assert.IsTrue(gmailmailboxespage.LoggedIn());
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Click COMPOSE...");
            gmailmailboxespage.clickCompose();
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Create new message with subject \"{0}\"...", MESSUBJECT);
            gmailmailboxespage.composeMessage(MESEMAIL, MESSUBJECT, MESTEXT);
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Click Save & Close...");
            gmailmailboxespage.closeNewMessageWindow();
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Select Drafts folder...");
            gmailmailboxespage.selectDraftsMailbox();
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Verifying presence of the message with the subject \"{0}\"...", MESSUBJECT);
            Assert.IsTrue(gmailmailboxespage.messageWithSubjectPresents(MESSUBJECT));
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Open message with the subject \"{0}\"...", MESSUBJECT);
            gmailmailboxespage.openMessageWithSubject(MESSUBJECT);
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Verifying data in message to email filed. Expecting data = \"{0}\"...", MESEMAIL);
            Assert.IsTrue(gmailmailboxespage.getDraftMessageEmail().Contains(MESEMAIL));
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Verifying data in message subject filed. Expecting data = \"{0}\"...", MESSUBJECT);
            Assert.AreEqual(MESSUBJECT, gmailmailboxespage.getDraftMessageSubject());
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Verifying data in message text filed. Expecting data = \"{0}\"...", MESTEXT);
            Assert.AreEqual(MESTEXT, gmailmailboxespage.getDraftMessageText());
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Click 'Send' button...");
            gmailmailboxespage.clickSendButton();
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Refresh page...");
            gmailmailboxespage.pageRefresh();
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Select Drafts folder...");
            gmailmailboxespage.selectDraftsMailbox();
            Console.WriteLine("PASSED!");
            Thread.Sleep(5000);
            Console.WriteLine("Verifying absence of the message with the subject \"{0}\"...", MESSUBJECT);
            Assert.IsFalse(gmailmailboxespage.messageWithSubjectPresents(MESSUBJECT));
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Select SentMail folder...");
            gmailmailboxespage.selectSentMailbox();
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Verifying presence of the message with the subject \"{0}\"...", MESSUBJECT);
            Assert.IsTrue(gmailmailboxespage.messageWithSubjectPresents(MESSUBJECT));
            Console.WriteLine("PASSED!");
            Thread.Sleep(1000);
            Console.WriteLine("Logging out...");
            gmailmailboxespage.logOut();
            Console.WriteLine("PASSED!");
        }

    }
}
