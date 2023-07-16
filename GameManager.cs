using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using System;

public class GameManager : MonoBehaviour
{
    Spaner spaner;     //スポナー
    Block activeBlock; //生成されたブロックの格納

    //鼻の座標の位置を受け取り
    public static GameManager Instance { get; private set; }

    //鼻の座標の格納変数
    public Vector2 nosePosition;
    public Vector2 eyePosition;
    public float rotate_num;

    //入力受付けタイマー
    float nextKeyDownTimer;
    float nextKeyLeftRightTimer;
    float nextKeyRotateTimer;
    

    [SerializeField]
    private float dropInterval = 0.25f; //次にブロックが落ちるまでのインターバル
    float nextdropTimer; //次にブロックが落ちるまでの時間
    Board board;

    [SerializeField]
    //入力インターバル
    private float nextKeyDownInterval = 0.20f;
    private float nextKeyLeftRightInterval = 0.40f;
    private float nextKeyRotateInterval = 0.25f;

    //パネルの格納
    [SerializeField]
    private GameObject gameOverPanel;

    //ゲームオーバー判定
    bool gameOver;

    private void Awake(){
        // GameManagerのインスタンスを設定
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
           // Destroy(gameObject);
        }
    }

    private void Start(){
        //スポナーオブジェクトをスポナー変数に格納する
        spaner = GameObject.FindObjectOfType<Spaner>();

        //ボードを変数に格納する
        board = GameObject.FindObjectOfType<Board>();

        spaner.transform.position = Rounding.Round(spaner.transform.position);

        //タイマーの初期化
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

        //スポナークラスからブロック瀬々関数を読んで変数に格納する
        if (!activeBlock){
            activeBlock = spaner.SpawnBlock();
        }

        //ゲームオーバーパネルの非表示設定
        if (gameOverPanel.activeInHierarchy){
            gameOverPanel.SetActive(false);
        }



    }

    private void Update(){
        if(gameOver){
            return;
        }
        Debug.Log(Math.Abs(eyePosition.x - eyePosition.y));
//        if(Time.time > nextdropTimer){
//            nextdropTimer = Time.time + dropInterval;
//                if(activeBlock){
//                activeBlock.MoveDown();
//
//                //UpdateでBoardクラスの関数を呼び出してブロックがボードから出ていないか確認
//                if (!board.CheckPosition(activeBlock)){
//                    activeBlock.MoveUp();
//
//                    board.SaveBlockIngrid(activeBlock);
//
//                    activeBlock = spaner.SpawnBlock();
//                }
//            }
//        }

        PlayerInput();

        
    }

    void PlayerInput(){
        rotate_num = Math.Abs(eyePosition.x - eyePosition.y);

        if(nosePosition.x >= 0.6 && Time.time > nextKeyLeftRightTimer){
        //if(Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTimer) || Input.GetKeyDown(KeyCode.D)){
            activeBlock.MoveRigth();
            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
    
            if(!board.CheckPosition(activeBlock)){
                activeBlock.MoveLeft();
            }
        } else if(nosePosition.x <= 0.4 && Time.time > nextKeyLeftRightTimer){
        //else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTimer) || Input.GetKeyDown(KeyCode.A)){
            activeBlock.MoveLeft();
            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
    
            if(!board.CheckPosition(activeBlock)){
                activeBlock.MoveRigth();
            }
        } else if ((rotate_num >= 0.09) && (Time.time > nextKeyRotateTimer)){
        //else if ((Input.GetKey(KeyCode.E) && (Time.time > nextKeyRotateTimer)){
            Thread.Sleep(1000);
            activeBlock.RotateRight();
            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;
    
            if(!board.CheckPosition(activeBlock)){
                activeBlock.RotateLeft();
            }
        } else if((nosePosition.y >= 0.7) && (Time.time > nextKeyLeftRightTimer) || (Time.time > nextdropTimer)){
        //else if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyDownTimer) || (Time.time > nextdropTimer)){
            Thread.Sleep(100);
            activeBlock.MoveDown();
            nextKeyDownTimer = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;
    
            if(!board.CheckPosition(activeBlock)){
                if (board.OverLimit(activeBlock)){
                    GameOver();
                } else {
                    BottomBoard();
                }
            }
        }
    }


    void BottomBoard(){
        activeBlock.MoveUp();
        board.SaveBlockIngrid(activeBlock);

        activeBlock = spaner.SpawnBlock();

        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;

        board.ClearAllRows();
    }

    //ゲームオーバー
    void GameOver(){
        activeBlock.MoveUp();

        if (!gameOverPanel.activeInHierarchy){
            gameOverPanel.SetActive(true);
        }

        gameOver = true;
    }

    //シーンを再読み込み(ボタン押下で呼ぶ)
    public void Restart(){
        SceneManager.LoadScene(0);
    }
}


