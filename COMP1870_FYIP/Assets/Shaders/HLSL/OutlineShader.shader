Shader "Unlit/OutlineShader"
{

    Properties
    {
        //==== my custom inputs ====//
        _outlineDepth ("outlineDepth", Range(0, 0.05)) = 0.05
        _outlineColour ("outlineColour", Color) = (0, 0, 0, 1)
        _baseTex("baseTexture", 2D) = "white" { }
        _baseNormal("baseNormal", 2D) = "white" { }

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

        //do outline pass
        Pass
        {
            Name "Outline"

            Tags { "Queue" = "Background" "LightMode" = "SRPDefaultUnlit"}
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

        //Render materials pass
        Pass    
        {
            Name "Materials"

            Tags { "Queue" = "Overlay" "LightMode" = "UniversalForward" }
            ZWrite Off
            ZTest LEqual
            ColorMask RGB
            
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
                float3 tangentNormal : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };


            //==== my custom inputs ====//
            sampler2D _baseTex;
            float4 _baseTex_ST;
            sampler2D _baseNormal;
            float4 _baseNormal_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.tangentNormal = v.normal;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = TRANSFORM_TEX(i.uv, _baseTex);
                float2 normaluv = TRANSFORM_TEX(i.uv, _baseNormal);

                half4 col = tex2D(_baseTex, uv);
                half3 norm = tex2D(_baseNormal, normaluv).xyz * 2.0 - 1.0;

                float3 finalNormal = normalize(i.tangentNormal + norm);

                float3 lightDir = normalize(float3(0.9, 0.6, 0.8));

                float NdotL = saturate(dot(finalNormal, lightDir));

                col.rgb *= (0.5 + 0.5 * NdotL); 

                return col;
            }
            ENDHLSL
        }

        
    }

    Fallback "Diffuse"
}
