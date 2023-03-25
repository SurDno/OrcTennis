 // Custom shader based on Unity Legacy Diffuse.
 // Adds team color property that colors a part of the textue into a selected color based on mask.
 
 Shader "TeamColor" {
 Properties {
     _Color ("Main Color", Color) = (1,1,1,1)
     _MainTex ("Base (RGB)", 2D) = "white" {}
     _TeamColor ("Team Color", Color) = (1,1,1,1)
     [NoScaleOffset] _TeamColorMask ("Team Color Mask", 2D) = "black" {}
 }
 
 SubShader {
     Tags { "RenderType"="Opaque" }
     LOD 200

 CGPROGRAM
 #pragma surface surf Lambert

 sampler2D _MainTex;
 fixed4 _Color;
 sampler2D _TeamColorMask;

struct Input {
    float2 uv_MainTex;
 };
 fixed4 _TeamColor;
 void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
    fixed4 mask = tex2D(_TeamColorMask, IN.uv_MainTex);
    // Apply team coloring
    o.Albedo = c.rgb + mask.r * _TeamColor.rgb;
    o.Alpha = c.a;
 }
 ENDCG
 }

 Fallback "Legacy Shaders/VertexLit"
 }
