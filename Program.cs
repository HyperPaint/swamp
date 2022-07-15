using System;
using System.Collections.Generic;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;

namespace Swamp
{
    class Program
    {
        public static Random random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            List<VideoMode> videoModes = new List<VideoMode>();
            foreach (var item in VideoMode.FullscreenModes)
            {
                if (item.BitsPerPixel == 32)
                {
                    videoModes.Insert(0, item);
                }
            }
            // консоль
            foreach (var item in videoModes)
            {
                Console.WriteLine(item.Width + "x" + item.Height);
            }

            RenderWindow renderWindow = new RenderWindow(videoModes[7], "My Swamp");
            renderWindow.Closed += (object sender, EventArgs e) => renderWindow.Close();
            renderWindow.SetFramerateLimit(60);

            // всего плиток
            int tilesHorizontal = (int)renderWindow.Size.X / Tile.tileSize;
            int tilesVertical = (int)renderWindow.Size.Y / Tile.tileSize;

            const string tileSet = "tileset.png";

            // вспомогательная переменная
            Tile currentTile;

            // вода
            Tile[,] waterTiles = new Tile[tilesVertical, tilesHorizontal];
            for (int y = 0; y < tilesVertical; y++)
            {
                for (int x = 0; x < tilesHorizontal; x++)
                {
                    currentTile = new Tile(Tile.Type.Water, tileSet, 0, 0);
                    currentTile.setPosition(Tile.tileSize * x, Tile.tileSize * y);
                    waterTiles[y, x] = currentTile;
                }
            }

            // земля
            Tile[,] earthTiles = new Tile[tilesVertical, tilesHorizontal];
            for (int y = 0; y < tilesVertical; y++)
            {
                currentTile = new Tile(Tile.Type.Static, tileSet, 1, 0);
                currentTile.setPosition(0, Tile.tileSize * y);
                earthTiles[y, 0] = currentTile;
            }
            // верх и низ берег
            for (int x = 2; x < tilesHorizontal; x++)
            {
                currentTile = new Tile(Tile.Type.Static, tileSet, 1, 2);
                currentTile.setPosition(Tile.tileSize * x, 0);
                earthTiles[0, x] = currentTile;
                currentTile = new Tile(Tile.Type.Static, tileSet, 1, 2);
                currentTile.setPosition(Tile.tileSize * x, Tile.tileSize * (tilesVertical - 1));
                currentTile.setRotate(Tile.Rotate.d180);
                earthTiles[tilesVertical - 1, x] = currentTile;
            }
            // левый берег
            for (int y = 1; y < tilesVertical - 1; y++)
            {
                currentTile = new Tile(Tile.Type.Static, tileSet, 1, 2);
                currentTile.setPosition(Tile.tileSize, Tile.tileSize * y);
                currentTile.setRotate(Tile.Rotate.d270);
                earthTiles[y, 1] = currentTile;
            }
            // уголки
            currentTile = new Tile(Tile.Type.Static, tileSet, 1, 1);
            currentTile.setPosition(Tile.tileSize, 0);
            earthTiles[0, 1] = currentTile;
            currentTile = new Tile(Tile.Type.Static, tileSet, 1, 1);
            currentTile.setPosition(Tile.tileSize, Tile.tileSize * (tilesVertical - 1));
            currentTile.setRotate(Tile.Rotate.d270);
            earthTiles[tilesVertical - 1, 1] = currentTile;

            // кувшинки и лягушки
            Tile[,] personTiles = new Tile[tilesVertical, tilesHorizontal];
            int x_, y_;
            x_ = 3; y_ = 1;
            currentTile = new Tile(Tile.Type.Static, tileSet, 2, 0);
            currentTile.setPosition(Tile.tileSize * x_, Tile.tileSize * y_);
            earthTiles[y_, x_] = currentTile;
            currentTile = new Tile(Tile.Type.Frog, tileSet, 3);
            currentTile.setPosition(Tile.tileSize * x_, Tile.tileSize * y_);
            personTiles[y_, x_] = currentTile;
            x_ = 4; y_ = 3;
            currentTile = new Tile(Tile.Type.Static, tileSet, 2, 0);
            currentTile.setPosition(Tile.tileSize * x_, Tile.tileSize * y_);
            currentTile.setRotate(Tile.Rotate.d90);
            earthTiles[y_, x_] = currentTile;
            currentTile = new Tile(Tile.Type.Frog, tileSet, 4);
            currentTile.setPosition(Tile.tileSize * x_, Tile.tileSize * y_);
            personTiles[y_, x_] = currentTile;
            x_ = 6; y_ = 2;
            currentTile = new Tile(Tile.Type.Static, tileSet, 2, 0);
            currentTile.setPosition(Tile.tileSize * x_, Tile.tileSize * y_);
            currentTile.setRotate(Tile.Rotate.d270);
            earthTiles[y_, x_] = currentTile;
            currentTile = new Tile(Tile.Type.Frog, tileSet, 5);
            currentTile.setPosition(Tile.tileSize * x_, Tile.tileSize * y_);
            personTiles[y_, x_] = currentTile;
            x_ = 8; y_ = 3;
            currentTile = new Tile(Tile.Type.Static, tileSet, 2, 0);
            currentTile.setPosition(Tile.tileSize * x_, Tile.tileSize * y_);
            currentTile.setRotate(Tile.Rotate.d180);
            earthTiles[y_, x_] = currentTile;
            currentTile = new Tile(Tile.Type.Frog, tileSet, 6);
            currentTile.setPosition(Tile.tileSize * x_, Tile.tileSize * y_);
            personTiles[y_, x_] = currentTile;
            // одна лягушка на берегу
            x_ = 0; y_ = 3;
            currentTile = new Tile(Tile.Type.Frog, tileSet, 7);
            currentTile.setPosition(Tile.tileSize * x_, Tile.tileSize * y_);
            personTiles[y_, x_] = currentTile;

            // главный цикл
            while (renderWindow.IsOpen)
            {
                renderWindow.DispatchEvents();

                renderWindow.Clear();
                for (int y = 0; y < tilesVertical; y++)
                {
                    for (int x = 0; x < tilesHorizontal; x++)
                    {
                        if (waterTiles[y,x] != null)
                        {
                            renderWindow.Draw(waterTiles[y, x]);
                        }
                        if (earthTiles[y, x] != null)
                        {
                            renderWindow.Draw(earthTiles[y, x]);
                        }
                        if (personTiles[y, x] != null)
                        {
                            renderWindow.Draw(personTiles[y, x]);
                        }
                    }
                }
                renderWindow.Display();
            }
        }
    }
}
