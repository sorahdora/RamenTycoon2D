using UnityEngine;
using UnityEngine.EventSystems;

public class ServingTray : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        PotDropZone pot =
            eventData.pointerDrag?.GetComponent<PotDropZone>();

        if (pot == null)
        {
            return;
        }

        pot.TryServeRamen();
    }
}