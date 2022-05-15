Shader "Unlit/SimpleToonUnlit"
{
    Properties
    {
         _MainTex("MainTex", 2D) = "white" {}
        _MainColor("Main Color", Color) = (1,1,1)
        _ShadowColor("Shadow Color", Color) = (0.7, 0.7, 0.8)
        _ShadowRange("Shadow Range", Range(0, 1)) = 0.5
        _ShadowSmooth("Shadow Smooth", Range(0, 1)) = 0.2

        [Space(10)]
        _OutlineWidth("Outline Width", Range(0, 0.05)) = 0.001
        _OutLineColor("OutLine Color", Color) = (0.5,0.5,0.5,1)

        [Space(10)]
        _RimColor("Rim Color", Color) = (0.7, 0.7, 0.8, 0.2)
        _RimSmooth("Rim Smooth", Range(0, 1)) = 0.2
        _RimMin("Rim Min", Range(0, 1)) = 0.8
        _RimMax("Rim Max", Range(0, 1)) = 1

    }
    SubShader   
    {
        Tags {"LightMode" = "ForwardBase"}

        Cull Back

        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half3 _MainColor;
            half3 _ShadowColor;
            half _ShadowRange;
            half _ShadowSmooth;
            float4 _RimColor;
            half _RimSmooth;
            half _RimMin;
            half _RimMax;


            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };


            v2f vert (appdata v)
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 col = 1;
                half4 mainTex = tex2D(_MainTex, i.uv);
                half3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
                half3 worldNormal = normalize(i.worldNormal);
                half3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
                half halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;
                half ramp = smoothstep(0, _ShadowSmooth, halfLambert - _ShadowRange);
                half3 diffuse = lerp(_ShadowColor, _MainColor, ramp);
                diffuse *= mainTex;
                half f = 1.0 - saturate(dot(viewDir, worldNormal));
                half rim = smoothstep(_RimMin, _RimMax, f);
                rim = smoothstep(0, _RimSmooth, rim);
                half3 rimColor = rim * _RimColor.rgb * _RimColor.a;
                col.rgb = _LightColor0 * (diffuse + rimColor);
                return col;
            }
            ENDCG
        }
     Pass
        {
            Tags {"LightMode" = "ForwardBase"}

            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            half _OutlineWidth;
            half4 _OutLineColor;

            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 vertColor : COLOR;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };


            v2f vert(a2v v)
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                float4 pos = UnityObjectToClipPos(v.vertex);
                float3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal.xyz);
                float3 ndcNormal = normalize(TransformViewToProjection(viewNormal.xyz)) * pos.w;//将法线变换到NDC空间
                ndcNormal.x *= abs(_ScreenParams.y / _ScreenParams.x);
                pos.xy += _OutlineWidth * ndcNormal.xy;
                o.pos = pos;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                return _OutLineColor;
            }
            ENDCG
        }
    UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

    }
}
