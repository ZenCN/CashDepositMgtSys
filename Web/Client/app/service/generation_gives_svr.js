﻿(function() {
    'use strict';

    angular.module('app.service')
        .factory('generation_gives_svr', generation_gives_svr);

    generation_gives_svr.$inject = ['svr'];

    function generation_gives_svr(svr) {

        return {
            review: {
                state_name: state_name,
                change_state: change_state
            },
            delete: delete_saleman
        };

        function delete_saleman(ids, callback) {
            svr.http('generation_gives/delete?ids=' + ids.join(","), callback);
        }

        function state_name(val) {
            switch (Number(val)) {
                case 0:
                    return '未处理';
                case 1:
                    return '等待市级领导审核';
                case 2:
                    return '等待省级领导审核';
                case -2:
                    return '市级领导审核未通过';
                case 3:
                    return '省级领导审核已通过';
                case -3:
                    return '省级领导审核未通过';
            }
        }

        function change_state(ids, state, callback) {
            svr.http('generation_gives/changereviewstate?ids=' + ids.join(",") + '&state=' + state, callback);
        }
    }
})();