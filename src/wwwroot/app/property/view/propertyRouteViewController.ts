/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class PropertyRouteViewController {

        constructor(private $scope: ng.IScope, property: Dto.IProperty, private latestViewsProvider: LatestViewsProvider) {
            var propertyViewModel = new Business.PropertyView(property);

            $scope['property'] = propertyViewModel;

            this.saveRecentViewedProperty(propertyViewModel.id);
        }

        private saveRecentViewedProperty(propertyId: string){
            this.latestViewsProvider.addView({
                entityId : propertyId,
                entityType : EntityType.Property
            });
        }
    }

    angular.module('app').controller('PropertyRouteViewController', PropertyRouteViewController);
};