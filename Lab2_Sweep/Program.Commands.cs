using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Lab2_Sweep {
    partial class Program {
        private static Matrix _matrix;
        private static double[] _roots;

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
                Description = "Enter dimensions and initialize matrix",
                Action = Init
            },
            new CommandDefinition("2") {
                Description = "Show initial matrix",
                Action = ShowInitialMatrix
            },
            new CommandDefinition("3") {
                Description = "Solve the system of equations",
                Action = Solve
            },
            new CommandDefinition("4") {
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

        private static void Init() {
            // just for test
            // var data = new double[5, 6] {
            //     {2, -1, 0, 0, 0, -25},
            //     {-3, 8, -1, 0, 0, 72},
            //     {0, -5, 12, 2, 0, -69},
            //     {0, 0, -6, 18, -4, -156}, 
            //     {0, 0, 0, -5, 10, 20}
            // };

            var dim = Utils.GetValueFromUser<int>("Enter dimension: ");
            var data = new double[dim, dim + 1];
            for (var i = 0; i < dim; i++) {
                for (var j = 0; j < dim; j++) {
                    data[i, j] = Utils.GetValueFromUser<double>($"Enter A{i}{j}: ");
                }

                data[i, dim] = Utils.GetValueFromUser<double>($"Enter F{i}: ");
            }

            _matrix = new Matrix(data);
        }

        private static void ShowInitialMatrix() {
            if (_matrix == null) {
                throw new Exception("There is no matrix");
            }

            Console.WriteLine("Initial matrix: ");
            Console.WriteLine(_matrix.ToString());
        }

        private static void Solve() {
            if (_matrix == null) {
                throw new Exception("There is no system!");
            }

            var ps = new double[_matrix.RowsNum];
            var qs = new double[_matrix.RowsNum];

            var ai = 0.0;
            var bi = _matrix[0, 0];
            var ci = _matrix[0, 1];
            var di = _matrix[0, _matrix.ColsNum - 1];

            ps[0] = -ci / bi;
            qs[0] = di / bi;

            for (var i = 1; i < _matrix.RowsNum; i++) {
                ai = _matrix[i, i - 1];
                bi = _matrix[i, i];
                ci = _matrix[i, i + 1];
                di = _matrix[i, _matrix.ColsNum - 1];

                var denom = (ai * ps[i - 1] + bi);
                ps[i] = -ci / denom;
                qs[i] = (di - ai * qs[i - 1]) / denom;
            }

            Console.Write("ps: ");
            Utils.ShowArr(ps);
            Console.Write("\nqs: ");
            Utils.ShowArr(qs);

            _roots = new double[_matrix.RowsNum];
            _roots[_matrix.RowsNum - 1] = qs[qs.Length - 1];
            for (var i = _matrix.RowsNum - 2; i >= 0; i--) {
               _roots[i] = ps[i] * _roots[i + 1] + qs[i]; 
            }

            Console.WriteLine("\n\nResult: ");
            ShowResult(_roots, true);
        }        

        private static void ShowResult(IList<double> list, bool format = false) {
            for (var i = 0; i < list.Count; i++) {
                var value = format ? list[i].ToString("0.####") : list[i].ToString(CultureInfo.CurrentCulture);
                Console.WriteLine($"x{i + 1} = {value}");
            }
        }

        private static void Check() {
            if (_matrix == null || _roots == null) {
                throw new Exception("Matix or roots are not defined");
            }
            
            if (_matrix.RowsNum != _roots.Length) {
                throw new Exception($"Inconsistency between matrix dim({_matrix.RowsNum}) and roots dim({_roots.Length})");
            }

            for (var i = 0; i < _matrix.RowsNum; i++) {
                var value = 0.0;
                for (var j = 0; j < _matrix.ColsNum - 1; j++) {
                    value += _matrix[i, j] * _roots[j];
                }

                Console.WriteLine($"{value} = {_matrix[i, _matrix.ColsNum - 1]}");
            }
        }
    }
}