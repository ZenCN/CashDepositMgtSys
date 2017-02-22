(function () {
    'use strict';

    angular.module('app.service')
        .factory('generation_gives_svr', generation_gives_svr);

    generation_gives_svr.$inject = ['svr'];

    function generation_gives_svr(svr) {

        return {
            save: save
        };

        function save(buckle, gives, callback) {
            return svr.http(buckle, gives, callback);
        };
    }
})();