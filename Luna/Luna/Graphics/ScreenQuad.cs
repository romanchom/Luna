using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

using Buffer = SharpDX.Direct3D11.Buffer;

namespace Luna.Graphics {
	static class ScreenQuad {
		private static Buffer vertices;
		private static InputLayout layout;
		private static CompilationResult vertexShaderByteCode;
		private static VertexShader vertexShader;
		private static VertexBufferBinding binding;

		[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
		private struct Vertex {
			public Vector4 position;
			public Vector2 uv;

			public Vertex(Vector4 position, Vector2 uv) {
				this.position = position;
				this.uv = uv;
			}

			public static readonly InputElement[] elements = new[]	{
					new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
					new InputElement("TEXTURE_COORD", 0, Format.R32G32_Float, 16, 0),
				};

			public const int stride = 24;
        }

		public static void Init(SharpDX.Direct3D11.Device device) {
			vertexShaderByteCode = ShaderBytecode.CompileFromFile("Graphics/VertexShader.hlsl", "vertex", "vs_4_0");
			vertexShader = new VertexShader(device, vertexShaderByteCode);
			
			vertices = Buffer.Create(device, BindFlags.VertexBuffer, new[]
				{
					new Vertex(new Vector4(-1, -1, 0.5f, 1.0f), new Vector2(0, 1)),
					new Vertex(new Vector4(-1,  1, 0.5f, 1.0f), new Vector2(0, 0)),
					new Vertex(new Vector4( 1, -1, 0.5f, 1.0f), new Vector2(1, 1)),

					new Vertex(new Vector4(-1,  1, 0.5f, 1.0f), new Vector2(0, 0)),
					new Vertex(new Vector4( 1,  1, 0.5f, 1.0f), new Vector2(1, 0)),
					new Vertex(new Vector4( 1, -1, 0.5f, 1.0f), new Vector2(1, 1)),
				}, 6 * 4 * 6);

			layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), Vertex.elements);

			binding = new VertexBufferBinding(vertices, Vertex.stride, 0);
        }

		public static void Draw(DeviceContext context) {
			context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			context.InputAssembler.InputLayout = layout;
			context.InputAssembler.SetVertexBuffers(0, binding);
			context.VertexShader.Set(vertexShader);
			context.Draw(6, 0);
		}

		public static void Dispose() {
			layout.Dispose();
			vertices.Dispose();
			vertexShader.Dispose();
			vertexShaderByteCode.Dispose();
		}
	}
}
