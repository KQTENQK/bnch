using MathNet.Numerics.Interpolation;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace LAB2.P.WPF
{
    public partial class MainWindow : Window
    {
        private DataTable _table = new DataTable();
        private DataTable _normalizedTable = new DataTable();
        private string _name = "this";
        private int _cpuScore = 0;
        private int _gpuScore = 0;
        private int _memoryScore = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDataGridWithDefault();

            CPUTest();
            GPUTest();
            MemoryTest();

            _table.Rows.Add(_name, _cpuScore, _gpuScore, _memoryScore, (_cpuScore + _gpuScore + _memoryScore) / 3);

            int maxCPU = 0;
            int maxGPU = 0;
            int maxMemory = 0;
            int maxOverall = 0;

            for (int i = 0; i < _table.Rows.Count - 1; i++)
            {
                if (int.Parse(_table.Rows[i][1].ToString()) > maxCPU)
                    maxCPU = int.Parse(_table.Rows[i][1].ToString());

                if (int.Parse(_table.Rows[i][2].ToString()) > maxGPU)
                    maxGPU = int.Parse(_table.Rows[i][2].ToString());

                if (int.Parse(_table.Rows[i][3].ToString()) > maxMemory)
                    maxMemory = int.Parse(_table.Rows[i][3].ToString());

                if (int.Parse(_table.Rows[i][4].ToString()) > maxOverall)
                    maxOverall = int.Parse(_table.Rows[i][4].ToString());
            }

            for (int i = 0; i < _table.Rows.Count - 1; i++)
            {
                float normalizedCPU = (float)int.Parse(_table.Rows[i][1].ToString()) / (float)maxCPU;
                float normalizedGPU = (float)int.Parse(_table.Rows[i][2].ToString()) / (float)maxGPU;
                float normalizedMemory = (float)int.Parse(_table.Rows[i][3].ToString()) / (float)maxMemory;
                float normalizedOverall = (float)int.Parse(_table.Rows[i][4].ToString()) / (float)maxOverall;

                _normalizedTable.Rows.Add($"PC{i + 1}", normalizedCPU, normalizedGPU, normalizedMemory, normalizedOverall);
            }

            float normalizedCPUThis = (float)int.Parse(_table.Rows[_table.Rows.Count - 1][1].ToString()) / (float)maxCPU;
            float normalizedGPUThis = (float)int.Parse(_table.Rows[_table.Rows.Count - 1][2].ToString()) / (float)maxGPU;
            float normalizedMemoryThis = (float)int.Parse(_table.Rows[_table.Rows.Count - 1][3].ToString()) / (float)maxMemory;
            float normalizedOverallThis = (float)int.Parse(_table.Rows[_table.Rows.Count - 1][4].ToString()) / (float)maxOverall;

            _normalizedTable.Rows.Add(_name, normalizedCPUThis, normalizedGPUThis, normalizedMemoryThis, normalizedOverallThis);
        }

        private void InitializeDataGridWithDefault()
        {

            _table.Columns.Add("Name");
            _table.Columns.Add("CPU Score");
            _table.Columns.Add("GPU Score");
            _table.Columns.Add("Memory Score");
            _table.Columns.Add("Overall");
            _dataGrid.ItemsSource = _table.DefaultView;

            _table.Rows.Add("PC1", 165, 196, 179, 180);
            _table.Rows.Add("PC2", 213, 513, 311, 346);
            _table.Rows.Add("PC3", 297, 677, 434, 469);

            _normalizedTable.Columns.Add("Name");
            _normalizedTable.Columns.Add("CPU Score");
            _normalizedTable.Columns.Add("GPU Score");
            _normalizedTable.Columns.Add("Memory Score");
            _normalizedTable.Columns.Add("Overall");

            _dataGridNormalized.ItemsSource = _normalizedTable.DefaultView;
        }

        private void MemoryTest()
        {
            string filePath = @"C:\BenchMark\Test.txt";
            string directory = @"C:\BenchMark";
            Directory.CreateDirectory(directory);

            TimeSpan testDuration = new TimeSpan(0, 0, 5);
            Stopwatch stopwatch = new Stopwatch();

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    stopwatch.Start();
                    Random random = new Random();
                    while (stopwatch.Elapsed < testDuration)
                    {
                        streamWriter.WriteLine("fewfkojcjancjiqwjndjwngfrhbneqjdw   qjdnwqkevjndskvjndfkjvnijhfnjiwefndskvmn fkjdvnjkwerfn nbdijvnmewlfewmnvj");
                        streamWriter.WriteLine("fewfkojcjancjiqwjndjwngfrhbneqjdw   qjdnwqkevjndskvjndfkjvnijhfnjiwefndskvmn fkjdvnjkwerfn nbdijvnmewlfewmnvj");
                        streamWriter.WriteLine("fewfkojcjancjiqwjndjwngfrhbneqjdw   qjdnwqkevjndskvjndfkjvnijhfnjiwefndskvmn fkjdvnjkwerfn nbdijvnmewlfewmnvj");
                        streamWriter.WriteLine("fewfkojcjancjiqwjndjwngfrhbneqjdw   qjdnwqkevjndskvjndfkjvnijhfnjiwefndskvmn fkjdvnjkwerfn nbdijvnmewlfewmnvj");
                        _memoryScore++;
                    }
                }

                stopwatch.Restart();

                while (stopwatch.Elapsed < testDuration)
                {
                    using (StreamReader streamReader = new StreamReader(filePath))
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(streamReader.ReadToEnd());
                        _memoryScore++;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            _memoryScore /= 10000;

            File.Delete(filePath);
            Directory.Delete(directory);
        }

        private void GPUTest()
        {
            var videoTest = new GPUTestWindow();
            videoTest.Ended += OnGpuTestEnded;
            videoTest.ShowDialog();
        }

        private void OnGpuTestEnded(int score)
        {
            _gpuScore = score;
        }

        private void CPUTest()
        {
            Random rand = new Random();

            const double e = 0.001;
            TimeSpan testTime = new TimeSpan(0, 0, 50);

            Stopwatch testStopwatch = new Stopwatch();

            testStopwatch.Start();

            while (testStopwatch.Elapsed < testTime)
            {
                List<double> values = new List<double>();

                for (int i = 0; i < 5; i++)
                {
                    values.AddRange(SolveDerivative(GenerateRandomPoints(10, rand), GenerateRandomPoints(10, rand), e));
                }

                ShellSort(values);

                _cpuScore++;
            }
        }

        private List<double> SolveDerivative(double[] xControlPoints, double[] yControlPoints, double e)
        {
            Vector<double> xPoints = DenseVector.OfArray(xControlPoints);
            Vector<double> yPoints = DenseVector.OfArray(yControlPoints);
            CubicSpline cubicSpline = CubicSpline.InterpolateNatural(xPoints, yPoints);

            List<double> derivativesValues = new List<double>();

            for (double t = 0; t < xControlPoints[xControlPoints.Length - 1]; t += e)
            {
                derivativesValues.Add(cubicSpline.Differentiate(t));
            }

            return derivativesValues;
        }

        private double[] GenerateRandomPoints(int count, Random random)
        {
            double[] points = new double[count];

            for (int i = 0; i < count; i++)
            {
                points[i] = random.Next(-100, 100) + random.Next(0, 4) * random.NextDouble();
            }

            System.Threading.Thread.Sleep(1);

            ShellSort(points);

            return points;
        }

        private void ShellSort(double[] array)
        {
            for (int i = array.Length / 2; i > 0; i = i / 2)
            {
                for (int j = i; j < array.Length; ++j)
                {
                    for (int k = j - i; k >= 0 && array[k] > array[k + i]; k = k - i)
                    {
                        double temp = array[k];
                        array[k] = array[k + i];
                        array[k + i] = temp;
                    }
                }
            }
        }

        private void ShellSort(List<double> array)
        {
            for (int i = array.Count / 2; i > 0; i = i / 2)
            {
                for (int j = i; j < array.Count; ++j)
                {
                    for (int k = j - i; k >= 0 && array[k] > array[k + i]; k = k - i)
                    {
                        double temp = array[k];
                        array[k] = array[k + i];
                        array[k + i] = temp;
                    }
                }
            }
        }
    }
}
