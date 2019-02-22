class LogsCriteriaCtrl {
    static $inject = ["$http", "$scope"];
    
    httpService: ng.IHttpService;
    showCriteria: boolean;
    selectedConnection: string;
    machines: string[];
    loadingMachines: boolean;
    selectedMachines: string[];
    users: string[];
    loadingUsers: boolean;
    selectedUsers: string[];
    layers: string[];
    loadingLayers: boolean;
    selectedLayers: string[];
    beginDt: Date;
    endDate: Date;
    like: string;
    notLike: string;
    limitTo: number;
    logId: string;
    correlationId: string;    
    noEntries: boolean;
    noEnvironment: boolean;
    loadingLogEntries: boolean;
    logEntries: app.SuperView.LogEntry[];
    thescope: any;
    
    constructor($http: any, $scope: any) {        
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

    getLogsIfQueryStringHasValues() {       
        if (this.logId !== "0" || this.correlationId !== "") {    
            this.toggleCriteria();
            this.displayLogEntries(this);                    
        }
    }

    toggleCriteria() {        
        this.showCriteria = !this.showCriteria;
    }

    connectionChanged() {
        this.logEntries = [];
        this.noEntries = false;
        this.loadCriteria();
        this.noEnvironment = false;
    }    

    loadCriteria() {
        localStorage.setItem("connection", this.selectedConnection);        
        
        // ----------------------------------------------------------
        // machines
        // ----------------------------------------------------------
        this.loadingMachines = true;
        this.machines = [];                
        this.httpService.get("/api/errorlogs/machines?connKey=" + localStorage.getItem("connection"))
                    .success((response) => {  
                        this.machines = ["ALL"];          
                        this.machines = this.machines.concat(<Array<string>> response);
                        this.loadingMachines = false;
                        this.selectedMachines = ["ALL"];            
                    });    

        // ----------------------------------------------------------
        // users
        // ----------------------------------------------------------
        this.loadingUsers = true;
        this.users = [];        
        this.httpService.get("/api/errorlogs/users?connKey=" + localStorage.getItem("connection")).success((response) => {
            this.users = ["ALL"];
            this.users = this.users.concat(<Array<string>>response);
            this.loadingUsers = false;
            this.selectedUsers = ["ALL"];
        });    

        // ----------------------------------------------------------
        // categories
        // ----------------------------------------------------------
        this.loadingLayers = true;
        this.layers = [];        
        this.httpService.get("/api/errorlogs/layers?connKey=" + localStorage.getItem("connection")).success((response) => {
            this.layers = ["ALL"];
            this.layers = this.layers.concat(<Array<string>>response);
            this.loadingLayers = false;
            this.selectedLayers = ["ALL"];
        });    
    }


    getLogEntries(successCallback: Function): void {        
                
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
            + "&limitTo=" + this.limitTo.toString()           
           ).success((data, status) => {
            successCallback(data);
        });
    }

    displayLogEntries(scope: LogsCriteriaCtrl) {
        scope.loadingLogEntries = true;
        scope.logEntries = [];
        scope.noEntries = false;

        this.getLogEntries(data => {
            scope.logEntries = data;
            scope.noEntries = (data === undefined || data === null || data.length <= 0);            
            scope.loadingLogEntries = false;            
        });
    }

    searchLogEntries(scope: LogsCriteriaCtrl) {
        scope.logId = "0";
        scope.correlationId = "";
        scope.displayLogEntries(this);
    }
}

angular.module("superViewer").controller("LogsCriteriaCtrl", ["$http", "$scope", LogsCriteriaCtrl]);