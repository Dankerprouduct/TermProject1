using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermProject1.World
{
    public class TextureLoader
    {
        public static TextureLoader Instance;
        public Dictionary<int, string> TextureDictionary = new Dictionary<int, string>();
        public TextureLoader()
        {
            Instance = this; 

            TextureDictionary.Add(0, "isometric_pixel_0201");
            TextureDictionary.Add(1, "isometric_pixel_0036");
        }
    }
}
