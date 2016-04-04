/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Core.Directives {

    class NavBarController {
        constructor(private navigationService: Services.NavigationService) {}

        public goHome() {
            this.navigationService.goHome();
        }

        public goBack() {
            this.navigationService.goBack();
        }
    }

    function NavBarDirective() {
        return {
            restrict: 'A',
            scope: {
                title: '@',
                logoBig: '=',
                hamburguer: '=',
                back: '='
            },
            templateUrl: 'core/templates/nav-bar.template.html',
            controller: NavBarController,
            controllerAs: '$ctrl'
        };
    }

    angular.module('app.core')
        .directive('skiNavBar', NavBarDirective);
}
