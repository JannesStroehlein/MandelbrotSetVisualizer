using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using OpenTK.Mathematics;

namespace MandelbrotSetVisualizer

{   // Here we'll be elaborating on what shaders can do from the Hello World project we worked on before.
    // Specifically we'll be showing how shaders deal with input and output from the main program 
    // and between each other.
    public class Window : GameWindow
    {
        /*
        private readonly float[] _vertices =
        {
            -0.5f, -0.5f, 0.0f, // Bottom-left vertex
             0.5f, -0.5f, 0.0f, // Bottom-right vertex
             0.0f,  0.5f, 0.0f  // Top vertex
        };*/
        private readonly float[] _vertices =
        {
            1f, 1f, 0f,
            -1f, 1f, 0f,
            -1f, -1f, 0f,
            -1f, -1f, 0f,
            1f, -1f, 0f,
            1f, 1f, 0f
        };

        private const float _defaultmouseSensitivity = 0.005f;

        private Vector2 _lastShift;
        private float _mouseSensitivity => _defaultmouseSensitivity / _zoom;
        private float _zoom = 1.0f;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _lastShift = new Vector2(0, 0);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);


            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Vertex attributes are the data we send as input into the vertex shader from the main program.
            // So here we're checking to see how many vertex attributes our hardware can handle.
            // OpenGL at minimum supports 16 vertex attributes. This only needs to be called 
            // when your intensive attribute work and need to know exactly how many are available to you.
            GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAttributeCount);
            Debug.WriteLine($"Maximum number of vertex attributes supported: {maxAttributeCount}");

            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();


            this.MouseDown += Window_MouseDown;
            this.MouseWheel += Window_MouseWheel;
        }

        private void Window_MouseWheel(MouseWheelEventArgs obj)
        {
            if (obj.OffsetY >= 1.0)
            {
                this._zoom = obj.OffsetY;
                this._zoom = _zoom * _zoom;
            }
            else
            {
                this._zoom = 0.5f;
            }
        }

        private void Window_MouseDown(MouseButtonEventArgs obj)
        {
            this.MouseMove += Window_MouseMove;
            this.MouseUp += Window_MouseUp;
        }
        private void Window_MouseUp(MouseButtonEventArgs obj)
        {
            this.MouseUp -= Window_MouseUp;
            this.MouseMove -= Window_MouseMove;       
        }
        private void Window_MouseMove(MouseMoveEventArgs obj)
        {
            _lastShift.X -= obj.DeltaX * _mouseSensitivity;
            _lastShift.Y += obj.DeltaY * _mouseSensitivity;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();

            int vertextShiftLoc = GL.GetUniformLocation(_shader.Handle, "shift");
            int vertextZoomtLoc = GL.GetUniformLocation(_shader.Handle, "zoom");

            // Here we're assigning the ourColor variable in the frag shader 
            // via the OpenGL Uniform method which takes in the value as the individual vec values (which total 4 in this instance).
            GL.Uniform2(vertextShiftLoc, _lastShift);
            GL.Uniform1(vertextZoomtLoc, _zoom);

            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}