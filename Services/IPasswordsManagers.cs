using Usuarios.Models;

namespace Usuarios.Services
{
    public interface IPasswordsManagers
    {

        public string DecryptText(string password);

        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes);

        public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes);

        public string EncryptText(string password);

        public string GenerateToken(UserLogin user);
    }
}
