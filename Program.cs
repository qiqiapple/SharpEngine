using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        // private static Triangle triangle = new Triangle(
        // new Vertex[]{
        //         new Vertex(new Vector(0f, 0f), Color.Red),
        //         new Vertex(new Vector(1f, 0f), Color.Green),
        //         new Vertex(new Vector(0f, 1f), Color.Blue)
        //     }
        // );
        // private static Triangle triangle = new Triangle(
        //     new Vertex[]{
        //         new Vertex(new Vector(-0.1f, -0.1f), Color.Red),
        //         new Vertex(new Vector(0.1f, -0.1f), Color.Green),
        //         new Vertex(new Vector(0f, 0.1f), Color.Blue)
        //     }
        // );
        //
        // private static Triangle triangle2 = new Triangle(
        //     new Vertex[]{
        //         new Vertex(new Vector(0.4f, 0.4f), Color.Red),
        //         new Vertex(new Vector(0.6f, 0.4f), Color.Green),
        //         new Vertex(new Vector(0.5f, 0.6f), Color.Blue)
        //     }
        // );
        // private static Triangle triangle = new Triangle(0.2f, 0.2f, new Vector(0, 0));
        // private static Rectangle rectangle = new Rectangle(0.3f, 0.2f, new Vector(-0.15f, -0.1f));
        // private static Circle circle = new Circle(0.1f, new Vector(0.1f, -0.2f));
        // private static Cone cone = new Cone(0.1f, new Vector(-0.1f, -0.2f));

        // private static float multiplier_tri = 0.999f;
        // private static float multiplier_rec = 0.999f;
        // private static float multiplier_cir = 0.999f;
        // private static float multiplier_con = 0.999f;
        // private static float ratio;

        static void Main(string[] args)
        {
            var window = new Window();
            var material = new Material("shaders/world-position-color.vert", "shaders/vertex-color.frag");
            var scene = new Scene();
            window.Load(scene);
            var newShape = new Triangle(new Vertex[]
            {
                new Vertex(new Vector(-0.1f, 0f), Color.Red),
                new Vertex(new Vector(0.1f, 0f), Color.Green),
                new Vertex(new Vector(0f, 0.133f), Color.Blue)
            }, material);
            scene.Add(newShape);
            var multiplier = 0.999f;
            var ratioLocation = material.ratioLocation;

            // engine rendering loop
            while (window.IsOpen())
            {
                for (int i = 0; i < scene.shapes.Count; i++)
                {
                    var shape = scene.shapes[i];
                    if (shape.CurrentScale >= 1f) multiplier = 0.999f;
                    if (shape.CurrentScale <= 0.5f) multiplier = 1.001f;
                    //shape.Scale(multiplier);
                    shape.Move();
                    //shape.Rotate();
                    //shape.Bounce(window.WindowAspectRatio);
                }
                glUniform1f(ratioLocation, window.WindowAspectRatio);
                window.Render();
                //Play();
            }
        }
  
        // private static void Play()
        // {
        //     TrianglePlay();
        //     RectanglePlay();
        //     CirclePlay();
        //     ConePlay();
        // }
        //
        // private static void TrianglePlay()
        // {
        //     triangle.Bounce(ratio);
        //     triangle.Move();
        //     triangle.Rotate();
        //     if (triangle.CurrentScale >= 1f) multiplier_tri = 0.999f;
        //     if (triangle.CurrentScale <= 0.5f) multiplier_tri = 1.001f;
        //     triangle.Scale(multiplier_tri);
        // }
        //
        // private static void RectanglePlay()
        // {
        //     rectangle.Bounce(ratio);
        //     rectangle.Move();
        //     rectangle.Rotate();
        //     if (rectangle.CurrentScale >= 1.3f) multiplier_rec = 0.999f;
        //     if (rectangle.CurrentScale <= 0.3f) multiplier_rec = 1.001f;
        //     rectangle.Scale(multiplier_rec);
        // }
        //
        // private static void CirclePlay()
        // {
        //     circle.Bounce(ratio);
        //     circle.Move();
        //     circle.Rotate();
        //     if (circle.CurrentScale >= 0.9f) multiplier_cir = 0.999f;
        //     if (circle.CurrentScale <= 0.4f) multiplier_cir = 1.001f;
        //     circle.Scale(multiplier_cir);
        // }
        //
        // private static void ConePlay()
        // {
        //     cone.Bounce(ratio);
        //     cone.Move();
        //     cone.Rotate();
        //     if (cone.CurrentScale >= 0.7f) multiplier_con = 0.999f;
        //     if (cone.CurrentScale <= 0.3f) multiplier_con = 1.001f;
        //     cone.Scale(multiplier_con);
        // }
    }
}