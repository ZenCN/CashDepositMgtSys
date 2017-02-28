(function () {
    'use strict';

    angular.module('app.service')
        .factory('generation_buckle_svr', generation_buckle_svr);

    generation_buckle_svr.$inject = ['svr'];

    function generation_buckle_svr(svr) {

        return {
            review_state: {
                name: state_name,
                change: change_state
            },
            delete: delete_saleman
        };

        function delete_saleman(ids, callback) {
            svr.http('generation_buckle/delete?ids=' + ids.join(","), callback);
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
            svr.http('generation_buckle/changereviewstate?ids=' + ids.join(",") + '&state=' + state, callback);
        }
    }
})();