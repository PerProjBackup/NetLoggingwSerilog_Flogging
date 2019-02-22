/// <reference path="../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../node_modules/@types/kendo-ui/index.d.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var LogViewerModule;
(function (LogViewerModule) {
    var ViewModel = /** @class */ (function (_super) {
        __extends(ViewModel, _super);
        function ViewModel() {
            var _this = _super.call(this) || this;
            _this.selectedMachines = [];
            _this.selectedUsers = [];
            _this.selectedLayers = [];
            _this.likeTxt = "";
            _this.notLikeTxt = "";
            _this.limitTo = "100";
            _this.correlationId = "";
            _this.id = "";
            _this.loadingMachines = false;
            _this.loadingUsers = false;
            _this.loadingLayers = false;
            _this.includeInformational = false;
            _this.informationalOnly = false;
            _this.showCriteria = true;
            _this.performingSearch = false;
            _this.noEntries = false;
            _this.noEnvironment = false;
            _this.isError = false;
            _super.prototype.init.call(_this, _this);
            $.ajax({
                url: '/api/LogsApi/Environments',
                method: "GET",
                dataType: "json"
            }).done(function (data) {
                _this.set("availableEnvironments", data);
                var env = CommonUtilities.getQueryStringParameterByName("env");
                if (env) {
                    _this.set("selectedEnvironment", env);
                    _this.loadCriteriaOptions();
                }
                else {
                    var lastEnvironment = localStorage.getItem("flogViewerEnv");
                    if (lastEnvironment) {
                        _this.set("selectedEnvironment", lastEnvironment);
                        _this.loadCriteriaOptions();
                    }
                    else {
                        _this.set("noEnvironment", true);
                    }
                }
                var today = new Date();
                _this.set("beginDt", new Date(today.getFullYear(), today.getMonth(), today.getDate(), 0, 0, 0));
                var corrId = CommonUtilities.getQueryStringParameterByName("correlationId");
                if (corrId) {
                    _this.set("correlationId", corrId);
                }
                var id = CommonUtilities.getQueryStringParameterByName("id");
                if (id) {
                    _this.set("id", id);
                }
                if (_this.correlationId !== "" || _this.id !== "") {
                    _this.toggleCriteria();
                    _this.getLogEntries();
                }
            });
            return _this;
        }
        ViewModel.prototype.loadCriteriaOptions = function () {
            var _this = this;
            localStorage.setItem("flogViewerEnv", this.selectedEnvironment);
            this.set("noEnvironment", false);
            this.set("loadingUsers", true);
            this.set("loadingLayers", true);
            this.set("loadingMachines", true);
            this.set("isError", false);
            this.set("logEntries", []);
            $.ajax({
                url: '/api/LogsApi/Options?env=' + this.selectedEnvironment + '&fieldname=Hostname',
                method: "GET",
                dataType: "json"
            }).done(function (data) {
                _this.set("loadingMachines", false);
                _this.set("availableMachines", data);
                _this.set("selectedMachines", ["ALL"]);
            }).fail(function () {
                _this.set("isError", true);
            });
            $.ajax({
                url: '/api/LogsApi/Options?env=' + this.selectedEnvironment + '&fieldname=UserName',
                method: "GET",
                dataType: "json"
            }).done(function (data) {
                _this.set("loadingUsers", false);
                _this.set("availableUsers", data);
                _this.set("selectedUsers", ["ALL"]);
            }).fail(function () {
                _this.set("isError", true);
            });
            $.ajax({
                url: '/api/LogsApi/Options?env=' + this.selectedEnvironment + '&fieldname=Layer',
                method: "GET",
                dataType: "json"
            }).done(function (data) {
                _this.set("loadingLayers", false);
                _this.set("availableLayers", data);
                _this.set("selectedLayers", ["ALL"]);
            }).fail(function () {
                _this.set("isError", true);
            });
        };
        ViewModel.prototype.toggleCriteria = function () {
            this.set("showCriteria", !this.showCriteria);
        };
        ViewModel.prototype.getLogEntries = function () {
            var _this = this;
            this.set("logEntries", []);
            this.set("performingSearch", true);
            this.set("isError", false);
            this.set("noEntries", false);
            var getUrl = "/api/LogsApi/Entries?env=" + this.selectedEnvironment
                + "&machineList=" + this.selectedMachines.join()
                + "&layerList=" + this.selectedLayers.join()
                + "&userList=" + this.selectedUsers.join()
                + "&beginDate=" + encodeURIComponent(CommonUtilities.getStringFromDate(this.beginDt))
                + "&endDate=" + (this.endDt ? encodeURIComponent(CommonUtilities.getStringFromDate(this.endDt)) : "")
                + "&correlationId=" + this.correlationId
                + "&id=" + this.id
                + "&like=" + encodeURIComponent(this.likeTxt)
                + "&notLike=" + encodeURIComponent(this.notLikeTxt)
                + "&limitTo=" + this.limitTo
                + "&includeInformational=" + this.includeInformational
                + "&informationalOnly=" + this.informationalOnly;
            $.ajax({
                url: getUrl,
                method: "GET",
                dataType: "json"
            }).done(function (data) {
                _this.set("performingSearch", false);
                if (data && data.length > 0) {
                    _this.set("logEntries", data);
                }
                else {
                    _this.set("noEntries", true);
                }
                _this.set("id", "");
                _this.set("correlationId", "");
            }).fail(function () {
                _this.set("isError", true);
            });
        };
        return ViewModel;
    }(kendo.data.ObservableObject));
    LogViewerModule.ViewModel = ViewModel;
})(LogViewerModule || (LogViewerModule = {}));
$(function () {
    var viewModel = new LogViewerModule.ViewModel();
    kendo.bind($("#mainContainer"), viewModel);
});
//# sourceMappingURL=Index.js.map