﻿/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IPropertyViewControlConfig extends Antares.Common.Models.Dto.IControlConfig  {
        propertyId: Antares.Common.Models.Dto.IFieldConfig;
    }
}