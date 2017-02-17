(function() {
    'use strict';

    angular
        .module('app.widget')
        .controller('modal_ctrl', modal_ctrl)
        .controller('modal_instance_ctrl', modal_instance_ctrl);

    modal_ctrl.$inject = ['$scope', '$modal', '$timeout'];

    function modal_ctrl(vm, $modal, $timeout) {

        vm.open_dialog = function (type,html) {

            vm.modal = {};

            if(html == 'dk'){
                html = '../client/app/generation_buckle/generation_buckle_record.html';
                vm.modal.title = (type == 'add' ? '新增' : '修改') + '代扣数据';
            }else{
                html = '../client/app/generation_gives/generation_gives_record.html';
                vm.modal.title = '新增代付申请';
            }

            $modal.open({
                templateUrl: html,
                controller: 'modal_instance_ctrl',
                resolve: {
                    modal: function() {
                        return vm.modal;
                    }
                }
            }).result.then(function(refresh) {
                if (refresh) {
                    //vm.search.from_svr();
                }
            });
        };
    };

    modal_instance_ctrl.$inject = ['$scope', '$modalInstance', 'modal'];

    function modal_instance_ctrl(vm, $modalInstance, modal) {
        vm.modal = modal;

        vm.cancel = function() {
            $modalInstance.dismiss('cancel');
        };
    }
})();