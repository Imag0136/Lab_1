using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab0
{
    class Neuron
    {
        int t = 0;
        int error = 0;
        int limit = 0;
        double sum = 0;
        public double[][,] weightArray = new double[10][,];
        
        public double[,] weight = new double[10, 10]; //матрица весовых коэффициентов

        int[,] input = new int[100, 100];
        int[,] imgArray = new int[10, 10]; // матрица входов

        Bitmap img;

        double alpha = 0.5; //Скорость обучения
        double delta;

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

        public void Learn()
        {
            t += 1;
            error = 0;
            int y; //фактический результат
            int yk; //ожидаемый результат
            for (int k = 0; k < 100; k++)
            {
                //img = new Bitmap($"{k}.jpg");
                img = (Bitmap)Image.FromFile($"{k}.jpg");
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
                // Уменьшение массива
                for (var n = 0; n < imgArray.GetLength(0); n++)
                    for (var m = 0; m < imgArray.GetLength(1); m++)
                        imgArray[n, m] = 0;

                var pX = (double)imgArray.GetLength(0) / (double)input.GetLength(0);
                var pY = (double)imgArray.GetLength(1) / (double)input.GetLength(1);

                for (var n = 0; n < input.GetLength(0); n++)
                    for (var m = 0; m < input.GetLength(1); m++)
                    {
                        var posX = (int)(n * pX);
                        var posY = (int)(m * pY);
                        if (imgArray[posX, posY] == 0) imgArray[posX, posY] = input[n, m];
                    }

                for (int n = 0; n < 10; n++)
                {
                    sum = 0;
                    for (int i = 0; i < imgArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < imgArray.GetLength(1); j++)
                        {
                            sum += imgArray[i, j] * weightArray[n][i, j];
                        }
                    }
                    y = sum > limit ? 1 : 0;
                    yk = n == (k / 10) ? 1 : 0;
                    if (y != yk)
                    {
                        delta = yk - y;
                        for (int i = 0; i < imgArray.GetLength(0); i++)
                        {
                            for (int j = 0; j < imgArray.GetLength(1); j++)
                            {
                                if (imgArray[i, j] == 1) weightArray[n][i, j] += alpha * delta;
                            }
                        }
                        error++;
                    }
                }
            }
            Console.WriteLine($"{t}");
            Console.WriteLine($"{error}");
            if (error > 0) Learn();
            else Console.WriteLine($"t = {t}");
        }

        //public double Output(int n, Bitmap img)
        //{
        //    double sum = 0; //сумма
        //    int[,] input = new int[100, 100];
        //    imgArray = new int[10, 10];
        //    for (int i = 0; i < img.Width; i++)
        //    {
        //        for (int j = 0; j < img.Height; j++)
        //        {
        //            if (img.GetPixel(i, j) == Color.FromArgb(255, 0, 0, 0))
        //            {
        //                input[i, j] = 1;                        
        //            }
        //        }
        //    }
        //    LeadArray(input, imgArray);
        //    for (int i = 0; i < imgArray.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < imgArray.GetLength(1); j++)
        //        {
        //            sum += imgArray[i, j] * weightArray[n][i, j];
        //        }
        //    }
        //    return sum;
        //}

        // Пересчёт массива source в массив res, для приведения произвольного массива данных к массиву стандартных размеров.
        //public static int[,] LeadArray(int[,] source, int[,] res)
        //{
        //    for (var n = 0; n < res.GetLength(0); n++)
        //        for (var m = 0; m < res.GetLength(1); m++) res[n, m] = 0;

        //    var pX = (double)res.GetLength(0) / (double)source.GetLength(0);
        //    var pY = (double)res.GetLength(1) / (double)source.GetLength(1);

        //    for (var n = 0; n < source.GetLength(0); n++)
        //        for (var m = 0; m < source.GetLength(1); m++)
        //        {
        //            var posX = (int)(n * pX);
        //            var posY = (int)(m * pY);
        //            if (res[posX, posY] == 0) res[posX, posY] = source[n, m];
        //        }
        //    return res;
        //}
        public void Recognize(Bitmap img)
        {
            for (int n = 0; n < 10; n++)
            {
                //if (Output(n, img) > limit) MessageBox.Show($"Это {n}");
            }
        }
    }
}
