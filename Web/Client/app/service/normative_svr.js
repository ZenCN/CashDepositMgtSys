(function () {
    'use strict';

    angular.module('app.service')
        .factory('normative_svr', normative_svr);

    normative_svr.$inject = ['svr'];

    function normative_svr(svr) {

        return {
            search: {
                file_name: get_file_name,
                file: get_file
            },
            remove_file: remove_file,
            preview_file: preview_file
        };

        function get_file_name(table_id, year, name, callback) {
            return svr.http(table_id + '/search?year=' + year + '&name=' + name, callback);
        }

        function get_file(table_id, year, name, callback) {
            name = name ? name : '';
            return svr.http(table_id + '/index?year=' + year + '&name=' + name, callback);
        }

        function remove_file(table_id, index, callback) {
            return svr.http(table_id + '/delete?index=' + index, callback);
        }

        function preview_file(id, cur_dt, callback) {
            return svr.http('preview/index?id=' + id + '&table=' + cur_dt, callback)
        }
    }
})();