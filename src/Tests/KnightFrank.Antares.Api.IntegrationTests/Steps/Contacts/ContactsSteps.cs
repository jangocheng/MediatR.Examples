﻿namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Contacts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Contacts;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ContactsSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/contacts";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public ContactsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"All contacts have been deleted")]
        public void DeleteContacts()
        {
            this.fixture.DataContext.Contacts.RemoveRange(this.fixture.DataContext.Contacts.ToList());
        }

        [Given(@"Contacts exists in database")]
        public void CreateUsersInDb(Table table)
        {
            List<Contact> contacts = table.CreateSet<Contact>().ToList();
            this.fixture.DataContext.Contacts.AddRange(contacts);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(contacts, "Contacts");
        }

        [When(@"User creates contact using api with max length fields")]
        public void CreateUsersWithMaxFields()
        {
            const int max = 128;

            var contact = new Contact
            {
                FirstName = StringExtension.GenerateMaxAlphanumericString(max),
                Surname = StringExtension.GenerateMaxAlphanumericString(max),
                Title = StringExtension.GenerateMaxAlphanumericString(max)
            };

            this.CreateContact(contact);
        }

        [When(@"User creates contact using api with following data")]
        public void CreateContact(Table table)
        {
            var contact = table.CreateInstance<Contact>();
            this.CreateContact(contact);
        }

        [When(@"User retrieves all contact details")]
        public void GetAllContactsDetails()
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves contact details for (.*) id")]
        public void GetContactDetailsUsingId(string contactId)
        {
            if (contactId.Equals("latest"))
            {
                contactId = this.scenarioContext.Get<List<Contact>>("Contacts")[0].Id.ToString();
            }
            else if (contactId.Equals("invalid"))
            {
                contactId = Guid.NewGuid().ToString();
            }

            string requestUrl = $"{ApiUrl}/{contactId}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Contact details should be the same as already added")]
        public void CheckContactDetailsFromPost()
        {
            var contactId = JsonConvert.DeserializeObject<Guid>(this.scenarioContext.GetResponseContent());
            var contact = this.scenarioContext.Get<Contact>("Contact");
            contact.Id = contactId;

            Contact expectedContact = this.fixture.DataContext.Contacts.Single(x => x.Id.Equals(contactId));
            contact.ShouldBeEquivalentTo(expectedContact);
        }

        [Then(@"Get contact details should be the same as already added")]
        public void CheckContactDetailsFromGet()
        {
            var contact = JsonConvert.DeserializeObject<Contact>(this.scenarioContext.GetResponseContent());

            Contact expectedContact = this.fixture.DataContext.Contacts.Single(x => x.Id.Equals(contact.Id));
            contact.ShouldBeEquivalentTo(expectedContact);
        }

        [Then(@"Contacts details should have expected values")]
        public void CheckContactDetailsHaveExpectedValues()
        {
            var contactList = this.scenarioContext.Get<List<Contact>>("Contacts");

            var currentContactsDetails =
                JsonConvert.DeserializeObject<List<Contact>>(this.scenarioContext.GetResponseContent());
            currentContactsDetails.ShouldBeEquivalentTo(contactList);
        }

        private void CreateContact(Contact contact)
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, contact);
            this.scenarioContext.SetHttpResponseMessage(response);
            this.scenarioContext.Set(contact, "Contact");
        }
    }
}
