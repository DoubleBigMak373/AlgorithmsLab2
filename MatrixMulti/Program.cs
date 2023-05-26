using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace AlgorithmsLab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Алгоритм умножения матриц 

            // Размер матрицы A и B
            int matrixRang = int.Parse(Console.ReadLine());
            int matrixCol= int.Parse(Console.ReadLine());

            var matrix1 = RandomMatrix(matrixRang, matrixCol);
            var matrix2 = RandomMatrix(matrixRang, matrixCol);

            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            var result = MultiplyMatrix(matrix1, matrix2);
            sw.Stop();
            Console.WriteLine($"\n\n\n{sw.ElapsedMilliseconds}\n");

            for (int i = 0; i < matrixRang; i++)
            {
                for (int j = 0; j < matrixCol; j++)
                {
                    Console.WriteLine($"\n\n\n {result[i, j]}");
                }
            }
        }
        public static int[,] RandomMatrix(int matrixRang, int matrixCol)
        {
            int[,] A = new int[matrixRang, matrixCol];

            var rand = new Random();
            for (int i = 0; i < matrixRang; i++)
            {
                for (int j = 0; j < matrixCol; j++)
                {
                    A[i, j] = rand.Next(1, 5);
                    //Console.Write($"\n{A[i, j]} ");
                }
            }
            return A;
        }
        public static int RowsCount(int[,] matrix) 
        {
            return matrix.GetUpperBound(0) + 1;
        }
        public static int ColumsCount(int[,] matrix)
        {
            return matrix.GetUpperBound(1) + 1;
        }
        public static int[,] MultiplyMatrix(int[,] matrix1, int[,] matrix2)
        {
            var colsCount1 = ColumsCount(matrix1);
            var rowsCount1 = RowsCount(matrix1);

            var colsCount2 = ColumsCount(matrix2);
            var rowsCount2 = RowsCount(matrix2);
            
            
            if (colsCount1 != rowsCount2)
            {
                throw new Exception("Умножение невозможно! Количество столбцов первой матрицы не равно количеству строк второй матрицы.");
            }

            var matrix3 = new int[rowsCount1, colsCount2];    
            for(int i = 0; i < rowsCount1; i++)
            {
                for(int j = 0;j < colsCount2; j++)
                {
                    matrix3[i, j] = 0;
                    for(var k = 0; k < colsCount1; k++)
                    {
                        matrix3[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }
            return matrix3; 
        }
    }
}