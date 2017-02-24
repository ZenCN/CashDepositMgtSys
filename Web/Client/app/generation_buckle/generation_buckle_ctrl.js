(function () {
    'use strict';

    angular
        .module('app.page')
        .controller('generation_buckle_ctrl', generation_buckle_ctrl);

    generation_buckle_ctrl.$inject = ['$scope', 'generation_buckle_svr'];

    function generation_buckle_ctrl(vm, svr) {

        vm.delete = function() {

        };

        vm.review_state = {
            name: function(val) {
                if (vm.search.has_repeated) {
                    return '因数据库中已存在，导入失败';
                } else {
                    svr.review_state.name(val);
                }
            },
            change: function(state) {
                var ids = [], selected = [];
                $.each(vm.search.result, function () {
                    if (this.checked) {
                        ids.push(this.id);
                        selected.push(this);
                    }
                });

                if (ids.length > 0) {
                    svr.review_state.change(ids, state, function (response) {
                        if (response.data.result == 'success') {
                            $.each(selected, function () {
                                this.checked = false;
                                this.review_state = state;
                            });
                            msg('审核完成！');
                        } else {
                            throw msg(response.data.msg);
                        }
                    });
                } else {
                    msg('未选择销售人员！');
                }
            }
        }

        vm.export = function () {
            if (vm.search.has_repeated) {
                return msg('导入失败的数据，暂不支持导出', 4000);
            }

            var url = 'generation_buckle/export?page_index=' + vm.page.index + '&page_size=' + vm.page.size;

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
                    this.checked = !vm.select.checked;
                });
            }
        };

        vm.search = {
            has_repeated: undefined,
            condition: {
                salesman_card_id: undefined,
                salesman_name: undefined
            },
            result: [],
            from_svr: function () {
                vm.search.has_repeated = false;

                vm.page.inited = false;
                vm.page.index = 0;
                vm.page.load_data(vm.search);
            }  
        };

        vm.submit = function() {

        };
    }
})();