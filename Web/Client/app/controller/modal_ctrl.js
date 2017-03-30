(function() {
    "use strict";

    angular
        .module("app.widget")
        .controller("modal_ctrl", modal_ctrl)
        .controller("modal_instance_ctrl", modal_instance_ctrl);

    modal_ctrl.$inject = ["$scope", "$modal", "svr"];

    function modal_ctrl(vm, $modal, svr) {

        vm.open_dialog = function(action, type) {
            if (type != "remark") {
                var html = undefined;
                vm.model = {
                    salesman_sex: "男",
                    salesman_card_type: "身份证",
                    channel: "个险"
                };

                if (type == "buckle") {
                    html = "../client/app/generation_buckle/generation_buckle_record.html";

                    if (action == "add") {
                        vm.model.title = "新增代扣数据";
                    } else {
                        var selected = undefined;
                        $.each(vm.search.result, function() {
                            if (this.checked) {
                                selected = this;
                                return false;
                            }
                        });

                        if (!selected) {
                            return msg("未选择销售人员");
                        }

                        vm.model = angular.copy(selected);
                        vm.model.title = "修改代扣数据";
                        delete vm.model.$$hashKey;
                    }

                } else {
                    html = "../client/app/generation_gives/generation_gives_record.html";

                    if (action == "add") {
                        vm.model.title = "新增代付申请";
                    } else {
                        var selected = undefined;
                        $.each(vm.search.result, function() {
                            if (this.checked) {
                                selected = this;
                                return false;
                            }
                        });

                        if (!selected) {
                            return msg("未选择销售人员");
                        }

                        vm.model = angular.copy(selected);
                        vm.model.title = "修改代付申请";
                        delete vm.model.$$hashKey;
                    }
                }
                vm.model.action = action;

                var callback = function(type) {
                    if (type == "save") {
                        vm.search.from_svr();
                    } else {
                        $.each(vm.search.result, function(i) {
                            if (this.checked) {
                                if (typeof vm.model.salesman_hiredate == "object") {
                                    vm.model.salesman_hiredate = vm.model.salesman_hiredate.to_str("-");
                                }

                                vm.search.result[i] = vm.model;
                                return false;
                            }
                        });
                    }
                };

                if (type == "gives" && action == "modify") {
                    svr.http("generation_gives/getdeducteds?id=" + vm.model.id, function(response) {
                        if (response.data.result == "success") {
                            vm.model.deducted_items = response.data.extra.list;
                        }

                        $modal.open({
                            templateUrl: html,
                            controller: "modal_instance_ctrl",
                            resolve: {
                                model: function() {
                                    return vm.model;
                                },
                                review_state: function() {
                                    return vm.review_state;
                                }
                            }
                        }).result.then(callback);
                    });
                } else {
                    $modal.open({
                        templateUrl: html,
                        controller: "modal_instance_ctrl",
                        resolve: {
                            model: function() {
                                return vm.model;
                            },
                            review_state: {}
                        }
                    }).result.then(callback);
                }
            } else {
                var ids = [];
                $.each(vm.search.result, function() {
                    if (this.checked) {
                        ids.push(this.id);
                    }
                });

                if (ids.length > 0) {
                    $modal.open({
                        templateUrl: "../client/app/generation_gives/refuse_opinion.html",
                        controller: "modal_instance_ctrl",
                        resolve: {
                            model: function() {
                                return {
                                    title: "审核意见",
                                    type: type,
                                    ids: ids.join("，"),
                                    review_state: action,
                                    opinion: undefined
                                };
                            },
                            review_state: {}
                        }
                    }).result.then(function() {
                        vm.search.from_svr();
                    });
                } else {
                    msg("未选择销售人员！");
                }
            }
        };
    };

    modal_instance_ctrl.$inject = ["$scope", "$modalInstance", "model", "svr", "review_state"];

    function modal_instance_ctrl(vm, $modalInstance, model, svr, review_state) {
        vm.model = model;

        if (vm.model.type != "remark") {
            vm.level = parseInt($.cookie("user_level"));

            vm.generation_buckle = {
                save: function($valid) {
                    //if ($valid) {
                    if (!vm.model.id && typeof vm.model.salesman_hiredate == "object") {
                        vm.model.salesman_hiredate = vm.model.salesman_hiredate.to_str("-");
                    }

                    svr.http("generation_buckle/save?buckle=" + angular.toJson(vm.model), function(response) {
                        if (response.data.result == "success") {
                            if (vm.model.id > 0) {
                                msg("修改成功！");
                                $modalInstance.close("modify");
                            } else {
                                msg("保存成功！");
                                $modalInstance.close("save");
                            }
                        } else {
                            msg(response.data.msg);
                        }
                    });
                    /*} else {
                        msg('销售人员信息不完整，请完成所有必填项！', 1500);
                    }*/
                }
            };

            vm.generation_gives = {
                record: function() {
                    vm.model.deducted_items = vm.model.deducted_items || [];

                    if (!angular.isObject(vm.model.deducted) || !isString(vm.model.deducted.item)) {
                        return msg("未输入扣款项目！");
                    }

                    if (!angular.isObject(vm.model.deducted) || !(Number(vm.model.deducted.amount) > 0)) {
                        return msg("未输入扣款金额！");
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
                            msg("未选择扣款项目");
                        }

                        this.compute_salesman_refunds();
                    }
                },
                print: function() {
                    /*var exist = false;
                    $.each(vm.model.deducted_items, function () {
                        if (this.id == undefined) {
                            exist = true;
                            return false;
                        }
                    });
                    if (exist && confirm('新录入的扣款项目未保存，请先保存')) {
                        return;
                    }*/

                    if (confirm("打印完成后，请在代理人签字后再提交付费申请")) {
                        window.refund_statement = vm.model;
                        window.refund_statement.cash_deducted_total = calc.subtraction(vm.model.salesman_cash_deposit, vm.model.salesman_refunds);
                        window.refund_statement.operator = $.cookie("user_name");
                        window.refund_statement.operate_time = new Date().to_str("YYYY年MM月dd日 HH:mm");

                        window.open("generation_gives/refundstatement");
                    }
                },
                submit: function() {
                    if (confirm("代理人签字后才能提交付费申请，确定提交吗？")) {
                        review_state.change(1);
                        $modalInstance.dismiss();
                    }
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
                    //if ($valid) {
                    if (vm.model.deducted && isString(vm.model.deducted.item)) {
                        return msg('新的扣款项目尚未录入');
                    }

                    var generation_gives = angular.copy(vm.model);
                    delete generation_gives.$$hashKey;
                    delete generation_gives.deducted;

                    if (!generation_gives.id && typeof generation_gives.salesman_hiredate == "object") {
                        generation_gives.salesman_hiredate = generation_gives.salesman_hiredate.to_str("-");
                    }

                    generation_gives =
                        $.extend(generation_gives, {
                            recorder_code: $.cookie("user_code"),
                            review_state: 0,
                            agency_code: $.cookie("agency_code")
                        });

                    var deducted_items = generation_gives.deducted_items;
                    delete generation_gives.deducted_items;

                    svr.http("generation_gives/save?generation_gives=" + angular.toJson(generation_gives) +
                        "&deducted_items=" + (deducted_items ? angular.toJson(deducted_items) : ""), function(response) {
                            if (response.data.result == "success") {
                                msg("保存成功!");
                                $modalInstance.close("save");
                            } else {
                                msg(response.data.msg, 2000);
                            }
                        });

                    /*} else {
                        msg('销售人员信息不完整，请完成所有必填项！', 1500);
                    }*/
                }
            };
        } else {
            vm.save = function() {
                if (isString(vm.model.opinion)) {
                    svr.http("generation_gives/saveopinion?ids=" + vm.model.ids + "&opinion=" + vm.model.opinion + "&review_state=" + vm.model.review_state, function(response) {
                        if (response.data.result == "success") {
                            msg("保存成功!");
                            $modalInstance.close();
                        } else {
                            msg(response.data.msg, 2000);
                        }
                    });
                } else {
                    msg("未填写审核意见！");
                }
            };
        }

        vm.cancel = function() {
            $modalInstance.dismiss();
        };
    }
})();