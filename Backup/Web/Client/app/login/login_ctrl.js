(function () {
    'use strict';

    angular
        .module('app.page')
        .controller('login_ctrl', login_ctrl);

    login_ctrl.$inject = ['$scope', 'login_svr', '$state'];

    function login_ctrl(vm, login_svr, $state) {

    }
})();