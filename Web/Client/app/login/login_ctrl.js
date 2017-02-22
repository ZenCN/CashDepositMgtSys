(function () {
    'use strict';

    angular
        .module('app.page')
        .controller('login_ctrl', login_ctrl);

    login_ctrl.$inject = ['$scope', '$rootScope', 'login_svr', '$state'];

    function login_ctrl(vm, $r_scope, login_svr, $state) {
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

            var response = {
                data: {
                    "Success": true,
                    "ResultData": {
                        "__type": "UnifyManage.Model.MAPIUser, UnifyManage.Model",
                        "UserId": 669,
                        "ErpNo": "14308026",
                        "RealName": "曾志群",
                        "BranchNo": "430000",
                        "BranchName": "湖南省分公司",
                        "DeptNo": "19",
                        "DeptName": "信息技术部",
                        "Channel": "YG",
                        "Channels": "YG"
                    },
                    "ErrInfo": ""
                }
            };

            if (response.data.Success) {
                $.extend(vm.user, {
                    name: response.data.ResultData.RealName,
                    agency: {
                        code: response.data.ResultData.BranchNo,
                        name: response.data.ResultData.BranchName
                    },
                    dept_name: response.data.ResultData.DeptName
                });

                delete vm.user.password;
                delete vm.user.$$hashKey;
                $r_scope.user = vm.user;

                $state.go('dashboard.generation_buckle');
            } else {
                vm.validate.error = response.data.ErrInfo;
            }

            /*login_svr.validate(vm.user, function(response) {
                if (response.data.Success) {
                    $.extend(vm.user, {
                        name: response.data.ResultData.RealName,
                        agency: {
                            code: response.data.ResultData.BranchNo,
                            name: response.data.ResultData.BranchName
                        },
                        dept_name: response.data.ResultData.DeptName
                    });

                    delete vm.user.password;
                    delete vm.user.$$hashKey;
                    $r_scope.user = vm.user;

                    //console.log(vm.user);

                    $state.go('dashboard.generation_buckle');
                } else {
                    vm.validate.error = response.data.ErrInfo;
                }
            });*/
        };

        vm.validate.error = undefined;
    }
})();