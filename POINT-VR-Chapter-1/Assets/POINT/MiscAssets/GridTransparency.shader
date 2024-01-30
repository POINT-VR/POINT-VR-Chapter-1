Shader "Unlit/GridTransparency"
{
    Properties
    {
        _MainTex ("Color (RGB) Alpha (A)", 2D) = "gray" {}
        _HitPoint ("Hit point", Vector) = (1, 1, 1)
        _Distance ("Distance", float) = 4.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        #include "UnityCG.cginc"
        
        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        sampler2D _MainTex;
        half _TexUsage;
        float3 _HitPoint;
        fixed _Distance;

        void surf (Input IN, inout SurfaceOutputStandard o) {
            IN.uv_MainTex.x = frac(IN.uv_MainTex.x + frac(_Time.x));
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgba; // color??

            float dist = distance(_HitPoint, IN.worldPos);
            float minAlpha = 0.2;

            float st = step(_Distance, dist);
            float blend = (dist / _Distance) * (1 - st) + minAlpha * st;
            o.Alpha = blend; // try to make a smoother transition + color change for opaque sections
        }
        ENDCG
        
    }
    FallBack "Diffuse"
}
