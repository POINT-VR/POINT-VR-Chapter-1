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
                ///* old deflection function
                //project normal vectors onto view vector
                float3 project = dot(i.norm,i.view)*i.view;
                //subtract project out, gives vectors pointing inwards, relative to how far away from center. impact parameter
                float b =length(i.norm-project);
                //deflection function
                //float d = 1.0f/pow(b+0.8f,10);//deflection
                //o.worldRefl=d;
                
                
                
                //https://www.youtube.com/watch?v=nMv0OKAKkk4
                //spherical to linear. not working and causing fragments of the mesh. using old mmethod instead
                //float s2l = pow(1.0f-pow(dot(i.norm,i.view),2),0.5f);
                //smoothing function
                float smooth = b/(1.0f-b);
                //final deformation, 2 constants can be changed
                float const1 = 0.2f;
                float const2 = 2.0f;
                float d =const1/(const2*(const1-smooth));
                //build function to rotate view vector
                
                float3 axis=normalize(cross(i.norm,i.view));

                float s = sin(d*1.0f);
                float c = cos(d*1.0f);
                float oc = 1.0 - c;
                float3x3 rotmat = float3x3(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,
                oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,
                oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c);
                float3 outvector=mul(rotmat,i.view);

                //get skybox data from rotated vector
                half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, -outvector);
                half3 skyColor=DecodeHDR(skyData,unity_SpecCube0_HDR);
                fixed4 co = 0;
                co.rgb=skyColor;
                //if close to center, just draw the black hole.
                if(smooth<const1){
                    co.rgb=0;
                }
                //c.rgb = i.worldRefl;
                return co;
            }
            ENDCG
        }
    }
}
