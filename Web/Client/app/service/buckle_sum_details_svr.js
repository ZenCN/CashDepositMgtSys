(function () {
    'use strict';

    angular.module('app.service')
        .factory('buckle_sum_details_svr', buckle_sum_details_svr);

    buckle_sum_details_svr.$inject = ['svr'];

    function buckle_sum_details_svr(svr) {

        return {
            load_page_data: load_page_data
        };

        function load_page_data(params, callback) {
            svr.http({
                url: 'generation_buckle/sumdetails',
                params: params
            }, callback);
        }
    }
})();