using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class RadialBlurEffect : PostEffectsBase {
 
    public Shader radialBlurShader;
    private Material radialBlurMaterial;


    public Material material
    {
        get {
            radialBlurMaterial = CheckShaderAndCreateMaterial(radialBlurShader, radialBlurMaterial);
            return radialBlurMaterial;
        }
    }

    //模糊程度，不能过高
    [Range(0,0.05f)]
    public float blurFactor = 1.0f;
    //模糊中心（0-1）屏幕空间，默认为中心点
    public Vector2 blurCenter = new Vector2(0.5f, 0.5f);

    //降低分辨率
    [Range(1, 8)]
    public int downSample = 2;

    [Range(0, 4)]
    public int iterations = 3;

    //清晰图像与原图插值
    [Range(0.0f, 2.0f)]
    public float lerpFactor = 0.5f;
 
    //OnRenderImage绘制绘制完所有透明和不透明的物体后每一帧调用
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {


            int rtW = src.width / downSample;
            int rtH = src.height / downSample;

            //分配一块缓冲区
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear;

            //把src图像数据缩放后存到buffer0中，使用第一个Pass
            Graphics.Blit(src, buffer0);

            for (int i = 0; i < iterations; i++)
            {
  
                RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

                //使用降低分辨率的rt进行模糊:pass0
                material.SetFloat("_BlurFactor", blurFactor);
                material.SetVector("_BlurCenter", blurCenter);

                // Render the vertical pass，使用第二个pass（序号为1）处理buffer0的数据，然后把数据传到buffer1缓冲区中
                Graphics.Blit(buffer0, buffer1, material, 0);
                //释放buffer0缓冲区数据，操作完之后一定要解绑，跟opengl的vao和vbo一样
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
            }


            //使用rt2和原始图像lerp:pass1
            material.SetTexture("_BlurTex", buffer0);
            material.SetFloat("_LerpFactor", lerpFactor);
            Graphics.Blit(src, dest, material, 1);
            RenderTexture.ReleaseTemporary(buffer0);  //最后一定要解绑
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}