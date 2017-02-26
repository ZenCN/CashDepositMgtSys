(function () {
    'use strict';

    angular
        .module('app.page')
        .controller('generation_gives_ctrl', generation_gives_ctrl);

    generation_gives_ctrl.$inject = ['$scope', 'generation_gives_svr'];

    function generation_gives_ctrl(vm, svr) {
        vm.delete = function () {
            msg('删除功能尚未开放');
        };

        vm.export = function() {
            msg('此功能尚未开发');
        };

        vm.select = {
            checked: false,
            all: function () {
                $.each(vm.search.result, function () {
                    if (vm.show_check_box(this.review_state)) {
                        this.checked = !vm.select.checked;
                    } else {
                        this.checked = undefined;
                    }
                });
            },
            one: function (saleman) {
                if (vm.show_check_box(saleman.review_state)) {
                    saleman.checked = !saleman.checked;
                } else {
                    saleman.checked = undefined;
                }
            }
        };

        vm.show_check_box = function (state) {
            state = Number(state);
            switch (vm.user.level) {
                case 4:
                    if (state == 1)
                        return false;
                case 3:
                    if (state == 2)
                        return false;
                case 2:
                    if (state == 3)
                        return false;
            }

            return true;
        };

        vm.search = {
            condition: {
                salesman_card_id: undefined,
                salesman_name: undefined
            },
            result: [],
            from_svr: function () {
                vm.page.inited = false;
                vm.page.index = 0;
                vm.page.load_data(vm.search);
            }
        };

        vm.search.from_svr();

        vm.review_state = {
            name: svr.review.state_name,
            change: function (state) {
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
                            $.each(selected, function() {
                                this.checked = false;
                                this.review_state = state;
                            });

                            if (vm.user.level == 4)
                                msg('提交成功！');
                            else
                                msg('审核完成！');
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