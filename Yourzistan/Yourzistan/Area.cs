using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Jypeli;
using DotFuzzy;

class Area
{
    // Behind
    public double Wealth { get; set; }
    public double Police { get; set; }
    public double Heterogenity { get; set; }
    public double Pollution { get; set; }
    public double Corruption { get; set; }

    // Mid
    public double Infrastructure { get; set; }
    public double HealthServices { get; set; }
    public double Crime { get; set; }
    public double Religion { get; set; }
    public double Food { get; set; }
    public double Cleanness { get; set; }
    public double Employment { get; set; }
    public double Information { get; set; }

    // Mood
    public double Nourishment { get; set; }
    public double Health { get; set; }
    public double Shelter { get; set; }
    public double Trust { get; set; }
    public double Safety { get; set; }

    static public string BuildCellModel()
    {
        LinguisticVariable nourishment = new LinguisticVariable("Nourishment");
        nourishment.MembershipFunctionCollection.Add(new MembershipFunction("Hungry", 0, 0, 20, 40));
        nourishment.MembershipFunctionCollection.Add(new MembershipFunction("Nominal", 30, 50, 50, 70));
        nourishment.MembershipFunctionCollection.Add(new MembershipFunction("Bloated", 50, 80, 100, 100));

        LinguisticVariable health = new LinguisticVariable("Health");
        health.MembershipFunctionCollection.Add(new MembershipFunction("Sick", 0, 0, 10, 20));
        health.MembershipFunctionCollection.Add(new MembershipFunction("Functional", 10, 20, 40, 50));
        health.MembershipFunctionCollection.Add(new MembershipFunction("Healthy", 40, 50, 70, 80));
        health.MembershipFunctionCollection.Add(new MembershipFunction("Sporty", 70, 80, 100, 100));

        LinguisticVariable shelter = new LinguisticVariable("Shelter");
        shelter.MembershipFunctionCollection.Add(new MembershipFunction("Shacks", 0, 0, 10, 20));
        shelter.MembershipFunctionCollection.Add(new MembershipFunction("Barebones", 10, 20, 40, 50));
        shelter.MembershipFunctionCollection.Add(new MembershipFunction("Confortable", 40, 50, 70, 80));
        shelter.MembershipFunctionCollection.Add(new MembershipFunction("Luxorious", 70, 80, 100, 100));

        LinguisticVariable trust = new LinguisticVariable("Trust");
        trust.MembershipFunctionCollection.Add(new MembershipFunction("Hate", 0, 0, 20, 40));
        trust.MembershipFunctionCollection.Add(new MembershipFunction("Meh", 30, 50, 50, 70));
        trust.MembershipFunctionCollection.Add(new MembershipFunction("Awe", 50, 80, 100, 100));

        LinguisticVariable mood = new LinguisticVariable("Mood");
        mood.MembershipFunctionCollection.Add(new MembershipFunction("Angry", 0, 0, 10, 20));
        mood.MembershipFunctionCollection.Add(new MembershipFunction("Pissed", 10, 20, 40, 50));
        mood.MembershipFunctionCollection.Add(new MembershipFunction("OK", 40, 50, 70, 80));
        mood.MembershipFunctionCollection.Add(new MembershipFunction("Happy", 70, 80, 100, 100));

        FuzzyEngine fuzzyEngine = new FuzzyEngine();
        fuzzyEngine.LinguisticVariableCollection.Add(nourishment);
        //fuzzyEngine.LinguisticVariableCollection.Add(peacefulness);
        fuzzyEngine.LinguisticVariableCollection.Add(mood);

        fuzzyEngine.Consequent = "Mood";

        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF (Nourishment IS Bloated) AND (Peacefulness IS Low) THEN Mood IS Pissed"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF (Nourishment IS Bloated) AND (Peacefulness IS High) THEN Mood IS OK"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF (Nourishment IS Nominal) AND (Peacefulness IS Low) THEN Mood IS Angry"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF (Nourishment IS Nominal) AND (Peacefulness IS High) THEN Mood IS Happy"));
        fuzzyEngine.FuzzyRuleCollection.Add(new FuzzyRule("IF (Nourishment IS Hungry) THEN Mood IS Angry"));

        nourishment.InputValue = 5;
        //peacefulness.InputValue = 80;

        double angryness = fuzzyEngine.Defuzzify();
        mood.InputValue = angryness;
        return mood.Fuzzify() + " (" + angryness.ToString() + ")";
    }

    // Init with every property at 50 with in built randomness (uniform)
    public Area(double randomnessRange)
    {
        /* TODO: Using reflection to set properties with PropertyInfo.SetValue does not work in constructor?
        // Lazy init with reflection
        foreach (PropertyInfo propertyInfo in typeof(Area).GetProperties())
        {
            double value = 50.0 + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
            propertyInfo.SetValue(this, (object)value, null);
        }*/

        // IDIOTIC WAY OF SETTING THESE WITH CUT/COPY/PASTE
        Wealth = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Police = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Heterogenity = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Pollution = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Corruption = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Infrastructure = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        HealthServices = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Crime = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Religion = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Food = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Cleanness = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Employment = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Information = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Nourishment = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Health = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Shelter = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Trust = 50.0 + RandomGen.NextDouble(-randomnessRange/2, randomnessRange/2);
        Safety = 50.0 + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
    }

    public Area(Area parent, double randomnessRange)
    {
        Wealth = parent.Wealth + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Police = parent.Police + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Heterogenity = parent.Heterogenity + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Pollution = parent.Pollution + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Corruption = parent.Corruption + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Infrastructure = parent.Infrastructure + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        HealthServices = parent.HealthServices + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Crime = parent.Crime + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Religion = parent.Religion + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Food = parent.Food + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Cleanness = parent.Cleanness + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Employment = parent.Employment + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Information = parent.Information + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Nourishment = parent.Nourishment + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Health = parent.Health + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Shelter = parent.Shelter + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Trust = parent.Trust + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
        Safety = parent.Safety + RandomGen.NextDouble(-randomnessRange / 2, randomnessRange / 2);
    }

    double Similarity(Area other)
    {
        // Weighted similarity
        double similarity = ((this.Wealth - other.Wealth) * 3 +
            (this.Police - other.Police) * 3 +
            (this.Heterogenity - other.Heterogenity) * 3 +
            (this.Pollution - other.Pollution) * 3 +
            (this.Corruption - other.Corruption) * 3 +

            (this.Infrastructure - other.Infrastructure) * 2 +
            (this.HealthServices - other.HealthServices) * 2 +
            (this.Crime - other.Crime) * 2 +
            (this.Religion - other.Religion) * 2 +
            (this.Food - other.Food) * 2 +
            (this.Cleanness - other.Cleanness) * 2 +
            (this.Employment - other.Employment) * 2 +
            (this.Information - other.Information) * 2 +

            (this.Nourishment - other.Nourishment) * 1 +
            (this.Health - other.Health) * 1 +
            (this.Shelter - other.Shelter) * 1 +
            (this.Trust - other.Trust) * 1 +
            (this.Safety - other.Safety) * 1)
                / (3 * 5 + 2 * 8 + 1 * 5);
        return similarity;
    }

    public override string ToString()
    {
        return "Wealth=" + Wealth.ToString() + "\n" +
            "Wealth=" + Police.ToString() + "\n" +
            "Wealth=" + Heterogenity.ToString() + "\n" +
            "Wealth=" + Pollution.ToString() + "\n" +
            "Wealth=" + Corruption.ToString() + "\n\n" +

            "Wealth=" + Infrastructure.ToString() + "\n" +
            "Wealth=" + HealthServices.ToString() + "\n" +
            "Wealth=" + Crime.ToString() + "\n" +
            "Wealth=" + Religion.ToString() + "\n" +
            "Wealth=" + Food.ToString() + "\n" +
            "Wealth=" + Cleanness.ToString() + "\n" +
            "Wealth=" + Employment.ToString() + "\n" +
            "Wealth=" + Information.ToString() + "\n\n" +

            "Wealth=" + Nourishment.ToString() + "\n" +
            "Wealth=" + Health.ToString() + "\n" +
            "Wealth=" + Shelter.ToString() + "\n" +
            "Wealth=" + Trust.ToString() + "\n" +
            "Wealth=" + Safety.ToString();
    }

    public void Update(double amount)
    {
        // TODO: Use fuzzy model to update the Mid and Mood properties
    }
}

