(function() {
    'use strict';

    angular.module('app.service')
        .factory('acceptance_material_svr', acceptance_material_svr);

    acceptance_material_svr.$inject = ['svr'];

    function acceptance_material_svr(svr) {

        return {
            get_station_names: get_station_names,
            save: save,
            modify: modify,
            search: search,
            operate: operate,
            preview_file: preview_file
        };

        function get_station_names(key_words, level, callback) {
            return svr.http('dt04/querystation?key_words=' + key_words + '&level=' + level, callback);
        };

        function save(params, callback) {
            return svr.http('dt04/save?json=' + angular.toJson({
                DD1: params.county_code,
                DD2: params.station.name,
                DD3: params.city_name,
                DD4: params.county_name,
                D02: 1,
                D04: md5.acceptance_report.file_name,
                D05: md5.acceptance_data.file_name,
                D06: md5.acceptance_card.file_name,
                D07: md5.acceptance_report.file_path,
                D08: md5.acceptance_data.file_path,
                D09: md5.acceptance_card.file_path,
                D10: params.remark
            }), callback);
        };

        function modify(params, callback) {
            return svr.http('dt04/modify?json=' + angular.toJson({
                D01: params.id,
                DD2: params.station.name,
                D04: md5.acceptance_report.file_name,
                D05: md5.acceptance_data.file_name,
                D06: md5.acceptance_card.file_name,
                D07: md5.acceptance_report.file_path,
                D08: md5.acceptance_data.file_path,
                D09: md5.acceptance_card.file_path,
                D10: params.remark
            }), callback);
        };

        function search(params, callback) {
            return svr.http({
                url: 'dt04/query',
                params: params
            }, callback)
        };

        function operate(id, action, callback) {
            return svr.http('dt04/changestate?id=' + id + '&oper=' + action, callback);
        };

        function preview_file(id, type, callback) {
            return svr.http('preview/index?id=' + id + '&table=dt04&type=' + type, callback);
        };
    }
})();