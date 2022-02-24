using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemarkableIdentities : MonoBehaviour
{
    public void ClosePage()
    {
        GameObject currentPageOpen = GameObject.Find("Tab 3 - Remarkable Identities/Tabs").GetComponent<TabGroup>().currentPageOpen;
        currentPageOpen.SetActive(false);
    }
}
