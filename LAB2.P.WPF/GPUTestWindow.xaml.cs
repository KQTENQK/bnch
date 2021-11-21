using System;
using System.Diagnostics;
using System.Windows;

namespace LAB2.P.WPF
{

    public partial class GPUTestWindow : Window
    {
        private SharpGL.OpenGL _gl;
        private float _xzScreenSize;
        private Stopwatch _stopwatch = new Stopwatch();
        private TimeSpan _duration = new TimeSpan(0, 0, 50);
        private int _score = 0;
        private float _rotationAngle = 30;

        public event Action<int> Ended;

        public GPUTestWindow()
        {
            InitializeComponent();
        }

        private void OnOpenGLDraw(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            _stopwatch.Start();

            if (_stopwatch.Elapsed > _duration)
            {
                _stopwatch.Stop();
                int videoPoints = _score;
                Ended?.Invoke(videoPoints);
                Close();
            }

            _gl = args.OpenGL;
            _gl.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);

            _gl.LoadIdentity();
            _gl.Translate(0, 0f, -1f);
            _gl.Rotate(_rotationAngle, 1f, 1f, 1f);

            for (float xi = (-1 / _xzScreenSize) * 3; xi < (1 / _xzScreenSize) * 3; xi += .6f)
                for (float yi = -3; yi < 3; yi += .6f)
                    for (float zi = (-1 / _xzScreenSize) * 3; zi < (1 / _xzScreenSize) * 3; zi += .6f)
                        DrawPiramid(xi, yi, zi, ref _gl, .1f);

            _rotationAngle += 5;

            _score++;
        }

        private void DrawPiramid(float x, float y, float z, ref SharpGL.OpenGL gl, float size)
        {
            gl.Begin(SharpGL.Enumerations.BeginMode.Polygon);

            // down
            gl.Color((byte)250, (byte)0, (byte)189);
            gl.Normal(1.0f, 0.0f, 0.0f);
            gl.Vertex((x - 1.0f * size) * _xzScreenSize, 0, (z - 1.0f * size) * _xzScreenSize);
            gl.Vertex((x + 1.0f * size) * _xzScreenSize, 0, (z - 1.0f * size) * _xzScreenSize);
            gl.Vertex((x + 1.0f * size) * _xzScreenSize, 0, (z + 1.0f * size) * _xzScreenSize);
            gl.Vertex((x - 1.0f * size) * _xzScreenSize, 0, (z + 1.0f * size) * _xzScreenSize);

            gl.End();

            gl.Begin(SharpGL.Enumerations.BeginMode.Polygon);

            //first
            gl.Color((byte)9, (byte)37, (byte)217);
            gl.Normal(0.0f, 0.0, -1.0f);
            gl.Vertex((x - 1.0f * size) * _xzScreenSize, 0, (z - 1.0f * size) * _xzScreenSize);
            gl.Vertex((x - 1.0f * size) * _xzScreenSize, 0, (z + 1.0f * size) * _xzScreenSize);
            gl.Vertex(0, y + 1.0f * size, 0);

            gl.End();

            gl.Begin(SharpGL.Enumerations.BeginMode.Polygon);

            //Second
            gl.Color((byte)2, (byte)240, (byte)145);
            gl.Normal(0.0f, -1.0f, 0.0f);
            gl.Vertex((x - 1.0f * size) * _xzScreenSize, 0, (z - 1.0f * size) * _xzScreenSize);
            gl.Vertex((x + 1.0f * size) * _xzScreenSize, 0, (z - 1.0f * size) * _xzScreenSize);
            gl.Vertex(0, y + 1.0f * size, 0);

            gl.End();

            gl.Begin(SharpGL.Enumerations.BeginMode.Triangles);

            //Third
            gl.Color((byte)195, (byte)217, (byte)9);
            gl.Normal(0.0f, 1.0f, 0.0f);
            gl.Vertex((x + 1.0f * size) * _xzScreenSize, 0, (z + 1.0f * size) * _xzScreenSize);
            gl.Vertex((x + 1.0f * size) * _xzScreenSize, 0, (z - 1.0f * size) * _xzScreenSize);
            gl.Vertex(0, y + 1.0f * size, 0);

            gl.End();

            gl.Begin(SharpGL.Enumerations.BeginMode.Triangles);

            // Fourth
            gl.Color((byte)252, (byte)142, (byte)10);
            gl.Normal(0.0f, 0.0f, 1.0f);
            gl.Vertex((x - 1.0f * size) * _xzScreenSize, 0, (z + 1.0f * size) * _xzScreenSize);
            gl.Vertex((x + 1.0f * size) * _xzScreenSize, 0, (z + 1.0f * size) * _xzScreenSize);
            gl.Vertex(0, y + 1.0f * size, 0);

            gl.End();
        }

        private void OnOpenGLInitialized(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            SharpGL.OpenGL gl = args.OpenGL;
            gl.ClearColor(0.3f, 0.3f, 0.3f, 0.3f);
        }

        private void OnOpenGLControlResized(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            _xzScreenSize = (float)_glControl.ActualHeight / (float)_glControl.ActualWidth;
        }
    }
}
