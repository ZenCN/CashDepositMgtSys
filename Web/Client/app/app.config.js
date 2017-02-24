(function () {
    'use strict';

    angular
        .module('app')
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider'];

    function config($stateProvider, $urlRouterProvider, $ocLazyLoadProvider) {
        $ocLazyLoadProvider.config({
            debug: false,
            events: true
        });

        $urlRouterProvider.when('', '/login');
        $urlRouterProvider.otherwise('/login');

        var resolve_dep = function (config) {
            return {
                load: [
                    '$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load(config);
                    }
                ]
            };
        };

        $stateProvider
            .state('login', {
                url: '/login',
                controller: 'login_ctrl',
                templateUrl: '../client/app/login/login.html',
                resolve: resolve_dep([
                    '../client/app/login/login_ctrl.js',
                    '../client/app/service/login_svr.js'
                ])
            })
            .state('dashboard', {
                url: '/dashboard',
                controller: 'dashboard_ctrl',
                templateUrl: '../client/app/dashboard/dashboard.html',
                resolve: resolve_dep([
                    '../client/app/dashboard/dashboard_ctrl.js',
                    '../client/app/controller/datepicker_ctrl.js'
                ])
            })
            .state('dashboard.generation_buckle', {
                url: '/generation_buckle',
                controller: 'generation_buckle_ctrl',
                templateUrl: '../client/app/generation_buckle/generation_buckle.html',
                resolve: resolve_dep([
                    '../client/app/generation_buckle/generation_buckle_ctrl.js',
                    '../client/app/service/generation_buckle_svr.js',
                    '../client/bower_component/bootstrap-fileinput/css/fileinput.min.css',
                    '../client/bower_component/bootstrap-fileinput/js/fileinput.min.js',
                    '../client/app/directive/sing_upload.js',
                    '../client/app/controller/modal_ctrl.js'
                ])
            })
            .state('dashboard.generation_gives', {
                url: '/generation_gives',
                controller: 'generation_gives_ctrl',
                templateUrl: '../client/app/generation_gives/generation_gives.html',
                resolve: resolve_dep([
                    '../client/app/generation_gives/generation_gives_ctrl.js',
                    '../client/app/service/generation_gives_svr.js',
                    '../client/app/controller/modal_ctrl.js'
                ])
            })
            .state('dashboard.sys_settings', {
                url: '/sys_settings',
                controller: 'sys_settings_ctrl',
                templateUrl: '../client/app/sys_settings/sys_settings.html',
                resolve: resolve_dep([
                    '../client/app/sys_settings/sys_settings_ctrl.js'
                ])
            });
    }
})();