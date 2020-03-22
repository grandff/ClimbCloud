using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    float jumpForce = 680.0f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 2.0f;
    Animator animator;      // 애니메이션 적용
    float threshold = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 점프
        if(Input.GetKeyDown(KeyCode.Space) && this.rigid2D.velocity.y == 0){        // 점프 중인지 감지(중복 점프 방지). 플레이어의 속도는 velocity로 구할 수 있음
            this.animator.SetTrigger("JumpTrigger");        // trigger 실행
            this.rigid2D.AddForce(transform.up * this.jumpForce);       // AddForce 를 사용해 플레이어에 힘을 가함            
        }

        // 모바일 점프
        if(Input.GetMouseButtonDown(0) && this.rigid2D.velocity.y == 0){
            this.rigid2D.AddForce(transform.up * this.jumpForce);
        }
        
        // 좌우 이동
        int key = 0;
        if(Input.GetKey(KeyCode.RightArrow)) key = 1;
        if(Input.GetKey(KeyCode.LeftArrow)) key = -1;

        // 모바일 좌우 이동
        if(Input.acceleration.x > this.threshold) key = 1;      // acceleration 변수를 사용해 가속도 센서 값을 구함
        if(Input.acceleration.x < -this.threshold) key = -1;

        // 플레이어 속도
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        // 스피드 제한
        if(speedx < this.maxWalkSpeed){
            this.rigid2D.AddForce(transform.right * this.walkForce * key);      // 프레임마다 AddForce 메서드를 사용해 힘을 ㄱ속해서 가하면 플레이어는 가속함
        }

        // 움직이는 방향에 따라 반전시킴
        if(key != 0){
            transform.localScale = new Vector3(key, 1, 1);      // 스프라이트 배율을 바꾸려면 localScale 변수 값을 바꾸면 됨
        }

        // 플레이어 속도에 맞춰 애니메이션 속도를 바꿈
        if(this.rigid2D.velocity.y == 0){
            this.animator.speed = speedx / 2.0f;
        }else{
            this.animator.speed = 1.0f;
        }
        

        // 플레이어가 화면 밖으로 나갔다면 처음부터
        if(transform.position.y < -10){
            SceneManager.LoadScene("GameScene");
        }
    }

    // 깃발 도착 확인
    void OnTriggerEnter2D(Collider2D other) {       // OnTriggerEnter2D 는 충돌 상대의 오브젝트에 적용한 Collider가 전달됨
        Debug.Log("골");
        SceneManager.LoadScene("ClearScene");
    }
}
