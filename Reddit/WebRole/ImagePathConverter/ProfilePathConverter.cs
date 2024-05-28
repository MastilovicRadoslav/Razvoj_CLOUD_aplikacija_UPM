using System.IO;

namespace WebRole.ImagePathConverter
{
    public class ProfilePathConverter
    {
        public string ReplacePath(string path)
        {
            string fileName = Path.GetFileName(path);
            string newFormat = $"/Images/Profile/{fileName}";
            return newFormat;
        }
    }
}