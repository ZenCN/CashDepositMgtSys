﻿(function() {
    'use strict';

    angular
        .module('app.page')
        .controller('buckle_sum_details_ctrl', buckle_sum_details_ctrl);

    buckle_sum_details_ctrl.$inject = ['$scope', 'buckle_sum_details_svr'];

    function buckle_sum_details_ctrl(vm, svr) {
        vm.page.load_data = function() {
            svr.load_page_data({
                page_index: vm.page.index,
                page_size: vm.page.size,
                agency_code: vm.search.condition.agency_code,
                channel: vm.search.condition.channel,
                apply_start: vm.search.condition.apply_start.to_str() + ' 00:00:00',
                apply_end: vm.search.condition.apply_end.to_str() + ' 23:59:59',
                t: Math.random()
            }, function(response) {
                if (response.data.result == 'success') {
                    if (response.data.extra.details.length > 0) {
                        vm.search.result = response.data.extra;
                    } else {
                        vm.search.result = {};
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
                apply_start: new Date().get_day(-30),
                apply_end: new Date()
            },
            result: {
                sum: undefined,
                details: undefined
            },
            from_svr: function() {
                vm.page.inited = false;
                vm.page.index = 0;
                vm.page.load_data();
            }
        };

        vm.export = function () {
            if (angular.isObject(vm.search.result.sum)) {
                window.open((window.app_path ? window.app_path : '') + 'generation_buckle/exportsumdetails?page_index=' + vm.page.index + '&page_size=' + vm.page.size + '&agency_code=' +
                    vm.search.condition.agency_code + '&channel=' + vm.search.condition.channel + '&apply_start=' +
                    vm.search.condition.apply_start.to_str() + ' 00:00:00&apply_end=' + vm.search.condition.apply_end.to_str() + ' 23:59:59');
            } else {
                msg('没有数据可以导出');
            }
        };
    }
})();