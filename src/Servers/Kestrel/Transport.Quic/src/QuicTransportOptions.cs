// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal;

namespace Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;

/// <summary>
/// Options for Quic based connections.
/// </summary>
public sealed class QuicTransportOptions
{
    private long _defaultStreamErrorCode;
    private long _defaultCloseErrorCode;

    /// <summary>
    /// The maximum number of concurrent bi-directional streams per connection.
    /// </summary>
    public int MaxBidirectionalStreamCount { get; set; } = 100;

    /// <summary>
    /// The maximum number of concurrent inbound uni-directional streams per connection.
    /// </summary>
    public int MaxUnidirectionalStreamCount { get; set; } = 10;

    /// <summary>
    /// Sets the idle timeout for connections and streams.
    /// </summary>
    public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromSeconds(130); // Matches KestrelServerLimits.KeepAliveTimeout.

    /// <summary>
    /// The maximum read size.
    /// </summary>
    public long? MaxReadBufferSize { get; set; } = 1024 * 1024;

    /// <summary>
    /// The maximum write size.
    /// </summary>
    public long? MaxWriteBufferSize { get; set; } = 64 * 1024;

    /// <summary>
    /// The maximum length of the pending connection queue.
    /// </summary>
    public int Backlog { get; set; } = 512;

    /// <summary>
    /// Error code used when the stream needs to abort the read or write side of the stream internally.
    /// </summary>
    public long DefaultStreamErrorCode
    {
        get => _defaultStreamErrorCode;
        set
        {
            ValidateErrorCode(value);
            _defaultStreamErrorCode = value;
        }
    }

    /// <summary>
    /// Error code used when an open connection is disposed.
    /// </summary>
    public long DefaultCloseErrorCode
    {
        get => _defaultCloseErrorCode;
        set
        {
            ValidateErrorCode(value);
            _defaultCloseErrorCode = value;
        }
    }

    private static void ValidateErrorCode(long errorCode)
    {
        const long MinErrorCode = 0;
        const long MaxErrorCode = (1L << 62) - 1;

        if (errorCode < MinErrorCode || errorCode > MaxErrorCode)
        {
            throw new ArgumentOutOfRangeException(nameof(errorCode), errorCode, $"A value between {MinErrorCode} and {MaxErrorCode} is required.");
        }
    }

    internal ISystemClock SystemClock = new SystemClock();
}
