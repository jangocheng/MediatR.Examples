﻿declare module Antares.Common.Models.Dto {
    interface IUserData {
        name: string;
        email: string;
        country: string;
        division: IEnumTypeItem;
        roles: string[];
    }
}