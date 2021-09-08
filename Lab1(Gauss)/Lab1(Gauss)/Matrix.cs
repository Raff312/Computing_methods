using System;
using System.Globalization;

namespace Lab1_Gauss_ {
    public class Matrix {
        private readonly double[,] _data;

        public int RowsNum => _data.GetLength(0);
        public int ColsNum => _data.GetLength(1);

        public double this[int i, int j] {
            get => _data.GetValue(i, j) == null ? 0 : Convert.ToDouble(_data.GetValue(i, j));
            set => _data.SetValue(value, i, j);
        }

        public Matrix(int n, int m) {
            _data = new double[n, m];
            Fill(0);
        }

        public Matrix(double[,] data) {
            var rows = data.GetLength(0);
            var cols= data.GetLength(1);

            _data = new double[rows, cols];
            for (var i = 0; i < rows; i++) {
                for (var j = 0; j < cols; j++) {
                    _data[i, j] = data[i, j];
                }
            }
        }

        public void SwapRows(int row1, int row2) {
            if (row1 < 0 || row1 >= RowsNum || row2 < 0 || row2 >= RowsNum) {
                throw new ArgumentOutOfRangeException();
            }

            if (row1 == row2) return;

            for (var i = 0; i < ColsNum; i++) {
                var temp = _data[row1, i];
                _data[row1, i] = _data[row2, i];
                _data[row2, i] = temp;
            }
        }

        public string ToString(bool format = false) {
            var result = "";
            for (var i = 0; i < RowsNum; i++) {
                for (var j = 0; j < ColsNum; j++) {
                    var value = format ? _data[i, j].ToString("0.##") : _data[i, j].ToString(CultureInfo.CurrentCulture);
                    result += value + "\t";
                }

                result += "\n";
            }

            return result;
        }

        private void Fill(double x) {
            for (var i = 0; i < RowsNum; i++) {
                for (var j = 0; j < ColsNum; j++) {
                    _data[i, j] = x;
                }
            }
        }
    }
}