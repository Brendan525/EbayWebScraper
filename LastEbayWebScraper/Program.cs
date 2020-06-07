using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace LastEbayWebScraper
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            GetHtmlAsync();
            Console.ReadLine();
        }

        private static async void GetHtmlAsync()
        {

            var url = "https://www.ebay.com/sch/i.html?_from=R40&_nkw=Nintendo+Switch&_in_kw=1&_ex_kw=&_sacat=0&LH_Complete=1&_udlo=&_udhi=&_ftrt=901&_ftrv=1&_sabdlo=&_sabdhi=&_samilow=&_samihi=&_sadis=15&_stpos=48111&_sargn=-1%26saslc%3D1&_salic=1&_sop=13&_dmd=1&_ipg=25&_fosrp=1";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);




            // Get List

            var ProductsHtml = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("ResultSetItems")).ToList();

            var ProductListItems = ProductsHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("id", "")
                .Contains("item")).ToList();

            Console.WriteLine(ProductListItems.Count());
            Console.WriteLine();


            // Details

            foreach (var ProductListItem in ProductListItems)
            {

                // id
                Console.WriteLine(ProductListItem.GetAttributeValue("listingId", ""));

                // ProductName
                Console.WriteLine(ProductListItem.Descendants("h3")
                  .Where(node => node.GetAttributeValue("class", "")
                  .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                  );

                // Subtitle
                Console.WriteLine(ProductListItem.Descendants("div")
                  .Where(node => node.GetAttributeValue("class", "")
                  .Equals("lvsubtitle")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                  );

                // Price
                Console.WriteLine(
                    Regex.Match(
                    ProductListItem.Descendants("li")
                   .Where(node => node.GetAttributeValue("class", "")
                   .Equals("lvprice prc")).FirstOrDefault().InnerText
                   , @"\$\d+.\d+")
                  );

                // ListingType lvformat
                Console.WriteLine(
                    ProductListItem.Descendants("li")
                  .Where(node => node.GetAttributeValue("class", "")
                  .Equals("lvformat")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                  );


                // Url
                Console.WriteLine(
                    ProductListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", "").Trim('\r', '\n', '\t')
                    );

                Console.WriteLine();

            }


        }
    }
}




