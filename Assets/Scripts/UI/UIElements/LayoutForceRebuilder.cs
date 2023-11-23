using UnityEngine;
using UnityEngine.UI;


public class LayoutForceRebuilder : MonoBehaviour
{
    [SerializeField] private RectTransform layoutRoot;
    [SerializeField] private HorizontalOrVerticalLayoutGroup layoutGroup;
    [SerializeField] private ContentSizeFitter contentSizeFitter;


    private void LateUpdate()
    {
        UpdateLayout();
    }


    public void ForceRebuild()
    {
        UpdateLayout();
    }


    private void UpdateLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);

        if (contentSizeFitter != null)
        {
            contentSizeFitter.SetLayoutHorizontal();
            contentSizeFitter.SetLayoutVertical();
        }

        layoutGroup.CalculateLayoutInputHorizontal();
        layoutGroup.CalculateLayoutInputVertical();
    }
}
