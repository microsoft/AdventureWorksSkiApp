/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Dining.Controllers {
    'use strict';

    import DiningService = Dining.Services.DiningService;
    import Restaurant = Dining.Models.Restaurant;
    import LevelOfNoise = Dining.Models.LevelOfNoise;

    class DiningController {

        private loading: boolean;
        private restaurants: Array<Restaurant>;
        private filters: Array<any>;
        private orderBy: string;

        constructor(private diningService: DiningService) {
            this.getRestaurants();
            this.fillFilters();
            this.orderBy = '';
        }

        private getRestaurants(): void {
            this.loading = true;
            this.diningService.getNear()
                .then(restaurants => {
                    this.restaurants = restaurants;
                })
                .finally(() => {
                    this.loading = false;
                });
        }

        private fillFilters(): void {
            this.filters = this.diningService.getFilters();
        }

        public getImage(id: number): any {
            return this.diningService.getImage(id);
        }

        public getLevelOfNoise(id: number): string {
            return LevelOfNoise[id];
        }
    }

    angular.module('app.dining')
        .controller('DiningController', DiningController);
}
