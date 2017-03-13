(function () {
    'use strict';

    angular.module('app.service')
        .factory('sum_details_svr', sum_details_svr);

    sum_details_svr.$inject = ['svr'];

    function sum_details_svr(svr) {

        return {
            
        };
    }
})();