using System.Reflection;
using System.Text;
using dotnet_crypto;


internal class Program
{
    private static void Main(string[] args)
    {
        // var configPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config");
        // Console.WriteLine(configPath);

        var dataStr = "This is corporate research! Dont read me!";
        var data = Encoding.UTF8.GetBytes(dataStr);
        var keyLength = 2048; // size in bits
        CryptoService.GenerateKeys(keyLength, out var publicKey, out var privateKey);
        
        CryptoService.ExportKeys(privateKey, publicKey);
        CryptoService.ImportKeys(out var readPublicKey, out var readPrivateKey);
        // Console.WriteLine(readPrivateKey.ToString());

        var encryptedData = CryptoService.Encrypt(data, readPublicKey);
        var encryptedDataAsString = Convert.ToHexString(encryptedData);
        Console.WriteLine("Encrypted Value:\n" + encryptedDataAsString);

        var decryptedData = CryptoService.Decrypt(encryptedData, readPrivateKey);
        var decryptedDataAsString = Encoding.UTF8.GetString(decryptedData);
        Console.WriteLine(decryptedDataAsString);
    }
}