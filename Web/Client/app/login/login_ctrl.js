(function () {
    'use strict';

    angular
        .module('app.page')
        .controller('login_ctrl', login_ctrl);

    login_ctrl.$inject = ['$scope', 'login_svr', '$state'];

    function login_ctrl(vm, svr, $state) {
        vm.user = {
            code: undefined,
            password: undefined
        };

        vm.validate = function () {

            if (!isString(vm.user.code)) {
                vm.validate.error = '用户名不能为空！';
                return;
            }

            if (!isString(vm.user.password)) {
                vm.validate.error = '密码不能为空！';
                return;
            }

            var response = undefined;
            switch (vm.user.code) {
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
                case '株洲1':
                    response = {
                        data: {
                            "Success": true,
                            "ResultData": {
                                "ErpNo": "14354826",
                                "RealName": "株洲1",
                                "BranchNo": "430200",
                                "BranchName": "湖南省株洲市分公司"
                            },
                            "ErrInfo": ""
                        }
                    };
                    break;
                case '株洲2':
                    response = {
                        data: {
                            "Success": true,
                            "ResultData": {
                                "ErpNo": "14326854",
                                "RealName": "株洲2",
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

                $.cookie('sso', 0);
                $.cookie('user_level', level);
                $.cookie('user_code', response.data.ResultData.ErpNo);
                $.cookie('user_name', response.data.ResultData.RealName);
                $.cookie('agency_code', response.data.ResultData.BranchNo);
                $.cookie('agency_name', response.data.ResultData.BranchName);

                if (level < 4) {
                    svr.query_user_info(response.data.ResultData.ErpNo, function (response) {
                        if (response.data.result == 'success') {
                            $.cookie('user_role', response.data.extra.role);

                            if (level == 2 && response.data.extra.role == 'accountant') {
                                if (response.data.extra.authority != null) {
                                    $.cookie('user_authority', response.data.extra.authority);
                                }

                                if (response.data.extra.jurisdiction != null) {
                                    $.cookie('user_jurisdiction', response.data.extra.jurisdiction);
                                }

                                if (response.data.extra.agency != null) {
                                    $.cookie('agency', angular.toJson(response.data.extra.agency));
                                }
                            }

                            $state.go('dashboard.generation_buckle');
                        } else {
                            msg('无使用权限，非法操作！');
                        }
                    });
                } else {
                    $state.go('dashboard.generation_buckle');
                }
            } else {
                vm.validate.error = response.data.ErrInfo;
            }

            /*login_svr.validate(vm.user, function(response) {
                if (response.data.Success) {
                var level = undefined;
                if (response.data.ResultData.BranchNo.slice(2) == '0000') {
                    level = 2;
                } else if (response.data.ResultData.BranchNo.slice(4) == '00') {
                    level = 3;
                } else {
                    level = 4;
                }

                $.cookie('sso', 0);
                $.cookie('user_level', level);
                $.cookie('user_code', response.data.ResultData.ErpNo);
                $.cookie('user_name', response.data.ResultData.RealName);
                $.cookie('agency_code', response.data.ResultData.BranchNo);
                $.cookie('agency_name', response.data.ResultData.BranchName);

                if (level == 2) {
                    svr.query_user_info(response.data.ResultData.ErpNo, function (response) {
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
                            
                            $state.go('dashboard.generation_buckle');
                        } else {
                            msg('无使用权限，非法操作！');
                        }
                    });
                } else {
                    $state.go('dashboard.generation_buckle');
                }
            } else {
                vm.validate.error = response.data.ErrInfo;
            }
            });*/
        };

        vm.validate.error = undefined;
    }
})();