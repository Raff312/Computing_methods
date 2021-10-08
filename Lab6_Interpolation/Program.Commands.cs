using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab6_Interpolation {
    partial class Program {
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
                Description = "Run",
                Action = Run
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

        private static void Run() {
            var a = Utils.GetValueFromUser<double>("Enter a: ");
            var b = Utils.GetValueFromUser<double>("Enter b: ");

            var xList = new List<double> { -1.5, -0.75, 0, 0.75 };
            var fList = new List<double> { -14.1014, -0.931596, 0, 0.931596 };

            var result = Lagrange(xList, fList, 1.5);

            Console.WriteLine($"\n\nResult = {result}");
        }

        private static double Lagrange(List<double> xList, List<double> fList, double x) {
            var result = 0.0;
            var n = xList.Count;
            for (var i = 0; i < n; i++) {
                result += fList[i] * Omega(xList, x, n) / ((x - xList[i]) * OmegaDash(xList, i, n));
            }
            return result;
        }

        private static double Omega(List<double> xList, double x, int n) {
            var result = 1.0;
            for (var i = 0; i < n; i++) {
                result *= (x - xList[i]);
            }
            return result;
        }

        private static double OmegaDash(List<double> xList, int k, int n) {
            var result = 1.0;
            for (var i = 0; i < n; i++) {
                if (i == k) continue;
                result *= (xList[k] - xList[i]);
            }
            return result;
        }
    }
}