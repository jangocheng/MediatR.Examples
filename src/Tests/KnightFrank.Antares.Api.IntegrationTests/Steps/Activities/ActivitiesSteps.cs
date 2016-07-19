﻿namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Activity.QueryResults;
    using KnightFrank.Antares.Domain.Common.Enums;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ActivitiesSteps
    {
        private const string ApiUrl = "/api/activities";
        private readonly DateTime date = DateTime.UtcNow;
        private readonly BaseTestClassFixture fixture;
        private readonly ScenarioContext scenarioContext;
        private List<ActivityDepartment> activityDepartments;

        private User leadNegotiator;

        public ActivitiesSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"All activities have been deleted from database")]
        public void DeleteActivities()
        {
            this.fixture.DataContext.Activities.RemoveRange(this.fixture.DataContext.Activities.ToList());
        }

        [Given(@"Activity exists in database")]
        public void CreateActivityinDatabase(Table table)
        {
            string activityStatus = table.Rows[0]["ActivityStatus"];
            string activityType = table.Rows[0]["ActivityType"];

            Guid activityStatusId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    i => i.EnumType.Code.Equals(nameof(ActivityStatus)) && i.Code.Equals(activityStatus)).Id;
            Guid activityTypeId = this.fixture.DataContext.ActivityTypes.Single(i => i.Code.Equals(activityType)).Id;
            Guid propertyId = this.scenarioContext.Get<Property>("Property").Id;
            Guid activitySourceId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    i => i.EnumType.Code.Equals(nameof(ActivitySource)) && i.Code.Equals(nameof(ActivitySource.DirectEmail))).Id;
            Guid activitySellingReasonId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    i =>
                        i.EnumType.Code.Equals(nameof(ActivitySellingReason)) &&
                        i.Code.Equals(nameof(ActivitySellingReason.Relocation))).Id;

            this.leadNegotiator = this.fixture.DataContext.Users.First();

            var activityNegotiatorList = new List<ActivityUser>
            {
                new ActivityUser
                {
                    UserId = this.leadNegotiator.Id,
                    UserTypeId =
                        this.fixture.DataContext.EnumTypeItems.Single(
                            i =>
                                i.EnumType.Code.Equals(nameof(UserType)) &&
                                i.Code.Equals(nameof(UserType.LeadNegotiator))).Id
                }
            };

            this.activityDepartments = new List<ActivityDepartment>
            {
                new ActivityDepartment
                {
                    DepartmentId = this.leadNegotiator.DepartmentId,
                    DepartmentTypeId =
                        this.fixture.DataContext.EnumTypeItems.Single(
                            i =>
                                i.EnumType.Code.Equals(nameof(ActivityDepartmentType)) &&
                                i.Code.Equals(nameof(ActivityDepartmentType.Managing))).Id
                }
            };

            var activity = new Activity
            {
                PropertyId = propertyId,
                ActivityTypeId = activityTypeId,
                ActivityStatusId = activityStatusId,
                CreatedDate = this.date,
                LastModifiedDate = this.date,
                Contacts = new List<Contact>(),
                Attachments = new List<Attachment>(),
                ActivityUsers = activityNegotiatorList,
                ActivityDepartments = this.activityDepartments,
                SourceId = activitySourceId,
                SellingReasonId = activitySellingReasonId,
                AppraisalMeetingStart = this.date.AddHours(10),
                AppraisalMeetingEnd = this.date.AddHours(12),
                ChainTransactions = new List<ChainTransaction>()
            };

            this.fixture.DataContext.Activities.Add(activity);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(activity, "Activity");
        }

        [Given(@"User gets (.*) for ActivityType")]
        public void GetActivityTypeId(string activityTypeCode)
        {
            if (activityTypeCode.Equals("invalid"))
            {
                this.scenarioContext.Set(Guid.NewGuid(), "ActivityTypeId");
            }
            else
            {
                Guid activityTypeId = this.fixture.DataContext.ActivityTypes.Single(i => i.Code.Equals(activityTypeCode)).Id;
                this.scenarioContext.Set(activityTypeId, "ActivityTypeId");
            }
        }

        [When(@"User creates activity for given (.*) property id using api")]
        public void CreateActivityUsingApi(string id)
        {
            string requestUrl = $"{ApiUrl}";

            Guid activityStatusId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["PreAppraisal"];
            Guid managingDepartmentId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["Managing"];
            Guid sourceId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["DirectEmail"];
            Guid sellingReasonId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["Relocation"];
            Guid propertyId = id.Equals("latest") ? this.scenarioContext.Get<Property>("Property").Id : new Guid(id);
            var activityTypeId = this.scenarioContext.Get<Guid>("ActivityTypeId");

            List<Guid> vendors =
                this.fixture.DataContext.Ownerships.Where(x => x.PropertyId.Equals(propertyId) && x.SellDate == null)
                    .SelectMany(x => x.Contacts).Select(c => c.Id)
                    .ToList();

            User user = this.fixture.DataContext.Users.First();
            Guid leadNegotiatorId = user.Id;

            var activityCommand = new CreateActivityCommand
            {
                PropertyId = propertyId,
                ActivityStatusId = activityStatusId,
                ContactIds = vendors,
                ActivityTypeId = activityTypeId,
                SourceId = sourceId,
                SellingReasonId = sellingReasonId,
                LeadNegotiator = new UpdateActivityUser { UserId = leadNegotiatorId, CallDate = DateTime.Today.AddDays(3) },
                Departments =
                    new List<UpdateActivityDepartment>
                    {
                        new UpdateActivityDepartment { DepartmentId = user.DepartmentId, DepartmentTypeId = managingDepartmentId }
                    },
                AppraisalMeetingStart = this.date.AddHours(24),
                AppraisalMeetingEnd = this.date.AddHours(26)
            };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, activityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User updates activity (.*) id and (.*) status with following sale valuation")]
        public void UpdateActivitySaleValuation(string id, string status, Table table)
        {
            var updateActivityCommand = table.CreateInstance<UpdateActivityCommand>();
            var activityFromDatabase = this.scenarioContext.Get<Activity>("Activity");

            updateActivityCommand.Id = id.Equals("latest") ? activityFromDatabase.Id : new Guid(id);
            updateActivityCommand.ActivityTypeId = activityFromDatabase.ActivityTypeId;
            updateActivityCommand.SourceId = activityFromDatabase.SourceId;
            updateActivityCommand.SellingReasonId = activityFromDatabase.SellingReasonId;

            updateActivityCommand.AppraisalMeetingStart = activityFromDatabase.AppraisalMeetingStart ?? this.date.AddHours(24);
            updateActivityCommand.AppraisalMeetingEnd = activityFromDatabase.AppraisalMeetingStart ?? this.date.AddHours(26);

            updateActivityCommand.LeadNegotiator = new UpdateActivityUser
            {
                UserId = activityFromDatabase.ActivityUsers.First().UserId,
                CallDate = this.date.AddDays(1)
            };

            updateActivityCommand.ActivityStatusId = status.Equals("latest")
                ? activityFromDatabase.ActivityStatusId
                : new Guid(status);

            updateActivityCommand.Departments = new List<UpdateActivityDepartment>
            {
                new UpdateActivityDepartment
                {
                    DepartmentId = this.leadNegotiator.DepartmentId,
                    DepartmentTypeId =
                        this.fixture.DataContext.EnumTypeItems.Single(
                            i =>
                                i.EnumType.Code.Equals(nameof(ActivityDepartmentType)) &&
                                i.Code.Equals(nameof(ActivityDepartmentType.Managing))).Id
                }
            };

            activityFromDatabase = new Activity
            {
                Id = updateActivityCommand.Id,
                ActivityStatusId = updateActivityCommand.ActivityStatusId,
                ActivityTypeId = updateActivityCommand.ActivityTypeId,
                AgreedInitialMarketingPrice = updateActivityCommand.AgreedInitialMarketingPrice,
                KfValuationPrice = updateActivityCommand.KfValuationPrice,
                VendorValuationPrice = updateActivityCommand.VendorValuationPrice,
                ShortAgreedInitialMarketingPrice = updateActivityCommand.ShortAgreedInitialMarketingPrice,
                ShortKfValuationPrice = updateActivityCommand.ShortKfValuationPrice,
                ShortVendorValuationPrice = updateActivityCommand.ShortVendorValuationPrice,
                LongAgreedInitialMarketingPrice = updateActivityCommand.LongAgreedInitialMarketingPrice,
                LongKfValuationPrice = updateActivityCommand.LongKfValuationPrice,
                LongVendorValuationPrice = updateActivityCommand.LongVendorValuationPrice,
                DisposalTypeId = updateActivityCommand.DisposalTypeId,
                ServiceChargeAmount = updateActivityCommand.ServiceChargeAmount,
                ServiceChargeNote = updateActivityCommand.ServiceChargeNote,
                GroundRentAmount = updateActivityCommand.GroundRentAmount,
                GroundRentNote = updateActivityCommand.GroundRentNote,
                OtherCondition = updateActivityCommand.OtherCondition,
                DecorationId = updateActivityCommand.DecorationId
            };

            this.scenarioContext["Activity"] = activityFromDatabase;

            this.UpdateActivity(updateActivityCommand);
        }

        [When(@"User gets activity with (.*) id")]
        public void GetActivityWithId(string activityId)
        {
            this.GetActivityResponse(activityId.Equals("latest")
                ? this.scenarioContext.Get<Activity>("Activity").Id.ToString()
                : activityId);
        }

        [When(@"User gets activities")]
        public void GetAllActivities()
        {
            this.GetActivityResponse();
        }

        [Then(@"Activities list should be the same as in database")]
        public void CheckActivitiesList()
        {
            var propertyFromResponse = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());

            Activity activitiesFromDatabase =
                this.fixture.DataContext.Properties.Single(prop => prop.Id.Equals(propertyFromResponse.Id)).Activities.First();

            AssertionOptions.AssertEquivalencyUsing(
                options => options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation)).WhenTypeIs<DateTime>());

            propertyFromResponse.Activities.ToList()[0].ShouldBeEquivalentTo(activitiesFromDatabase, options => options
                .Excluding(x => x.Property)
                .Excluding(x => x.ActivityStatus)
                .Excluding(x => x.ActivityType)
                .Excluding(x => x.Attachments)
                .Excluding(x => x.ActivityUsers)
                .Excluding(x => x.ActivityDepartments)
                .Excluding(x => x.AppraisalMeetingAttendees)
                .Excluding(x => x.Contacts)
                .Excluding(x => x.Offers)
                .Excluding(x => x.Source)
                .Excluding(x => x.SellingReason)
                .Excluding(x => x.ChainTransactions));
        }

        [Then(@"Activity details should be the same as already added")]
        public void CompareActivities()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Activity actualActivity = this.fixture.DataContext.Activities.Single(x => x.Id.Equals(activity.Id));

            actualActivity.ShouldBeEquivalentTo(activity, options => options
                .Excluding(x => x.Property)
                .Excluding(x => x.ActivityStatus)
                .Excluding(x => x.ActivityType)
                .Excluding(x => x.Attachments)
                .Excluding(x => x.ActivityUsers)
                .Excluding(x => x.ActivityDepartments)
                .Excluding(x => x.AppraisalMeetingAttendees)
                .Excluding(x => x.Contacts)
                .Excluding(x => x.Offers)
                .Excluding(x => x.Source)
                .Excluding(x => x.ChainTransactions)
                .Excluding(x => x.SellingReason)
                .Excluding(x => x.SalesBoardType));

            actualActivity.ActivityStatus.Code.ShouldBeEquivalentTo("PreAppraisal");
            actualActivity.ActivityUsers.Should().Equal(activity.ActivityUsers, (c1, c2) =>
                c1.ActivityId == c2.ActivityId &&
                c1.UserTypeId == c2.UserTypeId &&
                c1.UserId == c2.UserId &&
                c1.CallDate.Equals(c2.CallDate));

            actualActivity.ActivityDepartments.ShouldAllBeEquivalentTo(activity.ActivityDepartments, opt => opt
                .Excluding(x => x.Activity)
                .Excluding(x => x.Department)
                .Excluding(x => x.DepartmentType));
        }

        [Then(@"Retrieved activity should have expected viewing")]
        public void CheckActivityViewing()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Viewing viewing = this.fixture.DataContext.Viewing.Single(x => x.ActivityId.Equals(activity.Id));

            activity.Viewings.Single().ShouldBeEquivalentTo(viewing, options => options
                .Excluding(v => v.Activity)
                .Excluding(v => v.Requirement)
                .Excluding(v => v.Negotiator));
        }

        [Then(@"Retrieved activity should have expected offer")]
        public void CheckActivityOffer()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Offer offer = this.fixture.DataContext.Offer.Single(x => x.ActivityId.Equals(activity.Id));

            activity.Offers.Single().ShouldBeEquivalentTo(offer, options => options
                .Excluding(o => o.Activity)
                .Excluding(o => o.Requirement)
                .Excluding(o => o.Negotiator)
                .Excluding(o => o.Status)
                .Excluding(o => o.OfferType));
        }

        [Then(@"Retrieved activities should be the same as in database")]
        public void CheckActivities()
        {
            var activity = JsonConvert.DeserializeObject<List<ActivitiesQueryResult>>(this.scenarioContext.GetResponseContent());
            Address addressDetails = this.scenarioContext.Get<Property>("Property").Address;

            var actualActivity = new ActivitiesQueryResult
            {
                Line2 = addressDetails.Line2,
                PropertyName = addressDetails.PropertyName,
                PropertyNumber = addressDetails.PropertyNumber,
                Id = this.scenarioContext.Get<Activity>("Activity").Id
            };

            actualActivity.ShouldBeEquivalentTo(activity.Single());
        }

        [Then(@"Departments should be the same as already added")]
        public void ThenDepartmentsShouldBeTheSameAsAlreadyAdded()
        {
            var activityFromdb = this.scenarioContext.Get<Activity>("Activity");
            List<ActivityDepartment> addedActivityDepartments =
                JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent()).ActivityDepartments.ToList();
            List<ActivityDepartment> updatedActivityDepartments =
                this.fixture.DataContext.ActivityDepartment.Select(x => x)
                    .Where(x => x.ActivityId.Equals(activityFromdb.Id))
                    .ToList();

            updatedActivityDepartments.ShouldAllBeEquivalentTo(addedActivityDepartments, opt => opt
                .Excluding(x => x.Activity)
                .Excluding(x => x.Department)
                .Excluding(x => x.DepartmentType));
        }

        private void GetActivityResponse(string activityId)
        {
            string requestUrl = $"{ApiUrl}/{activityId}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        private void GetActivityResponse()
        {
            string requestUrl = $"{ApiUrl}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        private void UpdateActivity(UpdateActivityCommand updateActivityCommand)
        {
            string requestUrl = $"{ApiUrl}";

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, updateActivityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
