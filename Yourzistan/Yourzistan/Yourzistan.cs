using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using System.Linq;
using DotFuzzy;

public class Yourzistan : Game
{
    readonly int MAP_WIDTH = 30;
    readonly int MAP_HEIGHT = 20;
    readonly int USSR_SIZE = 3;
    GameObject[,] cells;

    GameObject activeCell = null;
    GameObject activeBorder = null;
    Color activeCellOrigColor = Color.FromHexCode("DEFA97");

    public override void Begin()
    {
        // Kirjoita ohjelmakoodisi tähän

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

        Level.Background.Color = WorldGenerator.USSR_COLOR;
        Level.Background.Image = LoadImage("mappaper");
        Level.Size = Screen.Size;
        Level.Background.TileToLevel();

        double gridCellSize = Math.Min(Screen.Width / (MAP_WIDTH - USSR_SIZE), Screen.Height / (MAP_HEIGHT - USSR_SIZE));
        cells = new GameObject[MAP_WIDTH, MAP_HEIGHT];
        WorldGenerator.GenMap(5, USSR_SIZE, gridCellSize, ref cells);
        foreach (var cell in cells)
        {
            // Transparent
            cell.Color = new Color(cell.Color, 128); 
            Add(cell);
        }

        IsMouseVisible = true;

        Mouse.ListenMovement(0.01, OnMouseMove, "Move mouse and click a region");
        Mouse.Listen(MouseButton.Right, ButtonState.Down, OnContextMenuRequested, "Click right to make actions");
        Mouse.Listen(MouseButton.Left, ButtonState.Down, OnAreaInfoRequested, "Click left to make actions");
        Keyboard.Listen(Key.Space, ButtonState.Pressed, BuildCellModel, "DEBUG!");
    }

    void OnAreaInfoRequested()
    {
    
    }

    void OnContextMenuRequested()
    {
        if (activeCell != null || activeBorder!=null)
        {
            List<string> actions = new List<string>();
            if (activeCell != null)
            {
                // TODO: Slightly diferent actions for own/hostile cell.

                actions =  new List<string>{"Sabotage", "Invest", "Bribe", "Support", "Banking", "Propaganda"};
                // Invest -> industry (production), sentrifuges, chemlab, armybase
                // Bribe -> oppositing police, politicians, 
                // Support -> religion, organized crime, unions
                /* Actions:
                 * Build broadcast tower (propaganda) 
                 *  -> effect on yours, boost patriotism, 
                 *  -> effect on their, drop morale, rise jealousness
                */
            }
            else if (activeBorder!=null)
            {
                actions =  new List<string>{"Arbitage", "Import/export tariffs", "Border control", "Violation of borders"};
            }
            MultiSelectWindow alkuValikko = new MultiSelectWindow("Make an unconven-\ntional operation", actions.ToArray());
            double dx = Mouse.PositionOnScreen.X < 0 ? alkuValikko.Width / 2 : -alkuValikko.Width / 2;
            double dy = Mouse.PositionOnScreen.Y < 0 ? alkuValikko.Height / 2 : -alkuValikko.Width / 2;
            alkuValikko.Position = Mouse.PositionOnScreen + new Vector(dx, dy);

            /* double maxWidth = 0;
            foreach (var item in alkuValikko.Objects)
            {
                if (item.Width > maxWidth)
                {
                    maxWidth = item.Width;
                }
            }
            foreach (var item in alkuValikko.Objects)
            {
                item.Width = maxWidth;
            }*/
            
            Add(alkuValikko);
        }
    }

    void BuildCellModel()
    {
        LinguisticVariable nourishment = new LinguisticVariable("Nourishment");
        nourishment.MembershipFunctionCollection.Add(new MembershipFunction("Hungry", 0, 0, 20, 40));
        nourishment.MembershipFunctionCollection.Add(new MembershipFunction("Nominal", 30, 50, 50, 70));
        nourishment.MembershipFunctionCollection.Add(new MembershipFunction("Bloated", 50, 80, 100, 100));
        
        LinguisticVariable peacefulness = new LinguisticVariable("Peacefulness");
        peacefulness.MembershipFunctionCollection.Add(new MembershipFunction("Low", 0, 0, 25, 75));
        peacefulness.MembershipFunctionCollection.Add(new MembershipFunction("High", 25, 75, 100, 100));

        LinguisticVariable mood = new LinguisticVariable("Mood");
        mood.MembershipFunctionCollection.Add(new MembershipFunction("Angry", 0, 0, 10, 20));
        mood.MembershipFunctionCollection.Add(new MembershipFunction("Pissed", 10, 20, 40, 50));
        mood.MembershipFunctionCollection.Add(new MembershipFunction("OK", 40, 50, 70, 80));
        mood.MembershipFunctionCollection.Add(new MembershipFunction("Happy", 70, 80, 100, 100));

        FuzzyEngine fuzzyEngine = new FuzzyEngine();
        fuzzyEngine.LinguisticVariableCollection.Add(nourishment);
        fuzzyEngine.LinguisticVariableCollection.Add(peacefulness);
        fuzzyEngine.LinguisticVariableCollection.Add(mood);

        fuzzyEngine.Consequent = "Mood";
        
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF (Nourishment IS Bloated) AND (Peacefulness IS Low) THEN Mood IS Pissed"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF (Nourishment IS Bloated) AND (Peacefulness IS High) THEN Mood IS OK"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF (Nourishment IS Nominal) AND (Peacefulness IS Low) THEN Mood IS Angry"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF (Nourishment IS Nominal) AND (Peacefulness IS High) THEN Mood IS Happy"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF (Nourishment IS Hungry) THEN Mood IS Angry"));

        nourishment.InputValue = 5;
        peacefulness.InputValue = 80;

        double angryness = fuzzyEngine.Defuzzify();
        mood.InputValue = angryness;
        MessageDisplay.Add(mood.Fuzzify() + " (" + angryness.ToString() +")");
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
