var app;
(function (app) {
    var SuperView;
    (function (SuperView) {
        var PerfLogEntry = (function () {
            function PerfLogEntry(PerfId, PerfNm, BeginDs, MachineNm, UserName, ElapsedMilliseconds, AddtlInfo) {
                this.PerfId = PerfId;
                this.PerfNm = PerfNm;
                this.BeginDs = BeginDs;
                this.MachineNm = MachineNm;
                this.UserName = UserName;
                this.ElapsedMilliseconds = ElapsedMilliseconds;
                this.AddtlInfo = AddtlInfo;
            }
            return PerfLogEntry;
        }());
        SuperView.PerfLogEntry = PerfLogEntry;
    })(SuperView = app.SuperView || (app.SuperView = {}));
})(app || (app = {}));
//# sourceMappingURL=perfLog.js.map