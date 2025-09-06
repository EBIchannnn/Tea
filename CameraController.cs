using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] float moveSpeed = 5f;           // 通常移動速度
    [SerializeField] float sprintMultiplier = 2f;    // Shiftで加速
    // [SerializeField] float slowMultiplier = 0.3f;    // Altで減速

    // [Header("上昇/下降のキー（Space/LeftCtrlでも可）")]
    // [SerializeField] KeyCode upKey = KeyCode.E;      // 上昇
    // [SerializeField] KeyCode downKey = KeyCode.Q;    // 下降

    [Header("マウスルック設定")]
    // [SerializeField] bool enableMouseLook = true;    // 右クリックで視点回転
    [SerializeField] float lookSensitivity = 2.0f;   // マウス感度
    [SerializeField] float pitchMin = -89f;
    [SerializeField] float pitchMax = 89f;
    [Header("座りモーション設定")]
    [SerializeField] private float crouchHeight = 1.75f; // 座った時の高さ
    [SerializeField] float crouchMoveSpeed = 3f; // 座りモーションのスピード

    [Header("その他設定")]
    // 開始位置を指定
    public Vector3 startPosition = new Vector3(2.5f, 2.6f, -1.7f);
    public Vector3 startRotation = new Vector3(10, 0, 0);


    // 座りモーション用
    private bool isCrouching = false;
    private float targetY;
    // マウスルック用
    float yaw;
    float pitch;
    // UI表示状態（他のスクリプトで管理する想定）
    public static bool IsUIActive = true;

    void Awake()
    {
        // 初期回転を設定
        yaw = startRotation.y;
        pitch = startRotation.x;
    }
    void Start()
    {
        // カメラを開始位置へ移動
        transform.position = startPosition;
        // カメラの向きを設定
        transform.rotation = Quaternion.Euler(startRotation);
    }

    void Update()
    {
        // --- 視点回転（右クリック中） ---
        // if (enableMouseLook && Input.GetMouseButton(1))
        // {
        //     yaw   += Input.GetAxis("Mouse X") * lookSensitivity;
        //     pitch -= Input.GetAxis("Mouse Y") * lookSensitivity;
        //     pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        //     transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        //     Cursor.lockState = CursorLockMode.Locked;
        // }
        // else
        // {
        //     Cursor.lockState = CursorLockMode.None;
        // }

        // --- UI表示中はカーソル表示＆視点回転無効 ---
        if (IsUIActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return; // 視点回転・移動を無効化
        }

        // --- 視点回転（常時マウス移動で回転） ---
        yaw += Input.GetAxis("Mouse X") * lookSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * lookSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        Cursor.lockState = CursorLockMode.Locked;

        // --- 移動入力 ---
        float x = Input.GetAxisRaw("Horizontal"); // A/D
        float z = Input.GetAxisRaw("Vertical");   // W/S

        // float y = 0f;
        // if (Input.GetKey(upKey) || Input.GetKey(KeyCode.Space))        y += 1f;  // 上昇
        // if (Input.GetKey(downKey) || Input.GetKey(KeyCode.LeftControl)) y -= 1f; // 下降

        Vector3 inputDir = new Vector3(x, 0f, z);
        if (inputDir.sqrMagnitude > 1f) inputDir.Normalize();

        // --- 速度調整 ---
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) speed *= sprintMultiplier; // 加速
        // if (Input.GetKey(KeyCode.LeftAlt))   speed *= slowMultiplier;   // 減速

        // --- 移動（カメラの向き基準） ---
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 move = right * inputDir.x + forward * inputDir.z;
        if (move.sqrMagnitude > 1f) move.Normalize();
        transform.position += move * speed * Time.deltaTime;

        // 座りモーション（Cキーでy座標を下げる）
        // HandleCrouch();
    }

    private void HandleCrouch()
    {
        // Cキーで座る
        if (Input.GetKeyDown(KeyCode.C) && !isCrouching)
        {
            isCrouching = true;
            targetY = crouchHeight;
        }
        // Cキーで立つ
        else if (Input.GetKeyDown(KeyCode.C) && isCrouching)
        {
            isCrouching = false;
            targetY = startPosition.y;
        }

        // 毎フレーム目標座標へ移動
        MoveToY(targetY);
    }
    private void MoveToY(float targetY)
    {
        if (transform.position.y != targetY)
        {
            float newY = Mathf.MoveTowards(transform.position.y, targetY, crouchMoveSpeed * Time.deltaTime);
            var pos = transform.position;
            pos.y = newY;
            transform.position = pos;
        }
    }
}


