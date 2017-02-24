(function() {
    'use strict';

    angular.module('app.service')
        .factory('generation_gives_svr', generation_gives_svr);

    generation_gives_svr.$inject = ['svr'];

    function generation_gives_svr(svr) {

        return {
            save: save,
            review: {
                state_name: state_name,
                change_state: change_state
            }
        };

        function save(generation_gives, deducted_items, callback) {
            svr.http('generation_gives/save?generation_gives=' + angular.toJson(generation_gives) + '&deducted_items=' + angular.toJson(deducted_items), callback);
        }

        function state_name(val) {
            switch (Number(val)) {
            case 0:
                return '未处理';
            case 1:
                return '已提交审核';
            case 2:
                return '审核已通过';
            case 3:
                return '审核未通过，已提交至省财务';
                '省财务'
            }
        }

        function change_state(ids, state, callback) {
            svr.http('generation_gives/changereviewstate?ids=' + ids.join(",") + '&state=' + state, callback);
        }
    }
})();