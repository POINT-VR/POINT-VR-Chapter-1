// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/NewUnlitShader"
{
    SubShader
    {

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            //vertex to fragment, what needs to be brought over
            struct v2f
            {
                //float3 worldDefl : TEXCOORD0;
                //view vector
                float3 view : TEXCOORD1;
                //pos vector
                float4 pos : SV_POSITION;
                //normal vector
                float3 norm : NORMAL;
            };

            v2f vert (float4 vertex : POSITION, float3 normal : NORMAL)
            {
                //build the object that wwill hold the data to send
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                //not needed
                float3 worldPos = mul(unity_ObjectToWorld, vertex).xyz;
                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                float3 worldNormal = UnityObjectToWorldNormal(normal);
                /*
                //o.worldRefl = reflect(-worldViewDir,worldNormal);
                float3 project = dot(-worldNormal,worldViewDir)*worldViewDir;
                float b =length(-worldNormal-project);
                float d = 1.0f/pow(b+0.94f,10);//deflection
                //o.worldRefl=d;
                float3 axis=normalize(cross(worldViewDir,worldNormal));
                float s = sin(d);
                float c = cos(d);
                float oc = 1.0 - c;
                float3x3 rotmat = float3x3(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,
                oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,
                oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c);
                float3 outvector=mul(rotmat,worldViewDir);
                */
                //o.worldDefl=-outvector;
                o.norm=worldNormal;
                o.view=worldViewDir;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //project normal vectors onto view vector
                float3 project = dot(-i.norm,i.view)*i.view;
                //subtract project out, gives vectors pointing inwards, relative to how far away from center
                float b =length(-i.norm-project);
                //deflection function
                float d = 1.0f/pow(b+0.94f,10);//deflection
                //o.worldRefl=d;
                //build function to rotate view vector
                float3 axis=normalize(cross(i.view,i.norm));
                float s = sin(d*1.1f);
                float c = cos(d*1.1f);
                float oc = 1.0 - c;
                float3x3 rotmat = float3x3(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,
                oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,
                oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c);
                float3 outvector=mul(rotmat,-i.view);

                //get skybox data from rotated vector
                half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, outvector);
                half3 skyColor=DecodeHDR(skyData,unity_SpecCube0_HDR);
                fixed4 co = 0;
                co.rgb=skyColor;
                //if close to center, just draw the black hole.
                if(b<.3f){
                    co.rgb=0;
                }
                //c.rgb = i.worldRefl;
                return co;
            }
            ENDCG
        }
    }
}
