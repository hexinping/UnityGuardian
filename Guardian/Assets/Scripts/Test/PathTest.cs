using UnityEngine;
using System.Collections;

public class PathTest : MonoBehaviour
{
    //建立一个目标圈物体
    public Transform heart;

    public GameObject ball;

    //获得路径节点坐标
    public iTweenPath path;

    //转换作用，由于抛物线只需三点，所以数组为3
    public Vector3[] paths = new Vector3[3];

    //射线信息，之所以要定义为全局变量，是因为这个信息会在很多地方用到
    RaycastHit hit = new RaycastHit();
    // Use this for initialization
    void Start()
    {
        //nodes[0]就是起点，起点始终为中心，所以起始定义一次即可
        path.nodes[0] = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //在Update中新建一个函数以实时获取位置
        PlaceHeart();



    }
    void PlaceHeart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //定义一个射线，从摄像机发出，目标位置为鼠标的位置
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //如果射线碰到名为plane的物体···
            if (Physics.Raycast(cameraRay.origin, cameraRay.direction, out hit, 100) && hit.transform.name == "Plane")
            {
                //iTween函数，位置更新，三个参数分别为，要更新位置的目标物，更新到的位置，更新的时间
                //（这个是正常写法，可以加入Hash写法，这个以后会讲到）

                //iTween.MoveUpdate(heart.gameObject, new Vector3(hit.point.x, hit.point.y + .5f, hit.point.z), .5f);

                iTween.MoveTo(heart.gameObject, new Vector3(hit.point.x, hit.point.y + .5f, hit.point.z), 1.0f);




                //nodes[2]就是终点，终点为鼠标左键点击时目标圈位置
                path.nodes[2] = new Vector3(hit.point.x, hit.point.y + .5f, hit.point.z);
                //抛物线的最高点，由于起点为000，所以xz的坐标为目标圈位置/2，最高点在此处为2，可自由调整
                path.nodes[1] = new Vector3(hit.point.x / 2, 3, hit.point.z / 2);


                //建立预设，起始位置为000，并强制转换为gameobject，这样才能Destory
                GameObject ball1 = (GameObject)Instantiate(ball, new Vector3(0, 0, 0), Quaternion.identity);

                //转换，因为iTween只接受数组，不接受List，而原生的nodes存储在List中，所以在此必须转换
                paths[0] = path.nodes[0];
                paths[1] = path.nodes[1];
                paths[2] = path.nodes[2];
                //moveTo移动到目标位置，此处用的是hash写法，  目标物，HASH表（路径移动，移动速度，移动方式） （每两个为一组）
                iTween.MoveTo(ball1, iTween.Hash("path", paths, "speed", 20f, "easeType", iTween.EaseType.easeOutQuad));
                //iTween.MoveTo(ball1, new Vector3(0, 1, 0), 0.2f);
                //销毁，物体，时间
                Destroy(ball1, 2);

            }

        }
        
    }
}
