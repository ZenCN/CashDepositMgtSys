(function () {
    'use strict';

    angular
        .module('app')
        .config(config)
        .run(function($http) {
            if (location.href.indexOf('?') > 0) {
                var userno = location.href.substr(location.href.indexOf('userno=') + 7, location.href.indexOf('&pwd=') - location.href.indexOf('userno=') - 7);
                var pwd = location.href.substr(location.href.indexOf('&pwd=') + 5);

                var response = undefined;
                switch (decodeURI(userno)) {
                    case '湖南-会计-初审':
                        response = {
                            data: {
                                "Success": true,
                                "ResultData": {
                                    "ErpNo": "14308026",
                                    "RealName": "湖南会计初审",
                                    "BranchNo": "430000",
                                    "BranchName": "湖南省分公司"
                                },
                                "ErrInfo": ""
                            }
                        };
                        break;
                    case '湖南-会计-复审':
                        response = {
                            data: {
                                "Success": true,
                                "ResultData": {
                                    "ErpNo": "14308126",
                                    "RealName": "湖南会计复审",
                                    "BranchNo": "430000",
                                    "BranchName": "湖南省分公司"
                                },
                                "ErrInfo": ""
                            }
                        };
                        break;
                    case '湖南-财务资金部':
                        response = {
                            data: {
                                "Success": true,
                                "ResultData": {
                                    "ErpNo": "14308146",
                                    "RealName": "湖南财务资金部",
                                    "BranchNo": "430000",
                                    "BranchName": "湖南省分公司"
                                },
                                "ErrInfo": ""
                            }
                        };
                        break;
                    case '株洲':
                        response = {
                            data: {
                                "Success": true,
                                "ResultData": {
                                    "ErpNo": "14354826",
                                    "RealName": "株洲",
                                    "BranchNo": "430200",
                                    "BranchName": "湖南省株洲市分公司"
                                },
                                "ErrInfo": ""
                            }
                        };
                        break;
                    case '永州':
                        response = {
                            data: {
                                "Success": true,
                                "ResultData": {
                                    "ErpNo": "14354546",
                                    "RealName": "永州",
                                    "BranchNo": "432900",
                                    "BranchName": "湖南省永州市分公司"
                                },
                                "ErrInfo": ""
                            }
                        };
                        break;
                    case '茶陵':
                        response = {
                            data: {
                                "Success": true,
                                "ResultData": {
                                    "ErpNo": "14372546",
                                    "RealName": "株洲茶陵",
                                    "BranchNo": "430224",
                                    "BranchName": "湖南省株洲市茶陵县分公司"
                                },
                                "ErrInfo": ""
                            }
                        };
                        break;
                    case '零陵1':
                        response = {
                            data: {
                                "Success": true,
                                "ResultData": {
                                    "ErpNo": "14386546",
                                    "RealName": "零陵-1",
                                    "BranchNo": "432901",
                                    "BranchName": "湖南省永州市零陵县分公司"
                                },
                                "ErrInfo": ""
                            }
                        };
                        break;
                    case '零陵2':
                        response = {
                            data: {
                                "Success": true,
                                "ResultData": {
                                    "ErpNo": "14382446",
                                    "RealName": "零陵-2",
                                    "BranchNo": "432901",
                                    "BranchName": "湖南省永州市零陵县分公司"
                                },
                                "ErrInfo": ""
                            }
                        };
                        break;
                }

                if (response.data.Success) {
                    var level = undefined;
                    if (response.data.ResultData.BranchNo.slice(2) == '0000') {
                        level = 2;
                    } else if (response.data.ResultData.BranchNo.slice(4) == '00') {
                        level = 3;
                    } else {
                        level = 4;
                    }

                    $.cookie('sso', 1);
                    $.cookie('user_level', level);
                    $.cookie('user_code', response.data.ResultData.ErpNo);
                    $.cookie('user_name', response.data.ResultData.RealName);
                    $.cookie('agency_code', response.data.ResultData.BranchNo);
                    $.cookie('agency_name', response.data.ResultData.BranchName);

                    if (level == 2) {
                        $http.get('login/queryuserinfo?user_code=' + response.data.ResultData.ErpNo).then(function (response) {
                            if (response.data.result == 'success') {
                                $.cookie('user_role', response.data.extra.role);

                                if (response.data.extra.authority != null) {
                                    $.cookie('user_authority', response.data.extra.authority);
                                }

                                if (response.data.extra.jurisdiction != null) {
                                    $.cookie('user_jurisdiction', response.data.extra.jurisdiction);
                                }

                                if (response.data.extra.agency != null) {
                                    $.cookie('agency', angular.toJson(response.data.extra.agency));
                                }
                            } else {
                                msg('无使用权限，非法操作！');
                            }
                        });
                    }
                } else {
                    msg('菜单登录出错！');
                }

                /*$http.get('login/decode?userno=' + userno + '&pwd=' + pwd, function (response) {
                    $http.get('http://10.20.147.103:8080/api/json/reply/login?username=' + response.data.userno + '&password='
                        + response.data.pwd + '&t' + Math.random()).then(function (response) {
                        if (response.data.Success) {
                            var level = undefined;
                            if (response.data.ResultData.BranchNo.slice(2) == '0000') {
                                level = 2;
                            } else if (response.data.ResultData.BranchNo.slice(4) == '00') {
                                level = 3;
                            } else {
                                level = 4;
                            }

                            $.cookie('user_level', level);
                            $.cookie('user_code', response.data.ResultData.ErpNo);
                            $.cookie('user_name', response.data.ResultData.RealName);
                            $.cookie('agency_code', response.data.ResultData.BranchNo);
                            $.cookie('agency_name', response.data.ResultData.BranchName);

                            if (level == 2) {
                                $http.get('login/queryuserinfo?user_code=' + response.data.ResultData.ErpNo).then(function(response) {
                                    if (response.data.result == 'success') {
                                        $.cookie('user_role', response.data.extra.role);

                                        if (response.data.extra.authority != null) {
                                            $.cookie('user_authority', response.data.extra.authority);
                                        }

                                        if (response.data.extra.jurisdiction != null) {
                                            $.cookie('user_jurisdiction', response.data.extra.jurisdiction);
                                        }

                                        if (response.data.extra.agency != null) {
                                            $.cookie('agency', angular.toJson(response.data.extra.agency));
                                        }
                                    } else {
                                        msg('无使用权限，非法操作！');
                                    }
                                });
                            }
                        } else {
                            msg('菜单登录出错！');
                        }
                    });
                });*/
            }
        });

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
                    '../client/app/controller/datepicker_ctrl.js',
                    '../client/app/controller/modal_ctrl.js'
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
                    '../client/app/directive/regexp.js'
                ])
            })
            .state('dashboard.generation_gives', {
                url: '/generation_gives',
                controller: 'generation_gives_ctrl',
                templateUrl: '../client/app/generation_gives/generation_gives.html',
                resolve: resolve_dep([
                    '../client/app/generation_gives/generation_gives_ctrl.js',
                    '../client/app/service/generation_gives_svr.js',
                    '../client/app/directive/regexp.js'
                ])
            })
            .state('dashboard.buckle_sum_details', {
                url: '/buckle_sum_details',
                controller: 'buckle_sum_details_ctrl',
                templateUrl: '../client/app/buckle_sum_details/buckle_sum_details.html',
                resolve: resolve_dep([
                    '../client/app/buckle_sum_details/buckle_sum_details_ctrl.js',
                    '../client/app/service/buckle_sum_details_svr.js'
                ])
            })
            .state('dashboard.buckle_schedule', {
                url: '/buckle_schedule',
                controller: 'buckle_schedule_ctrl',
                templateUrl: '../client/app/buckle_schedule/buckle_schedule.html',
                resolve: resolve_dep([
                    '../client/app/buckle_schedule/buckle_schedule_ctrl.js',
                    '../client/app/service/buckle_schedule_svr.js'
                ])
            })
            .state('dashboard.gives_sum_details', {
                url: '/gives_sum_details',
                controller: 'gives_sum_details_ctrl',
                templateUrl: '../client/app/gives_sum_details/gives_sum_details.html',
                resolve: resolve_dep([
                    '../client/app/gives_sum_details/gives_sum_details_ctrl.js',
                    '../client/app/service/gives_sum_details_svr.js'
                ])
            })
            .state('dashboard.gives_schedule', {
                url: '/gives_schedule',
                controller: 'gives_schedule_ctrl',
                templateUrl: '../client/app/gives_schedule/gives_schedule.html',
                resolve: resolve_dep([
                    '../client/app/gives_schedule/gives_schedule_ctrl.js',
                    '../client/app/service/gives_schedule_svr.js'
                ])
            });
    }
})();