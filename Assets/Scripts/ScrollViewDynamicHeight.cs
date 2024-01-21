using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ScrollViewDynamicHeight : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _verticalLayout;

    private void OnEnable()
    {
        AdjustContentHeight();
    }

    //ScrollView içeriğini Content'in eleman sayısına göre düzenleme işlemi yapar
    public void AdjustContentHeight()
    {
        _verticalLayout = gameObject.GetComponent<VerticalLayoutGroup>();

        float contentHeight = _verticalLayout.padding.top + _verticalLayout.padding.bottom;

        foreach (Transform child in transform)
        {
            contentHeight += child.GetComponent<RectTransform>().rect.height + _verticalLayout.spacing;
        }
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, contentHeight);
    }
}
