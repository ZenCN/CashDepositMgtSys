(function () {
    'use strict';

    angular
        .module('app')
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider'];

    function config($stateProvider, $urlRouterProvider, $ocLazyLoadProvider) {
        //window.app_path = 'bzj/';   //应用程序路径，开发时应注释掉

        $ocLazyLoadProvider.config({
            debug: false,
            events: true
        });

        $urlRouterProvider.when('', '/login');
        $urlRouterProvider.otherwise('/login');

        var resolve_dep = function (config) {

            if (window.app_path) {
                $.each(config, function(i) {
                    config[i] = window.app_path + config[i];
                });
            }

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
                templateUrl: (window.app_path ? window.app_path : '') + 'client/app/login/login.html',
                resolve: resolve_dep([
                    'client/app/login/login_ctrl.js',
                    'client/app/service/login_svr.js'
                ])
            })
            .state('dashboard', {
                url: '/dashboard',
                controller: 'dashboard_ctrl',
                templateUrl: (window.app_path ? window.app_path : '') + 'client/app/dashboard/dashboard.html',
                resolve: resolve_dep([
                    'client/app/dashboard/dashboard_ctrl.js',
                    'client/app/controller/datepicker_ctrl.js',
                    'client/app/controller/modal_ctrl.js'
                ])
            })
            .state('dashboard.generation_buckle', {
                url: '/generation_buckle',
                controller: 'generation_buckle_ctrl',
                templateUrl: (window.app_path ? window.app_path : '') + 'client/app/generation_buckle/generation_buckle.html',
                resolve: resolve_dep([
                    'client/app/generation_buckle/generation_buckle_ctrl.js',
                    'client/app/service/generation_buckle_svr.js',
                    'client/bower_component/bootstrap-fileinput/css/fileinput.min.css',
                    'client/bower_component/bootstrap-fileinput/js/fileinput.min.js',
                    'client/app/directive/sing_upload.js',
                    'client/app/directive/regexp.js'
                ])
            })
            .state('dashboard.generation_gives', {
                url: '/generation_gives',
                controller: 'generation_gives_ctrl',
                templateUrl: (window.app_path ? window.app_path : '') + 'client/app/generation_gives/generation_gives.html',
                resolve: resolve_dep([
                    'client/app/generation_gives/generation_gives_ctrl.js',
                    'client/app/service/generation_gives_svr.js',
                    'client/app/directive/regexp.js'
                ])
            })
            .state('dashboard.buckle_sum_details', {
                url: '/buckle_sum_details',
                controller: 'buckle_sum_details_ctrl',
                templateUrl: (window.app_path ? window.app_path : '') + 'client/app/buckle_sum_details/buckle_sum_details.html',
                resolve: resolve_dep([
                    'client/app/buckle_sum_details/buckle_sum_details_ctrl.js',
                    'client/app/service/buckle_sum_details_svr.js'
                ])
            })
            .state('dashboard.buckle_schedule', {
                url: '/buckle_schedule',
                controller: 'buckle_schedule_ctrl',
                templateUrl: (window.app_path ? window.app_path : '') + 'client/app/buckle_schedule/buckle_schedule.html',
                resolve: resolve_dep([
                    'client/app/buckle_schedule/buckle_schedule_ctrl.js',
                    'client/app/service/buckle_schedule_svr.js'
                ])
            })
            .state('dashboard.gives_sum_details', {
                url: '/gives_sum_details',
                controller: 'gives_sum_details_ctrl',
                templateUrl: (window.app_path ? window.app_path : '') + 'client/app/gives_sum_details/gives_sum_details.html',
                resolve: resolve_dep([
                    'client/app/gives_sum_details/gives_sum_details_ctrl.js',
                    'client/app/service/gives_sum_details_svr.js'
                ])
            })
            .state('dashboard.gives_schedule', {
                url: '/gives_schedule',
                controller: 'gives_schedule_ctrl',
                templateUrl: (window.app_path ? window.app_path : '') + 'client/app/gives_schedule/gives_schedule.html',
                resolve: resolve_dep([
                    'client/app/gives_schedule/gives_schedule_ctrl.js',
                    'client/app/service/gives_schedule_svr.js'
                ])
            });
    }
})();