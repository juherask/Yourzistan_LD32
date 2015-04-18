using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;
using System.Linq;

public class Yourzistan : Game
{

    readonly int MAP_WIDTH = 30;
    readonly int MAP_HEIGHT = 20;
    GameObject[,] cells;

    GameObject activeCell = null;
    Color activeCellOrigColor = Color.Transparent;

    /* Actions:
     * Build broadcast tower (propaganda) 
     *  -> effect on yours, boost patriotism, 
     *  -> effect on their, drop morale, rise jealousness
    */

    public override void Begin()
    {
        // Kirjoita ohjelmakoodisi tähän

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

        Level.Background.Color = WorldGenerator.USSR_COLOR;

        double gridCellSize = Math.Min(Screen.Width / (MAP_WIDTH + 2), Screen.Height / (MAP_HEIGHT + 2));
        cells = new GameObject[MAP_WIDTH, MAP_HEIGHT];
        WorldGenerator.GenMap(5, gridCellSize, ref cells);
        foreach (var cell in cells)
        {
            Add(cell);
        }

        IsMouseVisible = true;

        Mouse.ListenMovement(0.01, OnMouseMove, "Move mouse and click a region");
    }

  

    void OnMouseMove(AnalogState anaSt)
    {
        GameObject hoverObject = GetObjectAt(Mouse.PositionOnScreen);

        if (activeCell != null && hoverObject != activeCell)
        {
            activeCell.Color = activeCellOrigColor;
            activeCell = null;
        }

        if (activeCell==null && hoverObject != null)
        {
            activeCellOrigColor = hoverObject.Color;
            activeCell = hoverObject;
            hoverObject.Color = Color.Lighter(hoverObject.Color, 100);
        }
    }
}
