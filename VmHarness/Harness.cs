// <auto-generated />

using Neo.Json;

namespace Neo.VM.Harness;

readonly record struct Result(string status, string errmsg, byte lastop, string estack);

class HarnessExecutionEngine : ExecutionEngine
{
    public Exception ex;
    public byte lastop;

    protected override void OnFault(Exception ex)
    {
        this.ex = ex;
        base.OnFault(ex);
    }

    protected override void PreExecuteInstruction(Instruction instruction)
    {
        lastop = ((byte)instruction.OpCode);
    }
}

class Harness
{
    public static Result Run(string[] args)
    {
        byte lastop = 0;
        if (args.Length != 1)
        {
            return new Result(status: "argument error", errmsg: "invalid number of arguments", lastop: lastop, estack: "[]");
        }
        byte[] bytes = default!;
        try
        {
            bytes = Convert.FromBase64String(args.First());
        }
        catch (Exception ex)
        {
            return new Result(status: "decoding error", errmsg: "invalid base64 string: " + ex.Message, lastop: lastop, estack: "[]");
        }
        Script script = default!;
        try
        {
            script = new Script(bytes, true);
        } catch (Exception ex)
        {
            return new Result(status: "VM error", errmsg: ex.Message, lastop: lastop, estack: "[]");
        }
        using HarnessExecutionEngine engine = new();
        engine.LoadScript(script);
        engine.Execute();
        lastop = engine.lastop;
        switch (engine.State)
        {
            case VMState.FAULT:
                return new Result(status: "VM error", errmsg: engine.ex.Message, lastop: lastop, estack: "[]");
            case VMState.HALT:
                return new Result(status: "VM halted", errmsg: "", lastop: lastop, estack: new JArray(engine.ResultStack.Select(p => p.ToJson())).ToString());
        }
        throw new Exception("unknown state");
    }
}
