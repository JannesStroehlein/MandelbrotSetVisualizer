#version 330 core

// the position variable has attribute position 0
layout(location = 0) in vec3 aPosition; 

uniform vec2 shift;
uniform float zoom;

// This variable uses the keyword out in order to pass on the value to the 
// next shader down in the chain, in this case the frag shader
out vec4 vertexColor;
out vec2 coord;
out vec2 coordShift;
out float coordZoom;

void main(void)
{
	// see how we directly give a vec3 to vec4's constructor
    gl_Position = vec4(aPosition, 1.0);
	coord = aPosition.xy;
	coordShift = shift;
	coordZoom = zoom;
	// Here we assign the variable a dark red color to the out variable
	vertexColor = vec4(1.0, 1.0, 1.0, 1.0);
}