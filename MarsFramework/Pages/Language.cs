﻿using MarsFramework.Global;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RelevantCodes.ExtentReports;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MarsFramework.Global.GlobalDefinitions;

namespace MarsFramework.Pages
{
    internal class Language
    {
        //Click on Language Tab
        [FindsBy(How = How.LinkText, Using = "Languages")]
        private IWebElement LanguageTab { get; set; }

        //Click on Add New Language Button
        [FindsBy(How = How.XPath, Using = "//h3[contains(text(),'Languages')]/../..//div[text()='Add New']")]
        private IWebElement AddNewLanguageButton { get; set; }

        //Add the Language
        [FindsBy(How = How.XPath, Using = "//input[@placeholder='Add Language']")]
        private IWebElement AddLanguage { get; set; }

        //Select Language Level
        [FindsBy(How = How.Name, Using = "level")]
        private IWebElement ChooseLanguageLevel { get; set; }

        //Click Add Language Button
        [FindsBy(How = How.XPath, Using = "//input[@value='Add']")]
        private IWebElement AddLanguageButton { get; set; }

        //Click Update Language Button
        [FindsBy(How = How.XPath, Using = "//input[@value='Update']")]
        private IWebElement UpdateLanguageButton { get; set; }

        //private static string LanguageTable => "//th[contains(text(),'Language')]/../../following-sibling::tbody";

        private static bool isLanguageFound;
        public Language()
        {
            PageFactory.InitElements(driver, this);
        }
        internal void NavigateToLanguageTab()
        {
            Extension.WaitForElementDisplayed(driver, By.XPath("//a[contains(text(),'Languages')]"), 2);
            LanguageTab.Click();
        }
        internal void AddNewLanguage()
        {
            //Click Add New button 
            AddNewLanguageButton.Click();

            //Enter the language and level
            Extension.WaitForElementDisplayed(driver, By.XPath("//input[@placeholder='Add Language']"), 2);
            AddLanguage.SendKeys(ExcelLib.ReadData(2, "Language"));
            SelectDropDown(ChooseLanguageLevel, "SelectByText", ExcelLib.ReadData(2, "Level"));

            //Click Add button
            AddLanguageButton.Click();

            //Validate message
            Extension.MessageValidation(ExcelLib.ReadData(2, "Language") + " has been added to your languages");

        }
        internal void UpdateLanguage()
        {
         
            //Get the Langugae value needs to be updated
            String expectedValue = ExcelLib.ReadData(2, "Language");

            //Get the rows count in language table
            IList<IWebElement> Tablerows = driver.FindElements(By.XPath("//h3[contains(text(),'Languages')]/../..//table/tbody/tr"));
            var rowCount = Tablerows.Count;
            
            //Get the actual Language value and compare with language needs to be updated, if matches update the record
            for (int i = 1; i <= rowCount; i++)
            {
                String actualValue = driver.FindElement(By.XPath("//h3[contains(text(),'Languages')]/../..//table/tbody[" + i + "]/tr/td[1]")).Text;
                if (expectedValue == actualValue)
                {
                    //Click on Edit icon
                    driver.FindElement(By.XPath("//h3[contains(text(),'Languages')]/../..//table/tbody[" + i + "]/tr/td[3]/span[1]/i")).Click();

                    //Clear the existing value and add new value 
                    Extension.WaitForElementDisplayed(driver, By.XPath("//input[@placeholder='Add Language']"), 2);
                    AddLanguage.Clear();
                    AddLanguage.SendKeys(ExcelLib.ReadData(2, "UpdateLanguage"));
                    SelectDropDown(ChooseLanguageLevel, "SelectByText", ExcelLib.ReadData(2, "UpdateLevel"));

                    //Click update button
                    UpdateLanguageButton.Click();

                    //Validate message
                    Extension.MessageValidation(ExcelLib.ReadData(2, "UpdateLanguage") + " has been updated to your languages");

                }

            }
        }
        internal void DeleteLanguage()
        {
            
            //Get the Language needs to be Deleted
            String expectedValue = ExcelLib.ReadData(2, "UpdateLanguage");

            //Get the rows count in language table
            IList<IWebElement> Tablerows = driver.FindElements(By.XPath("//h3[contains(text(),'Languages')]/../..//table/tbody/tr"));
            var rowCount = Tablerows.Count;

            //Get the actual Language value and compare with language needs to be updated, if matches delete the record
            for (int i = 1; i <= rowCount; i++)
            {
                string actualValue = driver.FindElement(By.XPath("//h3[contains(text(),'Languages')]/../..//table/tbody[" + i + "]/tr/td[1]")).Text;
                if (expectedValue == actualValue)
                {
                    //CliCk on Delete icon
                    driver.FindElement(By.XPath("//h3[contains(text(),'Languages')]/../..//table/tbody[" + i + "]/tr/td[3]/span[2]/i")).Click();

                    //Validate message
                    Extension.MessageValidation(ExcelLib.ReadData(2, "UpdateLanguage") + " has been deleted from your languages");

                }
            }
        }

