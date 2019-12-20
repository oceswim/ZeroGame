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
    private bool menuOn;
    RaycastHit hit;
    Vector3 destination;
    private bool walking;
    void Start()
    {
        walking = menuOn=false;
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
                            destination = hit.transform.position;
                            Debug.Log(destination);
                            theCharAnimator.SetBool("isWalking", true);
                            theNavMesh.destination = destination;
                            walking = true;

                        }
                        if (!theNavMesh.pathPending && theNavMesh.pathStatus == NavMeshPathStatus.PathComplete && theNavMesh.remainingDistance == 0 && walking == true)
                        {
                            Debug.Log("YO");
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
            menuOn = true;
            GameManager.instance.Personnalisation();
        }
        else if(other.transform.name.Equals("Zone2"))
        {
            GameManager.instance.RecipeResearch();
            menuOn = true;
        }
    }



}
