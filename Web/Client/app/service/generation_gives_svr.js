(function() {
    'use strict';

    angular.module('app.service')
        .factory('generation_gives_svr', generation_gives_svr);

    generation_gives_svr.$inject = ['svr'];

    function generation_gives_svr(svr) {

        return {
            review: {
                state_name: state_name,
                change_state: change_state
            }
        };

        function state_name(val) {
            switch (Number(val)) {
            case 0:
                return '未处理';
            case 1:
                return '已提交领导审核';
            case 2:
                return '审核已通过';
            case -2:
                return '审核未通过';
            }
        }

        function change_state(ids, state, callback) {
            svr.http('generation_gives/changereviewstate?ids=' + ids.join(",") + '&state=' + state, callback);
        }
    }
})();