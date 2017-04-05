(function() {
    'use strict';

    angular
        .module('app.page')
        .controller('generation_buckle_ctrl', generation_buckle_ctrl);

    generation_buckle_ctrl.$inject = ['$scope', 'generation_buckle_svr'];

    function generation_buckle_ctrl(vm, svr) {
        vm.import_channel = '个险';

        vm.authority = function(btn) {
            switch (btn) {
            case 'push':
                if (vm.user.level == 2 && vm.user.role == 'financial') {
                    return true;
                } else {
                    return false;
                }
            case 'modify':
            case 'new':
                if (vm.user.level == 4 || vm.user.level == 3 && vm.user.role == 'worker') {
                    return true;
                } else {
                    return false;
                }
            case 'check':
                if (vm.user.level == 3 && vm.user.role == 'leader') {
                    return true;
                } else {
                    return false;
                }
            case 'submit':
                if (vm.user.level == 4 || vm.user.level == 3 && vm.user.role == 'worker') {
                    return true;
                } else {
                    return false;
                }
            }
        };

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

        vm.review_state = {
            name: function(val) {
                if (vm.search.has_repeated) {
                    return '因数据库中已存在，导入失败';
                } else {
                    return svr.review_state.name(val);
                }
            },
            change: function(state) {
                var ids = [], selected = [];
                $.each(vm.search.result, function() {
                    if (this.checked) {
                        ids.push(this.id);
                        selected.push(this);
                    }
                });

                if (ids.length > 0) {
                    svr.review_state.change(ids, state, function(response) {
                        if (response.data.result == 'success') {
                            var remove = [-2, -3].exist(state);
                            $.each(selected, function() {
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
                            case 2:
                            case 3:
                                return msg('已通过！');
                            case -2:
                            case -3:
                                return msg('已拒绝！');
                            case 4:
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
        }

        vm.export = function() {
            if (vm.search.has_repeated) {
                return msg('导入失败的数据，暂不支持导出', 4000);
            }

            var url = 'generation_buckle/export?page_index=' + vm.page.index + '&page_size=' + vm.page.size + '&apply_start=' + vm.search.condition.apply_start.to_str()
                + ' 00:00:00&apply_end=' + vm.search.condition.apply_end.to_str() + ' 23:59:59';

            if (isString(vm.search.condition.salesman_card_id)) {
                url += '&salesman_card_id=' + vm.search.condition.salesman_card_id;
            }

            if (isString(vm.search.condition.salesman_name)) {
                url += '&salesman_name=' + vm.search.condition.salesman_name;
            }

            if (isString(vm.search.condition.review_state)) {
                url += '&review_state=' + vm.search.condition.review_state;
            }

            window.open(url);
        };

        vm.show_check_box = function(_this) {
            var state = Number(_this.review_state);
            switch (vm.user.level) {
            case 4:
                if ([0, -2].exist(state) || state == -5 && vm.user.agency.code == _this.agency_code) {
                    return true;
                } else {
                    return false;
                }
            case 3:
                if (vm.user.role == 'leader' && [1, -3].exist(state) ||
                    vm.user.role == 'worker' && ([0, -2].exist(state) || state == -5 && vm.user.agency.code == _this.agency_code)) {
                    return true;
                } else {
                    return false;
                }
            case 2:
                if (vm.user.role == 'financial' && [2, 3].exist(state)) {
                    return true;
                } else {
                    return false;
                }
            }
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

        vm.page.size = 15;
        vm.page.load_data = vm.load_page_data;

        vm.search = {
            has_repeated: undefined,
            condition: {
                salesman_card_id: undefined,
                salesman_name: undefined,
                review_state: '',
                apply_start: new Date().get_day(-30),
                apply_end: new Date()
            },
            result: [],
            from_svr: function() {
                vm.search.has_repeated = false;

                vm.page.inited = false;
                vm.page.index = 0;
                vm.page.load_data(vm.search);
            }
        };

        vm.search.from_svr();
    }
})();