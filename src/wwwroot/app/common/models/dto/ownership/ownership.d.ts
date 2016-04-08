﻿declare module Antares.Common.Models.Dto {
    interface IOwnership {
        id: string;
        createDate: Date;
        purchaseDate: Date;
        sellDate: Date;
        buyPrice?: number;
        sellPrice?: number;
        ownershipTypeId: string;
        ownershipType: IOwnershipType;
        contacts: IContact[];
    }
}