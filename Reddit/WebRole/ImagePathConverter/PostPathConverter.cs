using System.IO;

namespace WebRole.ImagePathConverter
{
    public class PostPathConverter
    {
        public string ReplacePath(string path)
        {
            string fileName = Path.GetFileName(path);
            string newFormat = $"/Images/Posts/{fileName}";
            return newFormat;
        }
    }
}