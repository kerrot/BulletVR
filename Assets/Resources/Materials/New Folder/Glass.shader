Shader "Show Insides" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader{
		Tags{ "Queue" = "Transparent" }
		Pass{
		Material{
		Diffuse [_Color]
	}
		Lighting Off
		Cull Back
	}
	}
}