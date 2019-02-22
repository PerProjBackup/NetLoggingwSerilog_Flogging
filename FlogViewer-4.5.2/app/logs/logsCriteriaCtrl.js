var LogsCriteriaCtrl = (function () {
    function LogsCriteriaCtrl($http, $scope) {
        this.showCriteria = true;
        this.httpService = $http;
        this.like = "";
        this.notLike = "";
        var today = new Date();
        this.beginDt = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 0, 0, 0);
        this.limitTo = 100;
        this.machines = [];
        this.selectedLayers = ["ALL"];
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
        this.logEntries = [];
        this.noEntries = false;
        this.logId = "0";
        var logId = CommonUtilities.getQueryStringParameterByName("logId");
        if (logId && logId !== "0") {
            this.logId = logId;
        }
        this.correlationId = "";
        var correlationId = CommonUtilities.getQueryStringParameterByName("correlationId");
        if (correlationId) {
            this.correlationId = correlationId;
        }
        $scope.vm = this;
        this.thescope = $scope;
        if (this.selectedConnection) {
            this.loadCriteria();
        }
        $scope.$watch(this.logEntries);
        $scope.$watch(this.noEntries);
        $scope.$watch(this.loadingLogEntries);
        this.getLogsIfQueryStringHasValues();
    }
    LogsCriteriaCtrl.prototype.getLogsIfQueryStringHasValues = function () {
        if (this.logId !== "0" || this.correlationId !== "") {
            this.toggleCriteria();
            this.displayLogEntries(this);
        }
    };
    LogsCriteriaCtrl.prototype.toggleCriteria = function () {
        this.showCriteria = !this.showCriteria;
    };
    LogsCriteriaCtrl.prototype.connectionChanged = function () {
        this.logEntries = [];
        this.noEntries = false;
        this.loadCriteria();
        this.noEnvironment = false;
    };
    LogsCriteriaCtrl.prototype.loadCriteria = function () {
        var _this = this;
        localStorage.setItem("connection", this.selectedConnection);
        // ----------------------------------------------------------
        // machines
        // ----------------------------------------------------------
        this.loadingMachines = true;
        this.machines = [];
        this.httpService.get("/api/errorlogs/machines?connKey=" + localStorage.getItem("connection"))
            .success(function (response) {
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
        this.httpService.get("/api/errorlogs/users?connKey=" + localStorage.getItem("connection")).success(function (response) {
            _this.users = ["ALL"];
            _this.users = _this.users.concat(response);
            _this.loadingUsers = false;
            _this.selectedUsers = ["ALL"];
        });
        // ----------------------------------------------------------
        // categories
        // ----------------------------------------------------------
        this.loadingLayers = true;
        this.layers = [];
        this.httpService.get("/api/errorlogs/layers?connKey=" + localStorage.getItem("connection")).success(function (response) {
            _this.layers = ["ALL"];
            _this.layers = _this.layers.concat(response);
            _this.loadingLayers = false;
            _this.selectedLayers = ["ALL"];
        });
    };
    LogsCriteriaCtrl.prototype.getLogEntries = function (successCallback) {
        this.httpService.get("/api/errorlogs/entries?connKey=" + localStorage.getItem("connection")
            + "&machineList=" + this.selectedMachines.valueOf()
            + "&layerList=" + this.selectedLayers.valueOf()
            + "&userList=" + this.selectedUsers.valueOf()
            + "&beginDate=" + encodeURIComponent(CommonUtilities.getStringFromDate(this.beginDt))
            + "&endDate=" + (this.endDate ? encodeURIComponent(CommonUtilities.getStringFromDate(this.endDate)) : "")
            + "&logId=" + this.logId
            + "&correlationId=" + this.correlationId
            + "&like=" + encodeURIComponent(this.like)
            + "&notLike=" + encodeURIComponent(this.notLike)
            + "&limitTo=" + this.limitTo.toString()).success(function (data, status) {
            successCallback(data);
        });
    };
    LogsCriteriaCtrl.prototype.displayLogEntries = function (scope) {
        scope.loadingLogEntries = true;
        scope.logEntries = [];
        scope.noEntries = false;
        this.getLogEntries(function (data) {
            scope.logEntries = data;
            scope.noEntries = (data === undefined || data === null || data.length <= 0);
            scope.loadingLogEntries = false;
        });
    };
    LogsCriteriaCtrl.prototype.searchLogEntries = function (scope) {
        scope.logId = "0";
        scope.correlationId = "";
        scope.displayLogEntries(this);
    };
    return LogsCriteriaCtrl;
}());
LogsCriteriaCtrl.$inject = ["$http", "$scope"];
angular.module("superViewer").controller("LogsCriteriaCtrl", ["$http", "$scope", LogsCriteriaCtrl]);
//# sourceMappingURL=logsCriteriaCtrl.js.map