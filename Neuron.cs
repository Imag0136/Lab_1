using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab0
{
    class Neuron
    {
        byte t = 0;
        byte error = 0;
        int limit = 0;
        public double[][,] weightArray = new double[10][,];
        public int[,] imgArray; // матрица входов
        public double[,] weight = new double[10, 10]; //матрица весовых коэффициентов
        double alpha = 0.5; //Скорость обучения
        double delta;
        int y; //фактический результат
        int yk; //ожидаемый результат

        public Neuron()
        {
            //Установление случайных весов
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int n = 0; n < 10; n++)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        weight[i, j] = Convert.ToDouble(rand.Next(-3, 4) / 10.0);
                    }
                }
                weightArray[n] = weight;
            }
        }

        public void Learn(Bitmap img)
        {
            t += 1;
            error = 0;
            double epsilon = 0;
            for (int k = 0; k < 100; k++)
            {
                img = new Bitmap($"{k}.jpg");
                int index = 0;
                for (int n = 0; n < 10; n++)
                {
                    if (Output(n, img) > epsilon) index = n;
                }
                if (index != k / 10)
                {
                    delta = yk - y;
                    for (int i = 0; i < imgArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < imgArray.GetLength(1); j++)
                        {
                            if (imgArray[i, j] == 1)
                            {
                                weightArray[index][i, j] += alpha * -1;
                                //weightArray[k / 10][i, j] += alpha;
                            }
                        }
                    }
                    error = 1;
                }
            }
            if (error == 1) Learn(img);
            else Console.WriteLine($"t = {t}");
        }

        public double Output(int n, Bitmap img)
        {
            double sum = 0; //сумма
            int[,] input = new int[100, 100];
            imgArray = new int[10, 10];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    if (img.GetPixel(i, j) == Color.FromArgb(255, 0, 0, 0))
                    {
                        input[i, j] = 1;                        
                    }
                }
            }
            LeadArray(input, imgArray);
            for (int i = 0; i < imgArray.GetLength(0); i++)
            {
                for (int j = 0; j < imgArray.GetLength(1); j++)
                {
                    sum += imgArray[i, j] * weightArray[n][i, j];
                }
            }
            return sum;
        }

        public void Recognize(Bitmap img)
        {
            for (int n = 0; n < 10; n++)
            {
                if (Output(n, img) > limit) MessageBox.Show($"Это {n}");
            }
        }

        // Пересчёт массива source в массив res, для приведения произвольного массива данных к массиву стандартных размеров.
        public static int[,] LeadArray(int[,] source, int[,] res)
        {
            for (var n = 0; n < res.GetLength(0); n++)
                for (var m = 0; m < res.GetLength(1); m++) res[n, m] = 0;

            var pX = (double)res.GetLength(0) / (double)source.GetLength(0);
            var pY = (double)res.GetLength(1) / (double)source.GetLength(1);

            for (var n = 0; n < source.GetLength(0); n++)
                for (var m = 0; m < source.GetLength(1); m++)
                {
                    var posX = (int)(n * pX);
                    var posY = (int)(m * pY);
                    if (res[posX, posY] == 0) res[posX, posY] = source[n, m];
                }
            return res;
        }
    }
}
