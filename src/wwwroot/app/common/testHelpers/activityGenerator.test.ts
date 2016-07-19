﻿/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ActivityGenerator {
        public static generateDto(specificData?: any): Dto.IActivity {

            var activity: Dto.IActivity = {
                activityStatusId: StringGenerator.generate(),
                activityTypeId: StringGenerator.generate(),
                contacts: [],
                attachments: [],
                createdDate: new Date(),
                id: StringGenerator.generate(),
                property: PropertyGenerator.generateDto(),
                propertyId: StringGenerator.generate(),
                activityUsers: [ActivityUserGenerator.generateDto(Enums.NegotiatorTypeEnum.LeadNegotiator)],
                solicitor: <Dto.IContact>{},
                solicitorCompany: <Dto.ICompany>{},
                activityDepartments: [],
                sellingReasonId: StringGenerator.generate(),
                sourceId: StringGenerator.generate(),
                keyNumber: StringGenerator.generate(),
                accessArrangements: StringGenerator.generate(),
                appraisalMeetingEnd: moment().toDate().toDateString(),
                appraisalMeetingStart: moment().toDate().toDateString(),
                appraisalMeetingInvitationText: StringGenerator.generate(),
                appraisalMeetingAttendees: [],
                kfValuationPrice: 21,
                agreedInitialMarketingPrice: 21,
                vendorValuationPrice: 21,
                shortKfValuationPrice: 23,
                shortAgreedInitialMarketingPrice: 24,
                shortVendorValuationPrice: 25,
                longKfValuationPrice: 26,
                longAgreedInitialMarketingPrice: 27,
                longVendorValuationPrice: 28,
                disposalTypeId: StringGenerator.generate(),
                decorationId: StringGenerator.generate(),
                serviceChargeAmount: 29,
                serviceChargeNote: StringGenerator.generate(),
                groundRentAmount: 30,
                groundRentNote: StringGenerator.generate(),
                otherCondition: StringGenerator.generate(),
                priceTypeId: StringGenerator.generate(),
                activityPrice: NumberGenerator.generate(),
                matchFlexibilityId: StringGenerator.generate(),
                matchFlexValue: NumberGenerator.generate(),
                matchFlexPercentage: NumberGenerator.generate(),
                rentPaymentPeriodId: StringGenerator.generate(),
                shortAskingWeekRent: NumberGenerator.generate(),
                shortAskingMonthRent: NumberGenerator.generate(),
                longAskingWeekRent: NumberGenerator.generate(),
                longAskingMonthRent: NumberGenerator.generate(),
                shortMatchFlexibilityId: StringGenerator.generate(),
                shortMatchFlexWeekValue: NumberGenerator.generate(),
                shortMatchFlexMonthValue: NumberGenerator.generate(),
                shortMatchFlexPercentage: NumberGenerator.generate(),
                longMatchFlexibilityId: StringGenerator.generate(),
                longMatchFlexWeekValue: NumberGenerator.generate(),
                longMatchFlexMonthValue: NumberGenerator.generate(),
                longMatchFlexPercentage: NumberGenerator.generate(),
                chainTransactions: []
            }
            return angular.extend(activity, specificData || {});

        }

        public static generateManyDtos(n: number): Dto.IActivity[] {
            return _.times(n, ActivityGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Activity[] {
            return _.map<Dto.IActivity, Business.Activity>(ActivityGenerator.generateManyDtos(n), (activity: Dto.IActivity) => { return new Business.Activity(activity); });
        }

        public static generate(specificData?: any): Business.Activity {
            return new Business.Activity(ActivityGenerator.generateDto(specificData));
        }

        public static generateActivityEdit(specificData?: any): Business.ActivityEditModel {
            return new Business.ActivityEditModel(ActivityGenerator.generateDto(specificData));
        }
    }
}