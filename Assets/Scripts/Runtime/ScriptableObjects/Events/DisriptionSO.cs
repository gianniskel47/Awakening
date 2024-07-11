using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DisriptionSO : ScriptableObject
{
    [SerializeField]protected string Listener;
    [SerializeField]protected string Brodcaster;
    [SerializeField][TextArea(5, 10)] private string Discription;
}
