Shader "Vertex Color Standard Specular"
{
    Properties
    {
        _Specular("_Specular", Color) = (.2, .2, .2, 1)
		_Smoothness("Smoothness", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf StandardSpecular fullforwardshadows vertex:vert
		#pragma multi_compile_instancing

        #pragma target 3.0

        struct Input
        {
			float3 vertexColor;
        };

        half _Specular;
        half _Smoothness;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertexColor = abs(v.color);
		}

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
			o.Albedo = IN.vertexColor;
            o.Smoothness = _Smoothness;
			o.Specular = _Specular;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
