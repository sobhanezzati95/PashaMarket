﻿using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Framework.Application;
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // 128 bit 
    private const int KeySize = 32; // 256 bit
    public PasswordHasher(IOptions<HashingOptions> options)
    {
        Options = options.Value;
    }
    private HashingOptions Options { get; }
    public string Hash(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Options.Iterations, HashAlgorithmName.SHA256);
        var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);
        return $"{Options.Iterations}.{salt}.{key}";
    }
    public (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
    {
        var parts = hash.Split('.', 3);
        if (parts.Length != 3)
            return (false, false);

        var iterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);
        var needsUpgrade = iterations != Options.Iterations;
        using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        var keyToCheck = algorithm.GetBytes(KeySize);
        var verified = keyToCheck.SequenceEqual(key);
        return (verified, needsUpgrade);
    }
}
public sealed class HashingOptions
{
    public int Iterations { get; set; } = 10000;
}