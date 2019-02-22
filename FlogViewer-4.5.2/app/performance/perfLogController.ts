//export interface IPerfScope extends ng.IScope {
//    getEntries: (vm: PerfLogController) => void;
//    noPerfEntries: boolean;    
//    loadingPerfLogEntries: boolean;
//    perfLogEntries: app.SuperView.PerfLogEntry[];        
//}

class PerfLogController {
    static $inject = ["$http", "$scope"];

    httpService: ng.IHttpService;
    showCriteria: boolean;
    selectedConnection: string;
    machines: string[];
    loadingMachines: boolean;
    selectedMachines: string[];
    perfs: string[];
    loadingPerfs: boolean;
    selectedPerfs: string[];
    users: string[];
    loadingUsers: boolean;
    selectedUsers: string[];    
    beginDt: Date;
    endDate: Date;
    like: string;    
    notLike: string;
    limitTo: number;
    noPerfEntries: boolean;
    noEnvironment: boolean;
    loadingPerfLogEntries: boolean;
    perfLogEntries: app.SuperView.PerfLogEntry[];    

    constructor($http: any, $scope: any) {
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

    
    toggleCriteria() {
        this.showCriteria = !this.showCriteria;
    }

    getPerfEntries() {
    }


    connectionChanged() {
        this.perfLogEntries = [];
        this.noPerfEntries = false;
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
        this.httpService.get("/api/perfLogs/machines?connKey=" + localStorage.getItem("connection")).success((response) => {
            this.machines = ["ALL"];
            this.machines = this.machines.concat(<Array<string>>response);
            this.loadingMachines = false;
            this.selectedMachines = ["ALL"];
        });

        // ----------------------------------------------------------
        // users
        // ----------------------------------------------------------
        this.loadingUsers = true;
        this.users = [];
        this.httpService.get("/api/perfLogs/users?connKey=" + localStorage.getItem("connection")).success((response) => {
            this.users = ["ALL"];
            this.users = this.users.concat(<Array<string>>response);
            this.loadingUsers = false;
            this.selectedUsers = ["ALL"];
        });       

        // ----------------------------------------------------------
        // perf items
        // ----------------------------------------------------------
        this.loadingPerfs = true;
        this.perfs = [];
        this.httpService.get("/api/perfLogs/perfs?connKey=" + localStorage.getItem("connection")).success((response) => {
            this.perfs = ["ALL"];
            this.perfs = this.perfs.concat(<Array<string>>response);
            this.loadingPerfs = false;
            this.selectedPerfs = ["ALL"];
        });        
    }    


    getPerfLogEntries(successCallback: Function): void {
        this.httpService.get("/api/perfLogs/entries?connKey=" + localStorage.getItem("connection")
            + "&machineList=" + this.selectedMachines.valueOf()            
            + "&userList=" + this.selectedUsers.valueOf()
            + "&perfList=" + this.selectedPerfs.valueOf()
            + "&beginDate=" + encodeURIComponent(CommonUtilities.getStringFromDate(this.beginDt))
            + "&endDate=" + (this.endDate ? encodeURIComponent(CommonUtilities.getStringFromDate(this.beginDt)) : "")
            + "&like=" + encodeURIComponent(this.like)
            + "&notLike=" + encodeURIComponent(this.notLike)
            + "&limitTo=" + this.limitTo.toString()
            
        ).success((data, status) => {            
            successCallback(data);
        });
    }

    displayPerfLogEntries(scope: PerfLogController) {        
        scope.loadingPerfLogEntries = true;
        scope.perfLogEntries = [];
        scope.noPerfEntries = false;

        this.getPerfLogEntries(data => {            
            scope.perfLogEntries = data;
            scope.noPerfEntries = (data === undefined || data === null || data.length <= 0);            

            scope.loadingPerfLogEntries = false;
        });
    }

    searchPerfLogEntries(scope: PerfLogController) {              
        scope.displayPerfLogEntries(this);
    }
}

angular.module("superViewer").controller("PerfLogController", ["$http", "$scope", PerfLogController]);