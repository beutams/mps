using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureComponent : BaseComponent
{
    public static List<ProcedureBase> allProcedures = new List<ProcedureBase>();
    public ProcedureBase currentProcedure { get; set; }
    private void Awake()
    {
        currentProcedure = allProcedures[0];
    }
    private void Update()
    {
        currentProcedure.OnStep();
    }
    public void Change<T>(string data = null) where T : ProcedureBase
    {
        foreach (ProcedureBase procedure in allProcedures)
        {
            if(procedure.GetType() == typeof(T))
            {
                currentProcedure.OnExit(data);
                currentProcedure = procedure;
                currentProcedure.OnEnter(data);
            }
        }
    }
}
public abstract class ProcedureBase
{
    public abstract void OnEnter(string data);
    public abstract void OnStep();
    public abstract void OnExit(string data);
}
