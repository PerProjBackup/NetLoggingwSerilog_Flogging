angular.module("superViewer", ["ngSanitize", "ngRoute", "kendo.directives"])
    .filter('unsafe', $sce => $sce.trustAsHtml)
    .config(function ($routeProvider: angular.route.IRouteProvider) {
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