﻿/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class SearchOptions {
        isEditable = false;
        minLength = 3;
        modelOptions = {
            debounce: {
                default: 500,
                blur: 250
            }
        };

        constructor(specificOptions?: any) {
            if (specificOptions) {
                angular.extend(this, specificOptions);
            }
        }
    }
}