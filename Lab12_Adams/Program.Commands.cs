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

            Console.WriteLine("\nExplicit scheme: ");
            Console.WriteLine("Coords: ");

            var xi = a;
            var yi = y0;

            var coords = new List<Tuple<double, double>>();
            coords.Add(Tuple.Create<double, double>(xi, yi));

            var counter = 0;
            while (xi <= b) {
                if (counter < 2) {
                    var k1 = Fxu(xi, yi);
                    var k2 = Fxu(xi + h / 2, yi + h * k1 / 2);
                    var k3 = Fxu(xi + h / 2, yi + h * k2 / 2);
                    var k4 = Fxu(xi + h, yi + h * k3);

                    yi += h * (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                    xi += h;

                    coords.Add(Tuple.Create<double, double>(xi, yi));
                    Console.WriteLine($"{xi},{yi}");

                    counter++;
                    continue;
                }

                yi = coords[counter].Item2 + h * ((23.0 / 12.0) * Fxu(coords[counter].Item1, coords[counter].Item2) - (4.0 / 3.0) * Fxu(coords[counter - 1].Item1, coords[counter - 1].Item2) + (5.0 / 12.0) * Fxu(coords[counter - 2].Item1, coords[counter - 2].Item2));
                xi += h;

                coords.Add(Tuple.Create<double, double>(xi, yi));
                Console.WriteLine($"{xi},{yi}");

                counter++;
            }

            Console.WriteLine("\n\nImplicit scheme: ");
            Console.WriteLine("Coords: ");

            xi = a;
            yi = y0;

            coords.Clear();
            coords.Add(Tuple.Create<double, double>(xi, yi));

            counter = 0;
            while (xi <= b) {
                if (counter < 2) {
                    var k1 = Fxu(xi, yi);
                    var k2 = Fxu(xi + h / 2, yi + h * k1 / 2);
                    var k3 = Fxu(xi + h / 2, yi + h * k2 / 2);
                    var k4 = Fxu(xi + h, yi + h * k3);

                    yi += h * (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                    xi += h;

                    coords.Add(Tuple.Create<double, double>(xi, yi));
                    Console.WriteLine($"{xi},{yi}");

                    counter++;
                    continue;
                }

                var yiApprox = coords[counter].Item2 + h * ((23.0 / 12.0) * Fxu(coords[counter].Item1, coords[counter].Item2) - (4.0 / 3.0) * Fxu(coords[counter - 1].Item1, coords[counter - 1].Item2) + (5.0 / 12.0) * Fxu(coords[counter - 2].Item1, coords[counter - 2].Item2));
                yi = coords[counter].Item2 + h * ((3.0 / 8.0) * Fxu(xi + h, yiApprox) + (19.0 / 24.0) * Fxu(coords[counter].Item1, coords[counter].Item2) - (5.0 / 24.0) * Fxu(coords[counter - 1].Item1, coords[counter - 1].Item2) + (1.0 / 24.0) * Fxu(coords[counter - 2].Item1, coords[counter - 2].Item2));
                xi += h;

                coords.Add(Tuple.Create<double, double>(xi, yi));
                Console.WriteLine($"{xi},{yi}");

                counter++;
            }
        }

        private static double Fxu(double x, double u) {
            return 2 * u + 4 * x;
        }
    }
}