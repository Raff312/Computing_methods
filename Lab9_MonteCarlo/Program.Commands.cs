namespace Lab9_MonteCarlo {
    partial class Program {
        private class CommandDefinition {
            public string[] Codes { get; }
            public string Description { get; set; }
            public Action? Action { get; set; }

            public CommandDefinition(params string[] codes) {
                Codes = codes;
                Description = string.Empty;
            }
        }

        private static readonly CommandDefinition[] CommandDefinitions = {
            new CommandDefinition("1") {
                Description = "Run",
                Action = Run
            },
            new CommandDefinition("0", "exit") {
                Description = "Exit from program",
                Action = null
            }
        };

        private static CommandDefinition? GetCommandDefinitionByCode(string code) {
            code = code.ToLowerInvariant();
            return CommandDefinitions.FirstOrDefault(x => x.Codes.Contains(code));
        }

        private static void Run() {
            var integralRes1 = ComputeIntegral(10000);
            Console.WriteLine($"Result (N = 10000) = {integralRes1}");
            
            var integralRes2 = ComputeIntegral(1000000);
            Console.WriteLine($"Result (N = 1000000) = {integralRes2}");
            
            var integralRes3 = ComputeIntegral(100000000);
            Console.WriteLine($"Result (N = 100000000) = {integralRes3}");

            var eps = Math.Abs(integralRes1 - integralRes2);
            Console.WriteLine($"\nEps1 = {eps}");
            
            eps = Math.Abs(integralRes2 - integralRes3);
            Console.WriteLine($"\nEps2 = {eps}");
        }

        private static double ComputeIntegral(int n) {
            var upperLimits = new List<double> { 2.0, 4.0, 5.0 };
            var lowerLimits = new List<double> { 1.0, 2.0, 1.0 };
            var dists = new List<double>();

            var ratio = 1.0;
            for (var i = 0; i < upperLimits.Count; i++) {
                var tmp = upperLimits[i] - lowerLimits[i];
                dists.Add(tmp);
                ratio *= tmp;
            }
            ratio /= n;

            var rnd = new Random();

            var sum = 0.0;
            for (var i = 0; i < n; i++) {
                var rndNum = rnd.NextDouble();
                var xi = lowerLimits[0] + (dists[0]) * rndNum;
                var yi = lowerLimits[1] + (dists[1]) * rndNum;
                var zi = lowerLimits[2] + (dists[2]) * rndNum;
                sum += Fx(xi, yi, zi);
            }

            return ratio * sum;
        }

        private static double Fx(double x, double y, double z) {
            // return x * Math.Pow(y, 2);
            return x * Math.Exp(y);
        }
    }
}