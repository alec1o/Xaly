using Silk.NET.OpenGLES;
using XalyEngine;

public class Mesh : Node
{
    private Silk.NET.OpenGLES.GL gl => Application.gl!;
    private uint vao, vbo, vertexShader, fragmentShader, program;
    private float[] vertices = [0.0f, 0.5f, 0.0f, -0.5f, -0.5f, 0.0f, 0.5f, -0.5f, 0.0f,];
    const string vertexCode = @"
#version 330 core

layout (location = 0) in vec3 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
}";
    const string fragmentCode = @"
#version 330 core

out vec4 out_color;

void main()
{
    out_color = vec4(1.0, 0.5, 0.2, 1.0);
}";
    public override void OnInitialize()
    {
        vao = gl.GenVertexArray();
        gl.BindVertexArray(vao);

        vbo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
        GlBufferData(vertices);

        gl.EnableVertexAttribArray(0);
        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), nint.Zero);

        gl.BindVertexArray(0);
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);

        vertexShader = gl.CreateShader(ShaderType.VertexShader);
        gl.ShaderSource(vertexShader, vertexCode);
        gl.CompileShader(vertexShader);
        gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out int vStatus);
        if (vStatus != (int)GLEnum.True) throw new Exception("Vertex shader failed to compile: " + gl.GetShaderInfoLog(vertexShader));

        fragmentShader = gl.CreateShader(ShaderType.FragmentShader);
        gl.ShaderSource(fragmentShader, fragmentCode);
        gl.CompileShader(fragmentShader);
        gl.GetShader(fragmentShader, ShaderParameterName.CompileStatus, out int fStatus);
        if (fStatus != (int)GLEnum.True) throw new Exception("Fragment shader failed to compile: " + gl.GetShaderInfoLog(fragmentShader));

        program = gl.CreateProgram();
        gl.UseProgram(program);
        gl.AttachShader(program, vertexShader);
        gl.AttachShader(program, fragmentShader);
        gl.LinkProgram(program);
        gl.GetProgram(program, ProgramPropertyARB.LinkStatus, out int lStatus);
        if (lStatus != (int)GLEnum.True) throw new Exception("Program failed to link: " + gl.GetProgramInfoLog(program));

        gl.DetachShader(program, vertexShader);
        gl.DetachShader(program, fragmentShader);
        gl.DeleteShader(vertexShader);
        gl.DeleteShader(fragmentShader);
    }

    public override void OnRender()
    {
        gl.BindVertexArray(vao);
        gl.UseProgram(program);
        gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    private unsafe void GlBufferData(float[] data)
    {
        fixed (float* buffer = data)
        {
            gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(data.Length * sizeof(float)), buffer, BufferUsageARB.StaticDraw);
        }
    }
}