module app.SuperView {
    export class LogEntry {

        constructor(public LogId: number,
                    public Message: string,
                    public Timestamp: Date,
                    public Hostname: string,
                    public Product: string,
                    public Layer: string,
                    public Location: string,
                    public UserId: string,
                    public ElapsedMilliseconds: number,
                    public CustomException: string,
                    public Exception: string,
                    public UserName: string,
                    public CorrelationId: string,
                    public AdditionalInfo: string) {            
        }        
    }
}