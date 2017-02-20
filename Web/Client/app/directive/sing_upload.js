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
                    uploadUrl: 'excel/upload',
                    allowedFileExtensions: ['xls', 'xlsx'],
                    layoutTemplates: {
                        btnBrowse: '<div tabindex="500" class="{css}"{status}>{icon}</div>',
                        caption: '<div class="form-control file-caption {class}">' +
                            '<div style="width:358px;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;" class="file-caption-name"></div>' +
                            '</div>'
                    }
                });
            });
        }
    }
})();