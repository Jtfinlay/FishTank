//
//  Copyright 2017 James Finlay
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;

namespace FishTank.Drawing
{
    /// <summary>
    /// Provides simple access to game resources such as fonts, strings, and textures.
    /// </summary>
    public class ContentBuilder
    {
        public static ContentBuilder Instance { get; private set; }

        public static ContentBuilder Instantiate(GraphicsDevice graphicsDevice, ContentManager content)
        {
            return Instance = new ContentBuilder(graphicsDevice, content);
        }

        /// <summary>
        /// Create a rectangular Texture2D with given asset name
        /// </summary>
        /// <param name="assetName">Key value to </param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Texture2D CreateRectangleTexture(string assetName, int width, int height)
        {
            Texture2D loadedAsset = null;
            _loadedTextures.TryGetValue(assetName, out loadedAsset);
            if (loadedAsset != null)
            {
                _loadedTextures.Remove(assetName);
            }

            loadedAsset = new Texture2D(_graphics, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.White;
            }
            loadedAsset.SetData(data);
            _loadedTextures.Add(assetName, loadedAsset);

            return loadedAsset;
        }

        public Texture2D LoadTextureByName(string assetName)
        {
            Texture2D loadedAsset = null;
            _loadedTextures.TryGetValue(assetName, out loadedAsset);
            if (loadedAsset == null)
            {
                try
                {
                    loadedAsset = _content.Load<Texture2D>(assetName);
                }
                catch (ContentLoadException)
                {
                    // If it doesn't exist, then load 'unknown' asset.
                    loadedAsset = _content.Load<Texture2D>(TextureNames.UnknownAsset);
                }
                _loadedTextures.Add(assetName, loadedAsset);
            }

            return loadedAsset;
        }

        public SpriteFont LoadFontByName(string fontName)
        {
            SpriteFont loadedFont = null;
            _loadedFonts.TryGetValue(fontName, out loadedFont);
            if (loadedFont == null)
            {
                loadedFont = _content.Load<SpriteFont>(fontName);
                _loadedFonts.Add(fontName, loadedFont);
            }

            return loadedFont;
        }

        public void UnloadContent()
        {
            _loadedTextures.Values.ToList().ForEach((resource) => resource.Dispose());
            Instance = null;
        }

        public string GetString(string resource)
        {
            return _resourceLoader.GetString(resource);
        }

        private ContentBuilder(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _graphics = graphicsDevice;
            _content = content;
            _loadedTextures = new Dictionary<string, Texture2D>();
            _loadedFonts = new Dictionary<string, SpriteFont>();
            _resourceLoader = new ResourceLoader();

            LoadTextureByName(TextureNames.UnknownAsset);
        }

        private GraphicsDevice _graphics;

        private ContentManager _content;

        private Dictionary<string, Texture2D> _loadedTextures;

        private Dictionary<string, SpriteFont> _loadedFonts;

        private ResourceLoader _resourceLoader;
    }
}
