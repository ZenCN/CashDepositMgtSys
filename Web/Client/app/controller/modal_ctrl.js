(function() {
    'use strict';

    angular
        .module('app.widget')
        .controller('modal_ctrl', modal_ctrl)
        .controller('modal_instance_ctrl', modal_instance_ctrl);

    modal_ctrl.$inject = ['$scope', '$modal', 'svr'];

    function modal_ctrl(vm, $modal, svr) {

        vm.open_dialog = function(action, type) {
            var html = undefined;
            vm.model = {
                title: undefined,
                salesman_sex: '男',
                salesman_card_type:  '身份证'
            };

            if (type == 'buckle') {
                html = '../client/app/generation_buckle/generation_buckle_record.html';

                if (action == 'add') {
                    vm.model.title = '新增代扣数据';
                } else {
                    var selected = undefined;
                    $.each(vm.search.result, function() {
                        if (this.checked) {
                            selected = this;
                            return false;
                        }
                    });

                    if (!selected) {
                        return msg('未选择销售人员');
                    }

                    vm.model = angular.copy(selected);
                    vm.model.title = '修改代扣数据';
                    delete vm.model.$$hashKey;
                }
                
            } else {
                html = '../client/app/generation_gives/generation_gives_record.html';

                if (action == 'add') {
                    vm.model.title = '新增代付申请';
                } else {
                    var selected = undefined;
                    $.each(vm.search.result, function () {
                        if (this.checked) {
                            selected = this;
                            return false;
                        }
                    });

                    if (!selected) {
                        return msg('未选择销售人员');
                    }

                    vm.model = angular.copy(selected);
                    vm.model.title = '修改代付申请';
                    delete vm.model.$$hashKey;
                }
            }

            var callback = function(type) {
                if (type == 'save') {
                    vm.search.from_svr();
                } else {
                    $.each(vm.search.result, function(i) {
                        if (this.checked) {
                            vm.search.result[i] = vm.model;
                            return false;
                        }
                    });
                }
            };

            if (type == 'gives' && action == 'modify') {
                svr.http('generation_gives/getdeducteds?id=' + vm.model.id, function(response) {
                    if (response.data.result == 'success') {
                        vm.model.deducted_items = response.data.extra.list;
                    }

                    $modal.open({
                        templateUrl: html,
                        controller: 'modal_instance_ctrl',
                        resolve: {
                            model: function() {
                                return vm.model;
                            }
                        }
                    }).result.then(callback);
                });
            } else {
                $modal.open({
                    templateUrl: html,
                    controller: 'modal_instance_ctrl',
                    resolve: {
                        model: function() {
                            return vm.model;
                        }
                    }
                }).result.then(callback);
            }
        };
    };

    modal_instance_ctrl.$inject = ['$scope', '$modalInstance', 'model', 'svr'];

    function modal_instance_ctrl(vm, $modalInstance, model, svr) {
        vm.model = model;

        vm.generation_buckle = {
            save: function($valid) {
                if ($valid) {
                    svr.http('generation_buckle/save?buckle=' + angular.toJson(vm.model), function(response) {
                        if (response.data.result == 'success') {
                            if (vm.model.id > 0) {
                                msg('修改成功！');
                                $modalInstance.close('modify');
                            } else {
                                msg('保存成功！');
                                $modalInstance.close('save');
                            }
                        } else {
                            msg(response.data.msg);
                        }
                    });
                } else {
                    msg('销售人员信息不完整，请完成所有必填项！', 1500);
                }
            }
        };

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

                this.compute_salesman_refunds();
            },
            delete: function() {
                if ($.isArray(vm.model.deducted_items)) {
                    var array = $.grep(vm.model.deducted_items, function(item) {
                        return !item.checked;
                    });

                    if (vm.model.deducted_items.length > array.length) {
                        vm.model.deducted_items = array;
                    } else {
                        msg('未选择扣款项目');
                    }

                    this.compute_salesman_refunds();
                }
            },
            print: function() {

            },
            submit: function() {

            },
            compute_salesman_refunds: function() {
                if (Number(vm.model.salesman_cash_deposit) > 0) {
                    var sub_amount = 0;
                    if ($.isArray(vm.model.deducted_items)) {
                        $.each(vm.model.deducted_items, function() {
                            sub_amount = calc.addition(sub_amount, this.amount);
                        });
                    }

                    vm.model.salesman_refunds = calc.subtraction(vm.model.salesman_cash_deposit, sub_amount);
                } else {
                    vm.model.salesman_refunds = undefined;
                }
            },
            save: function($valid) {
                if ($valid) {
                    var generation_gives = angular.copy(vm.model);
                    delete generation_gives.$$hashKey;
                    delete generation_gives.deducted;

                    $.extend(generation_gives, {
                        recorder_code: $.cookie('user_code'),
                        review_state: 0,
                        agency_code: $.cookie('agency_code')
                    });

                    var deducted_items = generation_gives.deducted_items;
                    delete generation_gives.deducted_items;

                    svr.http('generation_gives/save?generation_gives=' + angular.toJson(generation_gives) +
                        '&deducted_items=' + (deducted_items ? angular.toJson(deducted_items) : ''), function(response) {
                            if (response.data.result == 'success') {
                                msg('保存成功!');
                                $modalInstance.close('save');
                            } else {
                                msg(response.data.msg);
                            }
                        });
                } else {
                    msg('销售人员信息不完整，请完成所有必填项！', 1500);
                }
            }
        };

        vm.cancel = function() {
            $modalInstance.dismiss();
        }
    }
})();