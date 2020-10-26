using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Component;
using Loki2D.Core.Utilities;
using Microsoft.Xna.Framework;

namespace TermProject1.World
{
    public class Animation
    {
        private List<string> TextureNames = new List<string>();
        private int _currentIndex { get; set; } = 0; 
        private float FrameDelay { get; set; }
        private Timer _timer;
        private RenderComponent _renderComponent; 

        public Animation(RenderComponent renderComponent, List<string> names,  float time = 250)
        {
            _renderComponent = renderComponent; 
            FrameDelay = time;
            TextureNames = names;

            _timer = new Timer(time);

            _renderComponent.TextureName = TextureNames[_currentIndex];
        }

        public void Update(GameTime gameTime)
        {
            if (_timer.Update(gameTime.ElapsedGameTime.Milliseconds))
            {
                _renderComponent.TextureName = TextureNames[_currentIndex];
                _currentIndex++;
                if (_currentIndex >= TextureNames.Count)
                {
                    _currentIndex = 0; 
                }
            }
        }
    }
}
