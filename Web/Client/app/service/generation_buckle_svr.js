(function () {
    'use strict';

    angular.module('app.service')
        .factory('generation_buckle_svr', generation_buckle_svr);

    generation_buckle_svr.$inject = ['svr'];

    function generation_buckle_svr(svr) {

        return {
            search: search
        };

        function search(config, callback) {
            return svr.http(config, callback);
        };
    }
})();