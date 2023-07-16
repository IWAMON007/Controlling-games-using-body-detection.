using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private GameObject scoreText;
    private GameObject totalscoreText;

    //変数の作成
    private Transform[,] grid;

    [SerializeField]
    private Transform emptySprite; // ボード基板用の四角枠格納用    
    [SerializeField]
    private int height = 30, width = 10, header = 8;

    private void Awake(){
        grid = new Transform[width,height];
    }


    private void Start(){
        CreateBoard();
        scoreText = GameObject.Find("ScoreText");

    }


    //関数の作成    
    //ボードを生成する関数
    void CreateBoard(){
     if (emptySprite){
         for (int y = 0; y < height - header; y ++){
             for (int x = 0; x < width; x++){
                 Transform clone = Instantiate(emptySprite,
                     new Vector3(x,y,0), Quaternion. identity); 
                 clone.transform.parent = transform;
             }
         }
     }
    }

    //ブロックが枠内にあるのか判定する関数を呼ぶ関数
    public bool CheckPosition(Block block){
        foreach (Transform item in block.transform){
            Vector2 pos = Rounding.Round(item.position);
        
            if(!BoardOutCheck((int)pos.x, (int)pos.y)){
                return false;
            }

            if (BlockCheck((int)pos.x, (int)pos.y, block)){
                return false;
            }
    }
    return true;
}


    //ブロックが枠内にあるか判定する関数
    bool BoardOutCheck(int x, int y){

        //x軸が0以上width未満　y軸が０以上
        return (x >= 0 && x < width && y >=0);
    }

    //移動先にブロックがあるか判定する関数
    bool BlockCheck(int x, int y,Block block){
        //二次元配列が空でないのは他のブロックがあるとき
        return (grid[x,y] != null && grid[x,y].parent != block.transform);
    }

    //ブロックが落ちたポジションを記録する関数
    public void SaveBlockIngrid(Block block){
        foreach (Transform item in block.transform){
            Vector2 pos = Rounding.Round(item.position);

            grid[(int)pos.x, (int)pos.y] = item;
        }
    }

    //すべての行をチェックして、埋まっているなら行を削除する関数
    public void ClearAllRows(){
        for (int y = 0; y < height; y++){
            if (IsComplete(y)){
                ClearRow(y);

                ShiftRowsDown(y + 1);

                y--;
            }
        }
    }

    //すべての行をチェックする関数
    bool IsComplete(int y){
        for (int x = 0; x < width; x++){
            if (grid[x,y] == null){
                return false;
            }
        }

        return true;
        
    }

    //削除する関数
    void ClearRow(int y){
        scoreText.GetComponent<ScoreManager>().score = scoreText.GetComponent<ScoreManager>().score + 10;
        for (int x = 0; x < width; x++){
            if (grid[x, y] != null && grid[x, y].gameObject != null){
                Destroy(grid[x, y].gameObject);
            }
            grid[x, y] = null;
        }
    }


    //上にあるブロックを１段下げる関数
    void ShiftRowsDown(int Y){
        for( int y = Y; y < height; y++){
            for (int x = 0; x < width; x++){
                if (grid[x,y] != null){
                    grid[x, y - 1] = grid[x, y];
                    grid[x,y] = null;
                    grid[x,y-1].position += new Vector3(0,-1,0);
                }
            }
        }
    }

    public bool OverLimit(Block block){
        foreach (Transform item in block.transform){
            if (item.transform.position.y >= height - header){
                return true;
            }
        }

        return false;
    }

}
