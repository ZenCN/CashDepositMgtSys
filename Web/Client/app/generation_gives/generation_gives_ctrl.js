﻿(function() {
    'use strict';

    angular
        .module('app.page')
        .controller('generation_gives_ctrl', generation_gives_ctrl);

    generation_gives_ctrl.$inject = ['$scope', 'generation_gives_svr'];

    function generation_gives_ctrl(vm, svr) {

        vm.authority = function(btn) {
            switch (btn) {
            case 'push':
                if (vm.user.level == 2 && vm.user.role == 'financial') {
                    return true;
                } else {
                    return false;
                }
            case 'view':
                if (vm.user.level == 2 && vm.user.role == 'accountant' && vm.user.authority == 0) {
                    return true;
                } else {
                    return false;
                }
            case 'review':
                if (vm.user.level == 2 && vm.user.role == 'accountant' && vm.user.authority == 1) {
                    return true;
                } else {
                    return false;
                }
            }
        };

        vm.delete = function() {
            vm.delete = function() {
                var ids = [];
                $.each(vm.search.result, function() {
                    if (this.checked) {
                        ids.push(this.id);
                    }
                });

                if (ids.length > 0) {
                    if (confirm('确定要删除吗？')) {
                        svr.delete(ids, function(response) {
                            if (response.data.result == 'success') {
                                vm.search.result = $.grep(vm.search.result, function(_this) {
                                    return !ids.exist(_this.id);
                                });

                                msg('删除成功！');
                            } else {
                                throw msg(response.data.msg);
                            }
                        });
                    }
                } else {
                    msg('未选择销售人员！');
                }
            };
        };

        vm.export = function() {
            var url = 'generation_gives/export?page_index=' + vm.page.index + '&page_size=' + vm.page.size;

            if (isString(vm.search.condition.salesman_card_id)) {
                url += '&salesman_card_id=' + vm.search.condition.salesman_card_id;
            }

            if (isString(vm.search.condition.salesman_name)) {
                url += '&salesman_name=' + vm.search.condition.salesman_name;
            }

            window.open(url);
        };

        vm.select = {
            checked: false,
            all: function() {
                $.each(vm.search.result, function() {
                    if (vm.show_check_box(this.review_state)) {
                        this.checked = !vm.select.checked;
                    } else {
                        this.checked = undefined;
                    }
                });
            },
            one: function(saleman) {
                if (vm.show_check_box(saleman.review_state)) {
                    saleman.checked = !saleman.checked;
                } else {
                    saleman.checked = undefined;
                }
            }
        };

        vm.show_check_box = function(state) {
            state = Number(state);
            switch (vm.user.level) {
            case 4:
                if ([0, -2, -1].exist(state)) {
                    return true;
                } else {
                    return false;
                }
            case 3:
                if ([1, -3, -1].exist(state)) {
                    return true;
                } else {
                    return false;
                }
            case 2:
                switch (vm.user.role) {
                    case 'financial':
                        if (state >= 5) {
                            return false;
                        } else {
                            return true;
                        }
                    case 'accountant':
                        if (vm.user.authority == 1 && [4, 6].exist(state) ||
                            vm.user.authority == 0 && [3, 4, 6].exist(state)) {
                            return false;
                        } else {
                            return true;
                        }
                }
            }
        };

        vm.search = {
            condition: {
                salesman_card_id: undefined,
                salesman_name: undefined
            },
            result: [],
            from_svr: function() {
                vm.page.inited = false;
                vm.page.index = 0;
                vm.page.load_data(vm.search);
            }
        };

        vm.search.from_svr();

        vm.review_state = {
            name: svr.review.state_name,
            change: function(state) {
                var ids = [], selected = [];
                $.each(vm.search.result, function() {
                    if (this.checked) {
                        ids.push(this.id);
                        selected.push(this);
                    }
                });

                if (ids.length > 0) {
                    svr.review.change_state(ids, state, function(response) {
                        if (response.data.result == 'success') {
                            var remove = [-2, -3, -4].exist(state);
                            $.each(selected, function () {
                                if (remove) {
                                    vm.search.result.seek('id', this.id, 'del');
                                } else {
                                    this.checked = false;
                                    this.review_state = state;
                                }
                            });

                            switch (Number(state)) {
                            case 1:
                                return msg('提交成功！');
                            case 2:  //市级审核通过后，判断是否为上线后的数据，如果是则直接提交到省财务
                                vm.search.from_svr();
                            case 3:
                            case 4:
                                return msg('已通过！');
                            case -2:
                            case -3:
                                return msg('已拒绝！');
                            case 5:
                                return msg('已推送！');
                            default:
                                return msg('操作成功');
                            }
                        } else {
                            throw msg(response.data.msg);
                        }
                    });
                } else {
                    msg('未选择销售人员！');
                }
            }
        };
    }
})();