(function() {
    'use strict';

    angular
        .module('app.widget')
        .controller('modal_ctrl', modal_ctrl)
        .controller('modal_instance_ctrl', modal_instance_ctrl);

    modal_ctrl.$inject = ['$scope', '$modal', '$timeout'];

    function modal_ctrl(vm, $modal, $timeout) {

        vm.open_dialog = function(type, html) {

            vm.model = {};

            if (html == 'buckle') {
                html = '../client/app/generation_buckle/generation_buckle_record.html';
                vm.model.title = (type == 'add' ? '新增' : '修改') + '代扣数据';
            } else {
                html = '../client/app/generation_gives/generation_gives_record.html';
                vm.model = {
                    title: '新增代付申请',
                    salesman_sex: '男',
                    salesman_card_type: '身份证'
                };
            }

            $modal.open({
                templateUrl: html,
                controller: 'modal_instance_ctrl',
                resolve: {
                    model: function () {
                        return vm.model;
                    }
                }
            }).result.then(function(refresh) {
                if (refresh) {
                    //vm.search.from_svr();
                }
            });
        };
    };

    modal_instance_ctrl.$inject = ['$scope', '$rootScope', '$modalInstance', 'model', 'generation_gives_svr'];

    function modal_instance_ctrl(vm, $r_scope, $modalInstance, model, svr) {
        vm.model = model;

        vm.generation_gives = {
            record: function() {
                vm.model.deducted_items = vm.model.deducted_items || [];

                if (!angular.isObject(vm.model.deducted) || !isString(vm.model.deducted.item)) {
                    return msg('未输入扣款项目！');
                }

                if (!angular.isObject(vm.model.deducted) || !(Number(vm.model.deducted.amount) > 0)) {
                    return msg('未输入扣款金额！');
                }

                vm.model.deducted_items.push(vm.model.deducted);

                delete vm.model.deducted;
            },
            delete: function () {
                if ($.isArray(vm.model.deducted_items)) {
                    var array = $.grep(vm.model.deducted_items, function(item) {
                        return !item.checked;
                    });

                    if (vm.model.deducted_items.length > array.length) {
                        vm.model.deducted_items = array;
                    } else {
                        msg('未选择扣款项目');
                    }
                }
            },
            print: function() {
                
            },
            submit: function() {
                
            },
            compute_salesman_refunds: function() {
                if (Number(vm.model.salesman_cash_deposit) > 0) {
                    var sub_amount = 0;
                    $.each(vm.model.deducted_items, function() {
                        sub_amount += this.amount;
                    });

                    vm.model.salesman_refunds = vm.model.salesman_cash_deposit - sub_amount;
                } else {
                    vm.model.salesman_refunds = undefined;
                }
            },
            save: function ($valid) {
                if ($valid) {
                    var generation_gives = angular.copy(vm.model);
                    delete generation_gives.$$hashKey;
                    delete generation_gives.deducted;

                    $.extend(generation_gives, {
                        recorder_code: $r_scope.user.code,
                        review_state: 0,
                        agency_code: $r_scope.agency_code
                    });

                    var deducted_items = generation_gives.deducted_items;
                    delete generation_gives.deducted_items;

                    svr.save(generation_gives, deducted_items, function (response) {
                        
                    });
                } else {
                    msg('代付信息不完整，请填完所有红色框框再保存！');
                }
            },
            cancel: function() {
                $modalInstance.dismiss('cancel');
            }
        };
    }
})();