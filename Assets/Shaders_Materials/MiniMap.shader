Shader "Unlit/MiniMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Threshold", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags {
            "RenderType"="Transparent"
            "Queue" = "Transparent"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        GrabPass
        {
            "_BackgroundTexture"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float4 grabPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BackgroundTexture;
            float _Threshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                o.grabPos = ComputeGrabScreenPos(UnityObjectToClipPos(v.vertex));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 grabColor = tex2Dproj(_BackgroundTexture, i.grabPos);
                float meido = (grabColor.a + grabColor.g + grabColor.b) / 3.0;

                float4 texCol = tex2D(_MainTex, i.uv);

                float4 normalColor = i.color;
                normalColor.a = texCol.a;

                float4 reversedColor = fixed4(1, 1, 1, 1) - i.color;
                reversedColor.a = texCol.a;

                return  meido < _Threshold ? reversedColor : normalColor;
            }
            ENDCG
        }
    }
}
