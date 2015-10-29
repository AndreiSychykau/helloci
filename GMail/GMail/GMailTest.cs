using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using GMailTest.Pages.GMailPages;

namespace GMailTest
{
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(ChromeDriver))]
    public class GMailTest<TWebDriver> where TWebDriver : IWebDriver , new()
    {
        private static DateTime dt = DateTime.Now;
        private static string _userName = "johnsmithtestqwer";
        private static string _passWord = "1234rewq1234";
        private static string _mesEmail = "johnsmithtestqwer@gmail.com";
        private static string _mesSubject = "Test message - " + dt.ToString("MM/dd/yyyy HH:mm:ss");
        private static string _mesText = "Test message. Sent on " + dt.ToString("MM/dd/yyyy HH:mm:ss");
        private IWebDriver driver;

        [SetUp]
        public void BeforeTest()
        {
            driver= new TWebDriver();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
        }

        [Test(Description="Login Gmail, create message in draft, send it and logout...")]
        public void GmailTest()
        {
            GmailStartPage startpage = new GmailStartPage(driver);
            GmailHomePage homepage = startpage.LogIn(_userName, _passWord);
            homepage.ComposeNewMessage(_mesEmail, _mesSubject, _mesText, false);
            Assert.IsTrue(homepage.CheckMessageWithSubjectPresents("Drafts", _mesSubject), "Message with subject \"{0}\" was not found...", _mesSubject);
            homepage.CheckDraftsMessageContentAndSend(_mesEmail, _mesSubject, _mesText);
            Assert.IsTrue(homepage.CheckMessageWithSubjectAbsents("Drafts", _mesSubject), "Message with subject \"{0}\" was found...", _mesSubject);
            Assert.IsTrue(homepage.CheckMessageWithSubjectPresents("SentMail", _mesSubject), "Message with subject \"{0}\" was not found...", _mesSubject);
            homepage.LogOut();
        }
    }
}
