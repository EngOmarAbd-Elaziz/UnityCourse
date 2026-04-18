using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";
    private Animator animator;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
        // cuttingCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }

    // private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    // {
    //     animator.SetTrigger(CUT);
    // }

}
