(function () {
    'use strict';

    angular
        .module('app.layout')
        .controller('dashboard_ctrl', dashboard_ctrl);

    dashboard_ctrl.$inject = ['$scope', 'svr', '$state'];

    function dashboard_ctrl(vm, svr, $state) {
        if (location.href.indexOf('userno=') > 0) {
            var cur_url = window.frameElement != null ? window.frameElement.src : location.href;
            var userno = cur_url.substr(cur_url.indexOf('userno=') + 7, cur_url.indexOf('&pwd=') - cur_url.indexOf('userno=') - 7);
            var pwd = cur_url.substr(cur_url.indexOf('&pwd=') + 5);

            /*var response = undefined;
            switch (decodeURI(userno)) {
                case '湖南-会计-初审':
                    response = {
                        data: {
                            "Success": true,
                            "ResultData": {
                                "ErpNo": "14308026",
                                "RealName": "湖南会计初审",
                                "BranchNo": "430000",
                                "BranchName": "湖南省分公司"
                            },
                            "ErrInfo": ""
                        }
                    };
                    break;
                case '湖南-会计-复审':
                    response = {
                        data: {
                            "Success": true,
                            "ResultData": {
                                "ErpNo": "14308126",
                                "RealName": "湖南会计复审",
                                "BranchNo": "430000",
                                "BranchName": "湖南省分公司"
                            },
                            "ErrInfo": ""
                        }
                    };
                    break;
                case '湖南-财务资金部':
                    response = {
                        data: {
                            "Success": true,
                            "ResultData": {
                                "ErpNo": "14308146",
                                "RealName": "湖南财务资金部",
                                "BranchNo": "430000",
                                "BranchName": "湖南省分公司"
                            },
                            "ErrInfo": ""
                        }
                    };
                    break;
                case '株洲':
                    response = {
                        data: {
                            "Success": true,
                            "ResultData": {
                                "ErpNo": "14354826",
                                "RealName": "株洲",
                                "BranchNo": "430200",
                                "BranchName": "湖南省株洲市分公司"
                            },
                            "ErrInfo": ""
                        }
                    };
                    break;
                case '永州':
                    response = {
                        data: {
                            "Success": true,
                            "ResultData": {
                                "ErpNo": "14354546",
                                "RealName": "永州",
                                "BranchNo": "432900",
                                "BranchName": "湖南省永州市分公司"
                            },
                            "ErrInfo": ""
                        }
                    };
                    break;
                case '茶陵':
                    response = {
                        data: {
                            "Success": true,
                            "ResultData": {
                                "ErpNo": "14372546",
                                "RealName": "株洲茶陵",
                                "BranchNo": "430224",
                                "BranchName": "湖南省株洲市茶陵县分公司"
                            },
                            "ErrInfo": ""
                        }
                    };
                    break;
                case '零陵1':
                    response = {
                        data: {
                            "Success": true,
                            "ResultData": {
                                "ErpNo": "14386546",
                                "RealName": "零陵-1",
                                "BranchNo": "432901",
                                "BranchName": "湖南省永州市零陵县分公司"
                            },
                            "ErrInfo": ""
                        }
                    };
                    break;
                case '零陵2':
                    response = {
                        data: {
                            "Success": true,
                            "ResultData": {
                                "ErpNo": "14382446",
                                "RealName": "零陵-2",
                                "BranchNo": "432901",
                                "BranchName": "湖南省永州市零陵县分公司"
                            },
                            "ErrInfo": ""
                        }
                    };
                    break;
            }

            if (response.data.Success) {
                var level = undefined;
                if (response.data.ResultData.BranchNo.slice(2) == '0000') {
                    level = 2;
                } else if (response.data.ResultData.BranchNo.slice(4) == '00') {
                    level = 3;
                } else {
                    level = 4;
                }

                $.cookie('sso', 1);
                $.cookie('user_level', level);
                $.cookie('user_code', response.data.ResultData.ErpNo);
                $.cookie('user_name', response.data.ResultData.RealName);
                $.cookie('agency_code', response.data.ResultData.BranchNo);
                $.cookie('agency_name', response.data.ResultData.BranchName);

                if (level == 2) {
                    $http.get('login/queryuserinfo?user_code=' + response.data.ResultData.ErpNo).then(function (response) {
                        if (response.data.result == 'success') {
                            $.cookie('user_role', response.data.extra.role);

                            if (response.data.extra.authority != null) {
                                $.cookie('user_authority', response.data.extra.authority);
                            }

                            if (response.data.extra.jurisdiction != null) {
                                $.cookie('user_jurisdiction', response.data.extra.jurisdiction);
                            }

                            if (response.data.extra.agency != null) {
                                $.cookie('agency', angular.toJson(response.data.extra.agency));
                            }

                            if (response.data.extra.account_pay != null) {
                                $.cookie('account_pay', response.data.extra.account_pay);
                            }

                            if (response.data.extra.account_gather != null) {
                                $.cookie('account_gather', response.data.extra.account_gather);
                            }
                        } else {
                            msg('无使用权限，非法操作！');
                        }
                    });
                }
            } else {
                msg('菜单登录出错！');
            }*/

            $.ajax({
                url: (window.app_path ? window.app_path : '') + 'login/decodeuserinfo?userno=' + userno + '&pwd=' + pwd,
                async: false,
                success: function (data) {
                    data = JSON.parse(data);
                    if (data.Success) {
                        var level = undefined;
                        if (data.ResultData.BranchNo.slice(2) == '0000') {
                            level = 2;
                        } else if (data.ResultData.BranchNo.slice(4) == '00') {
                            level = 3;
                        } else {
                            level = 4;
                        }

                        $.cookie('sso', 1);
                        $.cookie('user_level', level);
                        $.cookie('user_code', data.ResultData.ErpNo);
                        $.cookie('user_name', data.ResultData.RealName);
                        $.cookie('agency_code', data.ResultData.BranchNo);
                        $.cookie('agency_name', data.ResultData.BranchName);

                        if (level < 4) {
                            $.ajax({
                                url: (window.app_path ? window.app_path : '') + 'login/queryuserinfo?user_code=' + data.ResultData.ErpNo,
                                async: false,
                                success: function (data) {
                                    data = JSON.parse(data);
                                    if (data.result == 'success') {
                                        $.cookie('user_role', data.extra.role);

                                        if (level == 2 && data.extra.role == 'accountant') {
                                            if (data.extra.authority != null) {
                                                $.cookie('user_authority', data.extra.authority);
                                            }

                                            if (data.extra.jurisdiction != null) {
                                                $.cookie('user_jurisdiction', data.extra.jurisdiction);
                                            }

                                            if (data.extra.agency != null) {
                                                $.cookie('agency', angular.toJson(data.extra.agency));
                                            }
                                        }
                                    } else {
                                        msg('无使用权限，非法操作！');
                                    }
                                }
                            });
                        }
                    } else {
                        msg('菜单登录出错！');
                    }
                }
            });
        }

        vm.user = {
            sso: Number($.cookie('sso')) > 0 ? true : false,
            level: Number($.cookie('user_level')),
            code: $.cookie('user_code'),
            name: $.cookie('user_name'),
            agency: {
                code: $.cookie('agency_code'),
                name: $.cookie('agency_name')
            }
        }

        if (typeof vm.user.level != "number" || vm.user.sso && location.href.indexOf('?') < 0) {
            $state.go('login');
        } else if (vm.user.level < 4) {
            vm.user.role = $.cookie('user_role');

            if (isString($.cookie('user_authority'))) {
                vm.user.authority = Number($.cookie('user_authority'));
            }

            if (isString($.cookie('user_jurisdiction'))) {
                vm.user.jurisdiction = $.cookie('user_jurisdiction').split('、');
            }

            if (isString($.cookie('account_pay'))) {
                vm.user.account_pay = $.cookie('account_pay');
            }

            if (isString($.cookie('account_gather'))) {
                vm.user.account_gather = $.cookie('account_gather');
            }

            if (isString($.cookie('agency'))) {
                vm.user.agency = JSON.parse($.cookie('agency'));
                var arr = [];
                $.each(vm.user.agency, function () {
                    arr.push({
                        code: this.code,
                        name: this.name
                    });
                    $.each(this.lower, function () {
                        arr.push({
                            code: this.code,
                            name: this.name,
                            is_lower: true
                        });
                    });
                });
                vm.user.agency = arr;
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
            turn_to: function (index) {
                if (index < 0) {
                    msg('已经是第一页');
                } else if (index > (this.all_items.length - 1)) {
                    msg('已经是最后一页');
                } else if (vm.page.all_items.length < vm.page.per_num && (index == 0 || index == vm.page.all_items.last() - 1)) { //如果总页数少于页集合的页号数量,即看得到首页和末页,则不需要分页
                    this.index = index;
                    this.load_data();
                } else {
                    var i = 0, start_index = 0, end_index = 0;
                    angular.forEach(vm.page.filtered, function (val, j) {
                        if (val == (index + 1)) {
                            i = j;
                            return false;
                        }
                    });

                    if (i == 0 || i + 1 == vm.page.filtered.length) { //the first one
                        $.each(vm.page.all_items, function (i, val) {
                            if (val == (index + 1)) {
                                return false;
                            }
                        });

                        if (i == 0) {
                            start_index = index - vm.page.offset_num;
                        } else {
                            start_index = index + vm.page.offset_num - (vm.page.per_num - 1);
                        }
                        start_index = start_index >= 0 ? start_index : 0;

                        end_index = start_index + vm.page.per_num;
                        if (end_index + 1 >= vm.page.all_items.length) {
                            start_index = vm.page.all_items.length - vm.page.per_num;
                            end_index = vm.page.all_items.length;
                        }

                        vm.page.filtered = vm.page.all_items.slice(start_index, end_index);
                    }

                    this.index = index;
                    this.load_data();
                }
            },
            load_data: function (search) {
                //load_data实现通用方法体 或 设置为空方法待子作用域实现
            }
        };

        vm.load_page_data = function (search) {
            svr.http({
                url: $state.$current.name.split('.').the_last() + '/search',
                params: {
                    page_index: vm.page.index,
                    page_size: vm.page.size,
                    salesman_card_id: search.condition.salesman_card_id,
                    salesman_name: search.condition.salesman_name,
                    salesman_code: search.condition.salesman_code,
                    review_state: search.condition.review_state,
                    apply_start: search.condition.apply_start.to_str() + ' 00:00:00',
                    apply_end: search.condition.apply_end.to_str() + ' 23:59:59',
                    t: Math.random()
                }
            }, function (response) {
                if (response.data.result == 'success') {
                    search.result = response.data.extra.list;

                    if (search.result.length == 0) {
                        msg('未搜索到任何数据！');
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
    }
})();