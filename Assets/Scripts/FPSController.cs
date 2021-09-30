using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    //移動用の変数を作成
    float x, z;

    //スピード調整用の変数を作成
    float speed = 0.1f;

    //入力に合わせてプレイヤーの位置を変更していく。



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame(毎フレーム）
    void Update()
    {
        
    }

    //(0.02秒ごと)
    private void FixedUpdate()
    {
        x = 0;
        z = 0;


        x = Input.GetAxisRaw("Horizontal") * speed;
        //Horizonntal=水平
        z = Input.GetAxisRaw("Vertical") * speed;
        //Vertical=垂直

        transform.position += new Vector3(x,0,z);

    }
        
}
