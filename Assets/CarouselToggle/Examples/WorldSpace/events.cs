using UnityEngine;
using UnityEngine.UI;

public class events : MonoBehaviour {
    public AsPerSpec.CarouselToggler carousel;
	// Use this for initialization
	void Start () {
        carousel.OnSnapStarted += LogStart;
        carousel.OnSnapMoved += LogMove;
        carousel.OnSnapEnded += LogEnd;
	}
	
	// Update is called once per frame
	void LogStart(Toggle target) {
        Debug.Log("started snapping");
	}

    void LogMove(Toggle target) {
        Debug.Log("moving");
    }

    void LogEnd(Toggle target) {
        Debug.Log("finished snapping");
    }
}
