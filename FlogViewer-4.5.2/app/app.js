angular.module("superViewer", ["ngSanitize", "ngRoute", "kendo.directives"])
    .filter('unsafe', function ($sce) { return $sce.trustAsHtml; })
    .config(function ($routeProvider) {
    $routeProvider
        .when("/logs", {
        templateUrl: "app/logs/logsCriteriaView.html"
    })
        .when("/perf", {
        templateUrl: "app/performance/perfLogView.html"
    })
        .when("/about", {
        templateUrl: "app/about/aboutview.html"
    })
        .otherwise({
        redirectTo: "/logs"
    });
});
//# sourceMappingURL=app.js.map