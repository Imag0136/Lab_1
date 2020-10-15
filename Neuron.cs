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
        public int[,] imgArray; // матрица входов
        public double[,] weightArray = new double[100, 100]; //матрица весовых коэффициентов
        double alpha = 0.4; //Скорость обучения
        double delta;
        int y; //фактический результат
        int yk; //ожидаемый результат

        public Neuron()
        {
            //Установление случайных весов
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    weightArray[i, j] = Convert.ToDouble(rand.Next(-3, 4) / 10.0);
                }
            }
        }

        public void Learn(Bitmap img)
        {
            t += 1;
            error = 0;
            for (int k = 0; k < 10; k++)
            {
                img = k < 5 ? new Bitmap($"Крестик{k}.jpg") : new Bitmap($"Нолик{k}.jpg");
                y = Output(img) > limit ? 1 : 0;
                yk = k < 5 ? 1 : 0;
                if (y != yk)
                {
                    delta = yk - y;
                    for (int i = 0; i < img.Width; i++)
                    {
                        for (int j = 0; j < img.Height; j++)
                        {
                            if (imgArray[i, j] == 1) weightArray[i, j] += alpha * delta;
                        }
                    }
                    error = 1;
                }
            }
            if (error == 1) Learn(img);
            else Console.WriteLine($"t = {t}");
        }

        public double Output(Bitmap img)
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
                        sum += imgArray[i, j] * weightArray[i, j];
                    }
                }
            }
            return sum;
        }

        public void Recognize(Bitmap img)
        {
            if (Output(img) > limit) MessageBox.Show("Это крестик");
            else MessageBox.Show("Это нолик");
        }
    }
}
