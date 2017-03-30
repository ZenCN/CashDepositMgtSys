(function() {
    'use strict';

    angular
        .module('app.widget')
        .directive('ngRegexp', ngRegexp);

    ngRegexp.$inject = [];

    function ngRegexp() {
        return function (vm, $element, $attrs) {
            $element.blur(function () {
                var result = undefined;
                var arr = $attrs['ngModel'].split('.');

                if (vm[arr[0]][arr[1]] == "" || vm[arr[0]][arr[1]] == undefined) {
                    $element.parent().removeClass('has-error');
                    $element.next().fadeOut();
                    return;
                }

                //正则表达式直接量也被定义为包含在一对斜杠'/'之间的字符
                switch ($attrs['ngRegexp']) {
                    case 'idcard':
                        result = /^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$/.test(vm[arr[0]][arr[1]]);
                        break;
                    case 'decimal':
                        result = /^[0-9]+\.{0,1}[0-9]{0,2}$/.test(vm[arr[0]][arr[1]]); //2表示位小数
                        break;
                    case 'phone_num':
                        result = /^1[3|4|5|7|8]\d{9}$/.test(vm[arr[0]][arr[1]]) || /\d{3}-\d{7}|\d{4}-\d{7}/.test(vm[arr[0]][arr[1]]);
                        break;
                    case 'bank_acc_num':
                        result = /^(\d{16}|\d{19})$/.test(vm[arr[0]][arr[1]]);
                        break;
                }

                if (!result) {
                    $element.parent().addClass('has-error');
                    $element.next().fadeIn();
                } else {
                    $element.parent().removeClass('has-error');
                    $element.next().fadeOut();
                }
            });
        }
    };
})();