        //Search the Language in the listing
        internal void SearchLanguage(String expectedLanguage, String expectedLanguageLevel)
        {
            //Setting the isLanguageFound variable to false
            isLanguageFound = false;

            //Get all the Language records
            IList<IWebElement> LanguageRecords = driver.FindElements(By.XPath("//h3[contains(text(),'Languages')]/../..//table/tbody/tr"));

            //if the expected and actual language matches, set the isLanguageFound to true
            for (int j = 1; j <= LanguageRecords.Count; j++)
            {
                String actualLanguage = driver.FindElement(By.XPath("//h3[contains(text(),'Languages')]/../..//table/tbody[" + j + "]/tr/td[1]")).Text;
                String actualLanguageLevel = driver.FindElement(By.XPath("//h3[contains(text(),'Languages')]/../..//table/tbody[" + j + "]/tr/td[2]")).Text;
                if (expectedLanguage == actualLanguage && expectedLanguageLevel == actualLanguageLevel)
                {
                    isLanguageFound = true;
                    break;
                }
            }

        }

        //Validate the Language added is dispalyed in the listing
        internal void ValidateAddedLanguage()
        {
            SearchLanguage(ExcelLib.ReadData(2, "Language"), ExcelLib.ReadData(2, "Level"));
            try
            {
                if (isLanguageFound == true)
                {
                    Base.test.Log(LogStatus.Pass, "Language Added Successful");
                    SaveScreenShotClass.SaveScreenshot(driver, "Add Language");
                    Assert.IsTrue(true);
                }
                else
                    Base.test.Log(LogStatus.Fail, "Add Language-Test Failed");
            }

            catch (Exception e)
            {
                Base.test.Log(LogStatus.Fail, "Caught Exception For Add Language", e.Message);
            }
        }

        //Validate the Language Updated is dispalyed in the listing
        internal void ValidateUpdateLanguage()
        {
            SearchLanguage(ExcelLib.ReadData(2, "UpdateLanguage"), ExcelLib.ReadData(2, "UpdateLevel"));
            try
            {
                if (isLanguageFound == true)
                {
                    Base.test.Log(LogStatus.Pass, "Language Updated Successful");
                    SaveScreenShotClass.SaveScreenshot(driver, "Update Language");
                    Assert.IsTrue(true);
                }
                else
                    Base.test.Log(LogStatus.Fail, "Update Language-Test Failed");
            }

            catch (Exception e)
            {
                Base.test.Log(LogStatus.Fail, "Caught Exception For Update Language", e.Message);
            }
        }

        //Validate the Language Deleted is not dispalyed in the listing
        internal void ValidateDeleteLanguage()
        {
            SearchLanguage(ExcelLib.ReadData(2, "UpdateLanguage"), ExcelLib.ReadData(2, "UpdateLevel"));
            try
            {
                if (isLanguageFound == false)
                {
                    Base.test.Log(LogStatus.Pass, "Language Deleted Successful");
                    SaveScreenShotClass.SaveScreenshot(driver, "Delete Language");
                    Assert.IsTrue(true);
                }
                else
                    Base.test.Log(LogStatus.Fail, "Delete Language-Test Failed");
            }

            catch (Exception e)
            {
                Base.test.Log(LogStatus.Fail, "Caught Exception For Delete Language", e.Message);
            }
        }
    }
}
            
    

