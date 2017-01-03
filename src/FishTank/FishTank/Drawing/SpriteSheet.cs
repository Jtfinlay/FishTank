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

namespace FishTank.Drawing
{
    /// <summary>
    /// Describes a handler for tiled multi-asset <see cref="SpriteSheet"/>s. Used when an image contains multiple
    /// assets within it to show different states or <see cref="Animation"/>.
    /// </summary>
    public class SpriteSheet
    {
        /// <summary>
        /// Path to the tiled multi-asset <see cref="SpriteSheet"/> image.
        /// </summary>
        public string AssetName { get; private set; }

        /// <summary>
        /// The width & height of the individual tiles in the <see cref="SpriteSheet"/> image.
        /// </summary>
        public Point TileSize { get; private set; }

        /// <summary>
        /// The width of the individual tiles in the <see cref="SpriteSheet"/> image.
        /// </summary>
        public int TileWidth => TileSize.X;

        /// <summary>
        /// The height of the individual tiles in the <see cref="SpriteSheet"/> image.
        /// </summary>
        public int TileHeight => TileSize.Y;

        /// <summary>
        /// The location of the first tile in the <see cref="SpriteSheet"/> image, as a source <see cref="Rectangle"/>.
        /// </summary>
        public Rectangle DefaultTile => new Rectangle(Point.Zero, TileSize);

        /// <summary>
        /// Creates a new instance of the <see cref="SpriteSheet"/> with the given image path and tile size.
        /// </summary>
        /// <param name="asset">Path to the tiled multi-asset <see cref="SpriteSheet"/> image.</param>
        /// <param name="tileSize">Width & height of the individual tiels in the <see cref="SpriteSheet"/> image.</param>
        public SpriteSheet(string asset, Point tileSize)
        {
            AssetName = asset;
            TileSize = tileSize;
        }
    }
}
