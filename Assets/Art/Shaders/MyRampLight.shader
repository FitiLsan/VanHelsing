Shader "MyShaders/MyRampLight"
{
   Properties 
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Ramp ("Ramp", 2D) = "gray" {}
    }

    SubShader 
    {
        Tags { "RenderType" = "Opaque" }
        Cull off
        CGPROGRAM
        #pragma surface surf Ramp        

        sampler2D _Ramp;
        
        half4 LightingRamp (SurfaceOutput s, half3 lightDir, half atten) 
        { 
            half NdotL = dot (s.Normal, lightDir);
            half diff = NdotL * 0.5 + 0.5;
            float2 rampUV = diff;
            half3 ramp = tex2D (_Ramp, rampUV).rgb; //tex2D (_Ramp, diff).rgb;
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (ramp);// * ramp;///= * atten;
            c.a = s.Alpha;
            return c;
        }
        
        struct Input 
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        
        void surf (Input IN, inout SurfaceOutput o) 
        {
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
        }
        ENDCG
    }
    Fallback "Diffuse"
}
