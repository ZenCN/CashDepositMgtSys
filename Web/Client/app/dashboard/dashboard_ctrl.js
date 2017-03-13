(function() {
    'use strict';

    angular
        .module('app.layout')
        .controller('dashboard_ctrl', dashboard_ctrl);

    dashboard_ctrl.$inject = ['$scope', 'svr', '$state'];

    function dashboard_ctrl(vm, svr, $state) {
        vm.user = {
            level: Number($.cookie('user_level')),
            code: $.cookie('user_code'),
            name: $.cookie('user_name'),
            agency: {
                code: $.cookie('agency_code'),
                name: $.cookie('agency_name')
            }
        }

        if (typeof vm.user.level != "number") {
            $state.go('login');
        } else if (vm.user.level == 2) {
            vm.user.role = $.cookie('user_role');

            if (isString($.cookie('user_authority'))) {
                vm.user.authority = Number($.cookie('user_authority'));
            }

            if (isString($.cookie('user_jurisdiction'))) {
                vm.user.jurisdiction = $.cookie('user_jurisdiction').split('、');
            }
        }

        vm.state = $state.$current.name.split('.').the_last();

        vm.page = {
            inited: false, //是否已初始化
            index: 0, //页号，0 表示第一页
            filtered: [1], //当前显示的页集合
            all_items: [], //所有的页号
            size: 15, //每页显示多少条记录
            per_num: 5, //页集合的页号数量
            offset_num: 3, //页号(左、右)偏移量
            record_count: 0, //总记录数
            turn_to: function(index) {
                if (index < 0) {
                    msg('已经是第一页');
                } else if (index > (this.all_items.length - 1)) {
                    msg('已经是最后一页');
                } else if ($scope.page.all_items.length < $scope.page.per_num && (index == 0 || index == $scope.page.all_items.last() - 1)) { //如果总页数少于页集合的页号数量,即看得到首页和末页,则不需要分页
                    this.index = index;
                    this.load_data();
                } else {
                    var i = 0, start_index = 0, end_index = 0;
                    angular.forEach($scope.page.filtered, function(val, j) {
                        if (val == (index + 1)) {
                            i = j;
                            return false;
                        }
                    });

                    if (i == 0 || i + 1 == $scope.page.filtered.length) { //the first one
                        $.each($scope.page.all_items, function(i, val) {
                            if (val == (index + 1)) {
                                return false;
                            }
                        });

                        if (i == 0) {
                            start_index = index - $scope.page.offset_num;
                        } else {
                            start_index = index + $scope.page.offset_num - ($scope.page.per_num - 1);
                        }
                        start_index = start_index >= 0 ? start_index : 0;

                        end_index = start_index + $scope.page.per_num;
                        if (end_index + 1 >= $scope.page.all_items.length) {
                            start_index = $scope.page.all_items.length - $scope.page.per_num;
                            end_index = $scope.page.all_items.length;
                        }

                        $scope.page.filtered = $scope.page.all_items.slice(start_index, end_index);
                    }

                    this.index = index;
                    this.load_data();
                }
            },
            load_data: function(search) {
                //load_data实现通用方法体 或 设置为空方法待子作用域实现

                svr.http({
                    url: $state.$current.name.split('.').the_last() + '/search',
                    params: {
                        page_index: vm.page.index,
                        page_size: vm.page.size,
                        salesman_card_id: search.condition.salesman_card_id,
                        salesman_name: search.condition.salesman_name,
                        t: Math.random()
                    }
                }, function(response) {
                    if (response.data.result == 'success') {
                        search.result = response.data.extra.list;

                        if (search.result.length == 0) {
                            msg('未搜索到数据！');
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
            }
        };
    }
})();