(function () {
    'use strict';

    angular.module('app.service')
        .factory('login_svr', login_svr);

    login_svr.$inject = ['svr'];

    function login_svr(svr) {

        return {
            validate: validate
        };

        function validate(user, callback) {
            return svr.http('http://10.20.147.103:8080/api/json/reply/login?username=' + user.code + '&password=' + user.password + '&t' + Math.random(), callback);
        };
    }
})();