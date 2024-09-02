using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartScreen
{
    public class TileBackend
    {
        public List<tileData> data = new List<tileData>();
        public List<tileData> userData = new List<tileData>();
        public void saveTile()
        {

        }
        public void addTile(string tileName, string programPath, tileSize size)
        {
            userData.Add(new tileData { Size = size, name = tileName, programPath = programPath, tilePosX = 0, tilePosY = 25565 });
        }
        public void loadAllTiles()
        {

        }
        public void getTile(tileData tileData)
        {
            foreach (var tile in data)
            {
                if (tile.name == tileData.name)
                {
                    // TODO: Do something when successfully got the specified tile
                }
            }
        }
        public void initDefaultTiles()
        {
            Logger.info("Initializing Default Tiles");
            data.Clear();
            data.Add(new tileData { Size = tileSize.Wide, name = "startScreen[specialTiles(desktop)];", programPath = "startScreen[hidefunc()];", tilePosX = 0, tilePosY = 0 });
            data.Add(new tileData { Size = tileSize.Wide, name = "Internet Explorer", programPath = "iexplore", tilePosX = 0, tilePosY = 1 });
            data.Add(new tileData { Size = tileSize.Wide, name = "Google", programPath = "http://google.com", tilePosX = 0, tilePosY = 1 });
            foreach(tileData tile in userData)
            {
                data.Add(tile);
            }
        }
        // Tile Data JSON Structure
        public class tileData
        {
            public tileSize Size { get; set; }
            public string name { get; set; }
            public string programPath { get; set; }
            public int tilePosX { get; set; }
            public int tilePosY { get; set; }
        }
        public enum tileSize
        {
            Small, // 0.5x0.5 rsmall
            Medium, // 1x1 small
            Wide, // 1x2 wide
            Large // 2x2 large
        }
    }
}
