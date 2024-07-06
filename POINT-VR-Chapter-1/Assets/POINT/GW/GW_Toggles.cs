using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GW_Toggles : MonoBehaviour
{
    // Attach this script to the GameObject that holds toggle menus.
    // Note: this file needs condensing - there is a lot of repeated code here that could be redesigned.

    [SerializeField] private GameObject sphere;
    [SerializeField] private GameObject tube;
    [SerializeField] private GameObject mesh;

    private GameObject player;
    private GameObject ModeMenuHolderToggles;
    private GameObject TheoryMenuHolderToggles;
    private GameObject RingMenuHolderToggles;

    private GW_Sphere SphereScript;
    private GW_Tube TubeScript;

    private GW_MeshSphere MeshSphere;
    private GW_MeshTube MeshTube;

    private int choice;

    // Start is called before the first frame update
    void Start()
    {
        RingMenuHolderToggles = gameObject.transform.GetChild(2).GetChild(0).GetChild(1).gameObject;
        ModeMenuHolderToggles = gameObject.transform.GetChild(3).GetChild(0).GetChild(1).gameObject;
        TheoryMenuHolderToggles = gameObject.transform.GetChild(4).GetChild(0).GetChild(1).gameObject;

        player = GameObject.FindGameObjectsWithTag("Player")[0];
        gameObject.transform.SetParent(player.transform);
        gameObject.transform.localPosition = new Vector3(9.93f, -3.12f, 19.37f);
        gameObject.transform.localRotation= Quaternion.Euler(-14.30f, -52.70f, 0.95f);

        choice = 0;

        SphereScript = sphere.GetComponent<GW_Sphere>();
        TubeScript = tube.GetComponent<GW_Tube>();

        MeshSphere = mesh.GetComponent<GW_MeshSphere>(); // MeshSphere is Instantaneous GWs
        MeshSphere.enabled = true;
        MeshTube = mesh.GetComponent<GW_MeshTube>(); // MeshTube is propgating
        MeshTube.enabled = false;

        sphere.SetActive(true);
        tube.SetActive(true); // Tube is set active here and inactive in Trails.cs
        mesh.SetActive(false);

        // set sphere toggle on
        RingMenuHolderToggles.transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn = true;
    }

    public void SetPlusMode(Toggle t) {
        if (choice == 0) {
            if (SphereScript.GetPlusMode() == 0) {
                SphereScript.SetPlusMode(30);
                t.isOn = true;
            } else {
                SphereScript.SetPlusMode(0);
                t.isOn = false;
            }
        } else if (choice == 1) {
            if (TubeScript.GetPlusMode() == 0) {
                TubeScript.SetPlusMode(30);
                t.isOn = true;
            } else {
                TubeScript.SetPlusMode(0);
                t.isOn = false;
            }
        } else {
            if (MeshSphere.GetPlusMode() == 0) {
                MeshSphere.SetPlusMode(30);
                t.isOn = true;
            } else {
                MeshSphere.SetPlusMode(0);
                t.isOn = false;
            }
            if (MeshTube.GetPlusMode() == 0) {
                MeshTube.SetPlusMode(30);
                t.isOn = true;
            } else {
                MeshTube.SetPlusMode(0);
                t.isOn = false;
            }
        }
    }

    public void SetCrossMode(Toggle t) {
        if (choice == 0) {
            if (SphereScript.GetCrossMode() == 0) {
                SphereScript.SetCrossMode(30);
                t.isOn = true;
            } else {
                SphereScript.SetCrossMode(0);
                t.isOn = false;
            }
        } else if (choice == 1) {
            if (TubeScript.GetCrossMode() == 0) {
                TubeScript.SetCrossMode(30);
                t.isOn = true;
            } else {
                TubeScript.SetCrossMode(0);
                t.isOn = false;
            }
        } else {
            if (MeshSphere.GetCrossMode() == 0) {
                MeshSphere.SetCrossMode(30);
                t.isOn = true;
            } else {
                MeshSphere.SetCrossMode(0);
                t.isOn = false;
            }
            if (MeshTube.GetCrossMode() == 0) {
                MeshTube.SetCrossMode(30);
                t.isOn = true;
            } else {
                MeshTube.SetCrossMode(0);
                t.isOn = false;
            }
        }
    }

    public void SetBreathingMode(Toggle t) {
        if (choice == 0) {
            if (SphereScript.GetBreathingMode() == 0) {
                SphereScript.SetBreathingMode(30);
                t.isOn = true;
            } else {
                SphereScript.SetBreathingMode(0);
                t.isOn = false;
            }
        } else if (choice == 1) {
            if (TubeScript.GetBreathingMode() == 0) {
                TubeScript.SetBreathingMode(30);
                t.isOn = true;
            } else {
                TubeScript.SetBreathingMode(0);
                t.isOn = false;
            }
        } else {
            if (MeshSphere.GetBreathingMode() == 0) {
                MeshSphere.SetBreathingMode(30);
                t.isOn = true;
            } else {
                MeshSphere.SetBreathingMode(0);
                t.isOn = false;
            }
            if (MeshTube.GetBreathingMode() == 0) {
                MeshTube.SetBreathingMode(30);
                t.isOn = true;
            } else {
                MeshTube.SetBreathingMode(0);
                t.isOn = false;
            }
        }
    }

    public void SetLongitudinalMode(Toggle t) {
        if (choice == 0) {
            if (SphereScript.GetLongitudinalMode() == 0) {
                SphereScript.SetLongitudinalMode(30);
                t.isOn = true;
            } else {
                SphereScript.SetLongitudinalMode(0);
                t.isOn = false;
            }
        } else if (choice == 1) {
            if (TubeScript.GetLongitudinalMode() == 0) {
                TubeScript.SetLongitudinalMode(30);
                t.isOn = true;
            } else {
                TubeScript.SetLongitudinalMode(0);
                t.isOn = false;
            }
        } else {
            if (MeshSphere.GetLongitudinalMode() == 0) {
                MeshSphere.SetLongitudinalMode(30);
                t.isOn = true;
            } else {
                MeshSphere.SetLongitudinalMode(0);
                t.isOn = false;
            }
            if (MeshTube.GetLongitudinalMode() == 0) {
                MeshTube.SetLongitudinalMode(30);
                t.isOn = true;
            } else {
                MeshTube.SetLongitudinalMode(0);
                t.isOn = false;
            }
        }
    }

    public void SetXMode(Toggle t) {
        if (choice == 0) {
            if (SphereScript.GetXMode() == 0) {
                SphereScript.SetXMode(30);
                t.isOn = true;
            } else {
                SphereScript.SetXMode(0);
                t.isOn = false;
            }
        } else if (choice == 1) {
            if (TubeScript.GetXMode() == 0) {
                TubeScript.SetXMode(30);
                t.isOn = true;
            } else {
                TubeScript.SetXMode(0);
                t.isOn = false;
            }
        } else {
            if (MeshSphere.GetXMode() == 0) {
                MeshSphere.SetXMode(30);
                t.isOn = true;
            } else {
                MeshSphere.SetXMode(0);
                t.isOn = false;
            }
            if (MeshTube.GetXMode() == 0) {
                MeshTube.SetXMode(30);
                t.isOn = true;
            } else {
                MeshTube.SetXMode(0);
                t.isOn = false;
            }
        }
    }

    public void SetYMode(Toggle t) {
        if (choice == 0) {
            if (SphereScript.GetYMode() == 0) {
                SphereScript.SetYMode(30);
                t.isOn = true;
            } else {
                SphereScript.SetYMode(0);
                t.isOn = false;
            }
        } else if (choice == 1) {
            if (TubeScript.GetYMode() == 0) {
                TubeScript.SetYMode(30);
                t.isOn = true;
            } else {
                TubeScript.SetYMode(0);
                t.isOn = false;
            }
        } else {
            if (MeshSphere.GetYMode() == 0) {
                MeshSphere.SetYMode(30);
                t.isOn = true;
            } else {
                MeshSphere.SetYMode(0);
                t.isOn = false;
            }
            if (MeshTube.GetYMode() == 0) {
                MeshTube.SetYMode(30);
                t.isOn = true;
            } else {
                MeshTube.SetYMode(0);
                t.isOn = false;
            }
        }
    }

    public void TheoryGeneralRelativity(Toggle t) {
        Toggle t0 = ModeMenuHolderToggles.transform.GetChild(0).GetComponent<Toggle>();
        Toggle t1 = ModeMenuHolderToggles.transform.GetChild(1).GetComponent<Toggle>();
        if (t0.isOn == t.isOn)
            SetPlusMode(t0);
        if (t1.isOn == t.isOn)
            SetCrossMode(t1);
        t.isOn = !t.isOn;
    }

    public void TheoryBransDicke(Toggle t) {
        Toggle t0 = ModeMenuHolderToggles.transform.GetChild(0).GetComponent<Toggle>();
        Toggle t1 = ModeMenuHolderToggles.transform.GetChild(1).GetComponent<Toggle>();
        Toggle t2 = ModeMenuHolderToggles.transform.GetChild(2).GetComponent<Toggle>();
        if (t0.isOn == t.isOn)
            SetPlusMode(t0);
        if (t1.isOn == t.isOn)
            SetCrossMode(t1);
        if (t2.isOn == t.isOn)
            SetBreathingMode(t2);
        t.isOn = !t.isOn;
    }

    public void TheoryEinsteinAether(Toggle t) {
        Toggle t0 = ModeMenuHolderToggles.transform.GetChild(0).GetComponent<Toggle>();
        Toggle t1 = ModeMenuHolderToggles.transform.GetChild(1).GetComponent<Toggle>();
        Toggle t2 = ModeMenuHolderToggles.transform.GetChild(2).GetComponent<Toggle>();
        Toggle t3 = ModeMenuHolderToggles.transform.GetChild(3).GetComponent<Toggle>();
        Toggle t4 = ModeMenuHolderToggles.transform.GetChild(4).GetComponent<Toggle>();
        Toggle t5 = ModeMenuHolderToggles.transform.GetChild(5).GetComponent<Toggle>();
        if (t0.isOn == t.isOn)
            SetPlusMode(t0);
        if (t1.isOn == t.isOn)
            SetCrossMode(t1);
        if (t2.isOn == t.isOn)
            SetBreathingMode(t2);
        if (t3.isOn == t.isOn)
            SetLongitudinalMode(t3);
        if (t4.isOn == t.isOn)
            SetXMode(t4);
        if (t5.isOn == t.isOn)
            SetYMode(t5);
        t.isOn = !t.isOn;
    }

    public void SetSphere(Toggle t) {
        if (t.isOn)
            sphere.SetActive(false);
        else
            sphere.SetActive(true);

        tube.SetActive(false);
        mesh.SetActive(false);

        // choice = 0;
        
        if (!t.isOn)
            SwitchStructures(0);
        
        t.isOn = !t.isOn;
    }

    public void SetTube(Toggle t) {
        if (t.isOn)
            tube.SetActive(false);
        else
            tube.SetActive(true);

        sphere.SetActive(false);
        mesh.SetActive(false);

        // choice = 1;
        
        if (!t.isOn)
            SwitchStructures(1);
        
        t.isOn = !t.isOn;
    }

    public void SetMesh(Toggle t) {
        if (t.isOn)
            mesh.SetActive(false);
        else
            mesh.SetActive(true);
            MeshSphere.enabled = true;
            MeshTube.enabled = false;

        sphere.SetActive(false);
        tube.SetActive(false);

        // choice = 2;
        
        if (!t.isOn)
            SwitchStructures(2);
        
        t.isOn = !t.isOn;
    }

    public void SetMeshTube(Toggle t) {
        if (t.isOn)
            mesh.SetActive(false);
        else
            mesh.SetActive(true);
            MeshSphere.enabled = false;
            MeshTube.enabled = true;

        sphere.SetActive(false);
        tube.SetActive(false);

        // choice = 2;
        
        if (!t.isOn)
            SwitchStructures(2);
        
        t.isOn = !t.isOn;
    }

    void SwitchStructures(int structure) {
        int old_choice = choice; // just for toggle resets
        int i = 0;
        foreach (Transform child in ModeMenuHolderToggles.transform) {
            Toggle x = child.GetComponent<Toggle>();
            choice = old_choice;
            if (x.isOn) {
                switch (i) { // determine which mode the child is for
                    case 0:
                        SetPlusMode(x); // first one will turn it off for the old mode
                        choice = structure;
                        SetPlusMode(x); // second one will set it for the new mode
                        break;
                    case 1:
                        SetCrossMode(x);
                        choice = structure;
                        SetCrossMode(x);
                        break;
                    case 2:
                        SetBreathingMode(x);
                        choice = structure;
                        SetBreathingMode(x);
                        break;
                    case 3:
                        SetLongitudinalMode(x);
                        choice = structure;
                        SetLongitudinalMode(x);
                        break;
                    case 4:
                        SetXMode(x);
                        choice = structure;
                        SetXMode(x);
                        break;
                    case 5:
                        SetYMode(x);
                        choice = structure;
                        SetYMode(x);
                        break;
                    default:
                        break;
                }
            }
            i++;
        }

        i = 0;

        foreach (Transform child in TheoryMenuHolderToggles.transform) {
            Toggle x = child.GetComponent<Toggle>();
            choice = old_choice;
            if (x.isOn) {
                x.isOn = false; // turn it off so it can be reset for the new choice
                switch (i) { // determine which mode the child is for
                    case 0:
                        TheoryGeneralRelativity(x);
                        choice = structure;
                        TheoryGeneralRelativity(x);
                        break;
                    case 1:
                        TheoryBransDicke(x);
                        choice = structure;
                        TheoryBransDicke(x);
                        break;
                    case 2:
                        TheoryEinsteinAether(x);
                        choice = structure;
                        TheoryEinsteinAether(x);
                        break;
                    default:
                        break;
                }
            }
            i++;
        }
        choice = structure;
    }
}
