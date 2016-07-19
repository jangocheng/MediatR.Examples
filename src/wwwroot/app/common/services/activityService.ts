﻿/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Dto = Common.Models.Dto;
    import Commands = Common.Models.Commands;

    export class ActivityService {
        private url: string = '/api/activities/';

        constructor(
            private $http: ng.IHttpService,
            private appConfig: Common.Models.IAppConfig)
        { }

        addActivity = (activityCommand: Commands.Activity.ActivityAddCommand): ng.IHttpPromise<Dto.IActivity> => {
            return this.$http.post(this.appConfig.rootUrl + this.url, activityCommand)
                .then<Dto.IActivity>((result: ng.IHttpPromiseCallbackArg<Dto.IActivity>) => result.data);
        }

        updateActivity = (activityCommand: Commands.Activity.ActivityEditCommand): ng.IHttpPromise<Dto.IActivity> => {
            return this.$http.put(this.appConfig.rootUrl + this.url, activityCommand)
                .then<Dto.IActivity>((result: ng.IHttpPromiseCallbackArg<Dto.IActivity>) => result.data);
        }

        getPortals = (): ng.IHttpPromise<Dto.IPortal[]> => {
            var portalsParams: ng.IRequestShortcutConfig = { params: { countryCode: 'GB' } };
            return this.$http.get(this.appConfig.rootUrl + this.url + 'portals', portalsParams)
                .then<Dto.IPortal[]>((result: ng.IHttpPromiseCallbackArg<Dto.IPortal[]>) => result.data);
        }
    }

    angular.module('app').service('activityService', ActivityService);
};