using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace SeleniumDemo
{
    public class Tests
    {
        string testUrl = "https://the-internet.herokuapp.com/";
        IWebDriver driver;

        [SetUp]
        public void Start_browser()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = testUrl;
        }

        [Test]
        public void Can_load_webpage()
        {
            Assert.That(driver.Title, Is.EqualTo("The Internet"));
        }

        [Test]
        public void Can_navigate_back_and_forth()
        {
            IWebElement link = driver.FindElement(By.LinkText("Status Codes"));
            link.Click();
            IWebElement code = driver.FindElement(By.LinkText("301"));
            code.Click();
            var codeUrl = driver.Url;
            driver.Navigate().Back();
            IWebElement differentCode = driver.FindElement(By.LinkText("500"));
            differentCode.Click();

            Assert.That(codeUrl, Does.EndWith("/status_codes/301"));
            Assert.That(driver.Url, Does.EndWith("/status_codes/500"));
        }

        [Test]
        public void Can_fill_out_form()
        {
            IWebElement link = driver.FindElement(By.LinkText("Form Authentication"));
            link.Click();
            driver.FindElement(By.Id("username")).SendKeys("tomsmith");
            driver.FindElement(By.Id("password")).SendKeys("SuperSecretPassword!");
            IWebElement button = driver.FindElement(By.ClassName("radius"));
            button.Click();

            Assert.That(driver.Url, Does.EndWith("/secure"));
        }

        [Test]
        public void Can_acknowledge_different_alerts()
        {
            IWebElement link = driver.FindElement(By.LinkText("JavaScript Alerts"));
            link.Click();

            //Alert
            IWebElement button = driver.FindElement(By.XPath("/html/body/div[2]/div/div/ul/li[1]/button"));
            button.Click();
            var confirm = driver.SwitchTo().Alert();
            confirm.Accept();
            IWebElement result = driver.FindElement(By.Id("result"));

            Assert.That(result.Text, Is.EqualTo("You successfully clicked an alert"));

            //Confirmation
            button = driver.FindElement(By.XPath("/html/body/div[2]/div/div/ul/li[2]/button"));
            button.Click();
            confirm = driver.SwitchTo().Alert();
            confirm.Accept();
            result = driver.FindElement(By.Id("result"));

            Assert.That(result.Text, Is.EqualTo("You clicked: Ok"));

            //Prompt
            button = driver.FindElement(By.XPath("/html/body/div[2]/div/div/ul/li[3]/button"));
            button.Click();
            confirm = driver.SwitchTo().Alert();
            confirm.SendKeys("Ground control to Major Tom");
            confirm.Accept();
            result = driver.FindElement(By.Id("result"));

            Assert.That(result.Text, Is.EqualTo("You entered: Ground control to Major Tom"));
        }

        [TearDown]
        public void Close_browser()
        {
            driver.Quit();
        }
    }
    
}