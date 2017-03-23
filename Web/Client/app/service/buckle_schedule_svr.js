(function () {
    'use strict';

    angular.module('app.service')
        .factory('buckle_schedule_svr', buckle_schedule_svr);

    buckle_schedule_svr.$inject = ['svr'];

    function buckle_schedule_svr(svr) {

        return {
            load_page_data: load_page_data
        };

        function load_page_data(params, callback) {
            svr.http({
                url: 'generation_buckle/queryschedule',
                params: params
            }, callback);
        }
    }
})();