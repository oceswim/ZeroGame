
using UnityEngine;
using UnityEngine.AI;
public class CharacterManager : MonoBehaviour
{
    public Camera theCamera;
    public GameObject theCursor, theRequester, theRecipe;
    private GameObject CursorPrefab;
    public static bool startedGame;
    public Animator theCharAnimator;
    public NavMeshAgent theNavMesh;
    public static bool menuOn;

    private bool walking;
    void Start()
    {
        walking = menuOn = false;
        startedGame = true;

    }
    private void Update()
    {

        if (startedGame)//allows to create our cursor  and initialise its color
        {
            CursorPrefab = Instantiate(theCursor);
            CursorPrefab.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            startedGame = false;
        }
        if (!menuOn)//if no menu is displayed
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//raycast thrown based on the mouse position
            if (Physics.Raycast(ray, out hit))
            {
                if (!hit.transform.tag.Equals("Player"))
                {
                    Transform objectHit = hit.transform;

                    CursorPrefab.transform.position = new Vector3(hit.point.x, objectHit.position.y + 10, hit.point.z);//cursor position set to where the raycast hits

                    if (hit.transform.tag.Equals("Obstacle"))//if object hit is an obstacle can't click
                    {
                        CursorPrefab.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                    }
                    else if (hit.transform.tag.Equals("Accessible"))//if object hit is accessible we can click and move our character
                    {
                        if (CursorPrefab.GetComponent<MeshRenderer>().material.color.Equals(Color.red))
                        {
                            CursorPrefab.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                        }
                        if (Input.GetMouseButtonDown(0))//if click detected
                        {


                            if (!walking)//is stopped set destination
                            {
                                GoToDestination(hit.point);
                            }
                            else//if already walking, reroute the navmesh
                            {
                                theNavMesh.isStopped = true;
                                GoToDestination(hit.point);
                            }


                        }

                    }
                }

            }
            if (!theNavMesh.pathPending && theNavMesh.remainingDistance <= theNavMesh.stoppingDistance && walking)
            {
                theNavMesh.isStopped = true;
                theCharAnimator.SetBool("isWalking", false);
                walking = false;
            }

        }


    }
    //everytime click detected 
    private void GoToDestination(Vector3 goal)
    {
        if (theNavMesh.isStopped)//if navmesh set to stopped we allow it to move again
        {
            theNavMesh.isStopped = false;
        }
        theCharAnimator.SetBool("isWalking", true);
        theNavMesh.destination = goal;
        walking = true;
    }
    private void OnTriggerEnter(Collider other)//if character detects the zones
    {
        if (other.transform.name.Equals("Zone1"))
        {
            GameManager.instance.LoadCustomisation();
        }
        else if (other.transform.name.Equals("Zone2"))
        {
            GameManager.instance.Pause();
            theRecipe.SetActive(true);
            theRequester.SetActive(true);
            menuOn = true;
        }
    }


}
