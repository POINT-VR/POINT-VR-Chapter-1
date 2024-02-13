using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;


    public class EndCreds : MonoBehaviour
    {
        public bool play;
        public int delay;
        public RectTransform textTrans;
        public Text text;
        public TextAsset creditsFile;

        public float lineHeight;
        public float yDistance;
        public float scrollSpeed;
        public int maxLinesOnScreen;

        private WaitForSeconds delayWFS;
        private float y;
        private Vector2 startingPos;
        private int linesDisplayed;

        private string[][] creditLines;
        private StringBuilder sb;

        public const string COLUMN_SEP = " - ";
        public const string ROW_SEP = "\n";

    public void Start()
    {
        delayWFS = new WaitForSeconds(delay);
        startingPos = textTrans.anchoredPosition;

        sb = new StringBuilder();
        text.text = "";

        textTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxLinesOnScreen * lineHeight);

        //Break up our credits file into a jagged array
        //Every return (\r\n) is a new row
        //Every comma (,) is a new column in that row
        string[] lines = creditsFile.text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        creditLines = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
            creditLines[i] = lines[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
            StartCoroutine(PlayCreditsDelayed());
    }

    private IEnumerator PlayCreditsDelayed()
    {
        yield return delayWFS;

        play = true;
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //  play = !play;
        //}

        if (!play)
            return;

        y += Time.deltaTime * scrollSpeed;

        while (y >= yDistance)
        {
            //Switch the alignment after the first credit has left the screen
            if (linesDisplayed > maxLinesOnScreen + 2 && text.alignment != TextAnchor.UpperCenter)
                text.alignment = TextAnchor.UpperCenter;

            LinesToText();

            y -= yDistance;

            linesDisplayed++;

            if (linesDisplayed > creditLines.Length)
                play = false;
        }

        textTrans.anchoredPosition = startingPos + new Vector2(0, y);
    }

    public void LinesToText()
    {
        sb.Length = 0;

        //The index will be at the first line, until it's off screen
        int rowIndex = Mathf.Max(0, linesDisplayed - maxLinesOnScreen);
        //Allows fill-in, full screen and fill-out
        int rowCount = Mathf.Min(linesDisplayed, maxLinesOnScreen, creditLines.Length - linesDisplayed);

        for (int i = 0; i < rowCount; i++)
        {
            //Build row
            for (int j = 0; j < creditLines[rowIndex].Length; j++)
            {
                //Only separator inbetween columns
                if (j > 0)
                    sb.Append(COLUMN_SEP);

                sb.Append(creditLines[rowIndex][j]);
            }
            rowIndex++;
            //Only separator inbetween rows
            if (i < rowCount - 1)
                sb.Append(ROW_SEP);
        }

        text.text = sb.ToString();
    }
}

