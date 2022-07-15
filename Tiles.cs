using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;

namespace Swamp
{
    class Tile : Drawable
    {
        public enum Type : byte
        {
            Static,
            Water,
            Frog,
        }

        private Sprite sprite;
        private Texture currentTexture;
        private IntRect currentTextureRect;
        private Vector2f currentPosition;
        private int frame = 0;

        public const int tileSize = 128;

        delegate void prepareTextureRect();
        event prepareTextureRect prepareTextureRectNotify;

        public Tile(Type type, string tilesetFileName, int row, int column = 0)
        {
            switch (type)
            {
                case Type.Static:
                    currentTexture = new Texture(tilesetFileName);
                    currentTextureRect = new IntRect(tileSize * column, tileSize * row, tileSize, tileSize);
                    currentPosition = new Vector2f(0f, 0f);
                    break;

                case Type.Water:
                    prepareTextureRectNotify += prepareTextureRect_Water;
                    currentTexture = new Texture(tilesetFileName);
                    currentTextureRect = new IntRect(0, tileSize * row, tileSize, tileSize);
                    currentPosition = new Vector2f(0f, 0f);
                    break;

                case Type.Frog:
                    prepareTextureRectNotify += prepareTextureRect_Frog;
                    currentTexture = new Texture(tilesetFileName);
                    currentTextureRect = new IntRect(tileSize * column, tileSize * row, tileSize, tileSize);
                    currentPosition = new Vector2f(0f, 0f);

                    frame = Program.random.Next() % 120;
                    break;

                default:
                    throw new Exception("No Tile.Type");
            }
            
            sprite = new Sprite();
            sprite.Texture = currentTexture;
            sprite.TextureRect = currentTextureRect;
            sprite.Position = currentPosition;
        }

        ~Tile()
        {

        }

        void Drawable.Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(sprite, states);
            if (prepareTextureRectNotify != null) prepareTextureRectNotify();
        }

        void prepareTextureRect_Water()
        {
            frame++;
            int currentTile = frame / 60;
            if (currentTile > 3)
            {
                frame = 0;
                currentTile = 0;
            }
            currentTextureRect.Left = currentTile * tileSize;
            sprite.TextureRect = currentTextureRect;
        }

        void prepareTextureRect_Frog()
        {
            frame++;
            int currentTile = frame / 30;
            if (currentTile > 4)
            {
                frame = 0;
                currentTile = 0;
            }
            currentTextureRect.Left = currentTile * tileSize;
            sprite.TextureRect = currentTextureRect;
        }

        public void setPosition(int x, int y)
        {
            currentPosition.X = x;
            currentPosition.Y = y;
            sprite.Position = currentPosition;
        }

        public enum Rotate : int
        {
            d0 = 0,
            d90 = 90,
            d180 = 180,
            d270 = 270,
        }
        public void setRotate(Rotate rotate)
        {
            switch (rotate)
            {
                case Rotate.d0:
                    sprite.Origin = new Vector2f(tileSize, 0f);
                    break;

                case Rotate.d90:
                    sprite.Origin = new Vector2f(0f, tileSize);
                    break;

                case Rotate.d180:
                    sprite.Origin = new Vector2f(tileSize, tileSize);
                    break;

                case Rotate.d270:
                    sprite.Origin = new Vector2f(tileSize, 0f);
                    break;

                default:
                    throw new Exception("No Tile.Rotate");
            }
            
            sprite.Rotation = (float)rotate;
        }
    }
}
