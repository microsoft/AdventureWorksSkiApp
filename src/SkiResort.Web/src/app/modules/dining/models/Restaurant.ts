module SkiResort.App.Dining.Models {
    'use strict';

    export class Restaurant {
        public address: string;
        public description: string;
        public familyFriendly: boolean;
        public foodType: number;
        public latitude: number;
        public levelOfNoise: string;
        public longitude: number;
        public mainPhoto: string;
        public name: string;
        public phone: string;
        public priceLevel: number;
        public rating: number;
        public restaurantId: number;
        public restaurantPhotos: Array<string>;
        public takeAway: boolean;
        public distance: number;
    }
}
