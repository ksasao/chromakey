//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- ColorKeyAlphaEffect
//
//--------------------------------------------------------------------------------------

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D  implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

// Chroma Key Shader
// ref. https://www.shadertoy.com/view/4dX3WN
float3 rgb2hsv(float3 rgb)
{
	float Cmax = max(rgb.r, max(rgb.g, rgb.b));
	float Cmin = min(rgb.r, min(rgb.g, rgb.b));
	float delta = Cmax - Cmin;

	float3 hsv = float3(0., 0., Cmax);

	if (Cmax > Cmin){
		hsv.y = delta / Cmax;

		if (rgb.r == Cmax){
			hsv.x = (rgb.g - rgb.b) / delta;
		}
		else{
			if (rgb.g == Cmax){
				hsv.x = 2. + (rgb.b - rgb.r) / delta;
			}
			else {
				hsv.x = 4. + (rgb.r - rgb.g) / delta;
			}
		}
		hsv.x = hsv.x / 6.;
		hsv.x = frac(hsv.x);
	}
	return hsv;
}

float chromaKey(float3 color)
{
	float3 backgroundColor = float3(0., 0.416, 1.); // screen cool
	float3 weights = float3(4., 1., 2.);

	float3 hsv = rgb2hsv(color);
	float3 target = rgb2hsv(backgroundColor);
	float dist = length(weights * (target - hsv));
	return clamp(3. * dist - 1.5, 0., 1.);
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float4 color = tex2D( implicitInputSampler, uv );
   
   float incrustation = chromaKey(color.rgb);
   float3 color3 = color.rgb;
   color.a = incrustation;
   if (incrustation < 0.005 || uv.x > 0.99 || uv.y > 0.99)
	   color.rgba = 0;
   
   return color;
}
