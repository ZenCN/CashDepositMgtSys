(function () {
    'use strict';

    angular.module('app.service')
        .factory('login_svr', login_svr);

    login_svr.$inject = ['svr'];

    function login_svr(svr) {

        return {
            validate: validate,
            load_units: load_units
        };

        function validate(code, pwd, callback) {
            return svr.http('login/login?areacode=' + code + '&pw=' + pwd + '&t' + Math.random(), callback);
        };

        function load_units(code, callback) {
            return svr.http('login/getareainfo?code=' + code, callback);
        };
    }
})();