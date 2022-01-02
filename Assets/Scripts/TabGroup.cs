using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
	public TabButton selectedTab;
	public List<GameObject> objectsToSwap;

	public Color tabIdle;
	public Color tabHover;
	public Color tabActive;
    
    public void Subscribe(TabButton button)
	{
        if(tabButtons == null)
		{
			tabButtons = new List<TabButton>();
		}

		tabButtons.Add(button);
		objectsToSwap.Add(button.objectToSwap);
	}

	public void OnTabEnter(TabButton button)
	{
		ResetTabs();
		if (selectedTab == null || button != selectedTab)
		{
			button.background.color = tabHover;
		}
	}

	public void OnTabExit(TabButton button)
	{
		ResetTabs();
	}

	public void OnTabSelected(TabButton button)
	{
		if(selectedTab != null)
		{
			selectedTab.Deselect();
		}

		selectedTab = button;

		selectedTab.Select();

		ResetTabs();
		button.background.color = tabActive;
		foreach (GameObject obj in objectsToSwap)
		{
			if (obj == button.objectToSwap)
			{
				obj.SetActive(true);
			}
			else
			{
				obj.SetActive(false);
			}
		}
	}

	public void ResetTabs()
	{
		foreach (TabButton button in tabButtons)
		{
			if(selectedTab != null && button == selectedTab) { continue; }
			button.background.color = tabIdle;
		}
	}
}
