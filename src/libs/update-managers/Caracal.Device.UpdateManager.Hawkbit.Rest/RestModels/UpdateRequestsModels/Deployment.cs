﻿// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.UpdateRequestsModels;

public sealed class Deployment
{
    [JsonPropertyName("download")]
    public required string DownloadType { get; set; }
    
    [JsonPropertyName("update")]
    public required string UpdateType { get; set; }

    [JsonPropertyName("chunks")] 
    public List<Chunk> Chunks { get; set; } = Array.Empty<Chunk>().ToList();
}