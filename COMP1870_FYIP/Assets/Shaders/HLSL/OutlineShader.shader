Shader "Unlit/OutlineShader"
{

    Properties
    {
        //==== my custom inputs ====//
        _outlineDepth ("outlineDepth", Range(0, 0.05)) = 0.05
        _outlineColour ("outlineColour", Color) = (0, 0, 0, 1)
        _baseTex("baseTexture", 2D) = "white" { }
        _darkness("darkness", Range(0, 1)) = 1
        _materialZTestMode("materialZTestMode", Float) = 4
        _outlineZTestMode("outlineZTestMode", Float) = 2

    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline"
            "UniversalMaterialType"="Unlit"
        }
        LOD 100


        //Regular passes for material and outline

        //Render materials pass AND ZTest LEqual
        Pass
        {
            Name "Materials"


            Tags { "Queue" = "Geometry" "LightMode" = "UniversalForward" }
            ZWrite On
            ZTest [_materialZTestMode]
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            Stencil {
                Ref 1
                Comp equal
                Pass keep
            }

            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#pragma multi_compile _ ZTEST_ALWAYS

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };


            //==== my custom inputs ====//
            sampler2D _baseTex;
            float4 _baseTex_ST;
            float _darkness;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = TRANSFORM_TEX(i.uv, _baseTex);

                half4 col = tex2D(_baseTex, uv);

                col.rgb *= _darkness;


                return col;
            }
            ENDHLSL
        }

        //Outline pass AND ZTest Less
        Pass
        {
            Name "Outline"

            Tags { "Queue" = "Overlay" "LightMode" = "SRPDefaultUnlit"}
            ZWrite On
            ZTest[_outlineZTestMode]
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            Stencil {
                Ref 1
                Comp always
                Pass replace
            }

            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#pragma multi_compile _ ZTEST_ALWAYS

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            //==== my custom inputs ====//
            float _outlineDepth;
            float4 _outlineColour;

            v2f vert (appdata v)
            {
                //Object Position Node
                float3 objectPos = v.vertex;

                //Object Scale Node
                float3 objectScale;
                objectScale.x = length(unity_ObjectToWorld._m00_m10_m20);
                objectScale.y = length(unity_ObjectToWorld._m01_m11_m21);
                objectScale.z = length(unity_ObjectToWorld._m02_m12_m22);

                //Camera Position Node
                float3 camPos = _WorldSpaceCameraPos;
                float3 camPosObjectSpace = mul(unity_WorldToObject, float4(camPos, 1.0)).xyz;

                //Object Space Position Node
                float3 posObjectSpace = v.vertex.xyz;

                //Object Space Normal Vector Node
                float3 normal = normalize(v.normal);

                v2f o;

                //based on Outline Shader Graph
                float objectScaleMean = (objectScale.x + objectScale.y + objectScale.z) / 3;
                float3 a = _outlineDepth / objectScaleMean;
                float3 b = distance(objectPos, camPosObjectSpace) * 0.25;
                
                float depth = a * b;

                float3 vertPos = posObjectSpace + (normal * depth);

                o.vertex = UnityObjectToClipPos(float4(vertPos, 1.0));

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {                
                fixed4 outline = _outlineColour;
                outline.a = 1.0;
                

                return outline;
            }
            ENDHLSL
        }

        
    }

    Fallback "Diffuse"
}
