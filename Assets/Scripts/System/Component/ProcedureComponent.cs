using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class ProcedureComponent : BaseComponent<ProcedureComponent>
{
    public string firstProcedure;
    public List<string> allProceduresName;
    public List<ProcedureBase> allProcedures = new List<ProcedureBase>();
    public ProcedureBase currentProcedure { get; set; }
    private void Awake()
    {
        foreach (string procedure in allProceduresName)
        {
            Type t = Type.GetType(procedure);
            try
            {
                var obj = t.Instantiate() as ProcedureBase;
                allProcedures.Add(obj);
            }
            catch(Exception e)
            {
                Debug.LogError($"Type {t} Add Exception, Exception {e}");
            }
        }
        currentProcedure = allProcedures.Find((s) => s.GetType().Name == firstProcedure);
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
public class ProcedureBase
{
    public virtual void OnEnter(string data) { }
    public virtual void OnStep() { }
    public virtual void OnExit(string data) { }
}
