using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;
using System.Linq;

static class WorldGenerator
{
    public static Color USSR_COLOR = Color.Red;
    public static Color YOURZISTAN = Color.Blue;

    public static void GenMap(int numberOfOtherCountries, int bigCountryThickness, double gridCellSize, ref GameObject[,] cells)
    {
        int MAP_WIDTH = cells.GetLength(0);
        int MAP_HEIGHT = cells.GetLength(1);

        // Grid corner vertices (with slight deform)
        Vector[,] gridPoints = new Vector[MAP_WIDTH + 1, MAP_HEIGHT + 1];
        for (int x = 0; x < MAP_WIDTH + 1; x++)
        {
            for (int y = 0; y < MAP_HEIGHT + 1; y++)
            {
                gridPoints[x, y] = new Vector(-gridCellSize * (MAP_WIDTH / 2) + x * gridCellSize, gridCellSize * (MAP_HEIGHT / 2) - y * gridCellSize)
                    + RandomGen.NextVector(gridCellSize / 5, gridCellSize / 3);
            }
        }

        IndexTriangle[] idxTriRect = new IndexTriangle[] { new IndexTriangle(0, 2, 1), new IndexTriangle(2, 0, 3) };
        for (int x = 0; x < MAP_WIDTH; x++)
        {
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                Vector cellPosition = (gridPoints[x, y] + gridPoints[x, y + 1] + gridPoints[x + 1, y] + gridPoints[x + 1, y + 1]) / 4;

                var polyshape = new Polygon(new ShapeCache(
                    new Vector[] { gridPoints[x, y] - cellPosition, gridPoints[x, y + 1] - cellPosition, gridPoints[x + 1, y + 1] - cellPosition, gridPoints[x + 1, y] - cellPosition }, // corners
                    idxTriRect// triangles
                ), false);
                var cell = new GameObject(gridCellSize, gridCellSize, polyshape);
                cell.Position = cellPosition;
                //cell.Color = RandomGen.NextColor();

                cells[x, y] = cell;
            }
        }

        int freeCellCnt = MAP_WIDTH * MAP_HEIGHT;
        // 1. Outside is USSR
        for (int x = 0; x < MAP_WIDTH; x++)
        {
            for (int y = 0; y < bigCountryThickness; y++)
            {
                cells[x, y].Color = USSR_COLOR; //TODO: Translucient
                //TODO: Set the contry to the tag 
                cells[x, MAP_HEIGHT - 1 - y].Color = USSR_COLOR; //TODO: Translucient
                freeCellCnt -= 2;
            }
        }
        for (int y = bigCountryThickness; y < MAP_HEIGHT - bigCountryThickness; y++)
        {
            for (int x = 0; x < bigCountryThickness; x++)
            {
                cells[x, y].Color = USSR_COLOR; //TODO: Translucient
                //TODO: Set the contry to the tag 
                cells[MAP_WIDTH - 1 - x, y].Color = USSR_COLOR; //TODO: Translucient
                freeCellCnt -= 2;
            }
        }

        // 2. You are at the center
        cells[MAP_WIDTH / 2, MAP_HEIGHT / 2].Color = YOURZISTAN;
        //TODO: Set tag to point to your country
        freeCellCnt--;

        // 3. Other countries are placed randomly
        for (int i = 0; i < numberOfOtherCountries; i++)
        {
            bool foundSpot = false;
            do
            {
                int x = RandomGen.NextInt(1, MAP_WIDTH - 2);
                int y = RandomGen.NextInt(1, MAP_HEIGHT - 2);
                if (cells[x, y].Color == Color.White)
                {
                    freeCellCnt--;
                    cells[x, y].Color = RandomGen.NextColor();
                    foundSpot = true;
                    //TODO: Set tag to point to a country
                }
            } while (!foundSpot);
        }

        // Evolve
        bool foundWhite = false;
        do
        {
            foundWhite = false;
            List<Tuple<int, int, Color>> toFill = new List<Tuple<int, int, Color>>();
            for (int x = 1; x < MAP_WIDTH - 1; x++)
            {
                for (int y = 1; y < MAP_HEIGHT - 1; y++)
                {
                    // Free to fill
                    if (cells[x, y].Color == Color.White)
                    {
                        foundWhite = true;
                        List<Color> nearbyColors = new List<Color>();
                        for (int dx = -1; dx < 2; dx++)
                        {
                            Color neighbourColor = cells[x + dx, y].Color;
                            if (neighbourColor != Color.White && (neighbourColor != USSR_COLOR || RandomGen.NextInt(10) == 1))
                                nearbyColors.Add(neighbourColor);
                        }
                        for (int dy = -1; dy < 2; dy++)
                        {
                            Color neighbourColor = cells[x, y + dy].Color;
                            if (neighbourColor != Color.White && (neighbourColor != USSR_COLOR || RandomGen.NextInt(10) == 1))
                                nearbyColors.Add(neighbourColor);
                        }
                        if (nearbyColors.Count > 0)
                        {
                            Color mostOccuring = nearbyColors.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
                            // Store into temp list so that for this round it is a level ground
                            toFill.Add(new Tuple<int, int, Color>(x, y, mostOccuring));

                        }
                    }
                }
            }
            foreach (var tf in toFill)
            {
                // Little bit of stochasticity
                if (RandomGen.NextInt(5) < 4)
                {
                    cells[tf.Item1, tf.Item2].Color = tf.Item3;
                    freeCellCnt--;
                }
            }
        } while (foundWhite);
    }

}
