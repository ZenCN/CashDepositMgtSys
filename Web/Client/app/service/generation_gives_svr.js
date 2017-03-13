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
            case -1:
                return '信息有误，银行扣款失败';
            case 2:
                return '等待省级会计初审';
            case -2:
                return '市级领导审核未通过';
            case 3:
                return '等待省级会计复审';
            case -3:
                return '省级会计初审未通过';
            case 4:
                return '等待省级财务资金部处理';
            case -4:
                return '省级会计复审未通过';
            case 5:
                return '代付处理中...';
            case 6:
                return '代付成功';
            case -6:
                return '代付失败';
            }
        }

        function change_state(ids, state, callback) {
            svr.http('generation_gives/changereviewstate?ids=' + ids.join(",") + '&state=' + state, callback);
        }
    }
})();