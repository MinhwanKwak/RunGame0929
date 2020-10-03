using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // 플레이어 상태
    public enum PlayerStatus
    {
        GROUND,
        JUMP,
        Coll,
        DASH,
        INVINCIBILITY, // 무적
        DEAD, // 사망
        CLEAR
    }

    public GameObject Materialobj;


    [Range(0, 50)]
    public float WalkSpeed = 10f;
    // clamp 조절용
    [SerializeField,Range(5,15)]
    private int minSpeed = 4;
    [SerializeField, Range(5, 15)]
    private int maxSpeed = 9;
    [SerializeField, Range(10, 25)]
    private int feverSpeed = 15;


    //jump 1단 
    [Range(0, 50)]
    public float OneJumpSpeed = 10f; //


    [Range(0, 30)]
    public float TwoJumpSpeed = 10f; //

    [Header("점프횟수")]
    public int Jumpcount = 2;

    private int startJumpcount = 0;

    //player 가 땅에 있는지 없는지 check
    public bool isGround = false;

    public GameObject player;

    public Rigidbody rb;

    public Animator animator;

    //무적시간 
    [Header("무적시간")]
    public float invincibilityTimeset = 1.0f;

    [SerializeField]
    private UiController uiController;

    private bool missonCheck = false;
    [SerializeField, Tooltip("상호작용 UI의 이동에 관련된 트랜스폼")]
    private RectTransform missonResultPos;

    [SerializeField, Header("데미지 나눌 퍼센트(장애물)")]
    private float damagePercnt1 = 0.3f;

    [SerializeField, Header("데미지 나눌 퍼센트(추락)")]
    private float damagePercnt2 = 0.5f;

    [SerializeField, Header("점프이펙트")]
    private GameObject JumpEffect;

    //현재 player 상태 저장 
    public PlayerStatus playerStatus = PlayerStatus.GROUND;

    //jump check
    private bool Jumpkey = false;

    //점프 강약 key check
    private float keyTime = 0f;

    //무적 switching
    private bool invincibility = false;

    //player에 rendering 정보 저장 
    private Renderer playerRenderer;

    //초기에 color값 저장
    private Color preplayerColor;

    private RaycastHit hit;

    //점프가 최고점에서 떨어질때를 체크 
    private bool checkJumpHigh = false;


    //점프 ray 사거리
    private float maxDistance = 0.2f;

    private bool isjump = false;

    //알약을 먹었을 때 플레이어 위치
    private Vector3 ItemEatPos;

    [SerializeField, Header("피버타임 시간")]
    private float fever = 2f;

    [SerializeField, Header("피버이펙트")]
    private GameObject feverEffect;

    [SerializeField, Header("아이템먹었을 때 위로 올라가는 높이")]
    private float ItemEatheight =1f;

    [SerializeField, Header("아이템 먹고 올라가는 시간")]
    private float ItemApexTime = 0.4f;

    public CapsuleCollider PlayerCapsule;

    private bool isObstacle = true;

    private bool isObstacleAnim = false;

    public float pushedShfitTime; // 쉬프트를 누른 시간 체크용
    public bool isMission = true; // 미션 수행 가능 확인
    public bool checkKey = false;
    private bool isDash = false;

    private void Awake()
    {
        GameManager.Instance.player = this;
    }

    void Start()
    {
        JumpEffect.SetActive(false);

        startJumpcount = Jumpcount;        
    }

    void Update()
    {

        //점프가 내려올때 체크 
        if (rb.velocity.y <= 0f && !checkJumpHigh)
        {
            checkJumpHigh = true;
        }


        //jump를 위한 raycast 
        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.5f))
        {

            if (Jumpcount >= 0)
            {
                if (playerStatus == PlayerStatus.DASH || playerStatus == PlayerStatus.CLEAR || playerStatus == PlayerStatus.DEAD) return;

                if (checkJumpHigh)
                {
                    checkJumpHigh = false;

                    if (hit.transform.gameObject.tag == "Ground")
                    {
                        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                        if (rb.velocity.y == 0)
                        {
                            animator.SetBool("DoubleJump", false);
                            animator.SetBool("Jump", false);
                        }

                        Jumpcount = startJumpcount;
                        playerStatus = PlayerStatus.GROUND;
                        JumpEffect.SetActive(false);
                    }
                }

            }
        }

      //  updateMissonResultPos(); // 테스트용

        if(playerStatus != PlayerStatus.DASH)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isjump && Jumpcount != 0)
            {
                isjump = true;
            }
        }

        // 키를 계속 누르고 있어서 미션을 성공 하는 것을 방지
        if (Input.GetKey(KeyCode.LeftShift))
        {
            checkKey = true; // 키를 누른 상태
            if (isMission)
            {               
                pushedShfitTime += Time.deltaTime;
               // print(pushedShfitTime += Time.deltaTime);
                if (pushedShfitTime > 0.5f)
                    isMission = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isMission = true;
            checkKey = false; // 키를 떈 상태
            pushedShfitTime = 0f; // 초기화
        }

        if(playerStatus == PlayerStatus.DEAD)
            animator.speed = 0;
    }

    // 물리 이동,점프 처리
    private void FixedUpdate()
    {
       
            if (isjump)
            {
                if (playerStatus == PlayerStatus.DASH) return;

                Jump();
            }


        // 죽으면 동작하지 않음
        //if (playerStatus == PlayerStatus.DEAD) return;

        if (playerStatus != PlayerStatus.CLEAR && playerStatus != PlayerStatus.DEAD)
        {
            var rig = rb.velocity;
            rig.x = 50 * Time.deltaTime * WalkSpeed;
            rb.velocity = rig;
        }
        else
            rb.velocity = new Vector3(0,rb.velocity.y,0); // 도착지점 도착 시 움직이지 못하게 처리함

        if (playerStatus != PlayerStatus.CLEAR && playerStatus != PlayerStatus.DEAD)
        {
            // 피버타임 날기 처리
            if (playerStatus == PlayerStatus.DASH)
            {
                Vector3 pos = rb.position;
                pos.y = Mathf.Lerp(rb.position.y, ItemEatPos.y, Time.deltaTime * ItemApexTime);

                rb.position = pos;
            }
        }
    }

    private void Jump()
    {
        animator.SetBool("Jump", true);

        playerStatus = PlayerStatus.JUMP;
        if(Jumpcount == 2)
        {
            if (playerStatus != PlayerStatus.DASH)
            {
                rb.AddForce(Vector3.up * OneJumpSpeed, ForceMode.Impulse);
            }
        }
        else
        {
            if (playerStatus != PlayerStatus.DASH && isDash == false)
            {
                animator.SetBool("DoubleJump", true);
                rb.AddForce(Vector3.up * TwoJumpSpeed, ForceMode.Impulse);
            }
        }
        JumpEffect.gameObject.transform.position = gameObject.transform.position;
       
        JumpEffect.SetActive(true);
        isjump = false;
        Jumpcount--;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 도착지점
        if (collision.gameObject.CompareTag("End"))
        {
            //print("끝(콜라이더)");
            //playerStatus = PlayerStatus.CLEAR;
            //animator.StopPlayback();
            //if (!rb.useGravity) // 중력 무시 상태일 때
            //    rb.useGravity = true;
            if (collision.gameObject.tag == "Ground")
            {
                JumpEffect.SetActive(false);
                isGround = true;
                Jumpcount = 2;
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            if (playerStatus == PlayerStatus.DASH)
            {
                playerStatus = PlayerStatus.GROUND;
                isDash = false;
                Physics.gravity = new Vector3(0, -9.81f, 0);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (playerStatus != PlayerStatus.DASH)
            {
                // 최고 속도로 도달했을 때 맥스 스피드 애니를 출력함
                if (WalkSpeed >= maxSpeed)
                {
                    animator.SetBool("Walk", false);
                    animator.SetBool("MaxSpeed", true);
                }
                else
                {
                    animator.SetBool("Walk", true);
                    animator.SetBool("MaxSpeed", false);
                }
            }
        }
    }

    private Vector3 respawnPos = Vector3.zero; // 리스폰 될 위치 저장
    [SerializeField, Header("장애물 넉백")]
    private float knockbackForce = 50f;

    private void OnTriggerEnter(Collider other)
    {
        // 우편물
        if (other.CompareTag("Mail"))
        {
            print("우편물");
            // ui 카운트 증가(최대 소지개수 15)
            uiController.mailCount++;
            uiController.mailCount = Mathf.Clamp(uiController.mailCount, 0, 15);
            // 캐릭터 이동속도 증가(우편물이 5개 이상일 때 부터)
            if (uiController.mailCount >= 5)
            {
                // 피버상태에서 원래 스피드로 되돌아 오는 현상 방지
                if (playerStatus == PlayerStatus.DASH) return;
                WalkSpeed++;
                WalkSpeed = Mathf.Clamp(WalkSpeed, minSpeed, maxSpeed);
                GameManager.Instance.cinemachineShake.SetFieldOfViewSizeParameters(2, +1); // 카메라 줌아웃 
            }
            // 우편물 삭제
            Destroy(other.gameObject);
        }

        // 장애물
        if (other.CompareTag("obstacle"))
        {
            print("장애물");


            if (!isObstacleAnim)
            {
                isObstacleAnim = true;
                animator.SetTrigger("Damage");
            }


            if (!isObstacle)
            {
                return;
            }

            StartCoroutine(startKnockback());

            // ui 카운트 감소
            float Damage = Mathf.Ceil((uiController.mailCount * damagePercnt1)); // 퍼센트로 나눈 값의 소수점 올림 처리하여 감소
            WalkSpeed--;
            WalkSpeed = Mathf.Clamp(WalkSpeed, minSpeed, maxSpeed);
            GameManager.Instance.cinemachineShake.SetFieldOfViewSizeParameters(2, -1); // 카메라 줌아웃 
            if (uiController.mailCount == 0)
                uiController.mailCount -= 1f;
            else
                uiController.mailCount -= Damage;
            // 죽음 판정
            checkedDead();
            uiController.mailCount = Mathf.Clamp(uiController.mailCount, 0, 15);
           
            //무적
            invincibility = true;

            StartCoroutine(invincibilityTime());

        }

        // 아이템
        if (other.CompareTag("Item"))
        {
            animator.SetTrigger("Drink");
            // 속도 증가
            StartCoroutine(feverTime());
            // 이펙트 활성화
          //  feverEffect.SetActive(true);
            //플레이어 현재 위치
            ItemEatPos = transform.position;
            ItemEatPos.y += ItemEatheight;
            // 아이템 삭제
            Destroy(other.gameObject);
            GameManager.Instance.cinemachineShake.SetFieldOfViewSizeParameters(fever+0.01f, 10); // 카메라 줌아웃 
        }

        // 리스폰 저장
        if (other.CompareTag("resSave"))
        {
            respawnPos = other.transform.position;
        }
        // 추락 후 리스폰 처리     
        if (other.CompareTag("resFall"))
        {
            // 데미지 처리
            float Damage = Mathf.Ceil((uiController.mailCount * damagePercnt2)); // 퍼센트로 나눈 값의 소수점 올림 처리하여 감소(50%)
            WalkSpeed--;
            WalkSpeed = Mathf.Clamp(WalkSpeed, minSpeed, maxSpeed);
            GameManager.Instance.cinemachineShake.SetFieldOfViewSizeParameters(2, -1); // 카메라 줌인
            if (uiController.mailCount == 0)
                uiController.mailCount -= 1f;
            else
                uiController.mailCount -= Damage;
            checkedDead();
            Invoke("respawnTime", 0.3f);
            // 죽음 판정
            uiController.mailCount = Mathf.Clamp(uiController.mailCount, 0, 15);
        }

        // 도착지점
        if (other.CompareTag("End"))
        {
            print("끝(트리거)");
            playerStatus = PlayerStatus.CLEAR;
            rb.velocity = Vector3.zero;
            animator.SetTrigger("success");
            StartCoroutine(endResult());
            //animator.StopPlayback();
           // animator.speed = 0;
            if (!rb.useGravity) // 중력 무시 상태일 때
                rb.useGravity = true;
        }
    }

    // 마무리 동작을 수행한 후 결과가 보여지도록 함 
    IEnumerator endResult()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Success") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        string CurrentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString(CurrentSceneName, uiController.culTimeCounts); // 베스트 스코어만 저장되도록 변경해야할듯
        uiController.ShowResult();
    }

    IEnumerator startKnockback()
    {
        float culSpeed = WalkSpeed; // 피버 전 스피드
        // 넉백
        rb.AddForce(Vector3.left * knockbackForce, ForceMode.Impulse);
        WalkSpeed = 1f; // 잠시 느려짐
        yield return new WaitForSeconds(0.5f);

        WalkSpeed = culSpeed; // 원래 스피드로 돌아옴
    }

    // 리스폰 시 걸리는 시간
    private void respawnTime()
    {
       // if (playerStatus == PlayerStatus.DEAD) return;
        transform.position = respawnPos;
    }

    // 죽음 체킹
    private void checkedDead()
    {
        if (uiController.mailCount < 0)
        {
            playerStatus = PlayerStatus.DEAD;
            uiController.GameOver();
           // Invoke("startReroad", 0.2f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 우편물 미션존
        if (other.CompareTag("Misson"))
        {
            // 타일 색
        
            print("미션존");
            // 우편물을 소지하고 있지 않으면
            if (uiController.mailCount <= 0) return;
            // 미션 가능 여부 채킹
            if ((checkKey && isMission) || playerStatus == PlayerStatus.DASH)
            {
                // 대쉬 상태에서 성공한 미션은 애니가 출력되지 않도록 함
                if(playerStatus != PlayerStatus.DASH)
                    animator.SetTrigger("SendMail");

                print("미션성공");
                missonCheck = true;
                // 우편물 카운트 감소
                uiController.mailCount--;
                uiController.mailCount = Mathf.Clamp(uiController.mailCount, 0, 15);
                if (uiController.mailCount >= 5 && WalkSpeed != 1)
                {
                    WalkSpeed--;
                    GameManager.Instance.cinemachineShake.SetFieldOfViewSizeParameters(2, -1); // 카메라 줌인
                }

                //미션 결과 활성화
                uiController.missionResultTx.text = "성공";
                uiController.missionResultTx.color = new Color32(0, 0, 255, 180);
                StartCoroutine(MissonResultAtive());

                // 미션 카운트 증가
                uiController.missionCount++;

                // 미션존 삭제
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Misson"))
        {
            if (missonCheck) return;
            //미션 결과 활성화
            uiController.missionResultTx.text = "실패";
            uiController.missionResultTx.color = new Color32(255, 0, 0, 180);
            StartCoroutine(MissonResultAtive());
        }
    }

    // 미션 결과 활성화
    IEnumerator MissonResultAtive()
    {
        missonResultPos.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        missonResultPos.gameObject.SetActive(false);
        missonCheck = false;
    }

    // 피버타임 활성화
    IEnumerator feverTime()
    {
        float culSpeed = WalkSpeed; // 피버 전 스피드

        WalkSpeed = feverSpeed; // 최고 스피드

        yield return new WaitForSeconds(0.01f);
        // 상태 변경
        playerStatus = PlayerStatus.DASH;

        isDash = true;

        // 중력 무시
        rb.useGravity = false;

        yield return new WaitForSeconds(fever);
        
        WalkSpeed = culSpeed; // 원래 스피드로 돌아옴
        animator.SetTrigger("Drink");
        rb.useGravity  = true; // 중력 활성화

        Physics.gravity = new Vector3(0, -20f, 0); // 중력 변경
      //  feverEffect.SetActive(false); // 이펙트 비활성화
    }

    // 미션 결과 ui 처리
    private void updateMissonResultPos()
    {
        //체력 UI가 할당되어 있지 않다면, 아래 코드 구문 실행 X
        if (missonResultPos == null) return;

        var UIPos = transform.position;
        UIPos = new Vector3(UIPos.x + 0.2f, UIPos.y += 1.2f, UIPos.z);

        missonResultPos.position = UIPos;
    }

    //피격당했을시에 무적시간  
    IEnumerator invincibilityTime()
    {
        int count = 0;    

        while(count < 10)
        {
            if (count % 2 == 0)
            {

                isObstacle = false;
                Materialobj.GetComponent<Renderer>().materials[1].SetColor("_BaseColor", new Color32(255, 255, 255, 255));
                Materialobj.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color32(255, 255, 255, 255));
                Color color =  Materialobj.GetComponent<Renderer>().material.GetColor("_BaseColor");
            }
            else
            {
                Materialobj.GetComponent<Renderer>().materials[1].SetColor("_BaseColor", new Color32(255, 255, 255, 0));
                Materialobj.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color32(255, 255, 255, 0));
            }



            yield return new WaitForSeconds(invincibilityTimeset);

            count++;
        }
        
         Materialobj.GetComponent<Renderer>().materials[1].SetColor("_BaseColor", new Color32(255, 255, 255, 255));
        Materialobj.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color32(255, 255, 255, 255));

        isObstacleAnim = false;
        isObstacle = true;
        //invincibility = false;
        
        yield return null;
    }

    

}
