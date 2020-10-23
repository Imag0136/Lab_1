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
        public double[,] weight = new double[100, 100]; //матрица весовых коэффициентов
        double alpha = 0.7; //Скорость обучения
        double delta;
        int y; //фактический результат
        int yk; //ожидаемый результат

        public Neuron()
        {
            //Установление случайных весов
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int n = 0; n < 10; n++)
            {
                for (int i = 0; i < 100; i++)
                {
                    for (int j = 0; j < 100; j++)
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
            int[] y = new int[10];
            int[] yk = new int[10];
            for (int k = 0; k < 100; k++)
            {
                img = new Bitmap($"{k}.jpg");
                for (int n = 0; n < 10; n++)
                {
                    y[n] = Output(n, img) > limit ? 1 : 0;
                    yk[n] = k / 10 == n ? 1 : 0;
                    delta = yk[n] - y[n];
                    if (delta != 0)
                    {
                        for (int i = 0; i < img.Width; i++)
                        {
                            for (int j = 0; j < img.Height; j++)
                            {
                                weightArray[n][i, j] += alpha * delta;
                            }
                        }
                    }
                }
                if (index != k / 10)
                {
                    //delta = yk - y;
                    for (int i = 0; i < img.Width; i++)
                    {
                        for (int j = 0; j < img.Height; j++)
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
            
            imgArray = new int[100, 100];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    if (img.GetPixel(i, j) == Color.FromArgb(255, 0, 0, 0))
                    {
                        imgArray[i, j] = 1;
                        sum += imgArray[i, j] * weightArray[n][i, j];
                    }
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
    }
}
