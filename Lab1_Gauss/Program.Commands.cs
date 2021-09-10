using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Lab1_Gauss {
    partial class Program {
        private static Matrix _matrix;
        private static IList<double> _roots;

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
            var dim = Utils.GetValueFromUser<int>("Enter dimension: ");
            var data = new double[dim, dim + 1];
            for (var i = 0; i < dim; i++) {
                for (var j = 0; j < dim; j++) {
                    data[i, j] = Utils.GetValueFromUser<double>($"Enter X{i}{j}: ");
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

            var triangleMatrix = GetTriangleMatrix(_matrix);
            Console.WriteLine("Triangle matrix: ");
            Console.WriteLine(triangleMatrix.ToString(true));

            _roots = GetRoots(triangleMatrix);
            if (_roots == null) {
                throw new Exception("Matrix determinant = 0");
            }

            Console.WriteLine("\nResult: ");
            ShowResult(_roots, true);
        }

        private static Matrix GetTriangleMatrix(Matrix matrix) {
            var resultMatrix = matrix.Copy();
            for (var j = 0; j < resultMatrix.ColsNum - 2; j++) {
                Rearrange(resultMatrix, j);

                for (var i = j + 1; i < resultMatrix.RowsNum; i++) {
                    if (resultMatrix[j, j] == 0) continue;
                    var alpha = -resultMatrix[i, j] / resultMatrix[j, j];
                    CombineRows(resultMatrix, j, i, alpha);
                }
            }

            return resultMatrix;
        }

        private static void Rearrange(Matrix matrix, int pivotIndex) {
            var maxValue = matrix[pivotIndex, pivotIndex];
            var newPivotIndex = pivotIndex;

            for (var i = pivotIndex; i < matrix.RowsNum; i++) {
                if (!(matrix[i, pivotIndex] > maxValue)) continue;
                maxValue = matrix[i, pivotIndex];
                newPivotIndex = i;
            }

            if (newPivotIndex != pivotIndex) {
                matrix.SwapRows(pivotIndex, newPivotIndex);
            }
        }

        private static void CombineRows(Matrix matrix, int row1, int row2, double alpha) {
            for (var i = 0; i < matrix.ColsNum; i++) {
                matrix[row2, i] += matrix[row1, i] * alpha;
            }
        }

        private static IList<double> GetRoots(Matrix matrix) {
            var result = new List<double>();
            for (var i = matrix.RowsNum - 1; i >= 0; i--) {
                var value = matrix[i, matrix.ColsNum - 1];
                var k = 0;
                for (var j = i + 1; j < matrix.ColsNum - 1; j++) {
                    value -= matrix[i, j] * (result.Count > k ? result[k++] : 1);
                }

                if (Math.Abs(matrix[i, i]) < 1.0E-20) {
                    return null;
                }

                result.Insert(0, value / matrix[i, i]);
            }

            return result;
        }

        private static void ShowResult(IList<double> list, bool format = false) {
            for (var i = 0; i < list.Count; i++) {
                var value = format ? list[i].ToString("0.##") : list[i].ToString(CultureInfo.CurrentCulture);
                Console.WriteLine($"x{i + 1} = {value}");
            }
        }

        private static void Check() {
            if (_matrix == null || _roots == null) {
                throw new Exception("Matix or roots are not defined");
            }
            
            if (_matrix.RowsNum != _roots.Count) {
                throw new Exception($"Inconsistency between matrix dim({_matrix.RowsNum}) and roots dim({_roots.Count})");
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