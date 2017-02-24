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
            }
        };

        function state_name(val) {
            switch (Number(val)) {
                case 0:
                    return '未处理';
                case 1:
                    return '已提交至审核';
                case 2:
                    return '市级审核已通过';
                case -2:
                    return '市级审核未通过';
                case 3:
                    return '省级审核已通过';
                case -3:
                    return '省级审核未通过';
            }
        }

        function change_state(ids, state, callback) {
            svr.http('generation_buckle/changereviewstate?ids=' + ids.join(",") + '&state=' + state, callback);
        }
    }
})();