(function () {
    'use strict';

    angular.module('app.service')
        .factory('login_svr', login_svr);

    login_svr.$inject = ['svr'];

    function login_svr(svr) {

        return {
            validate: validate,
            query_user_info: query_user_info
        };

        function validate(user, callback) {
            return svr.http('http://10.20.147.103:8080/api/json/reply/login?username=' + user.code + '&password=' + user.password + '&t' + Math.random(), callback);
        };

        function query_user_info(user_code, callback) {
            return svr.http('login/queryuserinfo?user_code=' + user_code, callback);
        };
    }
})();