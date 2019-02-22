var app;
(function (app) {
    var SuperView;
    (function (SuperView) {
        var LogEntry = (function () {
            function LogEntry(LogId, Message, Timestamp, Hostname, Product, Layer, Location, UserId, ElapsedMilliseconds, CustomException, Exception, UserName, CorrelationId, AdditionalInfo) {
                this.LogId = LogId;
                this.Message = Message;
                this.Timestamp = Timestamp;
                this.Hostname = Hostname;
                this.Product = Product;
                this.Layer = Layer;
                this.Location = Location;
                this.UserId = UserId;
                this.ElapsedMilliseconds = ElapsedMilliseconds;
                this.CustomException = CustomException;
                this.Exception = Exception;
                this.UserName = UserName;
                this.CorrelationId = CorrelationId;
                this.AdditionalInfo = AdditionalInfo;
            }
            return LogEntry;
        }());
        SuperView.LogEntry = LogEntry;
    })(SuperView = app.SuperView || (app.SuperView = {}));
})(app || (app = {}));
//# sourceMappingURL=log.js.map