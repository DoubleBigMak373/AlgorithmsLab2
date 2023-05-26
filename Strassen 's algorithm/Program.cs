using System.Diagnostics;

namespace Strassen__s_algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int matrixRang = int.Parse(Console.ReadLine());
            int matrixCol = int.Parse(Console.ReadLine());

            var matrix1 = RandomMatrix(matrixRang, matrixCol);
            var matrix2 = RandomMatrix(matrixRang, matrixCol);
            
            Strassen s = new Strassen();

            Stopwatch sw = Stopwatch.StartNew();

            sw.Start();
            s.Multiply(matrix1, matrix2);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            //for (int i = 0; i < matrixRang; i++)
            //{
            //    for (int j = 0; j < matrixCol; j++)
            //    {
            //        Console.WriteLine($"\n\n\n {result[i, j]}");
            //    }
            //}
        }
        public static int[,] RandomMatrix(int matrixRang, int matrixCol)
        {
            int[,] A = new int[matrixRang, matrixCol];

            var rand = new Random();
            for (int i = 0; i < matrixRang; i++)
            {
                for (int j = 0; j < matrixCol; j++)
                {
                    A[i, j] = rand.Next(1, 101);
                    //Console.Write($"{A[i, j]} ");
                }
            }
            return A;
        }
    }
    public class Strassen
    {
        public int[,] Multiply(int[,] A, int[,] B)
        {
            int n = A.GetLength(0);
            int half = n / 2;
            int[,] R = new int[n, n];

            //Определяем базовый случай
            if (n == 1)
            {
                R[0, 0] = A[0, 0] * B[0, 0];
            }
            else //Разделяем матрицы на подматрицы
            {
                int[,] A11 = new int[half, half];
                int[,] A12 = new int[half, half];
                int[,] A21 = new int[half, half];
                int[,] A22 = new int[half, half];

                int[,] B11 = new int[half, half];
                int[,] B12 = new int[half, half];
                int[,] B21 = new int[half, half];
                int[,] B22 = new int[half, half];

                //Разделяем матрицу A на 4 части
                Split(A, A11, 0, 0);
                Split(A, A12, 0, n / 2);
                Split(A, A21, n / 2, 0);
                Split(A, A22, n / 2, n / 2);

                //Разделяем матрицу B на 4 части
                Split(B, B11, 0, 0);
                Split(B, B12, 0, n / 2);
                Split(B, B21, n / 2, 0);
                Split(B, B22, n / 2, n / 2);

                /**
                * M1 = (A11 + A22)(B11 + B22) M2 = (A21 + A22) B11 M3 = A11 (B12 - B22) M4 =
                * A22 (B21 - B11) M5 = (A11 + A12) B22 M6 = (A21 - A11) (B11 + B12) M7 = (A12 -
                * A22) (B21 + B22)
                **/

                int[,] M1 = Multiply(Add(A11, A22), Add(B11, B22));
                int[,] M2 = Multiply(Add(A21, A22), B11);
                int[,] M3 = Multiply(A11, Sub(B12, B22));
                int[,] M4 = Multiply(A22, Sub(B21, B11));
                int[,] M5 = Multiply(Add(A11, A12), B22);
                int[,] M6 = Multiply(Sub(A21, A11), Add(B11, B12));
                int[,] M7 = Multiply(Sub(A12, A22), Add(B21, B22));

                /**
                * C11 = M1 + M4 - M5 + M7 C12 = M3 + M5 C21 = M2 + M4 C22 = M1 - M2 + M3 + M6
                **/
                int[,] C11 = Add(Sub(Add(M1, M4), M5), M7);
                int[,] C12 = Add(M3, M5);
                int[,] C21 = Add(M2, M4);
                int[,] C22 = Add(Sub(Add(M1, M3), M2), M6);

                Merge(R, C11, C12, C21, C22,n);
            }
            return R;
        }

        //Функция для разделения матрицы на подматрицы
        public void Split(int[,] parent, int[,] child, int rowStart, int colStart)
        {
            for (int i = 0, r = rowStart; i < child.GetLength(0); i++, r++)
                for (int j = 0, c = colStart; j < child.GetLength(1); j++, c++)
                    child[i, j] = parent[r, c];
        }

        //Функция для сложения матриц
        public int[,] Add(int[,] A, int[,] B)
        {
            int n = A.GetLength(0);
            int[,] result = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    result[i, j] = A[i, j] + B[i, j];
            return result;
        }

        //Функция для вычитания матриц
        public int[,] Sub(int[,] A, int[,] B)
        {
            int n = A.GetLength(0);
            int[,] C = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    C[i,j] = A[i,j] - B[i,j];
            return C;
        }

        ////Соединение подматриц
        //public void join(int[,] parent, int[,] child,int rowStart, int colStart)
        //{
        //    for (int i = 0, r = rowStart; i < child.GetLength(0); i++, r++)
        //        for (int j = 0, c = colStart; j < child.GetLength(1); j++, c++)
        //            parent[r,c] = child[i,j];
        //}

        public void Merge(int[,] C, int[,] C11, int[,] C12, int[,] C21, int[,] C22, int n)
        { 
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    C[i,j] = C11[i,j];
                    C[i,j + n / 2] = C12[i,j];
                    C[i + n / 2,j] = C21[i,j];
                    C[i + n / 2,j + n / 2] = C22[i,j];
                }
            }
        }

    }

}