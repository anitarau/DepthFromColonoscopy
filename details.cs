
// Set virtual camera intrinsics to match endoscope intrinsics

Matrix4x4 originalProjection = cam.projectionMatrix;
Matrix4x4 p = originalProjection;
int w = cam.pixelWidth;
int h = cam.pixelHeight;
float near = 0.01f;  
float far = 20f; // Far clipping distance is set to 20cm as that's the max. length of any segment
float f_x = 591.5604f;
float f_y = 623.043f;
float c_x = 640.2557f;
float c_y = 503.4438f;

p[0] = 2.0f * f_x / w;
p[1] = 0.0f;
p[2] = 0.0f;
p[3] = 0.0f;

p[4] = 0.0f;
p[5] = 2.0f * f_y / h;
p[6] = 0.0f;
p[7] = 0.0f;

p[8] = 2.0f * c_x / w - 1.0f;
p[9] = 2.0f * c_y / h - 1.0f;
p[10] = -(far + near) / (far - near);
p[11] = -1.0f;

p[12] = 0.0f;
p[13] = 0.0f;
p[14] = -2.0f * far * near / (far - near);
p[15] = 0.0f;


cam.projectionMatrix = p;


// To record depth we use a linear depth shader where depth in [0,1] and a depth of 1 corresponds to the far clipping plane distance (20cm).
sampler2D_float _CameraDepthTexture;
struct gbuffer_out
{
    half4 depth         : SV_Target;
};
gbuffer_out copy_gbuffer(v2f I)
{
    half depth = Linear01Depth(tex2D(_CameraDepthTexture, get_texcoord_gb(I)));
    depth = 1.0 - depth;

    gbuffer_out O;
    O.depth = half4(depth.rrr, 1.0);
    return O;
}

