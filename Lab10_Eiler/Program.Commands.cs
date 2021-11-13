namespace Lab10_Eiler {
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

            var differenceModules = new List<double>();

            Console.WriteLine("\nExplicit schema");
            Console.WriteLine("Coords: ");

            var xi = a;
            var yi = y0;
            while (xi <= b) {
                differenceModules.Add(Math.Abs(Ux(xi) - yi));
                yi += h * Fxu(xi, yi);
                xi += h;
                Console.WriteLine($"{xi},{yi}");
            }

            Console.WriteLine("\nDifference modules: ");

            foreach (var module in differenceModules) {
                Console.WriteLine(module);
            }

            Console.WriteLine("\n\nImplicit schema");
            Console.WriteLine("Coords: ");

            differenceModules.Clear();
            
            xi = a;
            yi = y0;
            while (xi <= b) {
                differenceModules.Add(Math.Abs(Ux(xi) - yi));

                var fi = yi + h * Fxu(xi, yi) / 2;
                var yiApprox = yi + h * Fxu(xi, yi);
                yi += h * (Fxu(xi, yi) + Fxu(xi + h, yiApprox)) / 2;
                xi += h;
                Console.WriteLine($"{xi},{yi}");
            }

            Console.WriteLine("\nDifference modules: ");

            foreach (var module in differenceModules) {
                Console.WriteLine(module);
            }
        }

        private static double Fxu(double x, double u) {
            return 2 * u + 4 * x;
        }

        private static double Ux(double x) {
            return -2 * x + 2 * Math.Exp(2 * x) - 1;
        }
    }
}