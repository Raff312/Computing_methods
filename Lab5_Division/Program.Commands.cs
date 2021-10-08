using System;
using System.Linq;

namespace Lab5_Division {
    partial class Program {
        private static double _root;

        private class CommandDefinition {
            public string[] Codes { get; }
            public string Description { get; set; }
            public Action Action { get; set; }

            public CommandDefinition(params string[] codes) {
                Codes = codes;
            }
        }

        private static readonly CommandDefinition[] CommandDefinitions = {
            new CommandDefinition("1") {
                Description = "Show initial equation",
                Action = ShowEquation
            },
            new CommandDefinition("2") {
                Description = "Solve equation",
                Action = Solve
            },
            new CommandDefinition("3") {
                Description = "Make a check",
                Action = Check
            },
            new CommandDefinition("0", "exit") {
                Description = "Exit from program",
                Action = null
            }
        };

        private static CommandDefinition GetCommandDefinitionByCode(string code) {
            code = code?.ToLowerInvariant();
            return CommandDefinitions.FirstOrDefault(x => x.Codes.Contains(code));
        }

        private static void ShowEquation() {
            Console.WriteLine("Initial equation: x^2 + 6x + 1 = 0");
        }

        private static void Solve() {
            var a = Utils.GetValueFromUser<double>("Enter a: ");
            var b = Utils.GetValueFromUser<double>("Enter b: ");
            var eps = Utils.GetValueFromUser<double>("Enter an epsilon: ");
            var countOfIterations = 1;

            if (Math.Abs(Fx(a)) < eps) {
                _root = a;
            } else if (Math.Abs(Fx(b)) < eps) {
                _root = b;
            } else {
                var c = a;
                while (b - a > eps) {
                    var dx = (b - a) / 2;
                    c = a + dx;
                    if (Math.Sign(Fx(a)) != Math.Sign(Fx(c))) {
                        b = c;
                    } else {
                        a = c;
                    }
                }

                _root = c;
            }

            Console.WriteLine($"\n\nResult: x = {_root.ToString("0.####")}");
            Console.WriteLine($"\nCountOfIterations: {countOfIterations}");
        }

        private static void Check() {
            var value = Fx(_root);
            var dvalue = Dfx(_root);
            Console.WriteLine($"{value.ToString("0.####")} = 0");
        }

        private static double Fx(double x) {
            return Math.Pow(x, 2) + 6 * x + 1;
        }

        private static double Dfx(double x) {
            return 2 * x + 6;
        }
    }
}