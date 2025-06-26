Shader "Unlit/BlinkBlink"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Color Change Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Generate a rainbow effect using time and sine waves
                float t = _Time.y * _Speed;
                float r = 0.5 + 0.5 * sin(t);
                float g = 0.5 + 0.5 * sin(t + 2.0);
                float b = 0.5 + 0.5 * sin(t + 4.0);

                fixed4 rainbowColor = fixed4(r, g, b, 1.0);
                return col * rainbowColor;
            }
            ENDCG
        }
    }
}
