using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium
{
    class Program
    {
       
        static void Main(string[] args)
        {
            SeleniumEntities db = new SeleniumEntities();
            var driver = new ChromeDriver();

            driver.Url = "https://www.gate.io/en/trade/BTC_USDT";
            var gun = 24;

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                IWebElement a = driver.FindElement(By.XPath("//*[@id=\"currPrice\"]"));
                string price = a.Text;
                double v1 = Convert.ToDouble(price);

                // price = price.Replace('.', ',');

                IWebElement b = driver.FindElement(By.XPath("//*[@id=\"ticker_vol_b\"]"));
                string volume = b.Text;
                volume = volume.Remove(volume.Length - 1);
                double v2 = Convert.ToDouble(volume);
                //   volume = volume.Replace('.', ',');


                db.SaveChanges();
                var songun = DateTime.UtcNow.AddHours(-gun);
                var mdl = db.TblHacim.Where(x => x.Tarih > songun).ToList();
                var btcprice = mdl.AsEnumerable().Average(o => o.Price);
                var r = btcprice.ToString();
                r = r.Replace('.', ',');
                var btcvolume = mdl.AsEnumerable().Average(o => o.Volume);
                var t = btcvolume.ToString();
                t = t.Replace('.', ',');
                TblHacim hcm = new TblHacim()
                {
                    Price = v1,
                    Volume = v2,
                    Tarih = DateTime.Now,
                    OrtPrice = btcprice.ToString(),
                    OrtVolume = btcvolume.ToString(),
                };
                db.TblHacim.Add(hcm);
              
                Console.WriteLine("Deger = " + price.ToString() + " Hacim =" + volume);
                Console.WriteLine("Ortalama Deger: " + r + "Ortalama Hacim: " + t);

            }
        }
    }
}
