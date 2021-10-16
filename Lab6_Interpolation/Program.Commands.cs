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
            var xList = new List<double> { 1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2.0 };
            var fList = new List<double> { 2.7182, 3.0041, 3.3201, 3.6692, 4.0552, 4.4816, 4.9530, 5.4739, 6.0496, 6.6858, 7.3890 };

            var lagrangeResult = Lagrange(xList, fList, 1.43);
            Console.WriteLine($"\n\nLagrange result = {lagrangeResult}");

            var eps = Utils.GetValueFromUser<double>("Enter eps: ");
            var aitkenResult = Aitken(xList, fList, 1.43, eps);
            Console.WriteLine($"\nAitken result = {aitkenResult}");

            var correctResult = Math.Exp(1.43);
            Console.WriteLine($"\nCorrect result = {correctResult}");
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

        private static double Aitken(List<double> xList, List<double> fList, double x, double eps) {
            var k = fList.Count;
            var i = 0;
            var step = 1;

            var minEps = double.MaxValue;
            var valueWithMinEps = double.MaxValue;

            while (k > 1) {
                var value = (fList[i] * (xList[i + step] - x) - fList[i + 1] * (xList[i] - x)) / (xList[i + step] - xList[i]);
                
                var currentEps = Math.Abs(fList[i] - value);
                if (k != fList.Count && currentEps < eps) {
                    return value;
                }

                if (currentEps < minEps) {
                    minEps = currentEps;
                    valueWithMinEps = value;
                }

                fList[i++] = value;

                if (i >= k - 1) {
                    i = 0;
                    k--;
                    step++;
                }
            }

            return valueWithMinEps != double.MaxValue ? valueWithMinEps : fList[0];
        }
    }
}