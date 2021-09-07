using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Lab1_Gauss_ {
    internal class Program {
        private static void Main(string[] args) {
            try {
                var data = new double[,] {
                    {2, 3, -1, 7},
                    {1, -1, 6, 14},
                    {6, -2, 1, 11}
                };

                if (data.GetLength(0) != data.GetLength(1) - 1) {
                    throw new Exception("Matrix isn't square");
                }

                var matrix = new Matrix(data);

                Console.WriteLine("Initial matrix: ");
                Console.WriteLine(matrix.ToString());

                TriangleMatrix(matrix);
                Console.WriteLine("Triangle matrix: ");
                Console.WriteLine(matrix.ToString(true));

                var list = Foo(matrix);
                Console.WriteLine("Result: ");
                ShowResult(list, true);
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        private static void TriangleMatrix(Matrix matrix) {
            for (var j = 0; j < matrix.ColsNum - 2; j++) {
                Rearrange(matrix, j);

                for (var i = j + 1; i < matrix.RowsNum; i++) {
                    if (matrix[j, j] == 0) continue;
                    var alpha = -matrix[i, j] / matrix[j, j];
                    CombineRows(matrix, j, i, alpha);
                }
            }
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

        private static IList<double> Foo(Matrix matrix) {
            var result = new List<double>();
            for (var i = matrix.RowsNum - 1; i >= 0; i--) {
                var value = matrix[i, matrix.ColsNum - 1];
                var k = 0;
                for (var j = i + 1; j < matrix.ColsNum - 1; j++) {
                    value -= matrix[i, j] * (result.Count > k ? result[k++] : 1);
                }
                
                result.Insert(0, value / matrix[i, i]);
            }

            return result;
        }

        private static void ShowResult(IList<double> list, bool format = false) {
            for (var i = 0; i < list.Count; i++) {
                var value = format ? list[i].ToString("0.##") : list[i].ToString(CultureInfo.CurrentCulture);
                Console.WriteLine($"x{i+1} = {value}");
            }
        }
    }
}
