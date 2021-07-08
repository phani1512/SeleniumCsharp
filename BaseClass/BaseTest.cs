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
using System.Net;
using System.Net.Mail;

namespace SeleniumCsharp.BaseClass
{

    public class BaseTest
    {

        public IWebDriver driver;
        public ExtentReports extent = null;
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
            var htmlReporter = new ExtentHtmlReporter(@"D:\Git\SeleniumCsharp\HtmlReports\Report" + ".html");
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
                screenshot.SaveAsFile("D:\\Git\\SeleniumCsharp\\Screenshots\\Screenshot" + DateTime.Now.Second + ".png", ScreenshotImageFormat.Png);
                return localpath;
            }
        }

        [OneTimeTearDown]
        public void Close()
        {
            driver.Quit();
            extent.Flush();
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("phaneendraphani20@gmail.com");
            mail.To.Add("phaneendraphani22@yahoo.in");
            mail.Subject = "Automation Report";
            mail.Body = "Hi," + "\n" + "Please, Find the Automation test Report " + "\n"
                    + "\n" + "Note : This is an automatic generated mail by Automation Script" + "\n" + "\n" + "\n"
                    + "\n" + "Thank you," + "\n" + "Phanindraa";

            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment("D:\\Git\\SeleniumCsharp\\HtmlReports\\index.html");
            mail.Attachments.Add(attachment);


            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("phaneendraphani20@gmail.com", "Phanindraa@15");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
     
    }
}
