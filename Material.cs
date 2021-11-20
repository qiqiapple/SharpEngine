using System.IO;
using static OpenGL.Gl;

namespace SharpEngine
{
    public class Material
    {
        private readonly uint program;
        public int ratioLocation;

        public Material(string vertexShaderPath, string fragmentShaderPath)
        {
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            //glShaderSource(vertexShader, File.ReadAllText("shaders/position-color.vert"));
            glShaderSource(vertexShader, File.ReadAllText(vertexShaderPath));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            //glShaderSource(fragmentShader, File.ReadAllText("shaders/vertex-color.frag"));
            glShaderSource(fragmentShader, File.ReadAllText(fragmentShaderPath));
            glCompileShader(fragmentShader);

            // create shader program - rendering pipeline
            program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            
            glDeleteShader(vertexShader);
            glDeleteShader(fragmentShader);
            
            //glUseProgram(program);

            ratioLocation = glGetUniformLocation(program, "aspectRatio");
        }

        public unsafe void SetTransform(Matrix matrix)
        {
            int transformLocation = glGetUniformLocation(this.program, "transform");
            glUniformMatrix4fv(transformLocation,1,true,&matrix.m11);
        }

        public unsafe void SetView(Matrix matrix)
        {
            int viewTransLocation = glGetUniformLocation(this.program, "view");
            glUniformMatrix4fv(viewTransLocation, 1, true, &matrix.m11);
        }

        public unsafe void SetProjection(Matrix matrix)
        {
            int projectionLocation = glGetUniformLocation(this.program, "projection");
            glUniformMatrix4fv(projectionLocation, 1, true, &matrix.m11);
        }

        public void Use()
        {
            glUseProgram(program);
        }
    }
}