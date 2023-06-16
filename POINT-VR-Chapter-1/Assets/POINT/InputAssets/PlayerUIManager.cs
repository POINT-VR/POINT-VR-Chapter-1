using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject SceneUIContainer;
    [SerializeField] private Image controlsImage;
    [Header("Instructions Text")]
    [SerializeField] private string instructionText;
    /**[SerializeField] private string turnText;
    [SerializeField] private string grabText;
    [SerializeField] private string pushPullText;
    [SerializeField] private string overText;
    [SerializeField] private string menuText;**/
    [Header("Instructions Timer")]
    [SerializeField] private int waitPeriod;

    // Cache
    private TMP_Text instructions = null;
    private Camera currentCamera = null;
    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        if (controlsImage != null)
        {
            instructions = controlsImage.GetComponentInChildren<TMP_Text>();
            controlsImage.gameObject.SetActive(false);
        }
        instructions = controlsImage.GetComponentInChildren<TMP_Text>();
        StartCoroutine(WaitForPlayerSpawn());
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentCamera != null)
        {
            this.transform.LookAt(currentCamera.transform);
            this.transform.Rotate(0, 180, 0);
        }
    }

    IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        // Start menu initialization
        currentCamera = Camera.current;
        this.GetComponent<Canvas>().worldCamera = currentCamera;
        player = currentCamera.transform.parent.gameObject;
        this.transform.SetParent(player.transform.parent, false);



        StartCoroutine(WaitUntilTime());

        yield break;
    }
    IEnumerator WaitUntilTime()
    {
        instructions.text = instructionText;
        controlsImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(waitPeriod);
        controlsImage.gameObject.SetActive(false);
        yield break;
    }
}
