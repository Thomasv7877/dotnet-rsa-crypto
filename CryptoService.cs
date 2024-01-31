using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace dotnet_crypto;

public static class CryptoService
{
public static void GenerateKeys(int keyLength, out RSAParameters publicKey, out RSAParameters privateKey)
{
    using (var rsa = RSA.Create())
    {
        rsa.KeySize = keyLength;
        publicKey = rsa.ExportParameters(includePrivateParameters: false);
        privateKey = rsa.ExportParameters(includePrivateParameters: true);
    }
}
public static void ExportKeys(RSAParameters privateKey, RSAParameters publicKey, string path = ""){
    using (var rsa = RSA.Create())
    {
        // rsa.KeySize = keyLength;
        rsa.ImportParameters(privateKey);
        // rsa.ImportParameters(publicKey);

        //string privateKeyXml = rsa.ToXmlString(includePrivateParameters: true);
        //File.WriteAllText("private.key", privateKeyXml);

        var privateKeyPem = rsa.ExportRSAPrivateKey();
        File.WriteAllBytes("private.pem", privateKeyPem);
        // string publicKeyXml = rsa.ToXmlString(includePrivateParameters: false);
        // File.WriteAllText("public.key", publicKeyXml);
    }
}
public static void ImportKeys(out RSAParameters publicKey, out RSAParameters privateKey, string path = ""){
    string privateKeyXml = File.ReadAllText("private.key");
    ReadOnlySpan<byte> privateKeyPem = File.ReadAllBytes("private.pem");
    // string publicKeyXml = File.ReadAllText("public.key");

    using (var rsa = RSA.Create())
    {
        // rsa.KeySize = keyLength;
        // rsa.FromXmlString(publicKeyXml);
        //rsa.FromXmlString(privateKeyXml);
        rsa.ImportRSAPrivateKey(privateKeyPem, out var bytesRead);
        publicKey = rsa.ExportParameters(includePrivateParameters: false);
        privateKey = rsa.ExportParameters(includePrivateParameters: true);
    }
}


// public static void ImportKeys(int keyLength, out RSAParameters publicKey, out RSAParameters privateKey, string path = ""){
//     string privateKeyAsString = File.ReadAllText(Path.Join(path, "private.key"));
//     string publicKeyAsString = File.ReadAllText(Path.Join(path, "public.key"));
//     RSAParameters privateKeyParam = JsonSerializer.Deserialize<RSAParameters>(privateKeyAsString);
//     RSAParameters publicKeyParam = JsonSerializer.Deserialize<RSAParameters>(publicKeyAsString);

//     using (var rsa = RSA.Create())
//     {
//         rsa.KeySize = keyLength;
//         rsa.ImportParameters(privateKeyParam);
//         rsa.ImportParameters(publicKeyParam);
//         publicKey = rsa.ExportParameters(includePrivateParameters: false);
//         privateKey = rsa.ExportParameters(includePrivateParameters: true);
//     }
// }
// public static void ExportKeys(RSAParameters privateKey, RSAParameters publicKey, string path = ""){
//     var privateKeyJson = JsonSerializer.Serialize(privateKey);
//     var publicKeyJson = JsonSerializer.Serialize(publicKey);
//     File.WriteAllText(Path.Join(path, "private.key"), privateKeyJson);
//     File.WriteAllText(Path.Join(path, "public.key"), publicKeyJson);
// }
public static byte[] Encrypt(byte[] data, RSAParameters publicKey)
{
    using (var rsa = RSA.Create())
    {
        rsa.ImportParameters(publicKey);
        var result = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
        return result;
    }
}
public static byte[] Decrypt(byte[] data, RSAParameters privateKey)
{
    using (var rsa = RSA.Create())
    {
        rsa.ImportParameters(privateKey);
        return rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
    }
}
}
