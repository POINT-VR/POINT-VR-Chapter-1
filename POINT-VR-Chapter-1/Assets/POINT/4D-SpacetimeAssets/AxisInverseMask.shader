Shader "Custom/AxisInverseMask"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)

        _RevealRadius ("Reveal Radius", Range(0, 100)) = 0.0
        _OpaqueRadius ("Opaque Radius", Range(0, 100)) = 0.0
        _ExponentialConstant ("Exponential Constant", Range(0, 2)) = 1.0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 200

        Pass {
            ZWrite On
            ColorMask 0
        }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows addshadow alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        fixed4 _Color;
        uniform float4 Grid_OriginPosition;
        half _RevealRadius;
        half _OpaqueRadius;
        half _ExponentialConstant;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Original color
            fixed4 c = _Color;

            fixed4 c_clear = fixed4(0, 0, 0, 0);
            half d = distance(Grid_OriginPosition, IN.worldPos);

            if (d >= _RevealRadius) {
                c = c_clear;
            } else if (d >= _OpaqueRadius) {
                c = lerp(c_clear, c, exp(_ExponentialConstant * (_OpaqueRadius - d)));
            }

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
