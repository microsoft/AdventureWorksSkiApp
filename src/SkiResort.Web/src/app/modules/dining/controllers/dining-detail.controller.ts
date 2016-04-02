/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Dining.Controllers {
    'use strict';
    import Restaurant = Dining.Models.Restaurant;
    import LevelOfNoise = Dining.Models.LevelOfNoise;
    import DiningService = Dining.Services.DiningService;

    class DiningDetailController {

        private restaurant: Restaurant;
        private recommendations: Array<Restaurant> = [];
        private loading: boolean = false;

        constructor(
            private $stateParams: angular.ui.IStateService,
            private diningService: DiningService
        ) {
            this.loading = true;
            var id = $stateParams['id'];
            if (!$stateParams['restaurant']) {
                this.diningService.getSingle(id).then(restaurant => {
                    this.restaurant = restaurant;
                    this.getExtraInformation();
                });
            } else {
                this.restaurant = $stateParams['restaurant'];
                this.getExtraInformation();
            }

        }

        private getExtraInformation(): void {
            this.diningService.getRecommendations(this.restaurant.name)
                .then((restaurants: Array<Restaurant>) => {
                    this.recommendations = restaurants;
                    this.loading = false;
                }
            );
        }

        public getImage(id: number): any {
            return this.diningService.getImage(id);
        }

        public getLevelOfNoise(id: number): string {
            return LevelOfNoise[id];
        }
    }

    angular.module('app.dining')
        .controller('DiningDetailController', DiningDetailController);
}
