var LogViewerModule;
(function (LogViewerModule) {
    var LogEntry = /** @class */ (function () {
        function LogEntry(id, env, timestamp, product, layer, location, message, hostname, userId, userName, exception, elapsedMilliseconds, correlationId, customException, additionalInfo) {
            this.id = id;
            this.env = env;
            this.timestamp = timestamp;
            this.product = product;
            this.layer = layer;
            this.location = location;
            this.message = message;
            this.hostname = hostname;
            this.userId = userId;
            this.userName = userName;
            this.exception = exception;
            this.elapsedMilliseconds = elapsedMilliseconds;
            this.correlationId = correlationId;
            this.customException = customException;
            this.additionalInfo = additionalInfo;
        }
        return LogEntry;
    }());
    LogViewerModule.LogEntry = LogEntry;
})(LogViewerModule || (LogViewerModule = {}));
//# sourceMappingURL=LogEntry.js.map