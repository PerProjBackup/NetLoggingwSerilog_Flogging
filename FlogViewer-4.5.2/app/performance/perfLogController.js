//export interface IPerfScope extends ng.IScope {
//    getEntries: (vm: PerfLogController) => void;
//    noPerfEntries: boolean;    
//    loadingPerfLogEntries: boolean;
//    perfLogEntries: app.SuperView.PerfLogEntry[];        
//}
var PerfLogController = (function () {
    function PerfLogController($http, $scope) {
        this.showCriteria = true;
        this.httpService = $http;
        this.like = "";
        this.notLike = "";
        var today = new Date();
        this.beginDt = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 0, 0, 0);
        this.limitTo = 100;
        this.machines = [];
        this.selectedMachines = ["ALL"];
        this.selectedUsers = ["ALL"];
        this.noEnvironment = false;
        this.selectedConnection = localStorage.getItem("connection");
        var connKey = CommonUtilities.getQueryStringParameterByName("connKey");
        if (connKey && this.selectedConnection !== connKey) {
            this.selectedConnection = connKey;
        }
        if (!this.selectedConnection) {
            this.noEnvironment = true;
        }
        this.perfLogEntries = [];
        this.noPerfEntries = false;
        //if (this.selectedConnection) {
        //    this.loadCriteria();
        //}
        //$scope.getEntries = this.getPerfEntries;
        $scope.$watch(this.perfLogEntries);
        $scope.$watch(this.noPerfEntries);
        $scope.$watch(this.loadingPerfLogEntries);
    }
    PerfLogController.prototype.toggleCriteria = function () {
        this.showCriteria = !this.showCriteria;
    };
    PerfLogController.prototype.getPerfEntries = function () {
    };
    PerfLogController.prototype.connectionChanged = function () {
        this.perfLogEntries = [];
        this.noPerfEntries = false;
        this.loadCriteria();
        this.noEnvironment = false;
    };
    PerfLogController.prototype.loadCriteria = function () {
        var _this = this;
        localStorage.setItem("connection", this.selectedConnection);
        // ----------------------------------------------------------
        // machines
        // ----------------------------------------------------------
        this.loadingMachines = true;
        this.machines = [];
        this.httpService.get("/api/perfLogs/machines?connKey=" + localStorage.getItem("connection")).success(function (response) {
            _this.machines = ["ALL"];
            _this.machines = _this.machines.concat(response);
            _this.loadingMachines = false;
            _this.selectedMachines = ["ALL"];
        });
        // ----------------------------------------------------------
        // users
        // ----------------------------------------------------------
        this.loadingUsers = true;
        this.users = [];
        this.httpService.get("/api/perfLogs/users?connKey=" + localStorage.getItem("connection")).success(function (response) {
            _this.users = ["ALL"];
            _this.users = _this.users.concat(response);
            _this.loadingUsers = false;
            _this.selectedUsers = ["ALL"];
        });
        // ----------------------------------------------------------
        // perf items
        // ----------------------------------------------------------
        this.loadingPerfs = true;
        this.perfs = [];
        this.httpService.get("/api/perfLogs/perfs?connKey=" + localStorage.getItem("connection")).success(function (response) {
            _this.perfs = ["ALL"];
            _this.perfs = _this.perfs.concat(response);
            _this.loadingPerfs = false;
            _this.selectedPerfs = ["ALL"];
        });
    };
    PerfLogController.prototype.getPerfLogEntries = function (successCallback) {
        this.httpService.get("/api/perfLogs/entries?connKey=" + localStorage.getItem("connection")
            + "&machineList=" + this.selectedMachines.valueOf()
            + "&userList=" + this.selectedUsers.valueOf()
            + "&perfList=" + this.selectedPerfs.valueOf()
            + "&beginDate=" + encodeURIComponent(CommonUtilities.getStringFromDate(this.beginDt))
            + "&endDate=" + (this.endDate ? encodeURIComponent(CommonUtilities.getStringFromDate(this.beginDt)) : "")
            + "&like=" + encodeURIComponent(this.like)
            + "&notLike=" + encodeURIComponent(this.notLike)
            + "&limitTo=" + this.limitTo.toString()).success(function (data, status) {
            successCallback(data);
        });
    };
    PerfLogController.prototype.displayPerfLogEntries = function (scope) {
        scope.loadingPerfLogEntries = true;
        scope.perfLogEntries = [];
        scope.noPerfEntries = false;
        this.getPerfLogEntries(function (data) {
            scope.perfLogEntries = data;
            scope.noPerfEntries = (data === undefined || data === null || data.length <= 0);
            scope.loadingPerfLogEntries = false;
        });
    };
    PerfLogController.prototype.searchPerfLogEntries = function (scope) {
        scope.displayPerfLogEntries(this);
    };
    return PerfLogController;
}());
PerfLogController.$inject = ["$http", "$scope"];
angular.module("superViewer").controller("PerfLogController", ["$http", "$scope", PerfLogController]);
//# sourceMappingURL=perfLogController.js.map