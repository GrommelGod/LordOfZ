using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public TMP_Text textArea;
    public GameObject[] arrows;
    public int currentArrowNumber = 0;
}
