(function () {
    'use strict';

    angular
        .module('app.widget')
        .controller('datepicker_ctrl', datepicker_ctrl);

    datepicker_ctrl.$inject = ['$scope', 'datepickerPopupConfig'];

    function datepicker_ctrl(vm, datepickerPopupConfig) {
        datepickerPopupConfig.appendToBody = true;  //append the body tag

        vm.format = 'yyyyƒÍMM‘¬dd»’';

        vm.today = function () {
            vm.dt = new Date();
        };
        vm.today();

        vm.clear = function () {
            vm.dt = null;
        };

        vm.minDate = vm.dt.getFullYear() - 1 + '-01-01';
        vm.maxDate = '2025-12-31';

        vm.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        //Disable weekend selection
        vm.disabled = function (date, mode) {
            return ( mode === 'day' && ( date.getDay() === 0 || date.getDay() === 6 ) );
        };

        vm.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope[$event.target.attributes['is-open'].value] = true;
        };
    }
})();