(function() {
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
            case 'add':
                if (vm.user.level == 3) {
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
            case 'modify':
                if (vm.user.level == 4 || vm.user.level == 2) {
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
            var url = 'generation_gives/export?page_index=' + vm.page.index + '&page_size=' + vm.page.size + '&apply_start=' + vm.search.condition.apply_start.to_str()
                + ' 00:00:00&apply_end=' + vm.search.condition.apply_end.to_str() + ' 23:59:59';

            if (isString(vm.search.condition.salesman_card_id)) {
                url += '&salesman_card_id=' + vm.search.condition.salesman_card_id;
            }

            if (isString(vm.search.condition.salesman_name)) {
                url += '&salesman_name=' + vm.search.condition.salesman_name;
            }

            if (isString(vm.search.condition.salesman_code)) {
                url += '&salesman_code=' + vm.search.condition.salesman_code;
            }

            if (isString(vm.search.condition.review_state)) {
                url += '&review_state=' + vm.search.condition.review_state;
            }

            window.open(url);
        };

        vm.select = {
            checked: false,
            all: function() {
                $.each(vm.search.result, function() {
                    if (vm.show_check_box(this)) {
                        this.checked = !vm.select.checked;
                    } else {
                        this.checked = undefined;
                    }
                });
            },
            one: function(saleman) {
                if (vm.show_check_box(saleman)) {
                    saleman.checked = !saleman.checked;
                } else {
                    saleman.checked = undefined;
                }
            }
        };

        vm.show_check_box = function(_this) {
            var state = Number(_this.review_state);
            switch (vm.user.level) {
            case 4:
                if ([0, -2].exist(state) || state == -6 && vm.user.agency.code == _this.agency_code) {
                    return true;
                } else {
                    return false;
                }
            case 3:
                if ([1, -3].exist(state) || state == -6 && vm.user.agency.code == _this.agency_code) {
                    return true;
                } else {
                    return false;
                }
            case 2:
                switch (vm.user.role) {
                case 'financial':
                    if (state >= 5 || state == -6) {
                        return false;
                    } else {
                        return true;
                    }
                case 'accountant':
                    if (vm.user.authority == 1 && [4, 6].exist(state) ||
                        vm.user.authority == 0 && [3, 4, 6].exist(state) || state == -6) {
                        return false;
                    } else {
                        return true;
                    }
                }
            }
        };

        vm.page.size = 15;
        vm.page.load_data = vm.load_page_data;

        vm.search = {
            condition: {
                salesman_card_id: undefined,
                salesman_name: undefined,
                salesman_code: undefined,
                review_state: '',
                apply_start: new Date().get_day(-30),
                apply_end: new Date()
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

                        if (state == 1) {
                            return false; //默认只修改第一个
                        }
                    }
                });

                if (ids.length > 0) {
                    svr.review.change_state(ids, state, function(response) {
                        if (response.data.result == 'success') {
                            var remove = [-2, -3, -4].exist(state);
                            var pass = [2, 3, 4].exist(state);
                            $.each(selected, function() {
                                if (remove) {
                                    vm.search.result.seek('id', this.id, 'del');
                                } else {
                                    this.checked = false;
                                    this.review_state = state;

                                    if (pass) {
                                        this.remark = undefined;
                                    }
                                }
                            });

                            switch (Number(state)) {
                            case 1:
                                return msg('提交成功！');
                            case 2: //市级审核通过后，判断是否为上线后的数据，如果是则直接提交到省财务
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