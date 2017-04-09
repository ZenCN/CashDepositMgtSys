(function() {
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

        vm.validate = function() {

            if (!isString(vm.user.code)) {
                vm.validate.error = '用户名不能为空！';
                return;
            }

            if (!isString(vm.user.password)) {
                vm.validate.error = '密码不能为空！';
                return;
            }

            svr.validate(vm.user, function(data) {
                if (data.Success) {
                    var level = undefined;
                    if (data.ResultData.BranchNo.slice(2) == '0000') {
                        level = 2;
                    } else if (data.ResultData.BranchNo.slice(4) == '00') {
                        level = 3;
                    } else {
                        level = 4;
                    }

                    $.cookie('sso', 0);
                    $.cookie('user_level', level);
                    $.cookie('user_code', data.ResultData.ErpNo);
                    $.cookie('user_name', data.ResultData.RealName);
                    $.cookie('agency_code', data.ResultData.BranchNo);
                    $.cookie('agency_name', data.ResultData.BranchName);

                    if (level < 4) {
                        svr.query_user_info(data.ResultData.ErpNo, function(response) {
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
            });
        };

        vm.validate.error = undefined;
    }
})();