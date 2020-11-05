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
        public int[] imgArray; // матрица входов
        public double[,] weight = new double[10, 10000]; //матрица весовых коэффициентов
        double alpha = 0.4; //Скорость обучения
        double delta;
        int y; //фактический результат
        int yk; //ожидаемый результат
 
        //public double[][,] weightArray = new double[10][,];
        
        //public double[,] weight = new double[10, 10]; //матрица весовых коэффициентов

        Bitmap img;

        public Neuron()
        {
            //Установление случайных весов
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10000; j++)
                {
                    weight[i, j] = Convert.ToDouble(rand.Next(-3, 4) / 10.0);
                }
            }
        }

        public void Learn()
        {
            t += 1;
            error = 0;
            for (int k = 0; k < 100; k++)
            {
                img = (Bitmap)Image.FromFile($"{k}.jpg");

                // Уменьшение массива________________________________________________________
                //for (var n = 0; n < imgArray.GetLength(0); n++)
                //    for (var m = 0; m < imgArray.GetLength(1); m++)
                //        imgArray[n, m] = 0;

                //var pX = (double)imgArray.GetLength(0) / (double)input.GetLength(0);
                //var pY = (double)imgArray.GetLength(1) / (double)input.GetLength(1);

                //for (var n = 0; n < input.GetLength(0); n++)
                //    for (var m = 0; m < input.GetLength(1); m++)
                //    {
                //        var posX = (int)(n * pX);
                //        var posY = (int)(m * pY);
                //        if (imgArray[posX, posY] == 0) imgArray[posX, posY] = input[n, m];
                //    }
                //__________________________________________________________________________

                for (int n = 0; n < 10; n++)
                {
                    y = Output(n, img) > limit ? 1 : 0;
                    yk = n == (k / 10) ? 1 : 0;
                    if (y != yk)
                    {
                        delta = yk - y;
                        for (int i = 0; i < 10000; i++)
                        {
                            if (imgArray[i] == 1) weight[n, i] += alpha * delta;
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

        public double Output(int n, Bitmap img)
        {
            double sum = 0; //сумма
            int t = 0;
            imgArray = new int[10000];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    if (img.GetPixel(i, j) == Color.FromArgb(255, 0, 0, 0))
                    {
                        imgArray[t] = 1;
                        sum += imgArray[t] * weight[n, t];
                    }
                    t++;
                }
            }
            return sum;
        }

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
