using System.Security.Cryptography;

namespace CryptoDemo._1251
{
  internal class Program
  {
    static void Main(string[] args)
    {
      // Sample of encrypting and decrypting using both RSA Asymmetric and AES Symmetric methods
      {
        // Host/Server Side - Get RSA public key to Remote/Client
        RSACryptoServiceProvider rsa = new(2048); // create provider, uses 2Kbits for key
        // Now rsa has a private and public key, 
        
        // Need to get public key for encoding to client - options available :
        byte[] BYTE_KEY = rsa.ExportRSAPublicKey(); // Sure, but is byte[] good to transfer to client ?

        // The key does not need to be secret - string data easy to transfer :
        string public_rsa_key = rsa.ToXmlString(false); // retrieve and save/send public key // CryptoAsymKeysFrame does this in CTOR

        // ----------------------------------------------
        // Remote/Client Side - take our public key, make our AES keys, encrypt and send back
        string remote_key = public_rsa_key;

        // Remote Creates a secret !
        Aes remote_aes = Aes.Create();
        // This is done explicitly because it can be reused
        remote_aes.GenerateKey(); // Now I have a secret
        remote_aes.GenerateIV();

        // Send secret back to rsa sender... must make remote's rsa object
        RSACryptoServiceProvider remote_rsa = new RSACryptoServiceProvider();// empty is fine, it will be set by inserting public key
        remote_rsa.FromXmlString(remote_key); // use sent public key to "init" the rsa,

        byte[] remote_encrypted_key = rsa.Encrypt(remote_aes.Key, false);// encrypt, false determines padding type, false = PKCS, wider support
        byte[] remote_encrypted_iv = rsa.Encrypt(remote_aes.IV, false);// encrypt

        // somehow send encrypted data to the "dark-side", use copy below to simulate transfer, also used in CryptoFrames for transfer of byte[]
        byte[] encrypted_key = new byte[remote_encrypted_key.Length];
        Array.Copy(remote_encrypted_key, encrypted_key, remote_encrypted_key.Length);
        byte[] encrypted_iv = new byte[remote_encrypted_iv.Length];
        Array.Copy(remote_encrypted_iv, encrypted_iv, remote_encrypted_iv.Length);

        // ----------------------------------------------
        // Host/Server Side : Remote sent encrypted aes key/iv, decrypt and save
        // Got data, decrypt using rsa private key
        byte[] aes_key = rsa.Decrypt(encrypted_key, false);
        byte[] aes_iv = rsa.Decrypt(encrypted_iv, false);

        // Got keys, "install" them and send something back...
        // Now Both Host and Remote have an Aes synchronized ready to exchange
        Aes aes = Aes.Create();
        aes.Key = aes_key;
        aes.IV = aes_iv;

        // Key/IV set, now can encrypt and remote can decrypt
        // Simulate a serialization...
        string secretMsg = "Hello from Host";
        //byte[] bMsg = System.Text.Encoding.UTF8.GetBytes(sMsg);

        // Encrypt - Host or Remote side, now that aes key/iv has been exchanged
        byte[] encrypted_msg = []; // the encrypted message

        // Encrypt
        using (MemoryStream ms = new MemoryStream())
        {
          // Create the encryptor and the CryptoStream
          using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
          {
            // Write the plaintext to the CryptoStream via a StreamWriter
            using (StreamWriter sw = new StreamWriter(cs))
            {
              sw.Write(secretMsg);
            }
          }
          // Return the encrypted bytes from the base MemoryStream
          encrypted_msg = ms.ToArray();
        }


        // Decrypt - Remote/Client
        using (MemoryStream ms = new MemoryStream(encrypted_msg))
        {
          // Create the decryptor and the CryptoStream
          using (CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
          {
            // Read the decrypted data via a StreamReader
            using (StreamReader sr = new StreamReader(cs))
            {
              string remote_msg = sr.ReadToEnd();
              Console.WriteLine("Snap ! : Got secret message : " + remote_msg);
            }
          }
        }
        Console.ReadKey();
      }
    }
  }
}

