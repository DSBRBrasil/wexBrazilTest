using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text.RegularExpressions; 
using OpenQA.Selenium; 
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace UnitTestProject1.Steps
{
    [Binding]
    class AmazonSearchFeatureSteps
    {
        private IWebDriver _currentDriver = null;
        List<Documents> _list = new List<Documents>();

        [Given(@"I navigate to www\.amazon\.com")]
        public void GivenNavigateToAmazonPage()
        {
             _currentDriver = new ChromeDriver(); 
             _currentDriver.Url = "https://www.amazon.com"; 
        }

        [When(@"I select the option books in the dropdown next to the search text input criteria")]
        public void SelectOptionBooksInDropDowndLIst(Table table)
        {
            dynamic word = table.CreateDynamicInstance();
            new SelectElement(_currentDriver.FindElement(By.Id("searchDropdownBox"))).SelectByText(word.keyword);
        }


        [Then(@"I search for Test automation")]
        public void TypeTestAutomation(Table table)
        {
            dynamic word = table.CreateDynamicInstance();
            _currentDriver.FindElement(By.Id("twotabsearchtextbox")).SendKeys(word.keyword);
            _currentDriver.FindElement(By.ClassName("nav-input")).Submit();
        }

       

        [Then(@"I select the cheapset book of the page without using any sorting method available")]
        public void SelectTheCheapsetBook()
        {
            
            IList<IWebElement> all = _currentDriver.FindElements(By.XPath("//a[@class='a-link-normal a-text-normal']"));
            
            var i = 1;  
            //define a pattern for expression regular
            string pattern = @"Paperback \$([0-9]{2} [0-9]{2})";

            //Iterator dynamically for elements for page
            foreach (IWebElement element in all)
            {
                //get childs of class sg_col_inner
                var getClassSgColInner = _currentDriver.FindElements(By.XPath($"//*[@id=\"search\"]/div[1]/div[2]/div/span[4]/div[1]/div[{i}]/div/span/div/div/div[2]/div[2]/div/div[2]/div[1]/div"));
                
                foreach (var item in getClassSgColInner)
                {
                    
                    var regex = new Regex(pattern);
                    var str = item.Text.Replace("\n", "").Replace("\r", " ");
                   
                    if (regex.IsMatch(str))
                    {
                        var strPaperBack = regex.Matches(str)[0].Value;
                        var strPrince = strPaperBack.Split('$')[1].Replace(' ','.');

                        _list.Add(new Documents
                        {
                            Name = element.Text,
                            Link = element.GetAttribute("href"),
                            PriceToPaperback = decimal.Parse(strPrince)
                        });

                    }

                }
                i++; 

            } 
             
        }

        [When(@"I reach the detailed book page, I check if the name in the header is the same name of the book that I selected previuosly")]
        public void WhenIReachTheDetailedBookPageICheckIfTheNameInTheHeaderIsTheSameNameOfTheBookThatISelectedPreviuosly()
        {

            //To sorting for a too cheap price of the book
            var cheapBookFirst = _list.OrderBy(n => n.PriceToPaperback).FirstOrDefault();
            if (cheapBookFirst != null)
            {
                _currentDriver.Url = cheapBookFirst.Link;
                Console.WriteLine("===================================================================================");
                Console.WriteLine("That's it the lowest price found in the first page, for a type paperback book.");
                Console.WriteLine($"Book Name: {cheapBookFirst.Name}");
                Console.WriteLine($"Link: {cheapBookFirst.Link}");
                Console.WriteLine($"Lowest price to Paperback: {cheapBookFirst.PriceToPaperback}");
                Console.WriteLine($"By Valnei Filho v_marinpietri@yahoo.com.br 71 99946-7636, thks so much!");
                Console.WriteLine("===================================================================================");
            }
            
             _currentDriver.Dispose();

        }
         
    }
    public class Documents
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public decimal PriceToPaperback { get; set; }
         
        
    }
}
