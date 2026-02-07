using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffable
{
    void AddBuff(IBuff buff);
    void RemoveBuff(IBuff buff);
}
