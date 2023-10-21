// ****************************************************************************
// * The MIT License(MIT)
// * Copyright © 2020 Thomas Due
// *
// * Permission is hereby granted, free of charge, to any person obtaining a
// * copy of this software and associated documentation files (the “Software”),
// * to deal in the Software without restriction, including without limitation
// * the rights to use, copy, modify, merge, publish, distribute, sublicense,
// * and/or sell copies of the Software, and to permit persons to whom the
// * Software is furnished to do so, subject to the following conditions:
// *
// * The above copyright notice and this permission notice shall be included in
// * all copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS
// * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// * IN THE SOFTWARE.
// ****************************************************************************

using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace ConanServerSwitcher.Services;

/// <summary>A static helper class for encrypting and decrypt text strings. </summary>
internal static class EncryptionHelper
{
    /// <summary>
    /// </summary>
    private static readonly byte[] RgbIv =  {
                0xAA, 0xE7, 0xE4, 0xDD, 0x73, 0x2D, 0x3B, 0x0D,
                0xAF, 0x7D, 0xA4, 0x66, 0x99, 0x11, 0xE1, 0x45
        };

    /// <summary>
    /// </summary>
    private static readonly byte[] RgbKey = {
                0xE6, 0xE7, 0xB7, 0x32, 0xDF, 0x03, 0x45, 0x00,
                0x84, 0x51, 0x51, 0x47, 0x4C, 0x84, 0x33, 0x9E,
                0x09, 0x10, 0x49, 0x41, 0xB8, 0x09, 0x13, 0xF4,
                0x43, 0x5A, 0x08, 0xF4, 0xBE, 0xAA, 0x08, 0x64 };

    /// <summary>Decrypts the specified <paramref name="password"/>.</summary>
    /// <param name="password">The password.</param>
    /// <exception cref="ArgumentNullException">password</exception>
    internal static string Decrypt(string password)
    {
        if (password == null)
        {
            throw new ArgumentNullException(nameof(password));
        }

        var cipherBytes = Convert.FromBase64String(password);

        using (var alg = new RijndaelManaged())
        {
            using (var decrypt = alg.CreateDecryptor(RgbKey, RgbIv))
            {
                using (var ms = new MemoryStream(cipherBytes))
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cs))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    /// <summary>Encrypts the specified <paramref name="password"/>.</summary>
    /// <param name="password">The password.</param>
    /// <exception cref="ArgumentNullException">password</exception>
    internal static string Encrypt(string password)
    {
        if (password == null)
        {
            throw new ArgumentNullException(nameof(password));
        }

        byte[] encrypted;

        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            rijAlg.Key = RgbKey;
            rijAlg.IV = RgbIv;

            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(password);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        return Convert.ToBase64String(encrypted);
    }


    static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
        {
            throw new ArgumentNullException(nameof(plainText));
        }

        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException(nameof(Key));
        }

        if (IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException(nameof(IV));
        }

        byte[] encrypted;

        // Create an RijndaelManaged object with the specified key and IV.
        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an RijndaelManaged object
        // with the specified key and IV.
        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }
}