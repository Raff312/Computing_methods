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
            var resForMediumRectangle = MediumRectangle(1, 3, 100);
            Console.WriteLine($"\n\nResult for MediumRectangle = {resForMediumRectangle}");

            var resForParabola = Parabola(1, 3, 100);
            Console.WriteLine($"\n\nResult for Parabola = {resForParabola}");
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