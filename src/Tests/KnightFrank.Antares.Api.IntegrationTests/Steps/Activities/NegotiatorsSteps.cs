﻿namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    [Binding]
    public class NegotiatorsSteps
    {
        private const string ApiUrl = "/api/activities";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        private Activity activity;
        private ActivityUser activityUser;

        private List<Guid> secondaryNegotiatorsList;
        private UpdateActivityUser updateActivityUser;

        public NegotiatorsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Lead negotiator with ActiveDirectoryLogin (.*) and today plus (.*) next call date exists in database")]
        public void GivenLeadNegotiatorWithActiveDirectoryLoginExistsInDatabase(string activeDirectoryLogin, double noOfDays)
        {
            User user = this.fixture.DataContext.Users.SingleOrDefault(x => x.ActiveDirectoryLogin.Equals(activeDirectoryLogin));
            this.updateActivityUser = new UpdateActivityUser
            {
                UserId = user?.Id ?? Guid.NewGuid(),
                CallDate = DateTime.UtcNow.AddDays(noOfDays)
            };
        }

        [Given(@"Next call is set to date is today plus (.*)")]
        public void GivenNextCallForDateIsSet(double todayPlusDays)
        {
            this.updateActivityUser.CallDate = DateTime.UtcNow.AddDays(todayPlusDays);
        }

        [Given(@"Following secondary negotiators exists in database")]
        public void GivenFollowingSecondaryNegotiatorsExistsInDatabase(Table table)
        {
            this.secondaryNegotiatorsList =
                table.Rows.Select(row => row["ActiveDirectoryLogin"])
                     .Select(login => this.fixture.DataContext.Users.Single(x => x.ActiveDirectoryLogin.Equals(login)).Id)
                     .ToList();
        }

        [When(@"User updates activity with defined negotiators")]
        public void UpdateActivityNegotiators()
        {
            string requestUrl = $"{ApiUrl}";

            this.secondaryNegotiatorsList = this.secondaryNegotiatorsList ?? new List<Guid>();
            this.activity = this.scenarioContext.Get<Activity>("Activity");

            var updateActivityCommand = new UpdateActivityCommand
            {
                Id = this.activity.Id,
                ActivityTypeId = this.activity.ActivityTypeId,
                ActivityStatusId = this.activity.ActivityStatusId,
                LeadNegotiator = this.updateActivityUser,
                SecondaryNegotiators =
                    this.secondaryNegotiatorsList.Select(
                        n => new UpdateActivityUser { UserId = n, CallDate = DateTime.UtcNow.AddDays(10) }).ToList()
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, updateActivityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User updates last call date by adding (.*) days for (.*) user")]
        public void UpdateLastCallDate(string noOfDaysToAdd, string user)
        {
            this.activity = this.scenarioContext.Get<Activity>("Activity");
            string requestUrl = $"{ApiUrl}/{this.activity.Id}/negotiators";
            this.activityUser = this.activity.ActivityUsers.First();

            Guid activityUserId = user.Equals("valid") ? this.activityUser.Id : Guid.NewGuid();

            DateTime? callDate = noOfDaysToAdd.Equals("null")
                ? (DateTime?)null
                : DateTime.UtcNow.AddDays(double.Parse(noOfDaysToAdd));

            var updateActivityUserCommand = new UpdateActivityUserCommand
            {
                CallDate = callDate,
                ActivityId = this.activity.Id,
                Id = activityUserId
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, updateActivityUserCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Last call date should be updated in data base")]
        public void ThenLastCallDateShouldBeUpdatedInDataBase()
        {
            var abc = JsonConvert.DeserializeObject<ActivityUser>(this.scenarioContext.GetResponseContent());
            ActivityUser updatedActivityUser = this.fixture.DataContext.ActivityUsers.Single(x => x.Id.Equals(this.activityUser.Id));

            updatedActivityUser.ShouldBeEquivalentTo(abc, options => options
                .Excluding(x => x.Activity)
                .Excluding(x => x.User)
                .Excluding(x => x.UserType));
        }
    }
}
