﻿///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Business = Antares.Common.Models.Business;
        import Dto = Antares.Common.Models.Dto;

        export class ActivitiesListController {
            activities: Dto.IActivityQueryResult[] = [];
            selectedActivity: string;
            componentId: string;
            isLoading: boolean = false;

            constructor(
                componentRegistry: Antares.Core.Service.ComponentRegistry,
                private dataAccessService: Antares.Services.DataAccessService) {

                componentRegistry.register(this, this.componentId);
            }

            setSelected = (itemToSelect: string) => {
                this.selectedActivity = itemToSelect;
            }

            getSelectedActivity = (): Dto.IActivityQueryResult => {
                var activities = this.activities.filter((a: Dto.IActivityQueryResult) => { return a.id === this.selectedActivity });
                return activities[0];
            }

            loadAllActivities = () => {
                this.isLoading = true;
                return this.dataAccessService.getActivityResource().query().$promise.then((activitiesResources: any) => {
                    this.activities = activitiesResources.map(
                        (item: Dto.IActivityQueryResult) => new Business.ActivityQueryResult(<Dto.IActivityQueryResult>item));
                }).finally(() => {
                    this.isLoading = false;
                    if (this.activities.length > 0) {
                        this.setSelected(this.activities[0].id);
                    }
                });
            }

            loadActivitiesForRequirement = (requirementTypeId: string, countryCode: string) => {
                this.isLoading = true;
                return this.dataAccessService.getActivityResource().getActivitiesForRequirement({ requirementTypeId: requirementTypeId, countryCode: countryCode }, null).$promise.then((activitiesResources: any) => {
                    this.activities = activitiesResources.map(
                        (item: Dto.IActivityQueryResult) => new Business.ActivityQueryResult(<Dto.IActivityQueryResult>item));
                }).finally(() => {
                    this.isLoading = false;
                    if (this.activities.length > 0) {
                        this.setSelected(this.activities[0].id);
                    }
                });
            }
        }

        angular.module('app').controller('activitiesListController', ActivitiesListController);
    }
}