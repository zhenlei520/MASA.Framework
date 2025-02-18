﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.Caching.Distributed.StackExchangeRedis;

internal static class RedisHelper
{
    public static T? ConvertToValue<T>(this RedisValue redisValue, JsonSerializerOptions jsonSerializerOptions)
    {
        var type = typeof(T);
        var compressMode = GetCompressMode(type, out Type actualType);

        if (compressMode == CompressMode.None)
            return (T?)Convert.ChangeType(redisValue, actualType);

        var byteValue = (byte[])redisValue;
        if (byteValue.Length == 0)
            return default;

        var value = Decompress(byteValue);

        if (compressMode == CompressMode.Compress)
        {
            var valueString = Encoding.UTF8.GetString(value);
            return (dynamic)valueString;
        }

        return JsonSerializer.Deserialize<T>(value, jsonSerializerOptions);
    }

    public static RedisKey[] GetRedisKeys(this IEnumerable<string> keys)
        => keys.Select(key => (RedisKey)key).ToArray();

    public static byte[] Decompress(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        using (GZipStream stream = new GZipStream(ms, CompressionMode.Decompress))
        using (MemoryStream outBuffer = new MemoryStream())
        {
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = stream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            return outBuffer.ToArray();
        }
    }

    public static RedisValue ConvertFromValue<T>(this T value, JsonSerializerOptions jsonSerializerOptions)
    {
        var type = value?.GetType() ?? typeof(T);
        dynamic redisValue;
        switch (GetCompressMode(type, out Type actualType))
        {
            case CompressMode.None:
                redisValue = value!;
                break;
            case CompressMode.Compress:
                redisValue = Compress(Encoding.UTF8.GetBytes(value?.ToString() ?? string.Empty));
                break;
            default:
                var jsonString = JsonSerializer.Serialize(value, jsonSerializerOptions);
                redisValue = Compress(Encoding.UTF8.GetBytes(jsonString));
                break;
        }
        return ConvertToRedisValue(actualType, redisValue);
    }

    private static byte[] Compress(byte[] data)
    {
        using MemoryStream msGZip = new MemoryStream();
        using GZipStream stream = new GZipStream(msGZip, CompressionMode.Compress, true);
        stream.Write(data, 0, data.Length);
        stream.Close();
        return msGZip.ToArray();

    }

    private static RedisValue ConvertToRedisValue(Type type, dynamic value)
    {
        if (type == typeof(byte) || type == typeof(ushort))
            return (long)value;

        if (type == typeof(decimal))
            return new RedisValue(value.ToString());

        return value;
    }

    private static CompressMode GetCompressMode(this Type type, out Type actualType)
    {
        actualType = Nullable.GetUnderlyingType(type) ?? type;

        switch (Type.GetTypeCode(actualType))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Double:
            case TypeCode.Single:
            case TypeCode.Decimal:
                return CompressMode.None;
            case TypeCode.String:
                return CompressMode.Compress;
            default:
                return CompressMode.SerializeAndCompress;
        }
    }
}
