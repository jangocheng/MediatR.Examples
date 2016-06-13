﻿namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class EditOfferPage : ProjectPageBase
    {
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator save = new ElementLocator(Locator.Id, "offer-edit-save");
        private readonly ElementLocator status = new ElementLocator(Locator.CssSelector, "#offer-status select");
        private readonly ElementLocator activity = new ElementLocator(Locator.CssSelector, "#section-vendor .requirement-view-offers .card-body");
        // Basic information
        private readonly ElementLocator offer = new ElementLocator(Locator.Id, "offer-price");
        private readonly ElementLocator offerDate = new ElementLocator(Locator.Id, "offer-date");
        private readonly ElementLocator specialConditions = new ElementLocator(Locator.Name, "specialConditions");
        private readonly ElementLocator proposedExchangeDate = new ElementLocator(Locator.Id, "offer-proposed-exchange-date");
        private readonly ElementLocator proposedCompletionDate = new ElementLocator(Locator.Id, "proposed-completion-date");
        // Progress summary
        private readonly ElementLocator mortgageStatus = new ElementLocator(Locator.CssSelector, "#offer-mortgage-status select");
        private readonly ElementLocator mortgageSurveyStatus = new ElementLocator(Locator.CssSelector, "#mortgage-survey-status select");
        private readonly ElementLocator additionalSurveyStatus = new ElementLocator(Locator.CssSelector, "#additional-survey-status select");
        private readonly ElementLocator searchStatus = new ElementLocator(Locator.CssSelector, "#search-status select");
        private readonly ElementLocator enquiries = new ElementLocator(Locator.CssSelector, "#enquiries select");
        private readonly ElementLocator contractApproved = new ElementLocator(Locator.CssSelector, "#contract-approved [ng-value = 'true']");
        private readonly ElementLocator contractNotApproved = new ElementLocator(Locator.CssSelector, "#contract-approved [ng-value = 'false']");
        // Mortgage details
        private readonly ElementLocator mortgageLoanToValue = new ElementLocator(Locator.Id, "mortgage-loan-to-value");
        private readonly ElementLocator editBroker = new ElementLocator(Locator.Id, "edit-broker");
        private readonly ElementLocator brokers = new ElementLocator(Locator.CssSelector, "#broker .card-body");
        private readonly ElementLocator editLender = new ElementLocator(Locator.Id, "edit-lender");
        private readonly ElementLocator lenders = new ElementLocator(Locator.CssSelector, "#lender .card-body");
        private readonly ElementLocator mortgageSurveyDate = new ElementLocator(Locator.Id, "mortgage-survey-date");
        private readonly ElementLocator mortgageEditSurveyor = new ElementLocator(Locator.Id, "edit-surveyor");
        private readonly ElementLocator mortgageSurveyors = new ElementLocator(Locator.CssSelector, "#surveyor .card-body");
        // Additional survey
        private readonly ElementLocator additionalSurveyDate = new ElementLocator(Locator.Id, "additional-survey-date");
        private readonly ElementLocator editAdditionalSurveyor = new ElementLocator(Locator.Id, "edit-additional-surveyor");
        private readonly ElementLocator additionalSurveyors = new ElementLocator(Locator.CssSelector, "#additional-surveyor .card-body");
        // Other details
        private readonly ElementLocator comment = new ElementLocator(Locator.Id, "comment");

        public EditOfferPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public List<string> Brokers => this.Driver.GetElements(this.brokers).Select(el => el.Text).ToList();
        
        public List<string> Lenders => this.Driver.GetElements(this.lenders).Select(el => el.Text).ToList();

        public List<string> MortgageSurveyors => this.Driver.GetElements(this.mortgageSurveyors).Select(el => el.Text).ToList();

        public List<string> AdditionalSurveyors => this.Driver.GetElements(this.additionalSurveyors).Select(el => el.Text).ToList();

        public EditOfferPage OpenEditOfferPageWithId(string id)
        {
            new CommonPage(this.DriverContext).NavigateToPageWithId("edit offer", id);
            return this;
        }

        public EditOfferPage SelectStatus(string offerStatus)
        {
            this.Driver.GetElement<Select>(this.status).SelectByText(offerStatus);
            return this;
        }

        public EditOfferPage SetOffer(string price)
        {
            this.Driver.SendKeys(this.offer, price);
            return this;
        }

        public EditOfferPage SetOfferDate(string date)
        {
            this.Driver.SendKeys(this.offerDate, date);
            return this;
        }

        public EditOfferPage SetSpecialConditions(string text)
        {
            this.Driver.SendKeys(this.specialConditions, text);
            return this;
        }

        public EditOfferPage SetProposedExchangeDate(string date)
        {
            this.Driver.SendKeys(this.proposedExchangeDate, date);
            return this;
        }

        public EditOfferPage SetProposedCompletionDate(string date)
        {
            this.Driver.SendKeys(this.proposedCompletionDate, date);
            return this;
        }

        public EditOfferPage SaveOffer()
        {
            this.Driver.Click(this.save);
            return this;
        }

        public EditOfferPage OpenActivityPreview()
        {
            this.Driver.Click(this.activity);
            return this;
        }

        public EditOfferPage WaitForSidePanelToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public EditOfferPage SelectMortgageStatus(string text)
        {
            this.Driver.GetElement<Select>(this.mortgageStatus).SelectByText(text);
            return this;
        }

        public EditOfferPage SelectMortgageSurveyStatus(string text)
        {
            this.Driver.GetElement<Select>(this.mortgageSurveyStatus).SelectByText(text);
            return this;
        }

        public EditOfferPage SelectAdditionalSurveyStatus(string text)
        {
            this.Driver.GetElement<Select>(this.additionalSurveyStatus).SelectByText(text);
            return this;
        }

        public EditOfferPage SelectSearchStatus(string text)
        {
            this.Driver.GetElement<Select>(this.searchStatus).SelectByText(text);
            return this;
        }

        public EditOfferPage SelectEnquiries(string text)
        {
            this.Driver.GetElement<Select>(this.enquiries).SelectByText(text);
            return this;
        }

        public EditOfferPage ApproveContract()
        {
            this.Driver.GetElement<Checkbox>(this.contractApproved).TickCheckbox();
            return this;
        }

        public EditOfferPage DisapproveContract()
        {
            this.Driver.GetElement<Checkbox>(this.contractNotApproved).TickCheckbox();
            return this;
        }

        public EditOfferPage SetMortgageLoanToValue(string text)
        {
            this.Driver.SendKeys(this.mortgageLoanToValue, text);
            return this;
        }

        public EditOfferPage EditBroker()
        {
            this.Driver.Click(this.editBroker);
            return this;
        }

        public EditOfferPage EditLender()
        {
            this.Driver.Click(this.editLender);
            return this;
        }

        public EditOfferPage SetMortgageSurveyDate(string date)
        {
            this.Driver.SendKeys(this.mortgageSurveyDate, date);
            return this;
        }

        public EditOfferPage EditMortgageSurveyor()
        {
            this.Driver.Click(this.mortgageEditSurveyor);
            return this;
        }

        public EditOfferPage SetAdditionalSurveyDate(string date)
        {
            this.Driver.SendKeys(this.additionalSurveyDate, date);
            return this;
        }

        public EditOfferPage EditAdditionalSurveyor()
        {
            this.Driver.Click(this.editAdditionalSurveyor);
            return this;
        }

        public EditOfferPage SetComment(string text)
        {
            this.Driver.SendKeys(this.comment, text);
            return this;
        }
    }
}
