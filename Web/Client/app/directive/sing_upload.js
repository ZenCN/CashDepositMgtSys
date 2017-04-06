(function() {
    'use strict';

    angular
        .module('app.widget')
        .directive('singleUpload', singleUpload);

    singleUpload.$inject = ['svr', '$state'];

    function singleUpload(svr, $state) {
        return function(vm, $element) {
            $(function() {
                $element.fileinput({
                    language: "zh",
                    uploadUrl: (window.app_path ? window.app_path : '') + $state.$current.name.split('.').the_last() + '/import',
                    allowedFileExtensions: ['xls', 'xlsx'],
                    layoutTemplates: {
                        btnBrowse: '<div tabindex="500" class="{css}"{status}>{icon}</div>',
                        caption: '<div class="form-control file-caption {class}">' +
                            '<div style="width:358px;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;" class="file-caption-name"></div>' +
                            '</div>'
                    },
                    uploadExtraData: function() {   //uploadExtraData must be an object or function
                        return { channel: vm.import_channel }
                    }
                }).on('fileuploaded', function(event, config) {

                    if (config.response.result == 'success') {
                        var text = config.filenames.join("、") + ' 导入成功';

                        if (angular.isArray(config.response.extra) && config.response.extra.length > 0) {
                            text += '，' + config.response.msg;

                            vm.search.has_repeated = true;
                            vm.search.result = config.response.extra;
                        } else {
                            vm.search.from_svr();
                        }

                        msg(text, 5000);
                    } else {
                        vm.search.has_repeated = false;
                        throw msg(config.response.msg);
                    }

                    svr.apply(vm);
                    //$element.fileinput('refresh');
                });
            });
        }
    }
})
();