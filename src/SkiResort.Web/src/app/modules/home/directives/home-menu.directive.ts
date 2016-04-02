module SkiResort.App.Home.Directives {

    function HomeMenuDirective() {

        return {
            restrict: 'E',
            templateUrl: 'home/templates/home-menu.template.html'
        };
    }

    angular.module('app.home')
        .directive('skiHomeMenu', HomeMenuDirective);
}
