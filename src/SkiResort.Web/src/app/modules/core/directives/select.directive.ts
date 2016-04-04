module SkiResort.App.Core.Directives {

    class SelectController {

        private classes = {
            modalOpened: 'ski-select-modal--opened'
        };

        private selectors = {
            modal: '.ski-select-modal'
        };

        constructor(private $scope, private $element) {}

        public showModal(): void {
            if (this.$scope.disabled) { return; }
            this.$element.find(this.selectors.modal).addClass(this.classes.modalOpened);
        }
        public closeModal(): void {
            this.$element.find(this.selectors.modal).removeClass(this.classes.modalOpened);
        }

        public updateSelection(option: any): void {
            if (this.$scope.disabled) { return; }
            this.$scope.model = option;
            this.closeModal();
        }

        public resetSelection(): void {
            if (this.$scope.disabled) { return; }
            this.updateSelection('');
        }
    }

    function SelectDirective() {
        return {
            restrict: 'E',
            scope: {
                model: '=',
                placeholder: '@',
                selectOptions: '=',
                required: '=',
                name: '@',
                disabled: '=',
                hasTabs: '@'
            },
            templateUrl: 'core/templates/select.template.html',
            controller: SelectController,
            controllerAs: '$ctrl'
        };
    }

    angular.module('app.core')
        .directive('skiSelect', SelectDirective);
}
