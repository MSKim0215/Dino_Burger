using System.IO;
using UnityEngine;

namespace MSKim.Manager
{
    public class FileManager : BaseManager
    {
        private string path;
        private string fileName = "/save";
        private string keyword = "sjahfiwpncvp!#$%*%! !#$";

        public override void Initialize()
        {
            base.Initialize();

            path = Application.persistentDataPath + fileName;
        }

        public PlayerData Load()
        {
            if (!File.Exists(path))
            {
                var data = new PlayerData();
                data.Initialize();
                Save(data);
            }

            return JsonUtility.FromJson<PlayerData>(EncryptAndDecrypt(File.ReadAllText(path)));
        }

        public void Save(PlayerData playerData)
        {
            File.WriteAllText(path, EncryptAndDecrypt(JsonUtility.ToJson(playerData)));
        }

        private string EncryptAndDecrypt(string data)
        {
            var result = string.Empty;

            for (int i = 0; i < data.Length; i++)
            {
                result += (char)(data[i] ^ keyword[i % keyword.Length]);
            }

            return result;
        }
    }
}