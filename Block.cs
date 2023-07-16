using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //変数の作成
    [SerializeField]
    private bool canRotate = true;

    //移動用関数
    void Move(Vector3 moveDirection){
        transform.position += moveDirection;
    }

    //移動用関数を呼ぶ関数（４種類）
    public void MoveLeft(){
        Move( new Vector3(-1,0,0));
    }

    public void MoveRigth(){
        Move( new Vector3(1,0,0));
    }

    public void MoveUp(){
        Move( new Vector3(0,1,0));
    }

    public void MoveDown(){
        Move( new Vector3(0,-1,0));
    }

    //回転関数（２種類）
    public void RotateRight(){
        if(canRotate){
            transform.Rotate(0,0,-90);
        }
    }

    public void RotateLeft(){
        if(canRotate){
            transform.Rotate(0,0,90);
        }
    }
}
