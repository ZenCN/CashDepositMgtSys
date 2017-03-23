(function () {
    'use strict';

    angular
        .module('app.page')
        .controller('gives_schedule_ctrl', gives_schedule_ctrl);

    gives_schedule_ctrl.$inject = ['$scope', 'gives_schedule_svr'];

    function gives_schedule_ctrl(vm, svr) {
        vm.page.load_data = function () {
            svr.load_page_data({
                page_index: vm.page.index,
                page_size: vm.page.size,
                agency_code: vm.search.condition.agency_code,
                channel: vm.search.condition.channel,
                apply_start: vm.search.condition.apply_start.to_str() + ' 00:00:00',
                apply_end: vm.search.condition.apply_end.to_str() + ' 23:59:59',
                t: Math.random()
            }, function (response) {
                if (response.data.result == 'success') {
                    if (response.data.extra.list.length > 0) {
                        vm.search.result = response.data.extra.list;
                    } else {
                        vm.search.result = undefined;
                        msg('未搜索到任何数据');
                    }

                    if (response.data.extra.page_count > 0) {
                        vm.page.all_items = [];
                        for (var i = 0; i < response.data.extra.page_count; i++) {
                            vm.page.all_items.push(i + 1);
                        }

                        vm.page.record_count = response.data.extra.record_count;
                        if (!vm.page.inited) {
                            if (vm.page.all_items.length >= vm.page.per_num) {
                                vm.page.filtered = vm.page.all_items.slice(0, vm.page.per_num);
                            } else {
                                vm.page.filtered = vm.page.all_items;
                            }
                            vm.page.inited = true;
                        }
                    }
                }
            });
        };

        vm.search = {
            condition: {
                agency_code: '',
                channel: "个险",
                apply_start: new Date().get_day(-7),
                apply_end: new Date()
            },
            result: undefined,
            from_svr: function () {
                vm.page.inited = false;
                vm.page.index = 0;
                vm.page.load_data();
            }
        };

        vm.export = function () {
            if (angular.isArray(vm.search.result)) {
                window.open('generation_gives/exportschedule?page_index=' + vm.page.index + '&page_size=' + vm.page.size + '&agency_code=' +
                    vm.search.condition.agency_code + '&channel=' + vm.search.condition.channel + '&apply_start=' +
                    vm.search.condition.apply_start.to_str() + ' 00:00:00&apply_end=' + vm.search.condition.apply_end.to_str() + ' 23:59:59');
            } else {
                msg('没有数据可以导出');
            }
        };
    }
})();