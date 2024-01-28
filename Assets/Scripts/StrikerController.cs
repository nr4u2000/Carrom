using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StrikerController : MonoBehaviour
{

    [SerializeField]
    Slider StrikerSlider;
    [SerializeField]
    GameObject WarningText;

    Transform ForcePoint;
    Rigidbody2D rb;

    bool StrickerTouched;
    float ScaleValue = 0;


    public bool Shoot;
    public static StrikerController Instance;

    RaycastHit2D hit;
    // Start is called before the first frame update
    void Start()
    {

        // Here Set the Inital Settings for Stricker

        Instance = this;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.gameObject.SetActive(true);
        Shoot = false;
        StrickerTouched = false;
        StrikerXPos(0);
        StrikerSlider.onValueChanged.AddListener(StrikerXPos);
        ForcePoint = transform.GetChild(0).GetChild(0);
        rb = GetComponent<Rigidbody2D>();
        WarningText.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {

        if (StrickerTouched)                            
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);            // ScaleUp Stricker Aim Bg Which show Stricker Direction
            hit = Physics2D.Raycast(ray.origin, ray.direction);
            ScaleValue = Vector2.Distance(transform.position, hit.point);
            StrikerSlider.interactable = false;


            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);    // Rotates Stricker 
            Vector2 direction = mousePosition - transform.position;
            float angle = Vector2.SignedAngle(Vector2.down, direction);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            if (!Shoot)
            {
                StrikerSlider.interactable = true;          //Stricker Slider Control to not overlap Any Puck When it is not in Shoot Mode
                StrikerSlider.value = transform.position.x;
            }
            else
            {
                if (GetComponent<Rigidbody2D>().velocity.magnitude <= .05)      // Back to Stricker initalial Setting when Stricker Once Shoot And the its Force Reduced
                {
                    Start();
                }

            }
        }

        if (Input.GetMouseButtonUp(0))          // Applying Force To Stricker And Hide Aim Bg
        {
            StrickerTouched = false;
            rb.AddForce(new Vector3(ForcePoint.position.x - transform.position.x, ForcePoint.position.y - transform.position.y, 0) * ScaleValue, ForceMode2D.Impulse);
            ScaleValue = 0;
        }

        float ClampedScaleValue = Mathf.Clamp(ScaleValue, -20, 20);         // Restrict Aim Bg To not Scale after 20;

        transform.GetChild(0).localScale = new Vector3(ClampedScaleValue, ClampedScaleValue, ClampedScaleValue);    
    }

    private void OnMouseDown()
    {
        if (!Shoot)     // Enabling Shoot And Stricker Touched Bool When Touched this GameObject i.e. Stricker
        {
            Shoot = true;
            StrickerTouched = true;
            print("Pressed");
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Pucks")
        {
            if(!Shoot && !WarningText.activeInHierarchy)
            {
                StartCoroutine(OverlapWarning());       // This is used to throw Warning Massage When Stricker Trying to Overlap Puck when it is not in Shoot Mode i.e. inital Position
            }
        }

    }

    public void StrikerXPos(float Value)
    {
        transform.position = new Vector3(Value, transform.parent.position.y, 0);        // This function used to Slide Stricker according to Slide Value
    }

    IEnumerator OverlapWarning()
    {
        WarningText.SetActive(true);
        yield return new WaitForSeconds(.5f);
        WarningText.SetActive(false);
        StopAllCoroutines();
    }
}
