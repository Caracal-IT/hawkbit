﻿// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.ControllerModels;

public sealed class Config
{
    [JsonPropertyName("polling")]
    public required Polling Polling { get; set; }
}