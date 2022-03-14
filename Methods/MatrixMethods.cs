using System.Text;

namespace Methods
{
    internal static class Extensions
    {
        public static string MatrixToString(this double[][] matrix)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    char ch1 = j == 0 ? '(' : ' ';
                    string ch2 = j != matrix.Length - 1 ? " " : ") \n";
                    sb.Append($"{ch1}{Math.Round(matrix[i][j], 2)}{ch2}");
                }
            }
            return sb.ToString();
        }
        public static string MatrixToString(this double[] matrix)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < matrix.Length; i++)
            {
                sb.Append($"( {Math.Round(matrix[i], 2)} )\n");
            }
            return sb.ToString();
        }
    }
    public class Matrix
    {
        public StringBuilder history;
        public Matrix()
        {
            history = new StringBuilder();
        }

        private double[] MultiplyMatrix(double[][] a, double[] b)
        {
            double[] result = new double[b.Length];
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].Length != b.Length)
                {
                    throw new Exception("matrix lengths should be equal");
                }
                result[i] = 0;
                for (int j = 0; j < a[i].Length; j++)
                {
                    result[i] += a[i][j] * b[j];
                }
            }
            return result;
        }
        private double[][] DevideMatrix(double[][] matrix, double devideBy)
        {
            var result = matrix;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    result[i][j] = result[i][j] / devideBy;
                }
            }
            return result;
        }
        private double[] DevideMatrix(double[] matrix, double devideBy)
        {
            var result = matrix;
            for (int i = 0; i < matrix.Length; i++)
            {

                result[i] = result[i] / devideBy;

            }
            return result;
        }

        private (int, double[][]) ConvertToTriangle(double[][] matrix)
        {
            int matrixSign = 1;
            int length = matrix.Length;
            double[][] newMatrix = new double[length][];
            int[] zeroCount = new int[length];
            for (int i = 0; i < length; i++)
            {
                newMatrix[i] = matrix[i][0..^0];
            }
            for (int i = 0; i < matrix.Length; i++)
            {
                int counter = 0;
                for (int j = 0; j < matrix.Length; j++)
                {

                    if (matrix[i][j] != 0)
                    {
                        break;
                    }
                    counter++;
                }
                counter = Math.Min(counter, matrix.Length - 1);
                zeroCount[i] = counter;


                //Console.WriteLine($"sign: {matrixSign}");
            }
            for (int i = 0; i < length; i++)
            {
                if (zeroCount[i] != zeroCount[zeroCount[i]])
                {
                    double[] tempData = newMatrix[zeroCount[i]][0..^0];
                    newMatrix[zeroCount[i]] = newMatrix[i];
                    newMatrix[i] = tempData;
                    matrixSign *= -1;
                    history.Append($"Поменяем местами строку {i} на {zeroCount[i]} \n");
                }
            }
            history.Append(newMatrix.MatrixToString());
            history.Append($"Приведём матрицу к треугольному виду: \n");
            for (int i = 1; i < length; i++)
            {
                for (int j = i; j < length; j++)
                {
                    double tempData = newMatrix[i - 1][i - 1];
                    if (tempData == 0) return (matrixSign, newMatrix);
                    if (newMatrix[j][i - 1] != 0)
                    {
                        double tempData2 = newMatrix[j][i - 1];
                        newMatrix[j][i - 1] = 0;
                        for (int k = i; k < length; k++)
                        {
                            newMatrix[j][k] = newMatrix[j][k] - tempData2 * newMatrix[i - 1][k] / tempData;
                        }
                    }
                }
            }
            history.Append(newMatrix.MatrixToString());
            return (matrixSign, newMatrix);
        }
        public double Determinant(double[][] array)
        {
            if (array.Length == 1) return array[0][0];
            if (array.Length == 2) return array[0][0] * array[1][1] - array[0][1] * array[1][0];
            foreach (var row in array)
            {
                if (row.Length != array.Length || array.Length <= 1)
                {
                    throw new Exception("Матрица должна быть квадратной");
                }
            }
            var (sign, convertedArray) = ConvertToTriangle(array);
            double determinant = 1;
            for (int i = 0; i < array.Length; i++)
            {
                determinant *= convertedArray[i][i];
                //Console.WriteLine($"conv:{convertedArray[i][i]}");
            }
            return determinant * sign;
        }
        public double[][] FindAlgebraicMatrix(double[][] matrix)
        {
            history.Append("Найдём матрицу алгебраических дополнений: \n");
            var returnMatrix = new double[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                returnMatrix[i] = new double[matrix.Length];
                double[][] newMatrix = matrix[0..i].Concat(matrix[(i + 1)..^0]).ToArray();
                for (int j = 0; j < matrix.Length; j++)
                {
                    var newNewMatrix = newMatrix.Select(row =>
                    {
                        return row[0..j].Concat(row[(j + 1)..^0]).ToArray();
                    }).ToArray();
                    returnMatrix[i][j] = Determinant(newNewMatrix) * ((i + j) % 2 == 0 ? 1 : -1);
                }
            }
            history.Append(returnMatrix.MatrixToString());
            return returnMatrix;
        }
        private double[][] TransponentMatrix(double[][] matrix)
        {
            history.Append("Транспонируем матрицу : \n");
            var transMatrix = new double[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                transMatrix[i] = new double[matrix.Length];
                for (int j = 0; j < matrix.Length; j++)
                {
                    transMatrix[i][j] = matrix[j][i];
                }
            }
            history.Append(transMatrix.MatrixToString());
            return transMatrix;
        }
        public (double[][], double) FindInverseMatrix(double[][] matrix)
        {
            var determinant = Determinant(matrix);
            if (determinant == 0)
            {
                throw new Exception("Обратная матрица не существует...");
            }
            var inversiveMatrix = TransponentMatrix(FindAlgebraicMatrix(matrix));

            return (inversiveMatrix, determinant);
        }
        public async Task<double[]> MatrixMethod(double[][] matrix, double[] b)
        {
            var task1 = Task.Run(() => FindInverseMatrix(matrix));
            var (inversiveMatrix, deteminant) = await task1;
            history.Append("Умножим транспонированную матрицу на B и получим: \n");
            var task2 = Task.Run(() => MultiplyMatrix(inversiveMatrix, b));
            var result = await task2;
            history.Append(result.MatrixToString());
            var task3 = await Task.Run(() => DevideMatrix(result, deteminant));
            history.Append($"Разделим результат на детерминант {deteminant} : \n");
            history.Append(task3.MatrixToString());

            return task3;
        }
        public double[][] SwapColumn(double[] column, double[][] matrix, int cIndex)
        {
            double[][] result = new double[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                result[i] = matrix[i][0..(cIndex)].Append(column[i]).Concat(matrix[i][(cIndex + 1)..^0]).ToArray();
            }
            return result;
        }
        public double[] CramersMethod(double[][] matrix, double[] b)
        {
            double determinant = Determinant(matrix);
            if (determinant == 0)
            {
                throw new Exception("Метод крамера не подходит, детерминант равен 0");
            }
            double[] results = new double[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                history.Append("найдем детерминант для: \n");
                var newArr = SwapColumn(b, matrix, i);
                history.Append(newArr.MatrixToString());
                results[i] = Determinant(newArr);
                history.Append($"Детерминант равен: {results[i]} \n");
            }
            var result = DevideMatrix(results, determinant);
            history.Append($"Разделим полученные детерменанты на детерминант матрицы: \n {result.MatrixToString()}");
            return result;
        }
    }

}
