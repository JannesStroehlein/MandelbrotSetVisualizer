#version 400 core

out vec4 outputColor;

in vec4 vertexColor;
in vec2 coord;
in vec2 coordShift;
in float coordZoom;

const float numOfIterations = 5000.0;

void main()
{
	float coordX = (coord.x / coordZoom) + coordShift.x;
	float coordY = (coord.y / coordZoom) + coordShift.y;
	float lastPointX = coordX;  
	float lastPointY = coordY;  

	float r2 = 0.0;
	int iter;
	for (iter = 0; iter < numOfIterations && r2 < 4.0; ++iter)
	{
		float tempreal = coordX;

		coordX = (tempreal * tempreal) - (coordY * coordY) + lastPointX;
		coordY = 2.0 * tempreal * coordY + lastPointY;
		r2   = (coordX * coordX) + (coordY * coordY);
	}
	vec3 color;

	if (r2 < 4.0)
		color = vec3(0.0f, 0.0f, 0.0f);
	else{
		if (iter < 8)
			color = vec3(1.0 / (8 - iter), 0.0, 0.0);
		else if (iter < 20)
			color = vec3(0.0, 1.0 / (20 - iter - 8), 0.0);
		else if (iter < 50)
			color = vec3(0.0, 0.0, 1.0);
		else if (iter < 500)
			color = vec3(0.0, 1.0, 1.0);
		else if (iter < 1000)
			color = vec3(1.0, 0.0, 1.0);
		else if (iter < 2000)
			color = vec3(0.5, 0.5, 0.5);
		else if (iter < 5000)
			color = vec3(1.0, 1.0, 0.0);
		else
			color = vec3(1.0f, 1.0f, 1.0f);
	}
	outputColor = vec4(color, 1.0);	
}