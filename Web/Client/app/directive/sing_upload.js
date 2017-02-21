(function() {
    'use strict';

    angular
        .module('app.widget')
        .directive('singleUpload', singleUpload);

    //singleUpload.$inject = [];

    function singleUpload() {
        return function($scope, $element, $attr) {
            $(function() {
                $element.fileinput({
                    language: "zh",
                    uploadUrl: 'generation_buckle/import',
                    allowedFileExtensions: ['xls', 'xlsx'],
                    layoutTemplates: {
                        btnBrowse: '<div tabindex="500" class="{css}"{status}>{icon}</div>',
                        caption: '<div class="form-control file-caption {class}">' +
                            '<div style="width:358px;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;" class="file-caption-name"></div>' +
                            '</div>'
                    }
                }).on('fileuploaded', function (event, config) {
                    var text = config.filenames.join("、") + ' 导入成功';

                    if (isString(config.response.extra)) {
                        config.response.extra = JSON.parse(config.response.extra);
                        text += '，但是' + config.response.msg;
                    }

                    msg(text);
                    console.log(config.response.data);
                    //$element.fileinput('refresh');
                });
            });
        }
    }
})();