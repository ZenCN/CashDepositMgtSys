(function () {
    'use strict';

    angular.module('app.service')
        .factory('gives_schedule_svr', gives_schedule_svr);

    gives_schedule_svr.$inject = ['svr'];

    function gives_schedule_svr(svr) {

        return {
            load_page_data: load_page_data
        };

        function load_page_data(params, callback) {
            svr.http({
                url: 'generation_gives/queryschedule',
                params: params
            }, callback);
        }
    }
})();