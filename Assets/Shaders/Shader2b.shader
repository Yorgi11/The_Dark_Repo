Shader "Custom/Specular_Shadow"
{
    Properties
    {
        _Color("Color", Color) = (1.0,1.0,1.0,1.0)
        _SpecColor("SpecColor", Color) = (1.0,1.0,1.0,1.0)
        _Shininess("Shininess", Float) = 1
    }
    SubShader
    {
        Tags {"LightMode" = "ForwardBase"}
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // shadow
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            // shadow
            #include "Lighting.cginc" 
            #include "AutoLight.cginc"

            uniform float4 _Color;
            //uniform float4 _SpecColor;
            uniform float _Shininess;
            //uniform float4 _LightColor0;

            struct vertexInput
            {
                float4 vertex: POSITION;
                float3 normal: NORMAL;
            };
            struct vertexOutput 
            {
                float4 pos: SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float4 normalDir : TEXCOORD1;
                // shadow
                SHADOW_COORDS(2)
                //
            };
            vertexOutput vert(vertexInput v) 
            {
                vertexOutput o;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject));
                o.pos = UnityObjectToClipPos(v.vertex);
                // shadow
                TRANSFER_SHADOW(o)
                //
                return o;
            }
            float4 frag(vertexOutput i) : COLOR
            {
               float3 normalDirection = i.normalDir;
               float atten = 1.0;
               // lighting
               float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
               float3 diffuseReflection = atten * _LightColor0.xyz * max(0.0, dot(normalDirection, lightDirection));
               // specular direction
               float3 lightReflectDirection = reflect(-lightDirection, normalDirection);
               float3 viewDirection = normalize(float3(float4(_WorldSpaceCameraPos.xyz, 1.0) - i.posWorld.xyz));
               float3 lightSeeDirection = max(0.0,dot(lightReflectDirection, viewDirection));
               float3 shininessPower = pow(lightSeeDirection, _Shininess);
               float3 specularReflection = atten * _SpecColor.rgb * shininessPower;
               float3 lightFinal = diffuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT;
               // shadow
               fixed shadow = SHADOW_ATTENUATION(i);
               //
               return float4(lightFinal * _Color.rgb, 1.0) * shadow;
            }
            ENDCG
        }
        // Cast Shadows
        Pass
        {
            Tags {"LightMode" = "ShadowCaster"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"
            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };
            struct v2f {
                V2F_SHADOW_CASTER;
            };
            v2f vert(appdata v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }
            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}