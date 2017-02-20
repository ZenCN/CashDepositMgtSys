(function() {
    'use strict';

    angular.module('app.widget')
        .directive('updatePwd', updatePwd);

    updatePwd.$inject = [];

    function updatePwd() {
        return function ($scope, $element) {

            $(function() {
                $element.click(function() {
                    easyDialog.open({
                        container: {
                            header: '修改密码',
                            content: $('#update_pwd').html(),
                            yesFn: function() {
                                var check_result = false,
                                    $table = $('#easyDialogBox table'),
                                    old_pwd = $table.find('input[name=old_pwd]').val(),
                                    new_pwd = $table.find('input[name=new_pwd]').val(),
                                    confirm_pwd = $table.find('input[name=confirm_pwd]').val();

                                if (typeof old_pwd != 'string' || old_pwd.trim().length == 0) {
                                    $table.find('td[name=error]').text('原密码不能为空！');
                                } else if (typeof new_pwd != 'string' || new_pwd.trim().length == 0) {
                                    $table.find('td[name=error]').text('新密码不能为空！');
                                } else if (typeof confirm_pwd != 'string' || confirm_pwd.trim().length == 0) {
                                    $table.find('td[name=error]').text('确认密码不能为空！');
                                } else if (new_pwd != confirm_pwd) {
                                    $table.find('td[name=error]').text('新密码与确认密码不一致！');
                                } else if (old_pwd.trim() == new_pwd.trim()) {
                                    $table.find('td[name=error]').text('原密码不能与新密码相同！');
                                } else {
                                    $.ajax({
                                        url: 'login/updatepw',
                                        data: {
                                            old_pw1: old_pwd,
                                            old_pw2: old_pwd,
                                            new_pw: new_pwd
                                        },
                                        success: function(response) {
                                            response = response.retVal ? 1 : response;

                                            if (response == 0) {
                                                $table.find('td[name=error]').text('原密码不正确');
                                            } else if (response > 0) {
                                                $('#overlay').fadeOut('normal', function() {
                                                    msg('密码更改成功！');
                                                });
                                            } else {
                                                $table.find('td[name=error]').text(response);
                                            }
                                        }
                                    });
                                }

                                return check_result;
                            },
                            noFn: true
                        }
                    });
                });
            });
        };
    }
})();