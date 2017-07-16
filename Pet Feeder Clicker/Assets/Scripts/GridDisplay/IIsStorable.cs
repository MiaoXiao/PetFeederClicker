using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIsStorable
{
    void SetStorage(Grid new_storage);
    Grid GetCurrentStorage();
    void SetPosition(Vector3 pos);
    Transform GetTransform();

    bool CanAcceptFood();
    string GetTypeName();
}
