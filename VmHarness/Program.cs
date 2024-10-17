﻿// <auto-generated />

using Neo.VM.Harness;
using System.Text.Json;

var res = Harness.Run(args);
var escapedErrmsg = JsonSerializer.Serialize(res.errmsg)[1..^1];

var msg = $"{{\"status\":\"{res.status}\",\"errmsg\":\"{escapedErrmsg}\",\"lastop\":{res.lastop},\"estack\":{res.estack}}}";
var options = new JsonSerializerOptions()
{
    WriteIndented = true
};

var jsonElement = JsonSerializer.Deserialize<JsonElement>(msg);

Console.Write(JsonSerializer.Serialize(jsonElement, options));
