using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium;
using System.IO;
using System;

namespace EasyDropAutoPromo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write Path to File, Example - \"" + @"C:\\myFolder\log.txt" + "\"");
            string path = Console.ReadLine();
            Console.WriteLine("Write Currency, Example - \"" + "RUB" + "\"");
            string currency = Console.ReadLine().ToUpper(); 
            Console.WriteLine("Write Value That Will be Target, Example - \"" + @"700" + "\"" + "Then Trigered Range be (700;+∞)");
            int value = int.Parse(Console.ReadLine());
            if (!File.Exists(path)) File.Create(path).Dispose();
            IWebDriver driver = new ChromeDriver(@"C:\");
            driver.Manage().Window.Maximize();
            WaitWeb(driver, "https://easydrop.top/", "promocode__code");
            string oldPromoText = "";
            Console.WriteLine("If You was Login in Steam, Writes Something, Otherwise Login and writes Something in Console.");
            Console.ReadLine();
            while (true)
            {
                string promoText = driver.FindElement(By.ClassName("promocode__amount-bonus")).Text;
                string promoCode = driver.FindElement(By.ClassName("promocode__code")).Text;
                if (promoText != oldPromoText)
                {
                    oldPromoText = promoText;
                    File.AppendAllText(path, $"[{DateTime.Now.ToString("yy.MM.dd.HH:mm:ss")}] " + promoText + " : " + promoCode + "\n");
                    if (promoText.Contains(currency) && int.Parse(promoText.Split(' ')[3]) > value)
                    {
                        driver.FindElement(By.CssSelector("body > div.layout > div.layout__header > header > div.header__site-head > div > div.site-head__bar > div.site-head__userbar-login > div > div.userbar__items > div")).Click();
                        Thread.Sleep(1000);
                        driver.FindElement(By.XPath(".//*[@id='refill_block']/div[1]/div[2]/div[2]/div/input")).SendKeys(promoCode);
                        Thread.Sleep(1000);
                        IWebElement button = driver.FindElement(By.XPath(".//*[@id='gotopay']"));
                        if (button.Enabled == false) driver.FindElement(By.XPath(".//*[@id='refill_block']/div[1]/div[2]/div[2]/form/div/label/span[2]")).Click();
                        Thread.Sleep(1000);
                        button.Click();
                        Thread.Sleep(2000);
                        WaitWeb(driver, "https://easydrop.top/", "promocode__code");
                    }
                }
            }
        }
        static void WaitWeb(IWebDriver driver, string site, string elementClass)
        {
            driver.Navigate().GoToUrl(site);
            while (true)
                if (driver.FindElements(By.ClassName(elementClass)).Count > 0) break;
                else { Thread.Sleep(1000); Console.Write("."); };
        }
    }
}
