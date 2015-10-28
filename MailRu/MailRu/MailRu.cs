using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

namespace MailRu
{
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(ChromeDriver))]
    public class MailRu<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        public IWebDriver driver;

        [SetUp]
        public void BeforeTest()
        {
                //driver = new FirefoxDriver();
            this.driver = new TWebDriver();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            driver.Manage().Window.Maximize();

        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
        }

        [Test]
        public void MailRuTest()
        {

            // Data for test
            DateTime dt = DateTime.Now;
            string[] loginData = { "johnsmithtest", "@mail.ru", "1234qwer" };
            string[] messageData = { "johnsmithtest@mail.ru", "Test message - " + dt.ToString("MM/dd/yyyy HH:mm:ss"), "Hello! This is test message was sent " + dt.ToString("MM/dd/yyyy HH:mm:ss")};

            // Test
            Console.WriteLine("Start browser and go to http://mail.ru...");
            driver.Navigate().GoToUrl("http://mail.ru");
            Console.WriteLine("PASSED!");

            Console.WriteLine("Entering login, domain, password and logging in...");
            driver.FindElement(By.Id("mailbox__login")).SendKeys(loginData[0]);
            driver.FindElement(By.Id("mailbox__login__domain")).SendKeys(loginData[1]);
            driver.FindElement(By.Id("mailbox__password")).SendKeys(loginData[2]);
            driver.FindElement(By.Id("mailbox__auth__button")).Click();
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Check that logging in was successful...");
            Assert.AreEqual(driver.FindElement(By.Id("PH_user-email")).Text, "johnsmithtest@mail.ru");
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Click New Message...");
            driver.FindElement(By.XPath("//div[2]/div/a/span")).Click();
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Entering message e-mail address...");
            driver.FindElement(By.XPath("//textarea[2]")).SendKeys(messageData[0]);
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Entering message subject...");
            driver.FindElement(By.Name("Subject")).SendKeys(messageData[1]);
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Entering message text...");
            driver.SwitchTo().Frame(driver.FindElement(By.XPath("//iframe[contains(@id,'composeEditor_ifr')]")));
            driver.FindElement(By.Id("tinymce")).SendKeys(Keys.Control + "a");
            driver.FindElement(By.Id("tinymce")).SendKeys(messageData[2]);
            driver.SwitchTo().DefaultContent();
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Clicking Save...");
            driver.FindElement(By.XPath("//span[contains(text(),'Сохранить')]")).Click();
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Verifying presence of the notification that message was saved in drafts...");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Сохранено в')]")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(text(),'черновиках')]")));
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Go to drafts folder...");
            driver.FindElement(By.XPath("//span[contains(text(),'Черновики')]")).Click();
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Verifying that message presents in drafts folder and open it...");
            try
            {
                //driver.FindElement(By.XPath("//div[contains(@id,'b-letters')]/descendant::a[@data-subject='" + messageData[1] + "'][@title='" + messageData[0] + "']")).Click();
                //driver.FindElement(By.XPath("//div[contains(@class,'b-datalist__item__subj')][contains(text(),'" + messageData[1] + "']")).Click();
                driver.FindElement(By.XPath("//div[contains(@id,'b-letters')]/descendant::a[@data-subject='" + messageData[1] + "'][@title='" + messageData[0] + "']/div[@class='b-datalist__item__panel']/div[@class='b-datalist__item__info']")).Click();           
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("Message to {0} with subject \"{1}\" was not found in draft folder...", messageData[0], messageData[1]);
                throw e;
            }
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Verifying e-mail address...");
            Assert.AreEqual(driver.FindElement(By.XPath("//div[@id='compose__header__content']/div[2]/div[2]/div/span[3]/span")).Text, messageData[0]);
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Verifying message subject...");
            Assert.AreEqual(driver.FindElement(By.Name("Subject")).GetAttribute("value"), messageData[1]);
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Verifying message text...");
            driver.SwitchTo().Frame(driver.FindElement(By.XPath("//iframe[contains(@id,'composeEditor_ifr')]")));
            Assert.AreEqual(driver.FindElement(By.Id("tinymce")).Text, messageData[2]);
            driver.SwitchTo().DefaultContent();
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);
            
            Console.WriteLine("Clicking Send...");
            driver.FindElement(By.XPath("//span[contains(text(),'Отправить')]")).Click();
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Checking whether we moved from drafts folder...");
            try
            {
                driver.FindElement(By.XPath("//div[contains(text(),'Получатели:')]"));
                driver.FindElement(By.XPath("//span[contains(text(),'johnsmithtest@mail.ru')]"));
            }
            catch (NoSuchElementException e)
            {
                throw e;
            }
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Go to drafts folder...");
            driver.FindElement(By.XPath("//span[contains(text(),'Черновики')]")).Click();
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Verifying abcence of the message in drafts folder...");
            Assert.IsTrue(driver.FindElements(By.XPath("//div[contains(@id,'b-letters')]/descendant::a[@data-subject='" + messageData[1] + "'][@title='" + messageData[0] + "']")).Count() == 0);
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Go to sent folder...");
            driver.FindElement(By.XPath("//span[contains(text(),'Отправленные')]")).Click();
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Verifying that message presents in sent folder...");
            try
            {
                driver.FindElement(By.XPath("//div[contains(@id,'b-letters')]/descendant::a[@data-subject='" + messageData[1] + "'][@title='" + messageData[0] + "']"));
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("Message to {0} with subject \"{1}\" was not found in sent folder...", messageData[0], messageData[1]);
                throw e;
            }
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Clicking log out...");
            driver.FindElement(By.Id("PH_logoutLink")).Click();
            Console.WriteLine("PASSED!");

            Thread.Sleep(1000);

            Console.WriteLine("Check that log out was successful...");
            Assert.AreEqual(driver.FindElement(By.Id("PH_authLink")).Text, "Вход");
            Console.WriteLine("PASSED!");
        }
    }
}
