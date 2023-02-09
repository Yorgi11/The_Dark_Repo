Shader "Custom/Ink_Toon" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _RampTex("Ramp Texture", 2D) = "white"{}

        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness("Outline Thickness", Range(0,1)) = 0.1
    }
        SubShader{
            Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

            CGPROGRAM
        #pragma surface surf ToonRamp

        float4 _Color;
        sampler2D _RampTex;

        float4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten) {
            float diff = dot(s.Normal, lightDir);
            float h = diff * 0.5 + 0.5;
            float2 rh = h;
            float3 ramp = tex2D(_RampTex, rh).rgb;

            float4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * ramp;
            c.a = s.Alpha;
            return c;
        }

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _Color.rgb;
        }
        ENDCG

                //The second pass where we render the outlines
                Pass{
                    Cull Front

                    CGPROGRAM

                //include useful shader functions
                #include "UnityCG.cginc"

                //define vertex and fragment shader
                #pragma vertex vert
                #pragma fragment frag

                //tint of the texture
                fixed4 _OutlineColor;
                float _OutlineThickness;

                //the object data that's put into the vertex shader
                struct appdata {
                    float4 vertex : POSITION;
                    float4 normal : NORMAL;
                };

                //the data that's used to generate fragments and can be read by the fragment shader
                struct v2f {
                    float4 position : SV_POSITION;
                };

                //the vertex shader
                v2f vert(appdata v) {
                    v2f o;
                    //convert the vertex positions from object space to clip space so they can be rendered
                    o.position = UnityObjectToClipPos(v.vertex + normalize(v.normal) * _OutlineThickness * 0.1);
                    return o;
                }

                //the fragment shader
                fixed4 frag(v2f i) : SV_TARGET{
                    return _OutlineColor;
                }

                ENDCG
            }
        }
            FallBack "Standard"
}