using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CharacterMovement : MonoBehaviour
{
    public Camera theCamera;
    public GameObject theCursor;
    private GameObject CursorPrefab;
    public static bool startedGame;
    public Animator theCharAnimator;
    public NavMeshAgent theNavMesh;
    public static bool menuOn;
    Vector3 destination;
    private bool walking;
    void Start()
    {
        walking = menuOn = false;
        destination = transform.position;
        startedGame = true;

    }
    private void Update()
    {

        if (startedGame)
        {
            CursorPrefab = Instantiate(theCursor);
            Debug.Log(CursorPrefab.name);
            CursorPrefab.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            startedGame = false;
        }
        if (!menuOn)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (!hit.transform.tag.Equals("Player"))
                {
                    Transform objectHit = hit.transform;

                    CursorPrefab.transform.position = new Vector3(hit.point.x, objectHit.position.y + 10, hit.point.z);

                    if (hit.transform.tag.Equals("Obstacle"))
                    {
                        CursorPrefab.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                    }
                    else if (hit.transform.tag.Equals("Accessible"))
                    {
                        if (CursorPrefab.GetComponent<MeshRenderer>().material.color.Equals(Color.red))
                        {
                            CursorPrefab.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            Debug.Log("CLICK");
                            theNavMesh.destination = hit.point;
                            
                            Debug.Log(hit.point);
                            theCharAnimator.SetBool("isWalking", true);
                            walking = true;

                        }
                        if (!theNavMesh.pathPending && theNavMesh.remainingDistance < 1f && walking == true)
                        { 
                            theCharAnimator.SetBool("isWalking", false);
                            walking = false;
                        }


                    }
                }
                // Do something with the object that was hit by the raycast.
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Equals("Zone1"))
        {
            GameManager.instance.loadCustomisation();
        }
        else if (other.transform.name.Equals("Zone2"))
        {
            GameManager.instance.RecipeResearch();
            menuOn = true;
        }
    }

   
}
