using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ScriptTest.Gui
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RunSimulation_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                // 1) Read settings
                var settings = new ScriptTest.SimulationSettings
                {
                    NoiseRate = ReadDouble(NoiseRateBox.Text, 0, 1, "Noise Rate"),
                    MutationRate = ReadDouble(MutationRateBox.Text, 0, 1, "Mutation Rate"),
                    RoundsPerMatch = ReadInt(RoundsBox.Text, 1, 1000, "Rounds per match"),
                };

                // 2) Read counts
                var counts = new ScriptTest.AgentCounts
                {
                    AlwaysCooperate = ReadInt(AlwaysCooperateBox.Text, 0, int.MaxValue, "Always Cooperate"),
                    AlwaysDefect = ReadInt(AlwaysDefectBox.Text, 0, int.MaxValue, "Always Defect"),
                    TitForTat = ReadInt(TitForTatBox.Text, 0, int.MaxValue, "Tit For Tat"),
                    TitForTwoTats = ReadInt(TitForTwoTatsBox.Text, 0, int.MaxValue, "Tit For Two Tats"),
                    Grim = ReadInt(GrimBox.Text, 0, int.MaxValue, "Grim"),
                    Random = ReadInt(RandomBox.Text, 0, int.MaxValue, "Random"),
                    GenerousTitForTat = ReadInt(GenerousTitForTatBox.Text, 0, int.MaxValue, "Generous Tit For Tat"),
                };

                // 3) Run your existing simulation engine
                //    (These classes must exist in your project or referenced core library.)
                var generator = new AgentGenerator();
                var agents = generator.GenerateFromCounts(counts);

                var engine = new EvolutionEngine(generator, settings);
                var (winner, generations) = engine.Run(agents);

                // 4) Show result
                ResultText.Text =
                    $"Winner: {winner.Name}\n" +
                    $"Generations: {generations}\n" +
                    $"Noise: {settings.NoiseRate}, Mutation: {settings.MutationRate}, Rounds: {settings.RoundsPerMatch}";
            }
            catch (Exception ex)
            {
                ResultText.Text = "Error: " + ex.Message;
            }
        }

        private static int ReadInt(string? text, int min, int max, string label)
        {
            if (!int.TryParse(text, out int value))
                throw new Exception($"{label} must be an integer.");

            if (value < min || value > max)
                throw new Exception($"{label} must be between {min} and {max}.");

            return value;
        }

        private static double ReadDouble(string? text, double min, double max, string label)
        {
            if (!double.TryParse(text, out double value))
                throw new Exception($"{label} must be a number.");

            if (value < min || value > max)
                throw new Exception($"{label} must be between {min} and {max}.");

            return value;
        }
    }
}
