//
//  Copyright 2016 James Finlay
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

using FishTank.Components;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FishTank.Screens
{
    public abstract class Screen : Component
    {
        public new Rectangle Area { get; protected set; } = Constants.VirtualArea;

        public event EventHandler<NavigationEventArgs> OnNavigate;

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewMatrix);

        public abstract void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);

        public override void LoadContent()
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(gameTime, spriteBatch, new Matrix());
        }

        protected void Navigate(NavigationEventArgs eventArgs)
        {
            OnNavigate?.Invoke(this, eventArgs);
        }
    }
}
