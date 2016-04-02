/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Core.Directives {

    function AvatarDirective() {

        function setBackgroundImage(element, url: string) {
            element.attr('style', 'background-image: url(\'' + url + '\');');
        }

        function link(scope, element, attr) {
            element.addClass('ski-avatar');

            attr.$observe('skiAvatar', function() {
                setBackgroundImage(element, attr.skiAvatar);
            });
        }

        return {
            link: link
        };
    }

    angular.module('app.core')
        .directive('skiAvatar', AvatarDirective);
}
