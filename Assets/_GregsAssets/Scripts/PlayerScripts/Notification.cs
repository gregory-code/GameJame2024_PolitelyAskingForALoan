using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Notification : MonoBehaviour
{
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private float secondsActive = 5;

    private List<GameObject> notifList = new List<GameObject>();

    public void CreateNotification(string text, Color textColor, Sprite textImage)
    {
        GameObject notif = Instantiate(popupPrefab);
        notif.transform.SetParent(transform);
        notif.transform.localScale = new Vector3(1, 1, 1);
        notif.transform.localPosition = new Vector2(-670, 650);

        notif.transform.Find("text").GetComponent<TextMeshProUGUI>().text = text;
        notif.transform.Find("text").GetComponent<TextMeshProUGUI>().color = textColor;

        notif.transform.Find("image").GetComponent<Image>().sprite = textImage;

        notifList.Insert(0, notif);

        StartCoroutine(DeleteAutomatically(notif));
    }

    private IEnumerator DeleteAutomatically(GameObject notif)
    {
        yield return new WaitForSeconds(secondsActive);
        if(notif != null)
        {
            RemoveNotif(notifList.IndexOf(notif));
        }
    }

    public void RemoveNotif(int index)
    {
        Destroy(notifList[index]);
        notifList.RemoveAt(index);
    }

    public void Update()
    {
        for(int i = 0; i < notifList.Count; i++)
        {
            Vector2 notif = Vector2.Lerp(notifList[i].transform.localPosition, new Vector2(-670, (200 - 200 * notifList.IndexOf(notifList[i]))), 12 * Time.deltaTime);
            notifList[i].transform.localPosition = notif;
        }
    }
}
