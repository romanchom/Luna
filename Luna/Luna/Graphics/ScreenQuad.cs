using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Runtime.InteropServices;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Luna.Graphics {
	static class ScreenQuad {
        private static VertexBufferBinding binding;
        private static Buffer buffer;
        private static cBuffer data;
        private static InputLayout layout;
        private static VertexShader vertexShader;
        private static CompilationResult vertexShaderByteCode;
        private static Buffer vertices;

        public static void Init(SharpDX.Direct3D11.Device device)
        {
            vertexShaderByteCode = ShaderBytecode.CompileFromFile("Graphics/VertexShader.hlsl", "vertex", "vs_4_0", ShaderFlags.OptimizationLevel1, EffectFlags.None, null, null);
            vertexShader = new VertexShader(device, vertexShaderByteCode, null);
            vertices = SharpDX.Direct3D11.Buffer.Create<Vertex>(device, BindFlags.VertexBuffer, new Vertex[]
            {
                new Vertex(new Vector4(-1f, -1f, 0.5f, 1f), new Vector2(0f, 1f)),
                new Vertex(new Vector4(-1f, 1f, 0.5f, 1f), new Vector2(0f, 0f)),
                new Vertex(new Vector4(1f, -1f, 0.5f, 1f), new Vector2(1f, 1f)),
                new Vertex(new Vector4(-1f, 1f, 0.5f, 1f), new Vector2(0f, 0f)),
                new Vertex(new Vector4(1f, 1f, 0.5f, 1f), new Vector2(1f, 0f)),
                new Vertex(new Vector4(1f, -1f, 0.5f, 1f), new Vector2(1f, 1f))
            }, 144, ResourceUsage.Default, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), Vertex.elements);
            binding = new VertexBufferBinding(vertices, 24, 0);
            buffer = new SharpDX.Direct3D11.Buffer(device, 64, ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            data.offset = 0f;
            data.scale = 1f;
        }

        public static void Draw(DeviceContext context, float scale = 1f, float offset = 0f)
        {
            data.scale = scale;
            data.offset = offset;
            context.UpdateSubresource<cBuffer>(ref data, buffer, 0, 0, 0, null);
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            context.InputAssembler.InputLayout = layout;
            context.InputAssembler.SetVertexBuffers(0, binding);
            context.VertexShader.Set(vertexShader);
            context.VertexShader.SetConstantBuffer(1, buffer);
            context.Draw(6, 0);
        }

        public static void Dispose()
        {
            layout.Dispose();
            vertices.Dispose();
            vertexShader.Dispose();
            vertexShaderByteCode.Dispose();
        }

        [StructLayout(LayoutKind.Explicit, Size = 64)]
        private struct cBuffer
        {
            [FieldOffset(0)]
            public float scale;

            [FieldOffset(4)]
            public float offset;
        }

        private struct Vertex
        {
            public Vector4 position;

            public Vector2 uv;

            public static readonly InputElement[] elements = new InputElement[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                new InputElement("TEXTURE_COORD", 0, Format.R32G32_Float, 16, 0)
            };

            public const int stride = 24;

            public Vertex(Vector4 position, Vector2 uv)
            {
                this.position = position;
                this.uv = uv;
            }
        }
    }
}
