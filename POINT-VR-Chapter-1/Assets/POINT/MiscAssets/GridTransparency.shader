Shader "Unlit/GridTransparency"
{
    Properties
    {
        _MainTex ("Color (RGB) Alpha (A)", 2D) = "gray" {}
        _CenterCol ("Color (RGB) Alpha (A)", 2D) = "red" {}
        _CenterPoint ("Center Point", Vector) = (1, 1, 1)
        _Distance ("Distance", float) = 4.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0

        #include "UnityCG.cginc"
        
        struct Input {
            float2 uv_MainTex;
            float2 uv_CenterCol;
            float3 worldPos;
        };

        sampler2D _MainTex;
        sampler2D _CenterCol;
        float3 _CenterPoint;
        fixed _Distance;

        void surf (Input IN, inout SurfaceOutputStandard o) {

            float dist = distance(_CenterPoint, IN.worldPos) * 1.75; // constant scales the opaque sphere
            float minAlpha = 0.01; // minimum alpha value the grid can reach

            float st = step(_Distance, dist);
            float blend = ((_Distance + ((minAlpha - 1) * dist))/_Distance) * (1-st) + (minAlpha * st);
            o.Alpha = blend;
            
            // currently set to gray, uncomment second portion to add a color component via CenterCol
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgba * dist/* + tex2D(_CenterCol, IN.uv_CenterCol).rgba * (1 - st) * dist*/;
        }
        ENDCG
        
    }
    FallBack "Diffuse"
}