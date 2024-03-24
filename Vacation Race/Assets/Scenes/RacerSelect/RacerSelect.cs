using UnityEngine;
using UnityEngine.UI;

public class RacerSelect : MonoBehaviour
{
    private Button button;
    public bool selected = false;
    public RacerProfile racer_profile;
    public GameObject listElementPrefab;
    private GameObject listElementInstance;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => Select());
    }

    public void Select()
    {
        selected = transform.parent.GetComponent<SelectionManager>().UpdateList(racer_profile);

        var col = button.colors;

        if (selected)
        {
            col.normalColor = Color.green;
            col.selectedColor = Color.green;

            listElementInstance = Instantiate(listElementPrefab, transform.parent.GetComponent<SelectionManager>().listUIContainer);
            listElementInstance.transform.GetChild(0).GetComponent<Text>().text = racer_profile._name;
            listElementInstance.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => Select());
        }
        else
        {
            col.normalColor = Color.white;
            col.selectedColor = Color.white;

            Destroy(listElementInstance);
        }

        button.colors = col;

    }
}
