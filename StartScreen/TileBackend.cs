using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartScreen
{
    class TileBackend
    {
        public List<tileData> data = new List<tileData>();
        public void saveTile()
        {

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
            data.Add(new tileData { Size = tileSize.wide, name = "startScreen[specialTiles(desktop)];", programPath = "startScreen[hidefunc()];", tilePosX = 0, tilePosY = 0 });
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
            rsmall, // 0.5x0.5
            small, // 1x1
            wide, // 1x2
            large // 2x2
        }
    }
}
