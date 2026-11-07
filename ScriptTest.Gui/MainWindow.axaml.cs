using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ScriptTest.Core;
using System.Text.Json;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace ScriptTest.Gui
{
    public partial class MainWindow : Window
    {

        private List<Dictionary<string, int>>? _latestHistory;
        public MainWindow()
        {
            InitializeComponent();
        }


        // Run simulation


     
        private void RunSimulation_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var settings = new SimulationSettings
                {
                    NoiseRate = ReadDouble(NoiseRateBox.Text, 0, 1, "Noise Rate"),
                    MutationRate = ReadDouble(MutationRateBox.Text, 0, 1, "Mutation Rate"),
                    RoundsPerMatch = ReadInt(RoundsBox.Text, 1, 1000, "Rounds per match")
                };

                var counts = new AgentCounts
                {
                    AlwaysCooperate = ReadInt(AlwaysCooperateBox.Text, 0, int.MaxValue, "Always Cooperate"),
                    AlwaysDefect = ReadInt(AlwaysDefectBox.Text, 0, int.MaxValue, "Always Defect"),
                    TitForTat = ReadInt(TitForTatBox.Text, 0, int.MaxValue, "Tit For Tat"),
                    TitForTwoTats = ReadInt(TitForTwoTatsBox.Text, 0, int.MaxValue, "Tit For Two Tats"),
                    Grim = ReadInt(GrimBox.Text, 0, int.MaxValue, "Grim"),
                    Random = ReadInt(RandomBox.Text, 0, int.MaxValue, "Random"),
                    GenerousTitForTat = ReadInt(GenerousTitForTatBox.Text, 0, int.MaxValue, "Generous Tit For Tat"),
                };

                var generator = new AgentGenerator();
                var agents = generator.GenerateFromCounts(counts);

                var engine = new EvolutionEngine(generator, settings);
                

                // UPDATED CALL
                var (winner, generations, history, converged, timedOut, maxGenHit) = engine.Run(agents);


                _latestHistory = history;

                //DISPLAY RESULTS

                string winnerName;
                string stopReason;

                // Case 1 Full convergence
                if (converged && winner != null)
                {
                    winnerName = winner.Name;
                    stopReason = "Converged";

                    ResultText.Text =
                        $"Winner: {winner.Name}\n" +
                        $"Generations: {generations}";
                }

                // Case 2 Timed out at 60 seconds
                else if (timedOut && winner != null)
                {
                    winnerName = winner.Name;
                    stopReason = "Timed out (60s)";

                    ResultText.Text =
                        $"⚠ No full convergence\n" +
                        $"Most common: {winner.Name}\n" +
                        $"Stopped after 60 seconds\n" +
                        $"Generations: {generations}";
                }

                // Case 3 Hit generation cap
                else if (maxGenHit && winner != null)
                {
                    winnerName = winner.Name;
                    stopReason = "Generation limit";

                    ResultText.Text =
                        $"⚠ No full convergence\n" +
                        $"Most common: {winner.Name}\n" +
                        $"Stopped at 10,000 generations\n" +
                        $"Generations: {generations}";
                }

                // Fallback
                else
                {
                    winnerName = "Unknown";
                    stopReason = "Unknown stop";

                    ResultText.Text =
                        $"⚠ Simulation stopped\n" +
                        $"Reason unknown\n" +
                        $"Generations: {generations}";
                }


                // Save to history
            
                ResultsLog.Append(
                    new Dictionary<string, object?>
                    {
                        { "NoiseRate", settings.NoiseRate },
                        { "MutationRate", settings.MutationRate },
                        { "RoundsPerMatch", settings.RoundsPerMatch },
                        { "AgentCounts", counts },
                        { "StopReason", stopReason }
                    },
                    winnerName,
                    generations,
                    history
                );

                
            }
            
            catch (Exception ex)
            {
                ResultText.Text = "Error: " + ex.Message;

                ResultText.InvalidateVisual();
            }
        }

        // History list 

        private void RefreshHistory_Click(object? sender, RoutedEventArgs e)
        {
            var results = ResultsLog.ReadAll();

            var list = new List<HistoryRunView>();

            int i = 1;

            foreach (var r in results)
            {
                list.Add(new HistoryRunView
                {
                    Title = $"Run #{i++}",

                    WinnerLine =
                        "Winner: " +
                        (r.TryGetProperty("winner", out var w)
                            ? w.GetString()
                            : "Unknown"),

                    GenerationLine =
                        "Generations: " +
                        (r.TryGetProperty("generations", out var g)
                            ? g.GetInt32()
                            : 0)
                });
            }

            HistoryList.ItemsSource = list;
        }


        // To plot a graph

        private void PlotDynamics_Click(object? sender, RoutedEventArgs e)
        {
            if (_latestHistory == null || _latestHistory.Count == 0)
            {
                ResultText.Text = "Run a simulation first.";
                return;
            }

            var seriesData = new Dictionary<string, List<int>>();

            foreach (var generation in _latestHistory)
            {
                foreach (var kv in generation)
                {
                    if (!seriesData.ContainsKey(kv.Key))
                        seriesData[kv.Key] = new List<int>();

                    seriesData[kv.Key].Add(kv.Value);
                }
            }

            var series = new List<ISeries>();

            foreach (var kv in seriesData)
            {
                series.Add(new LineSeries<int>
                {
                    Name = kv.Key,
                    Values = kv.Value,
                    GeometrySize = 0
                });
            }

            DynamicsChart.Series = series;
            
        }

   

        // UI helpers


        private void ResetPayoffs_Click(object? sender, RoutedEventArgs e)
        {
            CCBox.Text = "3";
            CDBox.Text = "0";
            DCBox.Text = "5";
            DDBox.Text = "1";
        }

        


        private void SetDefaultSimulation_Click(object? sender, RoutedEventArgs e)
        {
            NoiseRateBox.Text = "0.05";
            MutationRateBox.Text = "0.01";
            RoundsBox.Text = "10";
        }


        private void SetDefaultPopulation_Click(object? sender, RoutedEventArgs e)
        {
            AlwaysCooperateBox.Text = "10";
            AlwaysDefectBox.Text = "10";
            TitForTatBox.Text = "10";
            TitForTwoTatsBox.Text = "10";
            GrimBox.Text = "10";
            RandomBox.Text = "10";
            GenerousTitForTatBox.Text = "10";
        }


        private static int ReadInt(string? text, int min, int max, string label)
        {
            text = text?.Trim();

            if (string.IsNullOrWhiteSpace(text))
                throw new Exception($"{label} cannot be empty.");

            if (!int.TryParse(text, out int value))
                throw new Exception($"{label} must be an integer.");

            if (value < min || value > max)
                throw new Exception($"{label} must be between {min} and {max}.");

            return value;
        }

        private static double ReadDouble(string? text, double min, double max, string label)
        {
            text = text?.Trim();

            if (string.IsNullOrWhiteSpace(text))
                throw new Exception($"{label} cannot be empty.");

            if (!double.TryParse(text, out double value))
                throw new Exception($"{label} must be a number.");

            if (value < min || value > max)
                throw new Exception($"{label} must be between {min} and {max}.");

            return value;
        }

    }
}
