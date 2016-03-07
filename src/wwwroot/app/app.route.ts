﻿/// <reference path="typings/_all.d.ts" />

module Antares {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('contact-add', {
                url: '/contact/add',
                params: {},
                templateUrl: 'app/contact/add/contactAdd.html',
                controllerAs: 'vm',
                controller: 'ContactAddController'
            });

        $urlRouterProvider.otherwise('/app');
    }
}