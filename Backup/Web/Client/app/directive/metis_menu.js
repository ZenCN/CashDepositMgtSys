(function () {
    'use strict';

    angular
        .module('app.widget')
        .directive('metisMenu', metisMenu);

    //metisMenu.$inject = [];

    function metisMenu() {
        return function ($scope, $element) {
            $(function () {
                $element.metisMenu({toggle: false});
            });
        }
    }
})();