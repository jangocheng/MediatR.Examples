﻿/// <reference path="../../../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IAddressFormViewControlConfig extends Antares.Common.Models.Dto.IControlConfig {
        address: Antares.Common.Models.Dto.IFieldConfig,
        addressId: Antares.Common.Models.Dto.IFieldConfig
    }
}