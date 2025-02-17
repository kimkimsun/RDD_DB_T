using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody rigid;

    public Vector3 move;

    public int Lv;

    private void Awake()
    {
        var obj = FindObjectsOfType<Player_Controller>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.z = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + move * moveSpeed * Time.fixedDeltaTime);
    }
}
