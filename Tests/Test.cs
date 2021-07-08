using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using SeleniumCsharp.BaseClass;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports;
using Microsoft.Extensions.Configuration;

namespace SeleniumCsharp
{
  
    [TestFixture]
    public class Tests : BaseTest
        
    {
        IConfiguration Configuration = BaseTest.InitConfiguration();
            
        [Test, Order(0)]
        [Author("Phaneendra")]
        public void verifyTitle()
        {
            try
            {
             //   UITest(() =>
             //   {
                    //   Assert.Ignore("Defect ID: 1234");
                    test = extent.CreateTest("verifyTitle");
                    String actualTitle = driver.Title;
                
                Assert.AreEqual(actualTitle, Configuration.GetSection("expectedTitle").Value);
                    test.Log(Status.Info, "Verify Title");

             //   });
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());

                throw;
            }



        }

        [Test, Order(1)]
        [Author("Phaneendra")]
        public void noCredentials()
        {
            try
            {
             //   UITest(() =>
             //   {
             //       Assert.Ignore("Defect ID: 1235");
                    test = extent.CreateTest("noCredentials");
                    IWebElement userNameTextField = driver.FindElement(By.Id("emailInput"));
                    userNameTextField.Clear();
                    IWebElement password = driver.FindElement(By.XPath("(//input[@type='password'])[1]"));
                    password.Clear();
                    Assert.IsFalse(driver.FindElement(By.XPath("(//button[@type='submit'])[1]")).Enabled, "LOGIN button is disabled.");
                    test.Log(Status.Info, "No Credentials");
             //   });
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());

                throw;
            }
        }

        [Test, Order(2)]
        [Author("Phaneendra")]
        [Obsolete]
        public void InValidCredentials()
        {
            try
            {
             //   UITest(() =>
             //   {
                  //  Assert.Ignore("Defect ID: 1236");
                    test = extent.CreateTest("InvalidCredentials");
                    IWebElement userNameTextField = driver.FindElement(By.Id("emailInput"));
                    userNameTextField.Clear();
                    userNameTextField.SendKeys(Configuration.GetSection("inValiduserNameOne").Value);
                    Thread.Sleep(3000);
                    IWebElement password = driver.FindElement(By.XPath("(//input[@type='password'])[1]"));
                    password.Clear();
                    password.SendKeys(Configuration.GetSection("inValidPasswordOne").Value);
                    Assert.IsFalse(driver.FindElement(By.XPath("(//button[@type='submit'])[1]")).Enabled, "LOGIN button is disabled.");
                    test.Log(Status.Info, "Verify LOGIN button is disabled with less than 8 Charcters");
                    Thread.Sleep(3000);
                    password.Clear();
                    password.SendKeys(Configuration.GetSection("inValidPasswordTwo").Value);
                    Assert.IsTrue(driver.FindElement(By.XPath("(//button[@type='submit'])[1]")).Enabled, "LOGIN button is enabled.");
                    test.Log(Status.Info, "Verify LOGIN button is enabled with Greater than 8 Charcters");
                    Thread.Sleep(2000);
                    IWebElement click = driver.FindElement(By.XPath("(//button[@type='submit'])[1]"));
                    click.Click();

                WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    w.Until(ExpectedConditions.ElementExists(By.XPath("//div[@class='toast-message']")));

                    String errorMessage = driver.FindElement(By.XPath("//div[@class='toast-message']")).Text;
                   
                    Assert.AreEqual(errorMessage, Configuration.GetSection("expectederrorMessage").Value);
                    test.Log(Status.Info, "Invalid Credentials");
                    Thread.Sleep(5000);

             //   });
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());

                throw;
            }

        }
        [Test, Order(3)]
        [Author("Phaneendra")]
        [Obsolete]
        public void validCredentials()
        {
            try
            {
              //  UITest(() =>
             //   {
              //      Assert.Ignore("Defect ID: 1237");
                    test = extent.CreateTest("validCredentials");
                    IWebElement userNameTextField = driver.FindElement(By.Id("emailInput"));
                    userNameTextField.Clear();
                    userNameTextField.SendKeys(Configuration.GetSection("validuserName").Value);
                    Thread.Sleep(3000);
                    IWebElement password = driver.FindElement(By.XPath("(//input[@type='password'])[1]"));
                    password.Clear();
                    password.SendKeys(Configuration.GetSection("validPassword").Value);

                    IWebElement click = driver.FindElement(By.XPath("(//button[@type='submit'])[1]"));
                    click.Click();
                    Thread.Sleep(3000);

                    WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    w.Until(ExpectedConditions.ElementExists(By.XPath("(//p[@class='ng-binding'])[1]")));

                    String successMessage = driver.FindElement(By.XPath("(//p[@class='ng-binding'])[1]")).Text;
           
                    Assert.AreEqual(successMessage, configuration.GetSection("loginSuccessMessage").Value);
                    test.Log(Status.Info, "Verify LOGIN and Home Page");

              //  });
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());

                throw;
            }

        }
    }
}