using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using NUnit.Framework.Interfaces;
using Microsoft.Extensions.Configuration;

namespace SeleniumCsharp.BaseClass
{
    
    public  class BaseTest
    {
        
        public IWebDriver driver;
        public  ExtentReports extent = null;
        public ExtentTest test;
        public IConfiguration configuration;
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();
            return config;
        }

        [OneTimeSetUp]
        public void Open()
        {

            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            configuration = InitConfiguration();
            driver.Url = configuration.GetSection("url").Value;
            Thread.Sleep(5000);

            extent = new ExtentReports();
            var htmlReporter = new ExtentHtmlReporter(@"D:\Testing\SeleniumC#\SeleniumCsharp\HtmlReports\Reports" + ".html");
            extent.AttachReporter(htmlReporter);
            //Add QA system info to html report

            extent.AddSystemInfo("Host Name", "Local");
            extent.AddSystemInfo("Environment", "QAEnvironment");
            extent.AddSystemInfo("Username", "Phaneendra");

        }



        /*   public void UITest(Action action)
           {
               try
               {
                   action();

                   ITakesScreenshot ts = driver as ITakesScreenshot;
                   Screenshot screenshot = ts.GetScreenshot();
                   // screenshot.SaveAsFile("D:\\Testing\\SeleniumC#\\SeleniumCsharp\\Screenshots\\Screenshot.png, ScreenshotImageFormat.Png);
                   screenshot.SaveAsFile("D:\\Testing\\SeleniumC#\\SeleniumCsharp\\Screenshots\\Screenshot" + DateTime.Now.Second + ".png" , ScreenshotImageFormat.Png);

               }
               catch (Exception e)
               {
                   throw;
               }
           }*/
        [TearDown]
        public void GetResult()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = "<pre>" + TestContext.CurrentContext.Result.StackTrace + "</pre>";
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
            {
                string screenShotPath = GetScreenShot.Capture(driver, "screenShotName");
                test.Log(Status.Fail, stackTrace + errorMessage);
                test.Log(Status.Fail, "Snapshot below: " + test.AddScreenCaptureFromPath(screenShotPath));
               
            }
            
        }
        public class GetScreenShot
        {
            public static string Capture(IWebDriver driver, string screenShotName)
            {
                ITakesScreenshot ts = (ITakesScreenshot)driver;
                Screenshot screenshot = ts.GetScreenshot();
                string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
               string finalpth = pth.Substring(0, pth.LastIndexOf("bin")) + "ErrorScreenshots\\" + screenShotName + ".png";
               string localpath = new Uri(finalpth).LocalPath;
                screenshot.SaveAsFile("D:\\Testing\\SeleniumC#\\SeleniumCsharp\\Screenshots\\Screenshot" + DateTime.Now.Second + ".png", ScreenshotImageFormat.Png);
                return localpath;
            }
        }

        [OneTimeTearDown]
        public void Close()
        {
            driver.Quit();
            extent.Flush();
        }

    }
}
