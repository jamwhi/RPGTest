using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Blocking : MonoBehaviour, 
    IPointerClickHandler, 
    IDropHandler {

    public MouseController mouseController;
    public GameObject blockPrefab;

    // Use this for initialization
    protected virtual void Awake() {
        mouseController = GameObject.FindGameObjectWithTag("UI").GetComponent<MouseController>();      
        transform.SetParent(mouseController.transform);        
    }

    public void OnDrop(PointerEventData eventData) {
        blockingAction(eventData.position);
    }

    public void OnPointerClick(PointerEventData eventData) {
        blockingAction(eventData.position);
    }

    protected abstract void blockingAction(Vector2 pos);
}

public class FrontBlocker : Blocking {

    protected override void Awake() {
        base.Awake();
        gameObject.name = "FrontBlocker";
        transform.SetSiblingIndex(2);
        transform.localScale = Vector3.one;
    }

    protected override void blockingAction(Vector2 pos) {
        mouseController.DisableAllFrontLayer();
    }
}

public class BackBlocker : Blocking {

    protected override void Awake() {
        base.Awake();
        gameObject.name = "BackBlocker";
        transform.SetAsFirstSibling();
        transform.localScale = Vector3.one;
    }

    protected override void blockingAction(Vector2 pos) {
        if(mouseController.itemOnMouse != null) {
            mouseController.DropItemConfirm(pos);
        }
    }
}
