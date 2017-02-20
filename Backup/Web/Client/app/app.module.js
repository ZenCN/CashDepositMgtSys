(function() {
    'use strict';

    angular.module('app', [
        'app.core',
        'app.layout',
        'app.page',
        'app.widget',
        'app.service'
    ]);

    angular.module('app.core', [
        'ui.router',
        'oc.lazyLoad',
        'angular-loading-bar'
    ]);

    angular.module('app.layout', []);

    angular.module('app.page', []);

    angular.module('app.widget', [
        'ui.bootstrap'
    ]);

    angular.module('app.service', []);
})();