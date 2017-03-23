(function () {
    'use strict';

    angular.module('app.service')
        .factory('gives_sum_details_svr', gives_sum_details_svr);

    gives_sum_details_svr.$inject = ['svr'];

    function gives_sum_details_svr(svr) {

        return {
            load_page_data: load_page_data
        };

        function load_page_data(params, callback) {
            svr.http({
                url: 'generation_gives/sumdetails',
                params: params
            }, callback);
        }
    }
})();