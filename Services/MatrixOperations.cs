using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kcsj.Services
{
    public class MatrixOperations
    {
        private const double eps = 1e-2;
        private readonly double[,] values;

        private int RowCount { get; }
        private int ColumnCount { get; }

        public double this[int row, int column]
        {
            get => values[row, column];
            set => values[row, column] = value;
        }

        // 初始化矩阵大小
        public MatrixOperations(int rowCount, int columnCount)
        {
            if (rowCount <= 0 || columnCount <= 0)
            {
                throw new ArgumentException("矩阵行列数必须大于 0。");
            }

            RowCount = rowCount;
            ColumnCount = columnCount;
            values = new double[rowCount, columnCount];
        }

        // 从二维数组初始化矩阵
        public MatrixOperations(double[,] source)
        {
            RowCount = source.GetLength(0);
            ColumnCount = source.GetLength(1);
            values = new double[RowCount, ColumnCount];

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    values[i, j] = source[i, j];
                }
            }
        }

        public MatrixOperations Add(MatrixOperations other)
        {
            if (RowCount != other.RowCount || ColumnCount != other.ColumnCount)
            {
                throw new ArgumentException("矩阵维度不匹配，无法相加。");
            }
            MatrixOperations result = new MatrixOperations(RowCount, ColumnCount);
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    result[i, j] = this[i, j] + other[i, j];
                }
            }
            return result;
        }

        public MatrixOperations Sub (MatrixOperations other)
        {
            if (RowCount != other.RowCount || ColumnCount != other.ColumnCount)
            {
                throw new ArgumentException("矩阵维度不匹配，无法相减。");
            }
            MatrixOperations result = new MatrixOperations(RowCount, ColumnCount);
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    result[i, j] = this[i, j] - other[i, j];
                }
            }
            return result;

        }

        public MatrixOperations Multiply(MatrixOperations other)
        {
            if (ColumnCount != other.RowCount)
            {
                throw new ArgumentException("矩阵维度不匹配，无法相乘。");
            }
            MatrixOperations result = new MatrixOperations(RowCount, other.ColumnCount);
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < other.ColumnCount; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < ColumnCount; k++)
                    {
                        sum += this[i, k] * other[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }

        public MatrixOperations Transpose()
        {
            MatrixOperations result = new MatrixOperations(ColumnCount, RowCount);
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    result[j, i] = this[i, j];
                }
            }
            return result;
        }

        public MatrixOperations Inverse()
        {
            if (RowCount != ColumnCount)
            {
                throw new ArgumentException("只有方阵才有逆矩阵。");
            }
            int n = RowCount;
            MatrixOperations augmented = new MatrixOperations(n, 2 * n);
            // 构造增广矩阵 [A | I]
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = this[i, j];
                }
                augmented[i, n + i] = 1; // 添加单位矩阵部分
            }
            // 使用高斯消元法将左侧变为单位矩阵
            for (int i = 0; i < n; i++)
            {
                // 寻找主元素
                int pivotRow = i;
                for (int row = i + 1; row < n; row++)
                {
                    if (Math.Abs(augmented[row, i]) > Math.Abs(augmented[pivotRow, i]))
                    {
                        pivotRow = row;
                    }
                }
                if (Math.Abs(augmented[pivotRow, i]) < eps)
                {
                    throw new InvalidOperationException("矩阵不可逆。");
                }
                // 交换行
                if (pivotRow != i)
                {
                    for (int col = 0; col < 2 * n; col++)
                    {
                        double temp = augmented[i, col];
                        augmented[i, col] = augmented[pivotRow, col];
                        augmented[pivotRow, col] = temp;
                    }
                }
                // 将主元素归一化
                double pivotValue = augmented[i, i];
                for (int col = 0; col < 2 * n; col++)
                {
                    augmented[i, col] /= pivotValue;
                }
                // 消去其他行的当前列
                for (int row = 0; row < n; row++)
                {
                    if (row != i)
                    {
                        double factor = augmented[row, i];
                        for (int col = 0; col < 2 * n; col++)
                        {
                            augmented[row, col] -= factor * augmented[i, col];
                        }
                    }
                }
            }
            // 提取右侧的逆矩阵
            MatrixOperations inverse = new MatrixOperations(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inverse[i, j] = augmented[i, n + j];
                }
            }
            return inverse;
        }

    }
}
