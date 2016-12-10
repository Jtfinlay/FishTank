//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace FishTank.Content
{
    public class ContentBuilder
    {
        public static ContentBuilder Instance { get; private set; }

        public static ContentBuilder Instantiate(GraphicsDevice graphicsDevice, ContentManager content)
        {
            return Instance = new ContentBuilder(graphicsDevice, content);
        }

        private ContentBuilder(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _graphics = graphicsDevice;
            _content = content;
            _loadedTextures = new Dictionary<string, Texture2D>();
        }

        public Texture2D CreateRectangleTexture(string assetName, int width, int height)
        {
            Texture2D loadedAsset = null;
            _loadedTextures.TryGetValue(assetName, out loadedAsset);
            if (loadedAsset == null)
            {
                loadedAsset = new Texture2D(_graphics, width, height);
                Color[] data = new Color[width * height];
                for (int i = 0; i < data.Length; ++i)
                {
                    data[i] = Color.White;
                }
                loadedAsset.SetData(data);
                _loadedTextures.Add(assetName, loadedAsset);
            }

            return loadedAsset;
        }

        public Texture2D LoadTextureByName(string assetName)
        {
            Texture2D loadedAsset = null;
            _loadedTextures.TryGetValue(assetName, out loadedAsset);
            if (loadedAsset == null)
            {
                loadedAsset = _content.Load<Texture2D>(assetName);
                _loadedTextures.Add(assetName, loadedAsset);
            }

            return loadedAsset;
        }

        public void UnloadContent()
        {
            _loadedTextures.Values.ToList().ForEach((resource) => resource.Dispose());
            Instance = null;
        }

        private GraphicsDevice _graphics;

        private ContentManager _content;

        private Dictionary<string, Texture2D> _loadedTextures;
    }
}
