using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    int playCount = 0;
    public GameObject[] cells;
    GameObject currentSelectedObj;
    public Text youWinText;
    bool isGameOver;
    public Text turnText;
    public Sprite[] xoImg;
    string machineTag = "O";
    string playerTag = "X";
    bool isMachinePlayed = false;
    private void Start()
    {
        turnText.text = "Turn X";
    }
    public void CellPressed(int cellNumber)
    {
        if (!isGameOver)
        {
            playCount++;
            currentSelectedObj = cells[cellNumber];
            currentSelectedObj.transform.GetChild(0).gameObject.SetActive(true);
            if (playCount % 2 == 0)
            {
                currentSelectedObj.transform.GetChild(0).GetComponent<Image>().sprite = xoImg[1];
                currentSelectedObj.tag = "O";
            }
            else
            {
                currentSelectedObj.transform.GetChild(0).GetComponent<Image>().sprite = xoImg[0];
                currentSelectedObj.tag = "X";
            }
            CheckWinLostDraw(currentSelectedObj.tag);
            currentSelectedObj.GetComponent<Button>().interactable = false;
            if (MainMenuManager.isSinglePlayer && currentSelectedObj.tag != machineTag && !isGameOver)
            {
                isMachinePlayed = false;
                MachinePlay();
            }
        }
    }
    void CellChecker(int firstObj, int secondObj, int thirdObj,string ReqTag)
    {
        if (!isGameOver)
        {
            bool isWin = false;
            int cellNo = 0;
            if (cells[firstObj].tag == ReqTag && cells[secondObj].tag == ReqTag && cells[thirdObj].tag == "Untagged")
            {
                isWin = true;
                cellNo = thirdObj;
            }
            else if (cells[firstObj].tag == ReqTag && cells[secondObj].tag == "Untagged" && cells[thirdObj].tag == ReqTag)
            {
                isWin = true;
                cellNo = secondObj;
            }
            else if (cells[firstObj].tag == "Untagged" && cells[secondObj].tag == ReqTag && cells[thirdObj].tag == ReqTag)
            {
                isWin = true;
                cellNo = firstObj;
            }
            if (isWin)
            {
                if (!isMachinePlayed)
                {
                    CellPressed(cellNo);
                    isMachinePlayed = true;
                }
            }
        }
    }
    void MachinePlay()
    {
        //Block for Win Condition

        for (int i = 0; i < 2; i++)
        {
            string currentTag = "";
            if (i == 0)
                currentTag = machineTag;
            else
                currentTag = playerTag;
            CellChecker(0, 1, 2,currentTag);
            CellChecker(3, 4, 5, currentTag);
            CellChecker(6, 7, 8, currentTag);
            //0,1,2
            //3,4,5
            //6,7,8

            CellChecker(0, 3, 6, currentTag);
            CellChecker(1, 4, 7, currentTag);
            CellChecker(2, 5, 8, currentTag);
            //0,3,6
            //1,4,7
            //2,5,8

            CellChecker(0, 4, 8, currentTag);
            CellChecker(2, 4, 6, currentTag);
            //0,4,8
            //2,4,6
        }

        //Block for Random
        while (!isMachinePlayed)
        {
            int RandomCellNo = UnityEngine.Random.Range(0, 9);
            if (cells[RandomCellNo].tag == "Untagged")
            {
                    CellPressed(RandomCellNo);
                isMachinePlayed = true;
                break;
            }
        }

        //for (int i=0;i<9;i++)
        //{
        //    if (cells[i].tag=="Untagged")
        //    {
        //        CellPressed(i);
        //        break;
        //    }    
        //}
        //case 1: if there is win condition then win
        //case 2: resist opposite player
        //case 3: random
    }

    private void CheckWinLostDraw(string tagString)
    {
        bool isWin = false;
        //Checking for horizontal wins

        for (int i = 0; i <= 6; i += 3)
        {
            if (cells[i].tag == tagString && cells[i + 1].tag == tagString && cells[i + 2].tag == tagString)
            {
                WinningAnimation(cells[i], cells[i + 1], cells[i + 2]);
                isWin = true;
                break;
            }
        }
        //Checking for vertical wins

        for (int j = 0; j < 3; j++)
        {
            if (cells[j].tag == tagString && cells[j + 3].tag == tagString && cells[j + 6].tag == tagString)
            {
                WinningAnimation(cells[j], cells[j + 3], cells[j + 6]);
                isWin = true;
                break;
            }
        }
        //Checking for diagonal wins

        if (cells[0].tag == tagString && cells[4].tag == tagString && cells[8].tag == tagString)
        {
            isWin = true;
            WinningAnimation(cells[0], cells[4], cells[8]);
        }
        if (cells[2].tag == tagString && cells[4].tag == tagString && cells[6].tag == tagString)
        {
            isWin = true;
            WinningAnimation(cells[2], cells[4], cells[6]);
        }
        if (isWin)
        {
            youWinText.text =tagString + " Win";
            isGameOver = true;
        }
        if (!isWin && playCount == 9)
        {
            youWinText.text = "Draw";
            isGameOver = true;
        }
        if (!isWin && playCount != 9)
        {
            if (tagString == "X")
                turnText.text = "Turn O";
            else
                turnText.text = "Turn X";
        }
        else
        {
            turnText.text = "";
        }
    }
    void WinningAnimation(GameObject firstCell, GameObject secondCell, GameObject thirdCell)
    {
        //firstCell.GetComponent<Animator>().enabled = true;
        //secondCell.GetComponent<Animator>().enabled = true;
        //thirdCell.GetComponent<Animator>().enabled = true;
        LeanTween.scale(firstCell, Vector3.one * .75f, .5f).setOnComplete(
            () =>
            {
                LeanTween.scale(firstCell, Vector3.one, .5f);
            }
            );
        LeanTween.scale(secondCell, Vector3.one * .75f, .5f).setOnComplete(
            () =>
            {
                LeanTween.scale(secondCell, Vector3.one, .5f);
            }
            );
        LeanTween.scale(thirdCell, Vector3.one * .75f, .5f).setOnComplete(
            () =>
            {
                LeanTween.scale(thirdCell, Vector3.one, .5f);
            }
            );
    }
    public void RestartBtnClicked()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void BackBtnPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
