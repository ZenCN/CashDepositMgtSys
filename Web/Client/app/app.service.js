(function () {
    'use strict';

    angular.module('app.service')
        .factory('svr', svr);

    svr.$inject = ['$http'];

    function svr($http) {
        
        return {
            http: http,
            apply: apply
        }

        function http(config, complete) { //定义通用的http回调方法
            config = typeof config == 'string' ? { url: config } : config;

            return $http(config)
                .then(complete)
                .catch(function(error) {
                    if (angular.isObject(error)) {
                        if (error.data) {
                            throw msg(error.data, 5000);
                        } else {
                            throw msg('service：' + (angular.isObject(error.config) ? error.config.url : '') + '请求出错' + '，状态：' + error.status + ' ' + error.statusText);
                        }
                    } else {
                        throw msg(error.data, 5000);
                    }
                });
        };

        function apply (vm) {
            if (angular.isObject(vm)) {
                vm.$safe_apply = vm.$safe_apply || function() {
                    var $scope, fn, force = false;
                    if (arguments.length == 1) {
                        var arg = arguments[0];
                        if (typeof arg == 'function') {
                            fn = arg;
                        } else {
                            $scope = arg;
                        }
                    } else {
                        $scope = arguments[0];
                        fn = arguments[1];
                        if (arguments.length == 3) {
                            force = !!arguments[2];
                        }
                    }
                    $scope = $scope || this;
                    fn = fn || function() {};
                    if (force || !$scope.$$phase) {
                        $scope.$apply ? $scope.$apply(fn) : $scope.apply(fn);
                    } else {
                        fn();
                    }
                };
                vm.$safe_apply();
            } else {
                throw 'view model参数有误，应为object类型';
            }
        };
    }
})();