namespace Lab7_QuadratureFormulas {
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
            var from = Utils.GetValueFromUser<double>("Enter from: ");
            var to = Utils.GetValueFromUser<double>("Enter to: ");
            var eps = Utils.GetValueFromUser<double>("Enter eps: ");

            var countOfIterations = 0;
            var n = 1;

            var prevRes = MediumRectangle(from, to, n);
            var curRes = MediumRectangle(from, to, 2 * n);
            while ((1.0 / 3 * Math.Abs(prevRes - curRes)) > eps) {
                n *= 2;
                prevRes = curRes;
                curRes = MediumRectangle(from, to, 2 * n);
                countOfIterations++;
            }

            Console.WriteLine($"\n\nResult for MediumRectangle = {curRes}");
            Console.WriteLine($"Count of iterations = {countOfIterations}");

            countOfIterations = 0;
            n = 1;

            prevRes = Parabola(from, to, n);
            curRes = Parabola(from, to, 2 * n);
            while ((1.0 / 15 * Math.Abs(prevRes - curRes)) > eps) {
                n *= 2;
                prevRes = curRes;
                curRes = Parabola(from, to, 2 * n);
                countOfIterations++;
            }

            Console.WriteLine($"\nResult for Parabola = {curRes}");
            Console.WriteLine($"Count of iterations = {countOfIterations}");
        }

        private static double MediumRectangle(double a, double b, int n) {
            if (a > b) {
                Utils.Swap(ref a, ref b);
            }

            var sum = 0.0;
            var h = (b - a) / n;

            for (var i = 1; i <= n; i++) {
                var x0 = a + (i - 1) * h;
                var x1 = a + i * h;
                sum += h * Fx((x0 + x1) / 2);
            }

            return sum;
        }

        private static double Parabola(double a, double b, int n) {
            if (a > b) {
                Utils.Swap(ref a, ref b);
            }

            var sum = Fx(a) + Fx(b);
            var h = (b - a) / n;

            for (var i = 1; i < n; i++) {
                var x = a + i * h;

                if (Utils.IsEven(i)) {
                    sum += 2 * Fx(x);
                } else {
                    sum += 4 * Fx(x);
                }
            }

            return sum * h / 3;
        }

        private static double Fx(double x) {
            return Math.Exp(x) * Math.Sin(1 / Math.Pow(x, 3));
        }
    }
}