﻿/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    angular.module('app').component('activityAdd', {
        templateUrl: 'app/activity/edit/activityEdit.html',
        controllerAs : 'vm',
        controller: 'ActivityEditController',
        bindings: {
            property: '<'
        }
    });

    angular.module('app').component('activityEdit', {
        templateUrl: 'app/activity/edit/activityEdit.html',
        controllerAs: 'vm',
        controller: 'ActivityEditController',
        bindings: {
            activity: '=',
            config: '<'
        }
    });
}