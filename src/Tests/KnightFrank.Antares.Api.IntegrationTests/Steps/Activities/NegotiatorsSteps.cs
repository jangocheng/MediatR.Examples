﻿namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using TechTalk.SpecFlow;

    [Binding]
    public class NegotiatorsSteps
    {
        private const string ApiUrl = "/api/activities";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public NegotiatorsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Lead negotiator with ActiveDirectoryLogin (.*) exists in database")]
        public void GivenLeadNegotiatorWithActiveDirectoryLoginExistsInDatabase(string activeDirectoryLogin)
        {
            User user = this.fixture.DataContext.Users.SingleOrDefault(x => x.ActiveDirectoryLogin.Equals(activeDirectoryLogin));
            Guid leadNegotiatorId = user?.Id ?? new Guid(activeDirectoryLogin);
            this.scenarioContext.Set(leadNegotiatorId, "LeadNegotiatorId");
        }

        [Given(@"Following secondary negotiators exists in database")]
        public void GivenFollowingSecondaryNegotiatorsExistsInDatabase(Table table)
        {
            var list = new List<Guid>();

            if (table.Rows.Count > 0)
            {
                list =
                    table.Rows.Select(row => row["ActiveDirectoryLogin"])
                         .Select(
                             login => this.fixture.DataContext.Users.Single(x => x.ActiveDirectoryLogin.Equals(login)).Id)
                         .ToList();
            }

            this.scenarioContext.Set(list, "SecondaryNegotiatorId");
        }

        [When(@"User updates activity status with defined negotiators")]
        public void UpdateActivityNegotiators()
        {
            string requestUrl = $"{ApiUrl}";

            var leadNegotiatorId = this.scenarioContext.Get<Guid>("LeadNegotiatorId");
            var secondaryNegotiatorsIds = this.scenarioContext.Get<List<Guid>>("SecondaryNegotiatorId");

            var activityFromDatabase = this.scenarioContext.Get<Activity>("Activity");

            var updateActivityCommand = new UpdateActivityCommand
            {
                Id = activityFromDatabase.Id,
                ActivityTypeId = activityFromDatabase.ActivityTypeId,
                ActivityStatusId = activityFromDatabase.ActivityStatusId,
                LeadNegotiatorId = leadNegotiatorId,
                SecondaryNegotiatorIds = secondaryNegotiatorsIds
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, updateActivityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}