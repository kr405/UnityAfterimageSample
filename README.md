![](https://img.shields.io/badge/license-MIT-green)

# URP Afterimage Effects
This is a project for demonstrating afterimage effects in Unity.<br>
By utilizing RenderMesh, you can render afterimages without the overhead of creating extra GameObjects.<br>
Please note that this project has been tested on Unity 2022.3.17f1 using URP 14.0.9.

# Demo
<img name="24_1219_afterimage_demo" src="https://github.com/user-attachments/assets/26f386d1-0f22-40cc-8ead-1ac80793510a" width="500px">
<br>
<img name="24_1226_afterimage_demo" src="https://github.com/user-attachments/assets/953610a7-3d45-4ec9-a265-8a334e13189f" width="500px">

The "RenderAfterimages" RendererFeature allows you to render afterimages that are not influenced by post-processing effects. 

# Note
Be aware that the "RenderAfterimages" feature may cause issues in scenes containing transparent objects.<br>
This project below provides a more general solution for handling scenes with Transparent objects, specifically regarding this feature.<br>
[URP Ignore PostProcessing](https://github.com/kr405/UnityIgnorePostProcessing)
