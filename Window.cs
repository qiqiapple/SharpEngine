using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    public class Window
    {
        private readonly GLFW.Window window;
        private Scene scene;
        private int windowWidth, windowHeight;
        public float WindowAspectRatio { get; private set; }
        public bool IsOpen() => !Glfw.WindowShouldClose(window);

        public void Load(Scene scene)
        {
            this.scene = scene;
        }
        public Window()
        {
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.True);

            // create and launch a window
            window = Glfw.CreateWindow(768, 768, "SharpEngine", Monitor.None, GLFW.Window.None);
            Glfw.MakeContextCurrent(window);
            OpenGL.Gl.Import(Glfw.GetProcAddress);
        }
        
        private static void ClearScreen()
        {
            glClearColor(0.2f, 0.05f, 0.2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }
        
        public void Render()
        {
            Glfw.PollEvents();
            ClearScreen();
            this.scene?.Render();
            ChangeWindowSize();
            Glfw.SwapBuffers(window);
        }

        private void ChangeWindowSize()
        {
            Glfw.GetWindowSize(window, out windowWidth, out windowHeight);
            glViewport(0, 0, windowWidth, windowHeight);
            WindowAspectRatio = (float) windowWidth / windowHeight;
        }

        public bool GetKey(Keys key)
        {
            return Glfw.GetKey(this.window, key) == InputState.Press;
        }
    }
    
}