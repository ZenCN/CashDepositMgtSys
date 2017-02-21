(function () {
    'use strict';

    angular
        .module('app.page')
        .controller('generation_buckle_ctrl', generation_buckle_ctrl);

    generation_buckle_ctrl.$inject = ['$scope', 'generation_buckle_svr'];

    function generation_buckle_ctrl(vm, svr) {

        vm.delete = function() {

        };

        vm.export = function () {
            var url = 'generation_buckle/export?page_index=' + vm.page.index + '&page_size=' + vm.page.size;

            if (isString(vm.search.condition.salesman_card_id)) {
                url += '&salesman_card_id=' + vm.search.condition.salesman_card_id;
            }

            if (isString(vm.search.condition.salesman_name)) {
                url += '&salesman_name=' + vm.search.condition.salesman_name;
            }

            window.open(url);
        };

        vm.page.load_data = function() {
            svr.search({
                url: 'generation_buckle/search',
                params: {
                    page_index: vm.page.index,
                    page_size: vm.page.size,
                    salesman_card_id: vm.search.condition.salesman_card_id,
                    salesman_name: vm.search.condition.salesman_name
                }
            }, function (response) {
                if (response.data.result == 'success') {
                    vm.search.result = response.data.extra.list;

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
                salesman_card_id: undefined,
                salesman_name: undefined
            },
            result: [],
            from_svr: function() {
                vm.page.inited = false;
                vm.page.index = 0;
                vm.page.load_data();
            }  
        };
    }
})();