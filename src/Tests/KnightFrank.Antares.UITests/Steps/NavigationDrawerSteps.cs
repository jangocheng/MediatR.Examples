﻿namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class NavigationDrawerSteps
    {
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;

        private NavigationDrawerPage navigationDrawerPage;

        public NavigationDrawerSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;

            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [When(@"User opens navigation drawer menu")]
        public void OpenNavigationDrawerMenu()
        {
            this.navigationDrawerPage = new NavigationDrawerPage(this.driverContext).OpenNavigationDrawer();
        }

        [When(@"User selects (.*) menu item")]
        public void SelectMenuItem(string drawerMenuItem)
        {
            this.navigationDrawerPage.ClickDrawerMenuItem(drawerMenuItem);
        }

        [When(@"User clicks create button in drawer submenu")]
        public void ClickCreateButton()
        {
            this.scenarioContext.Set(this.navigationDrawerPage.ClickCreateButton(), "CreatePropertyPage");
        }

        [When(@"User closes navigation drawer menu")]
        public void CloseNavigationDrawerMenu()
        {
            new NavigationDrawerPage(this.driverContext).CloseNavigationDrawer();
        }

        [Then(@"Drawer menu should be displayed")]
        public void CheckIfDrawerMenuPresent()
        {
            Assert.True(new NavigationDrawerPage(this.driverContext).IsOpenedDrawerMenuPresent());
        }

        [Then(@"Drawer (.*) submenu should be displayed")]
        public void CheckIfDrawerSubMenuPresent(string drawerMenu)
        {
            Assert.True(new NavigationDrawerPage(this.driverContext).IsSubMenuVisible(drawerMenu));
        }
    }
}
