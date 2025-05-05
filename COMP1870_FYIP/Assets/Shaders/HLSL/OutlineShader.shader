Shader "Unlit/OutlineShader"
{

    Properties
    {
        //==== my custom inputs ====//
        _outlineDepth ("outlineDepth", Range(0, 0.05)) = 0.05
        _outlineColour ("outlineColour", Color) = (0, 0, 0, 1)
        _baseTex("baseTexture", 2D) = "white" { }
        _baseNormal("baseNormal", 2D) = "white" { }
        [HideInInspector]_usesTwoMaterials("usesTwoMaterials", Range(0,1)) = 0
        [HideInInspector]_baseTex2("baseTexture2", 2D) = "white" { }
        [HideInInspector]_baseNormal2("baseNormal2", 2D) = "white" { }
    }
    CustomEditor "OutlineShaderGUI"

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline"
            "UniversalMaterialType"="Unlit"
        }
        LOD 100

        Pass
        {
            Cull Front

            CGPROGRAM
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

            sampler2D _baseTex;

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

                float objectScaleMean = (objectScale.x + objectScale.y + objectScale.z) / 3;
                float3 a = _outlineDepth / objectScaleMean;
                float3 b = distance(objectPos, camPosObjectSpace) * 0.25;
                //b *= 0.5;

                float depth = a * b;

                float3 vertPos = posObjectSpace + (normal * depth);

                o.vertex = UnityObjectToClipPos(float4(vertPos, 1.0));

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //fixed4 tex = tex2D(_baseTex, i.uv);
                
                fixed4 col = _outlineColour;
                col.a = 1.0;
                return col /** tex*/;
            }
            ENDCG
        }

        Pass
        {
            
            
        }
    }
}
