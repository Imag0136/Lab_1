using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab0
{
    class Neuron
    {
        byte t = 0; //номер эпохи
        byte error = 0;
        int limit = 0;
        public int[] imgArray; // матрица входов
        public double[,] weight = new double[10, 10000]; //матрица весовых коэффициентов
        double alpha = 0.3; //Скорость обучения
        double delta;
        int y; //фактический результат
        int yk; //ожидаемый результат
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
            for (int k = 0; k < 50; k++)
            {
                img = (Bitmap)Image.FromFile($"../../Resources/{k}.jpg");
                for (int n = 0; n < 10; n++)
                {
                    y = Output(n, img) > limit ? 1 : 0;
                    yk = n == (k / 5) ? 1 : 0;
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
            Console.WriteLine($"t = {t}");
            Console.WriteLine($"error = {error}");
            if (error > 0) Learn();
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

        public void Recognize(Bitmap img)
        {
            for (int n = 0; n < 10; n++)
            {
                if (Output(n, img) > limit)
                {
                    MessageBox.Show($"Это {n}");
                    break;
                }
            }
        }
    }
}
