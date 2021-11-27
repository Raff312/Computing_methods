namespace Lab12_Adams {
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
            var y0 = Utils.GetValueFromUser<double>("Enter y0: ");
            var a = Utils.GetValueFromUser<double>("Enter a: ");
            var b = Utils.GetValueFromUser<double>("Enter b: ");
            var h = Utils.GetValueFromUser<double>("Enter h: ");

            if (a > b) {
                Utils.Swap(ref a, ref b);
            }

            Console.WriteLine("\n\nCoords: ");

            var xi = a;
            var yi = y0;
            while (xi <= b) {
                var k1 = Fxu(xi, yi);
                var k2 = Fxu(xi + h / 2, yi + h * k1 / 2);
                var k3 = Fxu(xi + h / 2, yi + h * k2 / 2);
                var k4 = Fxu(xi + h, yi + h * k3);
                yi += h * (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                xi += h;
                Console.WriteLine($"{xi},{yi}");
            }
        }

        private static double Fxu(double x, double u) {
            return 2 * u + 4 * x;
        }
    }
}