Shader "Unlit/HealthBarShader2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorMax ("ColorMax", Color) = (1,1,1,1)
        _ColorMin ("ColorMin", Color) = (1,1,1,1)
        _BackGroundColor ("BackGroudColor", Color) = (0,0,0,0)
        _MinPercent ("MinPercent",float ) = 0.2
        _MaxPercent ("MaxPercent",float ) = 0.8
        _Value ("Value", Range(0,1)) = 0.0
        _FlashSpeed ("FlashSpeed", Float) = 1
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutLineThickness ("Outline Thickness", Range(0.002, 0.10)) = 0.01
        _ObjectScale ("ObjectScale", Vector) = (1,1,1,1)


    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ColorMax;
            float4 _ColorMin;
            float _MinPercent;
            float _MaxPercent;
            float4 _BackGroundColor;
            float _Value;
            float _FlashSpeed;
            float4 _OutlineColor;
            float _OutLineThickness;
            float4 _ObjectScale;

            float MultiplyBy2PieAndFactor(float4 value, float factor)
            {
                float x = sin(value.x * (3.14159265359 * 2) * factor);
                float y = sin(value.y * (3.14159265359 * 2) * factor);

                return x * y;
            }
       

            float4 FlashingColor(float4 value, float4 value2, float factor)
            {
                return (MultiplyBy2PieAndFactor(value, factor) * value2);
            }

            float4 BarUVX(v2f i, float4 col)
            {
                float sineTime = sin(_Time.y);
                if (_Value > _MinPercent && _Value < _MaxPercent) col = lerp(_ColorMin, _ColorMax, _Value);
                if (_Value >= _MaxPercent)col = _ColorMax;
                if (_Value < i.uv.x) col = _BackGroundColor;
                if (_Value <= _MinPercent)
                {
                    if (_Value < i.uv.x) col = _BackGroundColor;
                    else
                    {
                       col = _ColorMin;
                    }
                }
                return col;
            }


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }
//   (0,0)  (1,0)  
            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);


                // draw outline according to outline thickness
                if (i.uv.y <= _OutLineThickness / _ObjectScale.y || i.uv.y >= 1 - _OutLineThickness / _ObjectScale.y ||
                    i.uv.x <= _OutLineThickness * _ObjectScale.x || i.uv.x >= 1 - _OutLineThickness * _ObjectScale.x)
                    col = _OutlineColor;

                // draw the bar according to the value
                else col = BarUVX(i, col);
                
                return col;
            }
            ENDCG
        }
    }
}