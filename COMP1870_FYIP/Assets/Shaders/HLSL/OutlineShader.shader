Shader "Unlit/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        //==== my custom inputs ====//
        _outlineDepth ("outlineDepth", Range(0, 0.5)) = 0.01
        _outlineColour ("outlineColour", Color) = (0, 0, 0, 1)
        //_baseMaterial ("baseMaterial", )
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

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

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
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

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

                //Object Space Position Node
                float3 posObjectSpace = v.vertex.xyz;

                //Object Space Normal Vector Node
                float3 normal = normalize(v.normal);
                //float3 normalObjectSpace = mul((float3x3)unity_ObjectToWorld, normal);


                v2f o;
                //o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                ////UNITY_TRANSFER_FOG(o,o.vertex);

                /*float3 a = _outlineDepth / ((objectScale.x + objectScale.y + objectScale.z) / 3);
                float3 b = (objectPos / camPos) / 2;
                float depth = a * b;

                float3 normalDepth = normalObjectSpace * depth;

                float3 vertPos = posObjectSpace + normalDepth;*/

                float3 a = _outlineDepth / ((objectScale.x + objectScale.y + objectScale.z) / 3);
                float3 b = distance(objectPos, camPos) * 0.5;

                float depth = a * b;

                float3 vertPos = posObjectSpace + (normal * depth);

                //float3 displacedPosition = v.vertex.xyz + normal * (_outlineDepth / objectScale.x);

                o.vertex = UnityObjectToClipPos(float4(vertPos, 1.0));

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col = _outlineColour;
                col.a = 1.0;
                return col;
            }
            ENDCG
        }
    }
}
