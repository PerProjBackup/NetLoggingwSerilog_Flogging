module app.SuperView {
    export class PerfLogEntry {

        constructor(public PerfId: number,
            public PerfNm: string,
            public BeginDs: Date,
            public MachineNm: string,
            public UserName: string,
            public ElapsedMilliseconds: number,
            public AddtlInfo: string) {
        }
    }
}