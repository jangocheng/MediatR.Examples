﻿/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {

    export class CardListController {
        public showItemAdd: () => void;
    }

    angular.module('app').controller('CardListController', CardListController);
}