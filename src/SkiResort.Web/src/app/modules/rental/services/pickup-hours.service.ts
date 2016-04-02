/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Services {
    'use strict';

    export class PickupHours {

        public all: Array<Models.IPickupHourOption> = [];

        public getById(i): Models.IPickupHourOption {
            var _hours = Math.floor(i / 60);
            var _minutes = i - (_hours * 60);
            var hours = _hours;
            var minutes = _minutes;
            var mer = 'am';
            if (hours > 12) {
                hours = hours - 12;
                mer = 'pm';
            }
            var hours_str = hours.toString();
            var minutes_str = minutes.toString();
            if (hours_str.length === 1) { hours_str = '0' + hours_str; }
            if (minutes_str.length === 1) { minutes_str = '0' + minutes_str; }

            return {
                id: i,
                label: `${hours_str}:${minutes_str} ${mer}`,
                hours: _hours,
                minutes: _minutes
            };
        }

        public generateAll() {
            var result = [];
            var minMinutes = 360;
            var maxMinutes = 1200;
            var steps = 15;

            for (var i = minMinutes; i <= maxMinutes; i = i + steps) {
                result.push(this.getById(i));
            }

            this.all = result;
        }
    }

    angular.module('app.rental')
        .service('pickupHours', PickupHours);
}
